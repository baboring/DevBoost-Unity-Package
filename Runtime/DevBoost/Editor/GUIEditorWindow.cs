/* ---------------------------------------------------------------------
 * Created on Mon Jan 20 2023 3:33:10 PM
 * Author : Benjamin Park
 * Description : GUI Window
--------------------------------------------------------------------- */

using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace DevBoost
{
    public interface IGUI
    {
        void OnGUI();
    }

    public abstract class GUIEditorWindow : EditorWindow
    {
        protected List<IGUI> gui = new List<IGUI>();

        protected string filePath => Application.persistentDataPath + $"/{this.GetType().Name}.cfg";

        protected abstract void OnGUI();
        protected virtual void Initialize()
        {
            gui.Clear();

        }
    }


    public class GUITextField : IGUI
    {
        public string text = "";
        public GUIContent label = new GUIContent();

        // Unused in my example, but you may want to check if
        // a textbox becomes empty for example.
        public event System.Action<string> TextChanged;

        public static GUITextField Create(string text = "", System.Action<string> onChange = null)
        {
            return new GUITextField() { text = text,TextChanged = onChange };
        }
        public static GUITextField Create(string label, string text, System.Action<string> onChange = null)
        {
            var gui = Create(text, onChange);
            gui.label.text = label;

            return gui;
        }

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

        public static GUIButton Create(string text = "", System.Action onClick = null)
        {
            return new GUIButton(text) { Clicked = onClick };
        }

        public GUIButton(string text = "")
        {
            label.text = text;
        }

        public void OnGUI()
        {
            if (GUILayout.Button(label) && Clicked != null)
                Clicked();
        }
    }

    public class GUIDropDown : IGUI
    {
        public int selected;
        public string[] options = null;
        public GUIContent label = new GUIContent();
        public event System.Action<string> Changed;

        public static GUIDropDown Create(string[] items, string label = "", string first = "", System.Action<string> onChange = null)
        {
            return new GUIDropDown(items, label, first) { Changed = onChange };
        }

        public GUIDropDown(string[] items, string text = "", string first = "")
        {
            selected = ArrayUtility.FindIndex( items, v => v == first);
            options = items;
            label.text = text;
        }

        public void OnGUI()
        {
            if (options != null)
            {
                int previous = selected;
                selected = EditorGUILayout.Popup(label, selected, options);
                if (EditorGUI.EndChangeCheck())
                {
                    //Debug.Log(options[selected]);
                    if (previous != selected && options.Length > 0)
                        Changed?.Invoke(selected < 0 ? options[0] : options[selected]);
                }
            }
        }
    }


    public class GUILabelField : IGUI
    {
        public GUIContent label = new GUIContent();
        public bool isBold;

        public static GUILabelField Create(string text, bool isBold = false)
        {
            return new GUILabelField(text) { isBold = isBold };
        }

        public GUILabelField(string text)
        {
            label.text = text;
        }

        public void OnGUI()
        {
            EditorGUILayout.LabelField(label, isBold? EditorStyles.boldLabel: GUIStyle.none);
        }
    }

    public class GUICheckBox : IGUI
    {
        public GUIContent label = new GUIContent();
        public bool isChecked;
        public bool isLeftToggle;
        public System.Action<bool> onChanged;


        public static GUICheckBox Create(string label, bool isChecked = false, System.Action<bool> onChanged = null)
        {
            return new GUICheckBox(label, isChecked) { onChanged = onChanged };
        }

        public GUICheckBox(string label, bool isChecked = false)
        {
            this.label.text = label;
            this.isChecked = isChecked;
        }
        
        public void AddListener(System.Action<bool> callback)
        {
            onChanged += callback;
        }

        public void RemoveListener(System.Action<bool> callback)
        {
            onChanged -= callback;
        }

        public void OnGUI()
        {
            if (isLeftToggle && isChecked != EditorGUILayout.ToggleLeft(label, isChecked))
            {
                isChecked = !isChecked;
                onChanged?.Invoke(isChecked);
            }
            else if (!isLeftToggle && isChecked != EditorGUILayout.Toggle(label, isChecked))
            {
                isChecked = !isChecked;
                onChanged?.Invoke(isChecked);
            }
        }
    }

    public class GUISpace : IGUI
    {
        public static GUISpace Create(int width = 0)
        {
            return new GUISpace() { width = width };
        }

        public float width = 0;
        public bool expand;
        public void OnGUI()
        {
            if (width > 0)
                EditorGUILayout.Space(width);
            else
                EditorGUILayout.Space();
        }
    }



}
