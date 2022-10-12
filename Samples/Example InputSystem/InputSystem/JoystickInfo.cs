/// ==============================================================
/// © VRGEN ALL RIGHTS RESERVED
/// ==============================================================
using System.Collections;
using System.Collections.Generic;

namespace Example.InputSystem
{
    public struct JoystickInfo
    {
        /// <summary>
        /// Index in the unity joystick list
        /// </summary>
        public int m_Index;
        public string m_DeviceName;

        /// <summary>
        /// Unique id of the current connected josticks
        /// </summary>
        public int m_Id;
        public bool m_isPlugedIn;

        /// <summary>
        /// device control id for players
        /// </summary>
        public int ControlNum
        {
            get { return m_DeviceName == null ? 0 : m_Index + 1; }
        }

        // device name
        public bool IsEmpty { get { return m_DeviceName == null; } }

        public string ControllerName { get { return m_DeviceName == null ? "" : InputBindings.JoystickName + ControlNum + " "; } }

        public override bool Equals(object obj)
        {
            if (!(obj is JoystickInfo))
            {
                return false;
            }

            var info = (JoystickInfo)obj;
            return GetHashCode() == info.GetHashCode();
        }

        public override int GetHashCode()
        {
            var hashCode = -1339606863;
            hashCode = hashCode * -1521134295 + m_Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(m_DeviceName);
            return hashCode;
        }
    }
}
