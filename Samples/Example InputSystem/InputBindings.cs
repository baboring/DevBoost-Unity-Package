using DevBoost.InputSystem;
using DevBoost.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Example.InputSystem
{
    /// <summary>
    /// Button Bind struct
    /// </summary>
    [System.Serializable]
    public struct GameButtonSprites
    {
        public ButtonCode buttonName;
        public Sprite buttonSprite;
        public Color buttonColor;
    }

    /// <summary>
    /// Button Bind struct
    /// </summary>
    [System.Serializable]
    public struct GameKeySprites
    {
        public KeyCode keyName;
        public Sprite buttonSprite;
        public Color buttonColor;
    }

    /// <summary>
    /// Button Bind struct
    /// </summary>
    [System.Serializable]
    public struct GameButtonBind
    {
        public InputNameCode nameCode;
        public ButtonCode buttonName;
    }

    [System.Serializable]
    public struct MenuKeyBind
    {
        public MenuNameCode nameCode;
        public KeyCode keyName;
    }

    [System.Serializable]
    public struct MenuButtonBind
    {
        public MenuNameCode nameCode;
        public ButtonCode buttonName;
    }

    /// <summary>
    /// KeyCode Bind struct
    /// </summary>
    [System.Serializable]
    public struct GameKeyCodeBind
    {
        public InputNameCode nameCode;
        public KeyCode keyCode;
    }

    /// <summary>
    /// Mouse Button bind
    /// </summary>
    [System.Serializable]
    public struct MouseButtonBind
    {
        public InputNameCode nameCode;
        public int mouseButton;
    }

    /// <summary>
    /// Struct used to define Keyboard bind
    /// </summary>
    [System.Serializable]
    public class KeyboardBind
    {
        public GameKeyCodeBind[] gameKeyBinds;
    }


    /// <summary>
    /// Struct used to define Joystick bind
    /// </summary>
    [System.Serializable]
    public class JoystickButtonBind
    {
        public GameButtonBind[] gameButtonBinds;

    }

    /// <summary>
    /// Menu name code.
    /// </summary>
    public enum MenuNameCode
    {
        Start,
        Submit,
        Cancel,
        LeftBumper,
        RightBumper,
        Option_1,      // This is the one at the top of the main action buttons on a controller (e.g Y on XBOX).
        Skip,
        Back,
        Axis,
        Option_2,
        LeftTrigger,
        RightTrigger,
    }

    /// <summary>
    /// Input trigger type.
    /// </summary>
    public enum InputTriggerType
    {
        None = -1,
        Any,
        Submit,         // (e.g A on XBOX) 
        Cancel,         // (e.g B on XBOX)
        Option_1,       // (e.g Y on XBOX)
        Option_2,       // (e.g X on XBOX)
        LeftBumper,
        RightBumper,
        Start,
        Skip,
        Back,
        // ---- Left axis
        Move_Left,
        Move_Up,
        Move_Right,
        Move_Down,
        LeftTrigger,
        RightTrigger,
        // ---- D-Pad
        dpad_left,
        dpad_right,
        dpad_up,
        dpad_down,
        // ---- Right axis
        Look_Left,
        Look_Up,
        Look_Right,
        Look_Down,
    }

    /// <summary>
    /// Button code.
    /// </summary>
    public enum ButtonCode
    {
        NONE,
        button0,
        button1,
        button2,
        button3,
        button4,
        button5,
        button6,
        button7,
        button8,
        button9,
        button10,
        button11,
        button12,
        button13,
        button14,
        button15,
        button16,
        button17,
        button18,
        button19,
        dpad_left,
        dpad_right,
        dpad_up,
        dpad_down,
        Axis1,
        Axis2,
        Axis3,
        Axis4,
        Axis5,
        Axis6,
        Axis7,
        Axis8,
        Axis9,
        Axis10,
        Axis11,
        Axis12,
        Axis13,
        Axis14,
        Axis15,
        Axis16,
        Axis17,
        Axis18,
        Axis19,
        Axis20,
        Axis21,
        Axis22,
        Axis23,
        Axis24,
        Axis25,
        Axis26,
        Axis27,
        Axis28,
        Axis29,
    }

    /// <summary>
    /// Enum used to define Keyboard input mode
    /// </summary>
    public enum KeyboardMode
    {
        Keys = 0,
        Mouse,
    }

    /// <summary>
    /// Input controller device type.
    /// </summary>
    public enum InputType
    {

        KEYBOARD,
        XBOX,
        PS,
        LOGITECH,
        DEFAULT,
        USB_JOYSTICK,
        XBOX_MAC,
        XBOX_360,
    }


    /// <summary>
    /// Get Input Bindings
    /// </summary>
    public class InputBindings : SingletonMono<InputBindings>
    {
        //[SerializeField] private string m_KeyIconsPath = "KeyboardIcons";
        [SerializeField] private Text m_KeyTextPrefab = null;
        [SerializeField] private Sprite m_KeyBackgroundSprite = null;
        [SerializeField] private DefaultKeyboardBind m_DefaultKeyboardBind = null;
        [SerializeField] private DefaultKeyboardBind m_DefaultMouseKeyboardBind = null;
        [SerializeField] private JoystickBind[] m_JoystickBinds = null;
        [SerializeField] private JoyStickSprites[] m_JoystickSprites = null;
        [SerializeField] private KeyboardMenuSprites m_KeyboardSprites = null;
        [SerializeField] private GameObject m_BlockMouseCanvas = null;

        public const string JoystickName = "Joystick ";
        public const string JoyName = "Joy";
        private bool m_MouseBlock = false;
        public bool MouseBlock
        { get { return m_MouseBlock; }
            set {
                m_MouseBlock = value;
                //Toggle cover image
                m_BlockMouseCanvas.SetActive(value);
                // make this on top
                if(value)
                    this.transform.SetAsLastSibling();
            }
        }

        private void BlockMouse()
        {
            //MouseBlock = true;
        }

        /// <summary>
        /// Get Key Display prefab
        /// </summary>
        public Text KeyDisplayPrefab {
            get { return m_KeyTextPrefab; }
        }

        /// <summary>
        /// Get Key background sprite
        /// </summary>
        public Sprite KeyBackgroundSprite {
            get { return m_KeyBackgroundSprite; }
        }


        //-------------------------
        #region GET SAVED BINDS
        /// <summary>
        /// Get Current keyboard bind
        /// </summary>
        public static KeyboardBind GetCurrentKeyboardBind()
        {
            return null;
        }


        #endregion

        #region Binding Functions
        /// <summary>
        /// Get Key with provided name code
        /// </summary>
        public static string GetKeyByNameCode(InputNameCode nameCode)
        {
            KeyboardBind bind = GetCurrentKeyboardBind();
            for(int kIndex = 0; kIndex < bind.gameKeyBinds.Length; kIndex++)
            {
                if(bind.gameKeyBinds[kIndex].nameCode == nameCode)
                {
                    return GetKeyName(bind.gameKeyBinds[kIndex].keyCode);
                }
            }
            return string.Empty;
        }

        public static KeyCode GetKeyboardKeycode(InputNameCode nameCode)
        {
            KeyboardBind bind = GetCurrentKeyboardBind();
            for(int kIndex = 0; kIndex < bind.gameKeyBinds.Length; kIndex++)
            {
                if(bind.gameKeyBinds[kIndex].nameCode == nameCode)
                {
                    return bind.gameKeyBinds[kIndex].keyCode;
                }
            }
            return KeyCode.None;
        }

        public static string GetKeyIconByCode(KeyCode code)
        {
            return null;
        }


        /// <summary>
        /// Get Game button sprite
        /// </summary>
        public Sprite GetGameButtonSprite(JoystickButtonBind savedButtonBind, InputNameCode nameCode, InputType inputType)
        {
            GameButtonBind[] buttonBind = savedButtonBind.gameButtonBinds;
            for(int bIndex = 0; bIndex < buttonBind.Length; bIndex++)
            {
                if(buttonBind[bIndex].nameCode == nameCode)
                {
                    return Instance.GetButtonSprite(inputType, buttonBind[bIndex].buttonName);
                }
            }
            return null;
        }

        public Color GetGameButtonColor(JoystickButtonBind savedButtonBind, InputNameCode nameCode, InputType inputType)
        {
            GameButtonBind[] buttonBind = savedButtonBind.gameButtonBinds;
            for(int bIndex = 0; bIndex < buttonBind.Length; bIndex++)
            {
                if(buttonBind[bIndex].nameCode == nameCode)
                {
                    return Instance.GetButtonColor(inputType, buttonBind[bIndex].buttonName);
                }
            }
            return default(Color);
        }

        #endregion


        /// <summary>
        /// Get device sprite by inputtype
        /// </summary>
        /// <param name="inputType"></param>
        /// <returns></returns>
        public Sprite GetDeviceSprite(InputType inputType)
        {

            if(inputType == InputType.KEYBOARD)
            {
                return m_KeyboardSprites.controllerSymbol;
            }
            else
            {
                foreach(JoyStickSprites joystickSprite in Instance.m_JoystickSprites)
                {
                    if(joystickSprite.inputType == inputType)
                        return joystickSprite.controllerSymbol;
                }
            }

            return null;
        }
        /// <summary>
        /// Get button sprite by inputtype
        /// </summary>
        /// <param name="inputType"></param>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        public Sprite GetButtonSprite(InputType inputType, ButtonCode buttonName)
        {
            // for joystick
            foreach(JoyStickSprites joystickSprite in m_JoystickSprites)
            {
                if(joystickSprite.inputType == inputType)
                    return GetJoystickButtonSprite(joystickSprite, buttonName);
            }
            return null;
        }

        public Color GetButtonColor(InputType inputType, ButtonCode buttonName)
        {
            foreach(JoyStickSprites joystickSprite in m_JoystickSprites)
            {
                if(joystickSprite.inputType == inputType)
                    return GetJoystickButtonColor(joystickSprite, buttonName);
            }
            return default(Color);
        }

        /// <summary>
        /// return button sprite
        /// </summary>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        private static Sprite GetJoystickButtonSprite(JoyStickSprites joystickSprites, ButtonCode buttonName)
        {
            foreach(GameButtonSprites buttonSprite in joystickSprites.buttonSprites)
            {
                if(buttonSprite.buttonName == buttonName)
                    return buttonSprite.buttonSprite;
            }

            return null;
        }

        private static Color GetJoystickButtonColor(JoyStickSprites joystickSprites, ButtonCode buttonName)
        {
            foreach(GameButtonSprites buttonSprite in joystickSprites.buttonSprites)
            {
                if(buttonSprite.buttonName == buttonName)
                    return buttonSprite.buttonColor;
            }

            return default(Color);
        }


        #region KeyCode in general
        /// <summary>
        /// Get keycode registered under name code
        /// </summary>
        public KeyCode GetMenuKeyCode(MenuNameCode nameCode)
        {
            for(int bIndex = 0; bIndex < m_DefaultKeyboardBind.menuButtonBinds.Length; bIndex++)
            {
                if(m_DefaultKeyboardBind.menuButtonBinds[bIndex].nameCode == nameCode)
                {
                    return m_DefaultKeyboardBind.menuButtonBinds[bIndex].keyName;
                }
            }
            return KeyCode.None;
        }


        /// <summary>
        /// Get key sprite by inputtype
        /// </summary>
        /// <param name="inputType"></param>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        public Sprite GetKeyCodeSprite(KeyCode keyName)
        {
            return GetKeyCodeSprite(m_KeyboardSprites, keyName);
        }
        /// <summary>
        /// return keyboard sprite
        /// </summary>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        private Sprite GetKeyCodeSprite(KeyboardMenuSprites keyboardSprites, KeyCode keyName)
        {
            foreach(GameKeySprites keySprite in keyboardSprites.keySprites)
            {
                if(keySprite.keyName == keyName)
                    return keySprite.buttonSprite;
            }

            return null;
        }


        public Color GetKeyCodeColor(KeyCode keyName)
        {
            return GetKeyCodeColor(m_KeyboardSprites, keyName);
        }

        /// <summary>
        /// return keyboard sprite
        /// </summary>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        private static Color GetKeyCodeColor(KeyboardMenuSprites keyboardSprites, KeyCode keyName)
        {
            foreach(GameKeySprites keySprite in keyboardSprites.keySprites)
            {
                if(keySprite.keyName == keyName)
                    return keySprite.buttonColor;
            }

            return default(Color);
        }




        #endregion

        #region Keyboard & Joysticks



        /// <summary>
        /// Get Default binds
        /// </summary>
        /// <param name="inputType"></param>
        /// <returns></returns>
        public JoystickBind GetDefaultJoystickBind(InputType inputType)
        {
            foreach(JoystickBind bind in m_JoystickBinds)
            {
                if(bind.InputType == inputType)
                    return bind;
            }
            return null;
        }

        /// <summary>
        /// Get Default binds
        /// </summary>
        /// <param name="inputType"></param>
        /// <returns></returns>
        public DefaultKeyboardBind GetDefaultKeyboardBind(InputType inputType = InputType.KEYBOARD)
        {
            return m_DefaultKeyboardBind;
        }

        // checking it is in bound
        public static bool IsAxisButton(ButtonCode code)
        {
            return code >= ButtonCode.Axis1 && code <= ButtonCode.Axis29;
        }

        /// <summary>
        /// Get Button name based on buttoncode
        /// </summary>
        /// <param name="buttonCode"></param>
        /// <returns></returns>
        public static string GetButtonCodeName(ButtonCode buttonCode)
        {
            switch(buttonCode)
            {
                case ButtonCode.button0:
                    return "button 0";
                case ButtonCode.button1:
                    return "button 1";
                case ButtonCode.button2:
                    return "button 2";
                case ButtonCode.button3:
                    return "button 3";
                case ButtonCode.button4:
                    return "button 4";
                case ButtonCode.button5:
                    return "button 5";
                case ButtonCode.button6:
                    return "button 6";
                case ButtonCode.button7:
                    return "button 7";
                case ButtonCode.button8:
                    return "button 8";
                case ButtonCode.button9:
                    return "button 9";
                case ButtonCode.button10:
                    return "button 10";
                case ButtonCode.button11:
                    return "button 11";
                case ButtonCode.button12:
                    return "button 12";
                case ButtonCode.button13:
                    return "button 13";
                case ButtonCode.button14:
                    return "button 14";
                case ButtonCode.button15:
                    return "button 15";
                case ButtonCode.button16:
                    return "button 16";
                case ButtonCode.button17:
                    return "button 17";
                case ButtonCode.button18:
                    return "button 18";
                case ButtonCode.button19:
                    return "button 19";
                // d-pad
                case ButtonCode.dpad_left:
                case ButtonCode.dpad_down:
                    return "d-pad negative";
                case ButtonCode.dpad_right:
                case ButtonCode.dpad_up:
                    return "d-pad positive";
                case ButtonCode.Axis1:
                case ButtonCode.Axis2:
                case ButtonCode.Axis3:
                case ButtonCode.Axis4:
                case ButtonCode.Axis5:
                case ButtonCode.Axis6:
                case ButtonCode.Axis7:
                case ButtonCode.Axis8:
                case ButtonCode.Axis9:
                case ButtonCode.Axis10:
                case ButtonCode.Axis11:
                case ButtonCode.Axis12:
                case ButtonCode.Axis13:
                case ButtonCode.Axis14:
                case ButtonCode.Axis15:
                case ButtonCode.Axis16:
                case ButtonCode.Axis17:
                case ButtonCode.Axis18:
                case ButtonCode.Axis19:
                case ButtonCode.Axis20:
                case ButtonCode.Axis21:
                case ButtonCode.Axis22:
                case ButtonCode.Axis23:
                case ButtonCode.Axis24:
                case ButtonCode.Axis25:
                case ButtonCode.Axis26:
                case ButtonCode.Axis27:
                case ButtonCode.Axis28:
                case ButtonCode.Axis29:
                    return buttonCode.ToString();
                default:
                    return "";
            }
        }

        /// <summary>
        /// Get Key Name based on keycode
        /// </summary> 
        public static string GetKeyName(KeyCode keycode)
        {
            switch(keycode)
            {
                case KeyCode.Colon:
                    return ":";

                case KeyCode.Semicolon:
                    return ";";

                case KeyCode.Mouse0:
                    return "Mouse 0";

                case KeyCode.Mouse1:
                    return "Mouse 1";

                case KeyCode.Mouse2:
                    return "Mouse 2";

                case KeyCode.Mouse3:
                    return "Mouse 3";

                case KeyCode.Mouse4:
                    return "Mouse 4";

                case KeyCode.Mouse5:
                    return "Mouse 5";

                case KeyCode.Mouse6:
                    return "Mouse 6";

                case KeyCode.LeftShift:
                    return "left shift";

                case KeyCode.RightShift:
                    return "right shift";

                case KeyCode.LeftControl:
                    return "left ctrl";

                case KeyCode.RightControl:
                    return "right ctrl";

                case KeyCode.LeftAlt:
                    return "left alt";

                case KeyCode.RightAlt:
                    return "right alt";

                case KeyCode.UpArrow:
                    return "up";

                case KeyCode.DownArrow:
                    return "down";

                case KeyCode.LeftArrow:
                    return "left";

                case KeyCode.RightArrow:
                    return "right";
            }
            return keycode.ToString();
        }

        public static void SetKeyByNameCode(InputNameCode nameCode, KeyCode key)
        {
            for(int kIndex = 0; kIndex < GetCurrentKeyboardBind().gameKeyBinds.Length; kIndex++)
            {
                if(GetCurrentKeyboardBind().gameKeyBinds[kIndex].nameCode == nameCode)
                {
                    GetCurrentKeyboardBind().gameKeyBinds[kIndex].keyCode = key;
                }
            }
        }
        public static JoystickBind GetJoystickBind(JoystickInfo info)
        {
            return GetJoystickBind(info.m_DeviceName);
        }
        /// <summary>
        /// Get Joystick Bind that belongs to this device name
        /// </summary>
        /// <returns></returns>
        public static JoystickBind GetJoystickBind(string joystickDeviceName)
        {
            if(string.IsNullOrEmpty(joystickDeviceName))
                return null;

            // find equal name in first
            for(int bIndex = 0; bIndex < Instance.m_JoystickBinds.Length; bIndex++)
            {
                if(Instance.m_JoystickBinds[bIndex].FindIndexName(joystickDeviceName) > -1)
                    return Instance.m_JoystickBinds[bIndex];
            }

            // find containing name in second
            for(int bIndex = 0; bIndex < Instance.m_JoystickBinds.Length; bIndex++)
            {
                if(Instance.m_JoystickBinds[bIndex].IsJoystickName(joystickDeviceName))
                {
                    return Instance.m_JoystickBinds[bIndex];
                }
            }
            // if no bind was found, return DEFAULT
            if(Instance.m_JoystickBinds.Length != 0)
            {
                return Instance.m_JoystickBinds[0];
            }
            Debug.LogError("No bind was found for " + joystickDeviceName + ". Returning NULL");
            return null;
        }

        #endregion

        #region UNITY_FUNCTIONS

        private void Update()
        {
            //if (MouseBlock)
            //    if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            //    {
            //        MouseBlock = false;
            //        //allow mouse input
            //    }
        }

        /// <summary>
        /// Verify data and update it with param
        /// </summary>
        /// <param name="keyBind"></param>
        /// <param name="isUpdate"></param>
        /// <returns></returns>
        public static bool VerifyKeyboard(KeyboardBind org, GameKeyCodeBind[] keyBind, bool isUpdate = false)
        {
            bool isValid = null != org && null != org.gameKeyBinds && org.gameKeyBinds.Length == keyBind.Length;

            if(!isValid && isUpdate && null != org)
            {
                org.gameKeyBinds = keyBind;
            }

            return isValid;
        }


        #endregion


    }
}
