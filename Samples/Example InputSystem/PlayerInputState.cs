

using UnityEngine;
using DevBoost.InputSystem;

namespace Example.InputSystem
{

    /// <summary>
    /// Used to handle input for a player character
    /// </summary>
    public abstract class PlayerInputState : IPlayerInputState
    {
        // controller number
        public int ControllerId { get; set; }
        // Get whether its type of controller
        public TypeOfInputControl Type { get; private set; }


        public CustomInputAxis CustomInput = new CustomInputAxis(); // general axis input
        public CustomInputAxis HoldInput = new CustomInputAxis();   // Especially use on motion charging.
        public CustomInputButton CustomInputButton = new CustomInputButton();

        // input name manager
        protected InputKeyNameMapper m_KeyMapper = new InputKeyNameMapper();


        //// instanciator
        //public static PlayerInputState Create(TypeOfInputControl type)
        //{
        //    switch (type)
        //    {
        //        case TypeOfInputControl.KeyboardAndMouse:
        //            return new PlayerInputStateKeyboard(type);
        //        case TypeOfInputControl.Joystick:
        //            return new PlayerInputStateJoystick(type);
        //        case TypeOfInputControl.RemoteControl:
        //            return new PlayerInputStateRemote(type);
        //        case TypeOfInputControl.VirtualControl:
        //            return new PlayerInputStateVirtual(type);
        //    }

        //    return null;
        //}
        
        // default constructor
        public PlayerInputState(TypeOfInputControl type)
        {
            Debug.LogFormat("[ IInputController ] {0}, {1}", type, this);
            Type = type;
        }

        // Merge input keys
        public void Merge(InputKeyNameMapper manager)
        {
            m_KeyMapper.Merge(manager);
        }

        // Perform mock Input trigger
        public bool CreateButtonTask(ButtonName buttonName, float takingTime = 0, float startTime = 0)
        {
            return CustomInputButton.CreateButtonTask(buttonName, takingTime, startTime);
        }

        /// <summary>
        /// get whether button is working or not
        /// </summary>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        public bool IsWorking(ButtonName buttonName)
        {
            return CustomInputButton.HasTask(buttonName);
        }

        // perform mock Input axis for control
        public bool SetAxisValue(InputNameCode buttonName, float value)
        {
            CustomInput.SetAxisValue(GetInputString(buttonName), value);
            return true;
        }

        // perform mock Input axis for hold
        public bool SetHoldAxisValue(InputNameCode buttonName, float value)
        {
            HoldInput.SetAxisValue(GetInputString(buttonName), value);
            return true;
        }

        // perform mock Input axis for control
        public float GetAxisValue(InputNameCode buttonName)
        {
            return CustomInput.GetAxis(GetInputString(buttonName));
        }

        // perform mock Input axis for control
        public float GetHoldAxisValue(InputNameCode buttonName)
        {
            return HoldInput.GetAxis(GetInputString(buttonName));
        }

        // Get its Key String
        protected string GetInputString(InputNameCode code)
        {
            return m_KeyMapper.GetString(code);
        }
        protected ButtonName GetInputButton(InputNameCode code)
        {
            return m_KeyMapper.GetButton(code);
        }

        protected NameToButton GetNameToButton(InputNameCode code)
        {
            return m_KeyMapper.GetNameToButton(code);
        }

        // intitalization
        public virtual void Initial(InputKeyNameMapper keyMapper = null)
        {
            // merge key mappings
            if(null != keyMapper)
                m_KeyMapper.Merge(keyMapper);

            // For dummy or remote player (movement)
            HoldInput.Register(GetInputString(InputNameCode.HorizontalMovement));
            HoldInput.Register(GetInputString(InputNameCode.VerticalMovement));

            HoldInput.Register(GetInputString(InputNameCode.Right_HorizontalAxis));
            HoldInput.Register(GetInputString(InputNameCode.Right_VerticalAxis));

            // For dummy or remote player (movement)
            CustomInputButton.Register(GetInputString(InputNameCode.JumpButton), ButtonName.Jump);
            CustomInputButton.Register(GetInputString(InputNameCode.RunButton), ButtonName.Run);
            CustomInputButton.Register(GetInputString(InputNameCode.DiveButton), ButtonName.Dive);
            CustomInputButton.Register(GetInputString(InputNameCode.DiveButtonExtra), ButtonName.DiveExtra);

            // for dummy or remote player (smash)
            CustomInputButton.Register(GetInputString(InputNameCode.SmashButton), ButtonName.Smash);
            //CustomInputButton.Register(GetInputString(InputNameCode.ThrowButton), ButtonName.Throw);
            CustomInputButton.Register(GetInputString(InputNameCode.CatchButton), ButtonName.Catch);
            CustomInputButton.Register(GetInputString(InputNameCode.BuntButton), ButtonName.Bunt);
            CustomInputButton.Register(GetInputString(InputNameCode.SpecialButton), ButtonName.Special);
            CustomInputButton.Register(GetInputString(InputNameCode.UltimateButton), ButtonName.Ultimate);
            CustomInputButton.Register(GetInputString(InputNameCode.BoostCardButton), ButtonName.Boost);
            CustomInputButton.Register(GetInputString(InputNameCode.SwitchPlayerButton), ButtonName.SwitchPlayer);

        }

