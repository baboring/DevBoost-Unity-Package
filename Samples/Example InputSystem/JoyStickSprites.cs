
using UnityEngine;
using NaughtyAttributes;

namespace Example.InputSystem
{
    /// <summary>
    /// Holds all buttoncode/sprite pairs for input type
    /// DO NOT FORGET TO ADD OBJECT TO INPUTBINDINGS LIST IN STARTUP
    /// </summary>
    [CreateAssetMenu(fileName = "Joystick Button Sprites", menuName = "Input/Button Sprites", order = 0)]
    public class JoyStickSprites : ScriptableObject
    {
        public InputType inputType;
        public Sprite controllerSymbol;

        [ReorderableList]
        public GameButtonSprites[] buttonSprites;
    }
}