using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DevBoost.Utilities;

namespace Example.InputSystem
{

    // type of button Trigger
    public enum ButtonTrigger
    { 
        None = -1,
        Button,
        AxisPositive,
        AxisNegative
    }


    /// <summary>
    /// Name and Button Pair class
    /// </summary>
    public struct NameToButton
    {
        public ButtonName Button;
        public string Name;
        public ButtonTrigger TriggerType;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="button"></param>
        /// <param name="name"></param>
        public NameToButton(ButtonName button, string name)
        {
            this.Button = button;
            this.Name = name;
            this.TriggerType = Parsing(name);
        }

        // name filter
        public static string FilterName(string varString)
        {
            string newName = "";
            foreach(var word in varString.Split(' '))
            {
                if(word == "negative" || word == "positive")
                    continue;
                newName += newName.Length == 0 ? word : (" " + word);
            }
            return newName;
        }

        // Parsing
        public static ButtonTrigger Parsing(string varString)
        {
            if(string.IsNullOrEmpty(varString))
                return ButtonTrigger.None;

            var words = varString.Split(' ');

            if(words.Length == 1)
            {
                if(varString.Contains("Axis"))
                    return ButtonTrigger.AxisPositive;
            }

            if(words[words.Length - 1] == "negative")
                return ButtonTrigger.AxisNegative;
            if(words[words.Length - 1] == "positive")
                return ButtonTrigger.AxisPositive;

            return ButtonTrigger.Button;
        }
    }

    public class VirtualInputButton
    {

        public enum ButtonType
        {
            None,
            Down,
            Up
        }

        /// <summary>
        /// Button Task Info
        /// </summary>
        public class ButtonTask
        {
            public bool isDone { get { return (m_timer != null) ? m_timer.IsElapsed(m_time) : false; } }
            public float StartTime { get; private set; }

            protected TimeStamp m_timer;
            protected float m_time;
            protected ButtonStatus m_Reference;
            public ButtonName ButtonName { get; protected set; }

            private System.Action[] m_TaskSeq;
            private int index;

            public ButtonTask(ButtonName ButtonName, ButtonStatus referer, float takingTime = 0, float startTime = 0)
            {
                m_Reference = referer;
                this.ButtonName = ButtonName;
                this.m_time = takingTime;
                this.m_timer = null;
                this.StartTime = Time.time + startTime;
                index = 0;
                m_TaskSeq = new System.Action[] { _Enter, _Working, _Leave, _Done };
            }

            public bool Update()
            {
                // routine
                if(index < m_TaskSeq.Length)
                {
                    m_TaskSeq[index]();
                    return false;
                }

                return true;
            }

            void _Enter()
            {
                m_timer = new TimeStamp(true);
                m_Reference.buttonState = ButtonType.Down;
                index++;

                // skip next step if time is zero. that means just trigger it
                if(m_time <= 0)
                    index++;
            }

            void _Working()
            {
                if (m_timer.IsElapsed(m_time))
                    index++;
            }

            void _Leave()
            {
                m_Reference.buttonState = ButtonType.Up;
                index++;
            }

            void _Done()
            {
                m_Reference.buttonState = ButtonType.None;
                index++;
            }
        }

        /// <summary>
        /// Button Status Info
        /// </summary>
        public class ButtonStatus
        {
            public NameToButton Key;
            public bool IsPushDown { get { return buttonState == ButtonType.Down; } }
            public bool IsPress { get { return buttonState == ButtonType.Down; } }
            public bool IsPullUp { get { return buttonState == ButtonType.Up; } }
            public ButtonType buttonState;

            public ButtonStatus(string name, ButtonName button)
            {
                this.Key.Name = name;
                this.Key.Button = button;
                this.Key.TriggerType = NameToButton.Parsing(name);
                // rename
                if(this.Key.TriggerType != ButtonTrigger.None || this.Key.TriggerType != ButtonTrigger.Button)
                {
                    this.Key.Name = NameToButton.FilterName(this.Key.Name);
                }
            }

            public void Reset()
            {
                buttonState = ButtonType.None;
            }
        }

        /// <summary>
        /// Implements
        /// </summary>
        protected Dictionary<ButtonName, ButtonStatus> m_ButtonTable = new Dictionary<ButtonName, ButtonStatus>();

        List<ButtonTask> m_tasks = new List<ButtonTask>();
        List<ButtonTask> m_eliminate = new List<ButtonTask>();
        List<ButtonTask> m_Schedule = new List<ButtonTask>();

        // task count
        public int TaskCount
        {
            get { return m_tasks.Count; }
        }

        // register buttons
        public void Register(string buttonName, ButtonName button)
        {
            if (string.IsNullOrEmpty(buttonName))
                return;

            if (m_ButtonTable.ContainsKey(button))
            {
                Debug.Log("Duplicate key : " + button);
                return;
            }

            m_ButtonTable.Add(button, new ButtonStatus(buttonName, button));
        }

