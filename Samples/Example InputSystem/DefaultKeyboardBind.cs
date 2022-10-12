using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Example.InputSystem
{
    /// <summary>
    /// Scriptable object used to define default keyboard binds
    /// </summary>
    [CreateAssetMenu(fileName = "Default KeyboardBind", menuName = "Input/Keyboard Binds", order = 0)]
    public class DefaultKeyboardBind : ScriptableObject
    {

        [ReorderableList] public GameKeyCodeBind[] gamekeyBinds;
        [ReorderableList] public MenuKeyBind[] menuButtonBinds;

        public GameKeyCodeBind[] CloneKeyboardBind 
        {
            get
            {
                List<GameKeyCodeBind> newBind = new List<GameKeyCodeBind>(gamekeyBinds);
                return newBind.ToArray();
            }
        }

        /// ----------------
        /// Get Menu Button Sprite
        /// ----------------
        public Sprite GetMenuButtonSprite(MenuNameCode nameCode)
        {
            for(int bIndex = 0; bIndex < menuButtonBinds.Length; bIndex++)
            {
                if(menuButtonBinds[bIndex].nameCode == nameCode)
                {
                    return InputBindings.Instance.GetKeyCodeSprite(menuButtonBinds[bIndex].keyName);
                }
            }
            return null;
        }

        /// <summary>
        /// Get Keycode registered under name code
        /// </summary>
        public KeyCode GetMenuKeyCode(MenuNameCode nameCode)
        {
            for(int bIndex = 0; bIndex < menuButtonBinds.Length; bIndex++)
            {
                if(menuButtonBinds[bIndex].nameCode == nameCode)
                {
                    return menuButtonBinds[bIndex].keyName;
                }
            }
            return KeyCode.None;
        }

    }

}
