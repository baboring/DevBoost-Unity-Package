/* ---------------------------------------------------------------------
 * Created on Mon Jan 20 2021 3:33:10 PM
 * Author : Benjamin Park
 * Description : Test Mode
--------------------------------------------------------------------- */

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Newtonsoft.Json;

namespace DevBoost
{

    public class PopReachToolWindow : EditorWindow
    {
        // Optional, but may be convenient.
        private List<IGUI> gui = new List<IGUI>();
        private static string config = "/debug.cfg";

        [MenuItem("PopReach/Tool/Tool Window")]
        public static void ShowWindow()
        {
            GetWindow<PopReachToolWindow>(false, "Tool", true);
        }

        //[MenuItem("PopReach/DebugMode/PlayMode (Mobile)")]
        static void RunTestModeMobile()
        {
            switchType(SystemPlatform.PLATFORM.MOBILE);
        }

        //[MenuItem("PopReach/DebugMode/PlayMode (WebGL)")]
        static void RunTestModeWebGL()
        {
            switchType(SystemPlatform.PLATFORM.WEBGLPLAYER);
        }

        private static void switchServer(string serverStr)
        {
            string newSymbols = serverStr;
            var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(GetCurrentTargetGroup()).Split(';');
            foreach (var symbol in symbols)
            {
                if (symbol.StartsWith("POPREACH"))
                    continue;
                if (newSymbols == "")
                    newSymbols += symbol;
                else
                    newSymbols += ";" + symbol;
            }

            PlayerSettings.SetScriptingDefineSymbolsForGroup(GetCurrentTargetGroup(), newSymbols);
        }

        private static BuildTargetGroup GetCurrentTargetGroup()
        {
            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.iOS: return BuildTargetGroup.iOS;
                case BuildTarget.Android: return BuildTargetGroup.Android;
            }
            return BuildTargetGroup.Unknown;
        }

        [InitializeOnEnterPlayMode]
        static void OnEnterPlaymodeInEditor(EnterPlayModeOptions options)
        {

            var filePath = Application.persistentDataPath + config;
            // check if file exists
            if (!File.Exists(filePath))
                return;

            var Data = ReadFromJsonFile<SystemPlatform.SaveData>(filePath);
            if (null != Data)
                SystemPlatform.mode = Data.mode;

            Debug.Log("Entering PlayMode : " + SystemPlatform.mode);


            File.Delete(filePath);
        }

        void OnEnable()
        {
            gui.Clear();

            this.maxSize = new Vector2(300, 300);

            var button = new GUIButton();
            button.label.text = "Add All view Sizes";
            button.Clicked += GameViewEditor.AddAllSize;
            gui.Add(button);

            button = new GUIButton();
            button.label.text = "Switch to Dev build";
            button.Clicked += () => switchServer("POPREACH_DEV");
            gui.Add(button);

            button = new GUIButton();
            button.label.text = "Switch to QA build";
            button.Clicked += () => switchServer("POPREACH_QA");
            gui.Add(button);

        }



        static void switchType(SystemPlatform.PLATFORM platform)
        {
            var Data = new SystemPlatform.SaveData()
            {
                mode = platform,
            };

            GameViewEditor.AddAllSize();
            if (Data.mode == SystemPlatform.PLATFORM.WEBGLPLAYER)
                GameViewEditor.SetGamesFaceBook();
            if (Data.mode == SystemPlatform.PLATFORM.MOBILE)
                GameViewEditor.SetGamesViewMobile();

            WriteToJsonFile<SystemPlatform.SaveData>(Application.persistentDataPath + config, Data);

            EditorApplication.isPlaying = true;
        }



        /// <summary>
        /// Writes the given object instance to a Json file.
        /// <para>Object type must have a parameterless constructor.</para>
        /// <para>Only Public properties and variables will be written to the file. These can be any type though, even other classes.</para>
        /// <para>If there are public properties/variables that you do not want written to the file, decorate them with the [JsonIgnore] attribute.</para>
        /// </summary>
        /// <typeparam name="T">The type of object being written to the file.</typeparam>
        /// <param name="filePath">The file path to write the object instance to.</param>
        /// <param name="objectToWrite">The object instance to write to the file.</param>
        /// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
        public static void WriteToJsonFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                var contentsToWriteToFile = JsonConvert.SerializeObject(objectToWrite);
                writer = new StreamWriter(filePath, append);
                writer.Write(contentsToWriteToFile);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        /// <summary>
        /// Reads an object instance from an Json file.
        /// <para>Object type must have a parameterless constructor.</para>
        /// </summary>
        /// <typeparam name="T">The type of object to read from the file.</typeparam>
        /// <param name="filePath">The file path to read the object instance from.</param>
        /// <returns>Returns a new instance of the object read from the Json file.</returns>
        public static T ReadFromJsonFile<T>(string filePath) where T : new()
        {
            TextReader reader = null;
            try
            {
                reader = new StreamReader(filePath);
                var fileContents = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(fileContents);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }





        void OnGUI()
        {
            foreach (var item in gui)
                item.OnGUI();
        }

        public interface IGUI
        {
            void OnGUI();
        }

        public class GUITextField : IGUI
        {
            public string text = "";
            public GUIContent label = new GUIContent();

            // Unused in my example, but you may want to check if
            // a textbox becomes empty for example.
            public event System.Action<string> TextChanged;

            public void OnGUI()
            {
                // Also I wanted to show you BeginChangeCheck and EndChangeCheck 
                // which is the Unity GUI way of checking if a GUI control changed...
                EditorGUI.BeginChangeCheck();
                text = EditorGUILayout.TextField(label, text);
                if (EditorGUI.EndChangeCheck() && TextChanged != null)
                    TextChanged(text);
            }
        }

        public class GUIButton : IGUI
        {
            public GUIContent label = new GUIContent();
            public event System.Action Clicked;

            public void OnGUI()
            {
                if (GUILayout.Button(label) && Clicked != null)
                    Clicked();
            }
        }
    }

}