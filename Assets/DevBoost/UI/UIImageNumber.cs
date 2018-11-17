using System;
using System.Collections.Generic;
using UnityEngine;

namespace DevBoost.UI
{
    [RequireComponent(typeof(UnityEngine.UI.Image))]
    public class UIImageNumber : MonoBehaviour
    {
        // 0 - 9 and comma
        [SerializeField] Sprite[] m_ImageNumber = new Sprite[10];
        [SerializeField] Sprite m_ImageComma = null;

        // member variable
        private int m_Number = 0;
        private bool m_IsComma = false;
        private UnityEngine.UI.Image m_Image = null;


        public int Value
        {
            get { return m_Number; }
            set {
                m_Number = Mathf.Clamp(value,0, 9);
                UpdateDisplay();
            }
        }

        public bool IsComma {
            get { return m_IsComma; }
            set {
                m_IsComma = value;
                UpdateDisplay();
            }
        }

        private void Awake()
        {
            m_Image = GetComponent<UnityEngine.UI.Image>();
        }

        // Use this for initialization
        private void Start()
        {
            UpdateDisplay();
        }

        // Update is called once per frame
        private void UpdateDisplay()
        {
            if(IsComma)
            {
                m_Image.sprite = m_ImageComma;
                return;
            }

            m_Image.sprite = m_ImageNumber[Value];
        }
    }

}