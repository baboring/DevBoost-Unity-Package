// Mostly from https://pastebin.com/rn3Z2zf9
// Don't put this in an Editor folder, it will cause the game to not compile when building for device.

using System;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Reflection;
 
/// <summary>
/// Stick this on a method to draw a button in the editor.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Method)]
public class EditorButtonAttribute : PropertyAttribute {
    private string name = string.Empty;

    public string Name {
        get {
            return this.name;
        }
    }

    /// <summary>
    /// Default constructor, uses the method name.
    /// </summary>
    public EditorButtonAttribute() {}

    /// <summary>
    /// Gives the button a specific label.
    /// </summary>
    /// <param name="buttonLabel">Label to give the button.</param>
    public EditorButtonAttribute(string buttonLabel) {
        this.name = buttonLabel;
    }
}
 
#if UNITY_EDITOR

/// <summary>
/// The editor button uses reflection to call its method.
/// </summary>
[CustomEditor(typeof(UnityEngine.Object), true), CanEditMultipleObjects]
public class EditorButton : Editor
{
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
 
       // Loop through all methods with no parameters
        var methods = this.target.GetType()
            .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(m => m.GetParameters().Length == 0);
        foreach (var method in methods)
        {
            // Get the ButtonAttribute on the method (if any)
            var ba = (EditorButtonAttribute)Attribute.GetCustomAttribute(method, typeof(EditorButtonAttribute));
            if (ba != null) {
                // If there is a button attribute on the method draw the button.
                var buttonName = String.IsNullOrEmpty(ba.Name) ? ObjectNames.NicifyVariableName(method.Name) : ba.Name;
                if (GUILayout.Button(buttonName)) {
                    foreach (var t in this.targets) {
                        method.Invoke(t, null);
                    }
                }

                GUI.enabled = true;
            }
        }
    }
}

#endif
