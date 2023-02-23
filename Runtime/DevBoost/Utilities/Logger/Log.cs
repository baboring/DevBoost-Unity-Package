#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX) && FILE_LOG
#define SAVE_ENABLED
#endif

using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

using System.IO;
using System.Text;

namespace DevBoost.Utilities {

    public class Log : SingletonMono<Log>
    {
        //-------------------------------------------------------------------------------------------------------------------------
        [SerializeField] private string logFile = "FileLog";
        [SerializeField] private string logFolder = "Logs";
        [SerializeField] private string timeStamp = "{0:yyyy-MM-dd_HH.mm.ss}{1}";
        [SerializeField] private bool isCaptureLog = true;
        [SerializeField] private bool isEchoToConsole = true;
        [SerializeField] private bool isAddtimeStamp = true;
        [SerializeField] private bool isDeleteOnStart = true;
        [SerializeField] private bool isPersistentDataPath = false;
        //-------------------------------------------------------------------------------------------------------------------------

        public bool IsCaptureLog { get { return this.isCaptureLog; } }
        private StreamWriter OutputStream;

        protected new void Awake()
        {
            base.Awake();
            CreatelogFile();

            entries = new List<Entry>(maxEntries);
            if (_isVisible)
            {
                _isVisible = false;
                SetVisible(true);
            }

            if(isCaptureLog)
                Application.logMessageReceived += CaptureLog;

        }

        [Conditional("SAVE_ENABLED")]
        void CreatelogFile()
        {
            int index = 0;
            DateTime now = DateTime.Now;
            string filename = logFile;

            // udate file name and path
            if (!Application.isEditor)
            {
                filename = logFile + " " + String.Format(timeStamp, now, index > 0 ? "_" + index : "");

                if (isPersistentDataPath)
                    logFolder = Application.persistentDataPath + "/" + logFolder;
            }

            filename += ".log";


            if (logFolder.Length > 0)
            {
                System.IO.Directory.CreateDirectory(logFolder);
                filename = logFolder + "/" + filename;
            }
            if (isDeleteOnStart)
                File.Delete(filename);
            // Open the log file to append the new log to it.
            OutputStream = new StreamWriter(filename, true);
            Log.Trace("<<< Logger Start >>>\n\n");
        }

        [Conditional("SAVE_ENABLED")]
        protected new void OnDestroy()
        {
            if (OutputStream != null)
            {
                OutputStream.Close();
                OutputStream = null;
            }
            base.OnDestroy();
        }

        [Conditional("SAVE_ENABLED")]
        private void Write(string message, bool istimeStamp = true)
        {

            // window debug message
            System.Diagnostics.Debug.WriteLine(message);

            if (isAddtimeStamp && istimeStamp)
            {
                DateTime now = DateTime.Now;
                message = string.Format("[{0:H:mm:ss}] {1}", now, message);
            }

            if (OutputStream != null)
            {
                OutputStream.WriteLine(message);
                OutputStream.Flush();
            }
        }


        [Conditional("FILE_LOG")]
        public static void Trace(String format, params object[] args)
        {
            if (Instance != null)
            {
                if (Instance.isEchoToConsole)
                {
                    UnityEngine.Debug.Log(string.Format(format, args));
                    if (Instance.isCaptureLog)
                        return;
                }

                Instance.Write(string.Format(format, args));
            }
            //else
            //    // Fallback if the debugging system hasn't been initialized yet.
            //    UnityEngine.Debug.Log(Message);
        }


        [Serializable]
        public class LogMask
        {
            public bool message;
            public bool warning;
            public bool error;

            public LogMask() { }

            public LogMask(bool message, bool warning, bool error)
            {
                this.message = message;
                this.warning = warning;
                this.error = error;
            }

            public bool Filter(LogType type)
            {
                switch (type)
                {
                    case LogType.Log: return message;
                    case LogType.Warning: return warning;
                    case LogType.Assert:
                    case LogType.Exception:
                    case LogType.Error: return error;
                }

                return true;
            }
        }

        private class CollapsedEntries : KeyedCollection<string, Entry>
        {
            protected override string GetKeyForItem(Entry entry)
            {
                return entry.key;
            }
        }

        private class Entry
        {
            public string key;
            public string log;
            public string logSpaced;
            public string stacktrace;
            public string stacktraceTabed;
            public LogType type;
            public bool expanded;
        }

        public enum Position
        {
            Bottom,
            Top,
        }

        public Position position = Position.Bottom;