        #region METHOD

        // Key Mapper
        public InputKeyNameMapper KeyMapper { get { return m_KeyMapper; } }

        /// <summary>
        /// Get horizontal input for general
        /// </summary>
        public virtual float HorizontalInput {
            get { return CustomInput.GetAxis(GetInputString(InputNameCode.HorizontalMovement)); }
        }

        public virtual float HorizontalRawInput {
            get { return CustomInput.GetAxisRaw(GetInputString(InputNameCode.HorizontalMovement)); }
        }

        /// <summary>
        /// Get vertical input for general
        /// </summary>
        public virtual float VerticalInput {
            get { return CustomInput.GetAxis(GetInputString(InputNameCode.VerticalMovement)); }
        }


        public virtual float VerticalRawInput {
            get { return CustomInput.GetAxisRaw(GetInputString(InputNameCode.VerticalMovement)); }
        }

        /// <summary>
        /// Get horizontal input for HOLD Input
        /// </summary>
        public virtual float HorizontalAimInput {
            get { return HoldInput.GetAxis(GetInputString(InputNameCode.HorizontalMovement)); }
        }
        /// <summary>
        /// Get vertical input for HOLD Input
        /// </summary>
        public virtual float VerticalAimInput {
            get { return HoldInput.GetAxis(GetInputString(InputNameCode.VerticalMovement)); }
        }


        public abstract bool IsDivePress { get; }
        public abstract bool IsRunPress { get; }
        public abstract bool IsJumpPress { get; }
        public abstract bool IsJumpHold { get; }

        /// <summary>
        /// Get wether player pressed down bunt button
        /// </summary>
        public virtual bool IsBuntPress {
            get { return CustomInputButton.GetButtonDown(GetInputString(InputNameCode.BuntButton)); }
        }

        /// <summary>
        /// Get wether player pressed down bunt button
        /// </summary>
        public virtual bool IsBuntHold {
            get { return CustomInputButton.GetButton(GetInputString(InputNameCode.BuntButton)); }
        }

    
        // --- bunt
        public virtual bool IsSmashPress {
            get { return CustomInputButton.GetButtonDown(GetInputString(InputNameCode.SmashButton)); }
        }        /// <summary>
        /// Get wether player pressed down bunt button
        /// </summary>
        public virtual bool IsSmashHold {
            get { return CustomInputButton.GetButton(GetInputString(InputNameCode.SmashButton)); }
        }

        public virtual bool IsBlockPress {
            get { return CustomInputButton.GetButton(GetInputString(InputNameCode.SpecialButton)); }
        }

        public abstract bool IsShieldHold { get; }
        public abstract bool IsUltimatePress { get; }
        public abstract bool IsBoostPress { get; }
        public abstract bool IsBoostHold { get; }
        public abstract bool IsSwitchPlayerPress { get; }
        public abstract bool IsCatchPress { get; }
        public abstract bool IsCatchingHold { get; }

        /// <summary>
        /// Get horizontal & vertical input from right stick
        /// </summary>
        public virtual float RHorizontalInput
        {
            get { return CustomInput.GetAxis(GetInputString(InputNameCode.Right_HorizontalAxis)); }
        }
        public virtual float RVerticalInput
        {
            get { return CustomInput.GetAxis(GetInputString(InputNameCode.Right_VerticalAxis)); }
        }

        public virtual float RHorizontalHoldInput
        {
            get { return HoldInput.GetAxis(GetInputString(InputNameCode.Right_HorizontalAxis)); }
        }
        public virtual float RVerticalHoldInput
        {
            get { return HoldInput.GetAxis(GetInputString(InputNameCode.Right_VerticalAxis)); }
        }

        #endregion
    }
}
