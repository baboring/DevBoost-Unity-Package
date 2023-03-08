using System;
using System.Linq;
using System.Reflection;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Text.RegularExpressions;
using DevBoost.Utilities;
using System.ComponentModel;

namespace DevBoost.DataTool
{
    public class SheetLoader
    {
        public const string URLFormat = @"https://docs.google.com/spreadsheets/export?id={0}&exportFormat=csv&gid={2}";
        //public const string URLFormat = @"https://docs.google.com/spreadsheets/d/{0}/gviz/tq?tqx=out:csv&sheet={1}";

        private readonly DataContainerBase container;
        private readonly FieldInfo[] listsInfos;
        private readonly string documentID;

        public Action<DataContainerBase> onComplete;

        public bool abort;

        private string output;
        public string Output
        {
            get => output;

            private set
            {
                output = value;
                onOutputChanged.Invoke();
            }
        }
        public Action onOutputChanged;

        public float progress;
        public float Progress
        {
            get => progress;

            private set
            {
                progress = Mathf.Clamp01(value);
                onProgressChanged.Invoke();
            }
        }
        public Action onProgressChanged;

        private float ProgressElementDelta => 1f / listsInfos.Length;

        public SheetLoader(DataContainerBase container, FieldInfo[] listsInfos)
        {
            this.container = container;
            this.listsInfos = listsInfos;
            this.documentID = container.documentID;
        }

        public async void Run()
        {
            abort = false;
            var webClient = new WebClient();

            for (int i = 0; i < listsInfos.Length && !abort; i++)
                await PopulateList(container, listsInfos[i], webClient);

            webClient.Dispose();

            onComplete.Invoke(container);
        }

        private async Task PopulateList(DataContainerBase container, FieldInfo listInfo, WebClient webClient)
        {
            var contentType = listInfo.FieldType.GetGenericArguments().SingleOrDefault();
            if (contentType is null)
            {
                Debug.LogError($"Could not identify type of defs stored in {listInfo.Name}");
                return;
            }

            #region Downloading page -------------------------------

            var googleSheetRef = (PageNameAttribute)Attribute.GetCustomAttribute(listInfo, typeof(PageNameAttribute));
            var pagename = googleSheetRef.name;
            var gid = googleSheetRef.gid;

            Output = $"Downloading page '{pagename}'...";

            var url = string.Format(URLFormat, documentID, pagename, gid);
            Task<string> request;

            try
            {
                request = webClient.DownloadStringTaskAsync(url);
            }
            catch (WebException)
            {
                Debug.LogError($"Bad URL '{url}'");
                abort = true;
                throw;
            }

            while (!request.IsCompleted)
                await Task.Delay(100);

            var rawTable = Regex.Split(request.Result, "\r\n|\r|\n");
            request.Dispose();

            Progress += 1 / 3f * ProgressElementDelta;

            #endregion

            #region Analyzing and splitting raw text ------------------------

            Output = $"Analysing headers...";

            var headersRaw = CsvUtil.Split(rawTable[0]);

            var idHeaderIdx = -1;
            var headers = new List<string>();
            var emptyHeadersIdxs = new List<int>();
            for (int i = 0; i < headersRaw.Length; i++)
            {
                if (string.IsNullOrEmpty(headersRaw[i]))
                {
                    emptyHeadersIdxs.Add(i);
                    continue;
                }

                if (idHeaderIdx == -1 && headersRaw[i].ToLower() == "id")
                    idHeaderIdx = i;

                headers.Add(headersRaw[i]);
            }

            var rows = new List<string[]>();
            for (int i = 1; i < rawTable.Length; i++)
            {
                var substrings = CsvUtil.Split(rawTable[i]);
                if (idHeaderIdx != -1 && string.IsNullOrEmpty(substrings[idHeaderIdx]))
                    continue;

                rows.Add(substrings.Where((val, index) => !emptyHeadersIdxs.Contains(index)).ToArray());
            }

            Progress += 1 / 3f * ProgressElementDelta;

            #endregion

            #region Parsing and populating list of defs 

            Output = $"Populating list of defs '{listInfo.Name}'<{contentType.Name}>...";

            FieldInfo[] fi = contentType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            var headersToFields = new Dictionary<string, FieldInfo>();
            foreach (var h in headers)
            {
                var found = fi.FirstOrDefault(v => v.Name == h || (v.FieldType.IsArray && $"{v.Name}[]" == h));
                if (found == null)
                {
                    Debug.LogWarning($"Header '{h}' match no field in {contentType.Name} type");
                    continue;
                }
                headersToFields.Add(h, found);
            }

            var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(contentType));
            var oldList = (IList)listInfo.GetValue(container);
            foreach (var row in rows)
            {
                int idx = list.Count;
                object item;
                if (idx < oldList.Count)
                    item = oldList[idx];
                else
                    item = Activator.CreateInstance(contentType);

                for (int i = 0; i < headers.Count && i < row.Length; i++)
                    if (headersToFields.TryGetValue(headers[i], out var field))
                        field.SetValue(item, field.FieldType == typeof(string) ? row[i] : CsvUtil.ParseString(row[i], field.FieldType));

                list.Add(item);
            }

            listInfo.SetValue(container, list);

            Progress += 1 / 3f * ProgressElementDelta;

            #endregion
        }

    }

    public class PageNameAttribute : Attribute
    {
        public readonly string name;
        public readonly int gid;

        public PageNameAttribute(string name, int gid)
        {
            this.name = name;
            this.gid = gid;
        }
    }
}
