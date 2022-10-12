
using System.Collections.Generic;
using UnityEngine;

namespace Example.InputSystem
{

    // input Name Manager
    public class InputKeyNameMapper
    {
        // Key mapping container
        protected Dictionary<InputNameCode, NameToButton> m_mapKey = new Dictionary<InputNameCode, NameToButton>();

        // Clear All
        public void Clear()
        {
            m_mapKey.Clear();
        }

        // register delegator
        public void Register(InputNameCode code, string varString, ButtonName button = ButtonName.None)
        {
            if (!m_mapKey.ContainsKey(code))
            {
                // checking if the button is button type of axis type
                m_mapKey.Add(code, new NameToButton(button, varString));
            }
        }

        // Get its Key String
        public string GetString(InputNameCode code)
        {
            if (!m_mapKey.ContainsKey(code))
            {
                Debug.LogWarning("Not found code :" + code);
                return string.Empty;
            }

            return m_mapKey[code].Name;
        }

        // Get its Key Name
        public ButtonName GetButton(InputNameCode code)
        {
            if (!m_mapKey.ContainsKey(code))
            {
                Debug.LogWarning("Not found code :" + code);
                return ButtonName.None;
            }

            return m_mapKey[code].Button;
        }

        // Get its Key info
        public NameToButton GetNameToButton(InputNameCode code)
        {
            if(!m_mapKey.ContainsKey(code))
            {
                Debug.LogWarning("Not found code :" + code);
                return default(NameToButton);
            }

            return m_mapKey[code];
        }

        // Merge Keys
        public InputKeyNameMapper Merge(InputKeyNameMapper from)
        {
            foreach (var val in from.m_mapKey)
                if (!m_mapKey.ContainsKey(val.Key))
                    m_mapKey.Add(val.Key, val.Value);

            return this;
        }
    }
}