        public LogMask captureLogMask = new LogMask { message = true, warning = true, error = true };
        public LogMask filterLogMask = new LogMask { message = true, warning = true, error = true };
        public LogMask autoShowOnLogMask = new LogMask { message = false, warning = true, error = true };

        public int maxEntries = 1000;

        public int windowheight = 150;

        public KeyCode showByKey = KeyCode.Tab;

        [SerializeField]
        private bool _isVisible = true;


        public GUISkin guiSkin = null;
        public int guiDepth = 0;


        public bool collapse = false;
        public bool autoScroll = true;
        public bool unlockCursorWhenVisible = true;

        private List<Entry> entries;
        private int messageCount;
        private int warningCount;
        private int errorCount;
        private Vector2 scrollPosition;
        private bool oldLockCursor;

        private static readonly Color[] typeColors =
        {
            Color.red,
            Color.magenta,
            Color.yellow,
            Color.white,
            Color.red
        };

        private const float WINDOW_MARGIN_X = 10;
        private const float WINDOW_MARGIN_Y = 10;

        public bool isVisible
        {
            get { return _isVisible; }
            set { SetVisible(value); }
        }


#if DEBUG_CONFIG            
        void Update()
        {
            if (showByKey != KeyCode.None && Input.GetKeyDown(showByKey) && Input.GetKey(KeyCode.LeftControl))
                SetVisible(!_isVisible);
        }
#endif
#if FILE_LOG
        void OnGUI()
        {
            if (!_isVisible) return;

            var oldSkin = GUI.skin;
            var oldDepth = GUI.depth;
            var oldColor = GUI.color;

            GUI.skin = guiSkin;
            GUI.depth = guiDepth;
            GUI.color = Color.white;

            float y = (position == Position.Bottom) ? Screen.height - windowheight - WINDOW_MARGIN_Y : WINDOW_MARGIN_Y;

            GUILayout.BeginArea(new Rect(WINDOW_MARGIN_X, y, Screen.width - WINDOW_MARGIN_X * 2, windowheight), GUI.skin.box);
            DrawGUI();
            GUILayout.EndArea();

            GUI.skin = oldSkin;
            GUI.depth = oldDepth;
            GUI.color = oldColor;
        }
#endif
        void DrawGUI()
        {
            if (null == entries)
                return;
            var stackTraceLabelStyle = new GUIStyle(GUI.skin.box);
            stackTraceLabelStyle.alignment = TextAnchor.UpperLeft;
            stackTraceLabelStyle.fontSize = 10;

            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();

            if (entries.Count == 0) GUI.enabled = false;

            if (GUILayout.Button("Clear", GUILayout.Width(50)))
                Clear();

            GUILayout.Space(5);

            if (GUILayout.Button("Save", GUILayout.Width(50)))
                Save();

            GUILayout.Space(5);

            if (entries.Count == 0)
                GUI.enabled = true;

            collapse = GUILayout.Toggle(collapse, " Collapse", GUILayout.ExpandWidth(false));

            GUILayout.Space(5);

            autoScroll = GUILayout.Toggle(autoScroll, " Auto Scroll", GUILayout.ExpandWidth(false));

            GUILayout.FlexibleSpace();

            IEnumerable<Entry> drawEntries;
            int drawMessageCount;
            int drawWarningCount;
            int drawErrorCount;

            if (collapse)
            {
                var collapsedEntries = new CollapsedEntries();
                drawMessageCount = 0;
                drawWarningCount = 0;
                drawErrorCount = 0;

                for (int i = 0; i < entries.Count; i++)
                {
                    var entry = entries[i];

                    if (!collapsedEntries.Contains(entry.key))
                    {
                        collapsedEntries.Add(entry);

                        switch (entry.type)
                        {
                            case LogType.Log:
                                drawMessageCount++;
                                break;
                            case LogType.Warning:
                                drawWarningCount++;
                                break;
                            case LogType.Assert:
                            case LogType.Exception:
                            case LogType.Error:
                                drawErrorCount++;
                                break;
                        }
                    }
                }

                drawEntries = collapsedEntries;
            }
            else
            {
                drawEntries = entries;
                drawMessageCount = messageCount;
                drawWarningCount = warningCount;
                drawErrorCount = errorCount;
            }

            GUI.color = drawMessageCount > 0 ? typeColors[(int)LogType.Log] : Color.grey;
            filterLogMask.message = GUILayout.Toggle(filterLogMask.message, " " + drawMessageCount + " Message(s)", GUILayout.ExpandWidth(false));
            GUI.color = Color.white;

            GUILayout.Space(5);

            GUI.color = drawWarningCount > 0 ? typeColors[(int)LogType.Warning] : Color.grey;
            filterLogMask.warning = GUILayout.Toggle(filterLogMask.warning, " " + drawWarningCount + " Warning(s)", GUILayout.ExpandWidth(false));
            GUI.color = Color.white;

            GUILayout.Space(5);

            GUI.color = drawErrorCount > 0 ? typeColors[(int)LogType.Error] : Color.grey;
            filterLogMask.error = GUILayout.Toggle(filterLogMask.error, " " + drawErrorCount + " Error(s)", GUILayout.ExpandWidth(false));
            GUI.color = Color.white;

            GUILayout.EndHorizontal();

            GUI.changed = false;
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUI.skin.box);
            if (GUI.changed) autoScroll = false;

