using DevBoost.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace Example.InputSystem
{

    public class JoystickLoader : Singleton<JoystickLoader>
    {
        // current joystick list
        private List<JoystickInfo> m_PlugedIn_JoystickList = null;
        private List<JoystickInfo> m_AllJoystickList = null;

        public System.Action<JoystickInfo> OnJoystickPlugedIn;
        public System.Action<JoystickInfo> OnJoystickPlugedOut;


        // Get Joystick Indices
        public static List<JoystickInfo> JoystickList {
            get
            {
                if(Instance.m_PlugedIn_JoystickList == null)
                {
                    Instance.m_PlugedIn_JoystickList = new List<JoystickInfo>();
                    Instance.m_AllJoystickList = new List<JoystickInfo>();
                }

                return Instance.m_PlugedIn_JoystickList;
            }
        }

        public static int JoystickCount {
            get {
                if(null == Instance)
                    return 0;
                return JoystickList.Count;
            }
        }

        /// <summary>
        /// Get Joystick Info
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static JoystickInfo GetJoystickInfoById(int Id)
        {
            return JoystickList.Find(va=>va.m_Id == Id);
        }
        public static JoystickInfo GetJoystickInfoByNum(int num)
        {
            return JoystickList.Find(va => va.ControlNum == num);
        }
        public static JoystickInfo GetJoystickInfoByIndex(int index)
        {
            return JoystickList.Find(va => va.m_Index == index);
        }

        /// <summary>
        /// Checking joystick states
        /// </summary>
        private void UpdateJoysticInfo()
        {
            if(m_PlugedIn_JoystickList == null)
            {
                m_PlugedIn_JoystickList = new List<JoystickInfo>();
                m_AllJoystickList = new List<JoystickInfo>();
            }

            // checking last state
            var JoystickNames = Input.GetJoystickNames();

            // MAC(OSX) reorders all joystick list.
            if(JoystickNames.Length < m_PlugedIn_JoystickList.Count)
                return;
            string joystickName;
            // - Removed
            foreach(var plugin in m_PlugedIn_JoystickList.ToArray())
            {
                joystickName = JoystickNames[plugin.m_Index];
                if(joystickName.Length < 1 || joystickName.GetHashCode() != plugin.m_DeviceName.GetHashCode())
                {
                    // disconnected
                    Debug.LogFormat("[ InputHelper ] Joystick Disconnected : ID {0}, idx {1}, {2} / {3}", plugin.m_Id, plugin.m_Index, plugin.m_DeviceName, plugin.ControllerName);

                    m_PlugedIn_JoystickList.Remove(plugin);
                    if(null != OnJoystickPlugedOut)
                        OnJoystickPlugedOut(plugin);
                    int idx = m_AllJoystickList.FindIndex(va => va.Equals(plugin));
                    if(idx > -1)
                    {
                        var found = m_AllJoystickList[idx];
                        found.m_isPlugedIn = false;
                        m_AllJoystickList[idx] = found;
                    }
                }
            }

            // - Added
            for(int i = 0; i < JoystickNames.Length; ++i)
            {
                string deviceName = JoystickNames[i];
                if(deviceName.Length > 0)
                {
                    var found = m_PlugedIn_JoystickList.FindIndex(va => va.m_DeviceName == deviceName && va.m_Index == i);
                    if(found < 0)
                    {
                        JoystickInfo plugin = default(JoystickInfo);

                        // determin Id, find out in previous data
                        int newId = 0;
                        int previous = m_AllJoystickList.FindIndex(va => va.m_isPlugedIn == false && va.m_DeviceName == deviceName);

                        //bool reasignId = previous < 0;
                        bool reasignId = true;

                        if (previous > -1)
                        {
                            plugin = m_AllJoystickList[previous];
                            plugin.m_Index = i;
                            plugin.m_isPlugedIn = true;

                            // remove item for remaining consistant
                            m_AllJoystickList.RemoveAt(previous);

                            newId = plugin.m_Id;
                        }

                        // assign ID
                        if(reasignId)
                        {
                            // find a new id
                            for(newId = 1; newId < m_PlugedIn_JoystickList.Count + 1; ++newId)
                            {
                                if(m_PlugedIn_JoystickList.FindIndex(va => va.m_Id == newId) == -1)
                                    break;
                            }
                        }

                        // new instance
                        if(previous == -1)
                        {
                            plugin = new JoystickInfo() {
                                m_Id = newId,
                                m_Index = i,
                                m_DeviceName = deviceName,
                                m_isPlugedIn = true,
                            };
                        }

                        // concurrent
                        m_PlugedIn_JoystickList.Add(plugin);
                        // add again
                        m_AllJoystickList.Add(plugin);

                        Debug.LogFormat("[ InputHelper ] Joystick Connected : ID {0}, idx {1}, {2} / {3}", plugin.m_Id, plugin.m_Index, plugin.m_DeviceName, plugin.ControllerName);
                        // notice
                        if(null != OnJoystickPlugedIn)
                            OnJoystickPlugedIn(plugin);

                    }
                }
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        private void Update()
        {
            UpdateJoysticInfo();
        }

    }

}