        // Button Name
        public bool TryGetButtonName(ButtonName button, out string ButtonName)
        {
            ButtonStatus bnStatus;
            if (m_ButtonTable.TryGetValue(button, out bnStatus))
            {
                ButtonName = bnStatus.Key.Name;
                return true;
            }
            ButtonName = "";
            return false;
        }

        // Get string from the button enum
        public string GetButtonName(ButtonName button)
        {
            ButtonStatus bnStatus;
            if (m_ButtonTable.TryGetValue(button, out bnStatus))
                return bnStatus.Key.Name;
            return "";
        }

        // Reset Input axes ( for working from scretch )
        public void ResetInput()
        {
            m_tasks.Clear();
            foreach (var info in m_ButtonTable.Values)
                info.Reset();
        }

        /// <summary>
        /// Create Virtual Button state
        /// </summary>
        /// <param name="buttonName"></param>
        /// <param name="takingTime"></param>
        /// <returns></returns>
        public bool CreateButtonTask(ButtonName buttonName, float takingTime, float startTime)
        {
            ButtonStatus bnStatus;
            if (m_ButtonTable.TryGetValue(buttonName, out bnStatus))
            {
                var newTask = new ButtonTask(buttonName, bnStatus, takingTime, startTime);

                if(startTime > 0)
                {
                    m_Schedule.Add(newTask);
                    return true;
                }

                return PutTask(newTask);

            }
            return false;
        }

        // has task to do 
        public bool HasTask(ButtonName buttonName)
        {
            return _IsWorkingTask(buttonName) || m_Schedule.Any(va => va.ButtonName == buttonName);
        }

        // Put Task
        private bool PutTask(ButtonTask buttonTask)
        {
            if (_IsWorkingTask(buttonTask.ButtonName))
                return false;
            m_tasks.Add(buttonTask);
            return true;
        }

        // checking working button internally
        private bool _IsWorkingTask(ButtonName buttonName)
        {
            return m_tasks.Any(va => va.ButtonName == buttonName);
        }

        /// <summary>
        /// Update call function
        /// </summary>
        public void UpdateTick()
        {
            // delete garbage
            if(m_eliminate.Count > 0)
            {
                foreach(var task in m_eliminate)
                    m_tasks.Remove(task);
                m_eliminate.Clear();
            }

            // Schedule
            if(m_Schedule.Count > 0)
            {
                foreach (var task in m_Schedule.Where(va => va.StartTime < Time.time).ToArray())
                {
                    PutTask(task);
                    m_Schedule.Remove(task);
                }
            }

            // update 
            foreach (var task in m_tasks)
            {
                if (task.Update())
                    m_eliminate.Add(task);
            }
        }

        /// <summary>
        /// Get whether the button is pressed or not
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool IsPressed(ButtonName button)
        {
            ButtonStatus bnStatus;
            if (m_ButtonTable.TryGetValue(button, out bnStatus))
                return bnStatus.IsPress;
            return false;
        }


        /// <summary>
        /// Get value whether input button is pressed
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        virtual public bool GetButton(string button)
        {
            Debug.Assert(button != string.Empty);
            if (button == string.Empty)
                return false;
            var findIndex = m_ButtonTable.Values.FirstOrDefault(va => va.Key.Name == button);
            if (findIndex == default(ButtonStatus))
                return false;
            return findIndex.IsPress;
        }

        /// <summary>
        /// Get value whether input button is gotten down
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        virtual public bool GetButtonDown(string button)
        {
            Debug.Assert(button != string.Empty);
            if (button == string.Empty)
                return false;
            var findIndex = m_ButtonTable.Values.FirstOrDefault(va => va.Key.Name == button);
            if (findIndex == default(ButtonStatus))
                return false;
            return findIndex.IsPushDown;
        }

        /// <summary>
        /// Get value whether input button is gotten up
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        virtual public bool GetButtonUp(string button)
        {
            Debug.Assert(button != string.Empty);
            if (button == string.Empty)
                return false;
            var findIndex = m_ButtonTable.Values.FirstOrDefault(va => va.Key.Name == button);
            if (findIndex == default(ButtonStatus))
                return false;
            return findIndex.IsPullUp;
        }


        /// <summary>
        /// Get value whether input button is presing or not
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        virtual public bool GetButton(ButtonName button)
        {
            if (!m_ButtonTable.ContainsKey(button))
                return false;

            return m_ButtonTable[button].IsPress;
        }


        /// <summary>
        /// Get value whether input button is down or not
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        virtual public bool GetButtonDown(ButtonName button)
        {
            if (!m_ButtonTable.ContainsKey(button))
                return false;

            return m_ButtonTable[button].IsPushDown;
        }

        /// <summary>
        /// Get value whether input button is up or not
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        virtual public bool GetButtonUp(ButtonName button)
        {
            if (!m_ButtonTable.ContainsKey(button))
                return false;

            return m_ButtonTable[button].IsPullUp;
        }

    }

}