            foreach (var entry in drawEntries)
            {
                if (filterLogMask.Filter(entry.type))
                {
                    int typeIndex = (int)entry.type;
                    GUI.color = (typeIndex < typeColors.Length) ? typeColors[typeIndex] : Color.white;

                    GUI.changed = false;
                    entry.expanded = GUILayout.Toggle(entry.expanded, entry.logSpaced, GUILayout.ExpandWidth(false));
                    if (GUI.changed) autoScroll = false;

                    if (entry.expanded)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Label(entry.stacktrace, stackTraceLabelStyle);
                        GUILayout.EndHorizontal();
                    }
                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        public void SetVisible(bool visibility)
        {
            if (_isVisible == visibility) return;
            _isVisible = visibility;

            if (unlockCursorWhenVisible)
            {
                if (visibility)
                {
                    oldLockCursor = Cursor.visible;
                }
                else
                {
                    Cursor.visible = oldLockCursor;
                }
            }
        }

        public void Clear()
        {
            entries.Clear();
            messageCount = 0;
            warningCount = 0;
            errorCount = 0;

            SetVisible(false);
        }

        [Conditional("SAVE_ENABLED")]
        public void Save()
        {
            int index = 0;
            DateTime now = DateTime.Now;
            string filename = String.Empty;

            do
            {
                filename = String.Format(timeStamp, now, index > 0 ? "_" + index : "");

                index++;
                if (index == 1000)
                {
                    UnityEngine.Debug.LogError("Failed to save console log, because too many log files with the same filename");
                    return;
                }

            } while (File.Exists(filename));

            var sb = new StringBuilder();

            for (int i = 0; i < entries.Count; i++)
            {
                var entry = entries[i];
                sb.AppendLine(entry.log);
                sb.AppendLine(entry.stacktraceTabed);
                sb.AppendLine();
            }

            UnityEngine.Debug.Log("Saving console log to " + filename);

            File.WriteAllText(filename, sb.ToString());
            //throw new NotSupportedException("Save is not supported on " + Application.platform);
        }

        void CaptureLog(string log, string stacktrace, LogType type)
        {
            if (!captureLogMask.Filter(type)) return;

            if (autoShowOnLogMask.Filter(type))
            {
                isVisible = true;
            }

            if (entries.Count == maxEntries)
            {
                var lastType = entries[0].type;
                entries.RemoveAt(0);

                switch (lastType)
                {
                    case LogType.Log: messageCount--; break;
                    case LogType.Warning: warningCount--; break;
                    case LogType.Assert:
                    case LogType.Exception:
                    case LogType.Error: errorCount--; break;
                }
            }

            stacktrace = stacktrace.Trim('\n');
            var key = String.Format("{0}:{1}\n{2}", (int)type, log, stacktrace);
            var logSpaced = ' ' + log.Replace("\n", "\n ");
            var stacktraceTabed = '\t' + stacktrace.Replace("\n", "\n\t");

            var entry = new Entry
            {
                key = key,
                log = log,
                logSpaced = logSpaced,
                stacktrace = stacktrace,
                stacktraceTabed = stacktraceTabed,
                type = type,
                expanded = false
            };

            entries.Add(entry);

            switch (type)
            {
                case LogType.Log: messageCount++; break;
                case LogType.Warning: warningCount++; break;
                case LogType.Assert:
                case LogType.Exception:
                case LogType.Error: errorCount++; break;
            }

            if (Log.Instance != null)
            {
                Log.Instance.Write(log);
                if (type != LogType.Log && type != LogType.Warning)
                    Log.Instance.Write(stacktrace, false);
            }


            if (autoScroll)
                scrollPosition.y = float.MaxValue;
        }
    }

}