
namespace Example.InputSystem
{
    /// <summary>
    /// Used to handle input for a player character
    /// </summary>
    public interface IPlayerInputState
    {

        bool IsDivePress { get; }
        bool IsRunPress { get; }
        bool IsJumpPress { get; }
        bool IsJumpHold { get; }

        bool IsSmashPress { get; }
        bool IsSmashHold { get; }

        bool IsBuntPress { get; }
        bool IsBuntHold { get; }


        bool IsShieldHold { get; }
        bool IsUltimatePress { get; }
        bool IsBoostPress { get; }
        bool IsBoostHold { get; }
        bool IsSwitchPlayerPress { get; }
        bool IsCatchPress { get; }
        bool IsCatchingHold { get; }


    }

    /// <summary>
    /// Mapped key with Where name defined outside
    /// </summary>
    public enum InputNameCode
    {
        HorizontalMovement,     // movment lef & right
        VerticalMovement,       // movment up & down
        JumpButton,             // Jumpping
        DiveButton,             // Dash primary button
        RunButton,        

        ThrowButton,
        SmashButton,
        BuntButton,
        CatchButton,
        SpecialButton,          // shield
        UltimateButton,         // ultimate

        Right_HorizontalAxis,   // aiming key axis (Game only - old)
        Right_VerticalAxis,     // aiming key axis (Game only - old)

        Horizontal_Positive,    // Horizontal positive (for keyboard)
        Horizontal_Negative,    // Horizontal negative (for keyboard)
        Vertical_Positive,      // Vertical positive (for keyboard)
        Vertical_Negative,      // Vertical negative (for keyboard)

        BoostCardButton,        // Butotn to be used for Arminng
        SwitchPlayerButton,     // Butotn to be used for switching player

        RHorizontal_Positive,    // Right Horizontal positive (for keyboard)
        RHorizontal_Negative,    // Right Horizontal negative (for keyboard)
        RVertical_Positive,      // Right Vertical positive (for keyboard)
        RVertical_Negative,      // Right Vertical negative (for keyboard)

        DiveButtonExtra,         // Dash secondary button
    }


    public struct NameToInputNameCode
    {
        public InputNameCode Code;
        public string Name;

        public NameToInputNameCode(InputNameCode code, string name)
        {
            this.Code = code;
            this.Name = name;
        }
    }

}
