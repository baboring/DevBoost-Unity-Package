using UnityEngine;

namespace Example.InputSystem
{
 
    // Customized Input Button
    public class CustomInputButton : VirtualInputButton
    {
        private bool m_isEnable = true;

        // Enable / Disable input values
        public bool IsEnable
        {
            get { return m_isEnable; }
            set { m_isEnable = value; }
        }

        /// <summary>
        /// Only work for AI & virtual input
        /// </summary>
        public bool IsVirtual { get; set; }

        // Get whether this button is pressed or not
        public override bool GetButton(string button)
        {
            if (!IsEnable || button == string.Empty) return false;

            if (IsVirtual) return base.GetButton(button);

            return Input.GetButton(button);
        }

        // Get whether this button is down or not
        public override bool GetButtonDown(string button)
        {
            if (!IsEnable || button == string.Empty) return false;

            if (IsVirtual) return base.GetButtonDown(button);

            return Input.GetButtonDown(button);
        }

        // Get whether this button is up or not
        public override bool GetButtonUp(string button)
        {
            if (!IsEnable || button == string.Empty) return false;

            if (IsVirtual) return base.GetButtonUp(button);

            return Input.GetButtonUp(button);
        }

        // Get whether this button is presed or not
        public override bool GetButton(ButtonName button)
        {
            if(!IsEnable) return false;
            if (IsVirtual) return base.GetButton(button);

            ButtonStatus bnStatus;
            if (m_ButtonTable.TryGetValue(button, out bnStatus))
                return Input.GetButton(bnStatus.Key.Name);

            return false;
        }

        // Get whether this button is up or not
        public override bool GetButtonUp(ButtonName button)
        {
            if(!IsEnable) return false;
            if(IsVirtual) return base.GetButtonUp(button);

            ButtonStatus bnStatus;
            if (m_ButtonTable.TryGetValue(button, out bnStatus))
                return Input.GetButtonUp(bnStatus.Key.Name);

            return false;
        }

        // Get whether this button is down or not
        public override bool GetButtonDown(ButtonName button)
        {
            if(!IsEnable) return false;
            if(IsVirtual) return base.GetButtonDown(button);

            ButtonStatus bnStatus;
            if(m_ButtonTable.TryGetValue(button, out bnStatus))
                return Input.GetButtonDown(bnStatus.Key.Name);
            return false;
        }


        // Get whether this button
        public bool IsButtonDownTrigger(ButtonName button)
        {
            if(!IsEnable) return false;
            if(IsVirtual) return base.GetButtonDown(button);

            ButtonStatus bnStatus;
            if(m_ButtonTable.TryGetValue(button, out bnStatus))
            {
                if(bnStatus.Key.TriggerType == ButtonTrigger.Button)
                    return Input.GetButtonDown(bnStatus.Key.Name);
                else if(bnStatus.Key.TriggerType == ButtonTrigger.AxisPositive)
                    return Input.GetAxis(bnStatus.Key.Name) > 0f;
                else if(bnStatus.Key.TriggerType == ButtonTrigger.AxisNegative)
                    return Input.GetAxis(bnStatus.Key.Name) < 0f;
            }

            return false;
        }


        // Get whether this button
        public bool IsButtonTrigger(ButtonName button)
        {
            if(!IsEnable) return false;
            if(IsVirtual) return base.GetButton(button);

            ButtonStatus bnStatus;
            if(m_ButtonTable.TryGetValue(button, out bnStatus))
            {
                var triggerType = bnStatus.Key.TriggerType;
                if(triggerType == ButtonTrigger.Button)
                    return Input.GetButton(bnStatus.Key.Name);
                else if(triggerType == ButtonTrigger.AxisPositive)
                    return Input.GetAxis(bnStatus.Key.Name) > 0f;
                else if(triggerType == ButtonTrigger.AxisNegative)
                    return Input.GetAxis(bnStatus.Key.Name) < 0f;
            }

            return false;
        }


        // Key Code
        public bool GetKeyDown(KeyCode code)
        {
            if (!IsEnable || IsVirtual) return false;

            return Input.GetKeyDown(code);
        }

        public bool GetKeyDown(string code)
        {
            if (string.IsNullOrEmpty(code) || !IsEnable || IsVirtual) return false;

            return Input.GetKeyDown(code);
        }

        public bool GetKeyUp(string code)
        {
            if (string.IsNullOrEmpty(code) || !IsEnable || IsVirtual) return false;

            return Input.GetKeyUp(code);
        }

        public bool GetKey(string code)
        {
            if (string.IsNullOrEmpty(code) || !IsEnable || IsVirtual) return false;

            return Input.GetKey(code);
        }
    }
}