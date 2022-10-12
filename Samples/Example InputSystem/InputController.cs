
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Example.InputSystem
{
    /// <summary>
    /// Used to handle input for a player character
    /// </summary>
    public abstract class InputController : MonoBehaviour
    {

        private bool m_isInputEnabled = true;

        // Input controller for adapter
        protected PlayerInputState m_Device;


        /// <summary>
        /// Get whether input is enabled
        /// </summary>
        public bool IsInputEnabled {
            get { return m_isInputEnabled && m_Device != null; }
            set { m_isInputEnabled = value; }
        }

        // Input controller for adapter
        public PlayerInputState Device {
            get { return m_Device; }
        }

    }
}
