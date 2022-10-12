
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Example.InputSystem
{
    /// <summary>
    /// Used to create joystick binds
    /// </summary>
    [CreateAssetMenu(fileName = "JoystickBind", menuName = "Input/JoystickBind", order = 0)]
    public class JoystickBind : ScriptableObject
    {
        [SerializeField] private InputType m_InputType;

        [SerializeField] private string[] prefixDpad;   // for d-pad

        [Header("Right Axis")]
        [SerializeField] public ButtonCode RightAxisH = ButtonCode.Axis3;   // for right axis ( horizontal ) 
        [SerializeField] public ButtonCode RightAxisV = ButtonCode.Axis4;   // for right axis ( Vertical )

        [Header("D-Pad Axis")]
        [SerializeField] public ButtonCode DPadAxisH = ButtonCode.Axis6;   // for D-Pad axis ( horizontal ) 
        [SerializeField] public ButtonCode DPadAxisV = ButtonCode.Axis7;   // for D-Pad axis ( Vertical )


        public InputType InputType
        {
            get { return m_InputType; }
        }

        [SerializeField] private string[] m_JoystickNames = null;
        
        /// <summary>
        /// Some Button need to be exclusive
        /// </summary>
        [ReorderableList] public ButtonCode[] exclusiveButtons;

        [ReorderableList] public GameButtonBind[] gameButtonBinds;
        [ReorderableList] public MenuButtonBind[] menuButtonBinds;

        public GameButtonBind[] CloneGameBinds {
            get {
                List<GameButtonBind> newBind = new List<GameButtonBind>(gameButtonBinds);
                return newBind.ToArray();
                //return (GameButtonBind[])gameButtonBinds.Clone();
            }
        }

        /// <summary>
        /// Checking the button is in exclusive list
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool IsExclusiveButton(ButtonCode code)
        {
            return System.Array.IndexOf(exclusiveButtons, code) > -1;
        }


        // get whether this is contained named in it
        public bool IsJoystickName(string joystickName)
        {
            for(int nIndex=0; nIndex < m_JoystickNames.Length; nIndex++)
            {
                if(joystickName.ToLower().Contains(m_JoystickNames[nIndex].ToLower()))
                {
                    return true;
                }
            }
            return false;
        }

        // find index of the joysic name
        public int FindIndexName(string joystickName)
        {
            for (int nIndex = 0; nIndex < m_JoystickNames.Length; nIndex++)
            {
                if (joystickName.ToLower().Equals(m_JoystickNames[nIndex].ToLower()))
                    return nIndex;
            }
            return -1;
        }

        /// <summary>
        /// Get Game button sprite
        /// </summary>
        //public Sprite GetGameButtonSprite(Gameplay.InputNameCode nameCode)
        //{
        //    for (int bIndex = 0; bIndex < gameButtonBinds.Length; bIndex++)
        //    {
        //        if (gameButtonbind.nameCode == nameCode)
        //        {
        //            return InputBindings.Instance.GetButtonSprite(m_InputType, gameButtonbind.buttonName);
        //        }
        //    }
        //    return null;
        //}

        /// ----------------
        /// Get Menu Button Sprite
        /// ----------------
        public Sprite GetMenuButtonSprite(MenuNameCode nameCode)
        {
            for (int bIndex = 0; bIndex < menuButtonBinds.Length; bIndex++)
            {
                if (menuButtonBinds[bIndex].nameCode == nameCode)
                {
                    return InputBindings.Instance.GetButtonSprite(m_InputType, menuButtonBinds[bIndex].buttonName);
                }
            }
            return null;
        }

        /// <summary>
        /// Get buttoncode registered under name code
        /// </summary>
        public ButtonCode GetMenuButtonCode(MenuNameCode nameCode)
        {
            for(int bIndex = 0; bIndex < menuButtonBinds.Length; bIndex++)
            {
                if(menuButtonBinds[bIndex].nameCode == nameCode)
                {
                    return menuButtonBinds[bIndex].buttonName;
                }
            }
            return ButtonCode.NONE;
        }

        public ButtonCode GetGameButtonCode(InputNameCode nameCode)
        {
            for(int bIndex = 0; bIndex < menuButtonBinds.Length; bIndex++)
            {
                if(gameButtonBinds[bIndex].nameCode == nameCode)
                {
                    return gameButtonBinds[bIndex].buttonName;
                }
            }
            return ButtonCode.NONE;
        }
        /// <summary>
        /// Get name of button registered to given input name
        /// </summary>
        public string GetGameButtonName(InputNameCode nameCode)
        {
            var button = GetGameButtonCode(nameCode);
            if(button != ButtonCode.NONE)
                return InputBindings.GetButtonCodeName(button);
            return null;
        }


        /// <summary>
        /// Get name of button registered under name code
        /// </summary>
        public string GetMenuButtonName(MenuNameCode nameCode, JoystickInfo joyInfo)
        {
            bool isFiltered;
            var button = GetMenuButtonCode(nameCode);
            if(button != ButtonCode.NONE)
                return GetBindButtonName(button, joyInfo.ControlNum, out isFiltered);
            return null;
        }

        /// <summary>
        /// Get button name filtered
        /// </summary>
        /// <param name="bind"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public string GetBindButtonName(GameButtonBind bind, JoystickInfo joyInfo)
        {
            bool isFiltered;
            return GetBindButtonName(bind.buttonName, joyInfo.ControlNum, out isFiltered);
        }


        public string GetBindButtonName(ButtonCode buttonCode, JoystickInfo joyInfo)
        {
            bool isFiltered;
            return GetBindButtonName(buttonCode, joyInfo.ControlNum, out isFiltered);
        }
        /// <summary>
        /// Get button name filtered
        /// </summary>
        /// <param name="bind"></param>
        /// <param name="controllerName"></param>
        /// <param name="isFiltered"></param>
        /// <returns></returns>
        public string GetBindButtonName(ButtonCode buttonCode, int controlNum, out bool isFiltered)
        {
            // make head name
            string joyName = InputBindings.IsAxisButton(buttonCode)? InputBindings.JoyName : InputBindings.JoystickName;

            if(this.prefixDpad.Length > 0 && (buttonCode == ButtonCode.dpad_left || buttonCode == ButtonCode.dpad_right))
            {
                isFiltered = true;
                return string.Format("{0} {1}{2} {3}", this.prefixDpad[0], joyName, controlNum, InputBindings.GetButtonCodeName(buttonCode));
            }
            else if(this.prefixDpad.Length > 1 && (buttonCode == ButtonCode.dpad_up || buttonCode == ButtonCode.dpad_down))
            {
                isFiltered = true;
                return string.Format("{0} {1}{2} {3}", this.prefixDpad[1], joyName, controlNum, InputBindings.GetButtonCodeName(buttonCode));
            }
            else if(InputBindings.IsAxisButton(buttonCode))
            {
                isFiltered = true;
                return string.Format("{0}{1}{2}", joyName, controlNum, InputBindings.GetButtonCodeName(buttonCode));
            }
            else
            {
                isFiltered = false;
                return string.Format("{0}{1} {2}", joyName, controlNum, InputBindings.GetButtonCodeName(buttonCode));
            }
        }
    }
}
