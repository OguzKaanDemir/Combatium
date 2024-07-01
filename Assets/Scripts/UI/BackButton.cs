using Scripts.Helpers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class BackButton : MonoBehaviour
    {
        private static BackButton m_Ins;
        public static BackButton Ins
        {
            get
            {
                if (!m_Ins)
                    m_Ins = FindObjectOfType<BackButton>();
                return m_Ins;
            }
        }

        [SerializeField] private Button m_BackButton;

        private List<UnityAction> m_BackButtonEvents = new();

        private void Start()
        {
            m_BackButton.SetButtonOnClick(Back);
        }

        public void Back()
        {
            var events = m_BackButtonEvents[^1];
            events?.Invoke();
            m_BackButtonEvents.Remove(events);

            if (m_BackButtonEvents.Count > 0)
                m_BackButton.gameObject.SetActive(false);
        }

        public void AddButtonEvents(UnityAction buttonEvents)
        {
            m_BackButtonEvents.Add(buttonEvents);

            m_BackButton.gameObject.SetActive(true);
        }
    }
}
