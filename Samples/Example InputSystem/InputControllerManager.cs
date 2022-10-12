using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Example.InputSystem
{
    using DevBoost.InputSystem;
    using DevBoost.Utilities;

    [System.Serializable]
    public class ControlSelect
    {
        public string controller;
        public string buttonName;

        public bool selected;

        public ControlSelect(string controller, string buttonName)
        {
            this.controller = controller;
            this.buttonName = buttonName;
            selected = false;
        }
    }

    /// <summary>
    /// Used to handle input for a player character
    /// </summary>
    public class InputControllerManager : SingletonMono<InputControllerManager>
    {
        /// <summary>
        /// Get whether input is enabled
        /// </summary>
        [HideInInspector]
        public bool IsInputEnabled = true;

        [Header("Buttons")]
        [SerializeField] protected string m_StartButtonName = "Submit";


        private List<ControlSelect> m_StartButtons = new List<ControlSelect>();

        #region | Properties |

        public List<ControlSelect> StartButtonNames { get { return m_StartButtons; } }


        #endregion

        private void Start()
        {
            // default Keyboard
            var defaultController = new JoystickInfo() {
                m_DeviceName = "keyboard"
            };
            Debug.LogFormat("[ InputControllerManager ] Bind {0},'{1}'", defaultController.m_DeviceName, m_StartButtonName);
            m_StartButtons.Add(new ControlSelect(defaultController.m_DeviceName, m_StartButtonName));

            foreach(var info in JoystickLoader.JoystickList)
                OnJoystickPlugedIn(info);

            JoystickLoader.Instance.OnJoystickPlugedIn += OnJoystickPlugedIn;
            JoystickLoader.Instance.OnJoystickPlugedOut += OnJoystickPlugedOut;
        }

        private void OnDestroy()
        {
            if(JoystickLoader.Instance != null)
            {
                JoystickLoader.Instance.OnJoystickPlugedIn -= OnJoystickPlugedIn;
                JoystickLoader.Instance.OnJoystickPlugedOut -= OnJoystickPlugedOut;
            }
        }

        private void OnJoystickPlugedIn(JoystickInfo info)
        {
            string controllerName = info.ControllerName;
            JoystickBind bind = InputBindings.GetJoystickBind(info);
            if(null != bind)
            {
                Debug.LogFormat("[ InputControllerManager ] Bind {0},{1},{2},'{0}{3}'", controllerName, bind, info.m_DeviceName, bind.GetMenuButtonName(MenuNameCode.Submit, info));
                var newSelect = new ControlSelect(controllerName, bind.GetMenuButtonName(MenuNameCode.Submit, info));
                if(m_StartButtons.FindIndex(va=>va.controller == controllerName) == -1)
                    m_StartButtons.Add(newSelect);
            }
        }

        private void OnJoystickPlugedOut(JoystickInfo info)
        {
            string controllerName = info.ControllerName;
            JoystickBind bind = InputBindings.GetJoystickBind(info);
            if(null != bind)
            {
                Debug.LogFormat("[ InputControllerManager ] Unbind {0},{1},{2}", controllerName, bind, info.m_DeviceName);
                int found = m_StartButtons.FindIndex(va => va.controller == controllerName);
                if(found != -1)
                    m_StartButtons.RemoveAt(found);
            }
        }

        public void ResetButtons()
        {
            foreach(var info in m_StartButtons)
                info.selected = false;
        }

        /// <summary>
        /// Checking whether the button is pressed or not
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public bool CheckButton(System.Func<int, int> callback = null)
        {
            for(int bIndex = 0; bIndex < m_StartButtons.Count; bIndex++)
            {
                if(!m_StartButtons[bIndex].selected && Input.GetButtonUp(m_StartButtons[bIndex].buttonName))
                {
                    if (null != callback)
                        m_StartButtons[bIndex].selected = callback(bIndex) != -1;
                    else
                        m_StartButtons[bIndex].selected = true;

                    return true;
                }
            }
            return false;
        }
    }
}
