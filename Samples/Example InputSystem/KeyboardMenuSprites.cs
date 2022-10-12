using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Example.InputSystem
{
    /// <summary>
    /// Holds all buttoncode/sprite pairs for input type
    /// DO NOT FORGET TO ADD OBJECT TO INPUTBINDINGS LIST IN STARTUP
    /// </summary>
    [CreateAssetMenu(fileName = "Keyboard Button Sprites", menuName = "Input/Keyboard Menu Sprites", order = 0)]
    public class KeyboardMenuSprites : ScriptableObject
    {
        public InputType inputType;
        public Sprite controllerSymbol;
        public GameKeySprites[] keySprites;
    }
}