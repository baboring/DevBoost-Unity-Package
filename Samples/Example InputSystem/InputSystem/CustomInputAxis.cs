using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevBoost.InputSystem
{

    public class CustomInputAxis
    {
        private bool m_isVirtual = false;
        private bool m_isCustomUse = true;
        protected bool m_isEnable = true;        
        private bool m_isRestTurnback = true;      // return to smooth recovery option
        private float m_sensitivity = 3.5f;           // smooth density
        private float m_thresold = 0.001f;

        private Dictionary<string, float?> m_inputValues = new Dictionary<string, float?>();


        public float Sensitivity
        {
            get { return m_sensitivity; }
            set { m_sensitivity = value; }
        }

        // handled in virtual
        public bool IsVirtual
        {
            get { return m_isVirtual; }
            set { m_isVirtual = value; }
        }

        // holding axis to zero
        public bool IsEnable
        {
            get { return m_isEnable; }
            set { m_isEnable = value; }
        }

        // Cutting the different direction
        public bool IsQuickBack
        {
            get { return m_isRestTurnback; }
            set { m_isRestTurnback = value; }
        }

        // Enable / Disable
        public bool IsCustomUse
        {
            get { return m_isCustomUse; }
            set { m_isCustomUse = value; }
        }

        // constructor
        public CustomInputAxis(float Sensitivity = 0)
        {
            this.Sensitivity = Sensitivity;
        }

        // set axis value
        public void SetAxisValue(string axisName, float value)
        {
            // check key exist
            if (!m_inputValues.ContainsKey(axisName))
                Register(axisName);

            m_inputValues[axisName] = value;
        }

        public void Register(string initKey)
        {
            if(string.IsNullOrEmpty(initKey))
                return;
            if(!m_inputValues.ContainsKey(initKey))
                m_inputValues[initKey] = new float();
        }

        public void Register(string[] initKeys)
        {
            foreach(var key in initKeys)
                if(!string.IsNullOrEmpty(key) && !m_inputValues.ContainsKey(key))
                    m_inputValues[key] = new float();
        }


        // Input Axis Value ( Smooth )
        public float GetAxis(string axisName)
        {
            if(string.IsNullOrEmpty(axisName))
                return 0;

            // check key exist
            if(!m_inputValues.ContainsKey(axisName))
                Register(axisName);

            // control by outside
            if (IsVirtual)
                return m_inputValues[axisName].Value;

            if (IsCustomUse)
                return GetAxisSmooth(axisName, IsQuickBack);

            return Input.GetAxis(axisName);
        }

        // get axis raw
        public float GetAxisRaw(string axisName)
        {
            if(string.IsNullOrEmpty(axisName))
                return 0;

            // check key exist
            if(!m_inputValues.ContainsKey(axisName))
                Register(axisName);

            // control by outside
            if (IsVirtual)
                return m_inputValues[axisName].Value;

            try
            {
                return Input.GetAxisRaw(axisName);
            }
            catch(System.Exception exp)
            {
                Debug.LogException(exp);
                return 0;
            }
        }

        // Smooth filter
        private float GetAxisSmooth(string axisName, bool isQuickBack = true)
        {
            if(string.IsNullOrEmpty(axisName))
                return 0;
            // value
            float value = m_inputValues[axisName].Value;

            float target = (!IsEnable) ? 0 : Input.GetAxisRaw(axisName);

            if(isQuickBack && ((target > 0 && value < 0) || (target < 0 && value > 0)))
                value = 0;

            if(m_sensitivity == 0)
                value = target;
            else
                value = Mathf.MoveTowards(value, target, m_sensitivity * Time.fixedDeltaTime);

            m_inputValues[axisName] = value;

            return (Mathf.Abs(value) < m_thresold) ? 0f : value;
        }

        // Reset Input axes ( for working from scratch )
        public void ResetInputAxes(float val = 0)
        {
            foreach(var info in m_inputValues)
            {
                m_inputValues[info.Key] = val;
            }
        }

    }
}