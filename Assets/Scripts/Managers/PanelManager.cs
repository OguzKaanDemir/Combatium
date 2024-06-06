using Scripts.UI;
using UnityEngine;
using Scripts.Enums;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Scripts.Managers
{
    public class PanelManager : MonoBehaviour
    {
        private static PanelManager m_Ins;
        public static PanelManager Ins
        {
            get
            {
                if (!m_Ins)
                    m_Ins = FindObjectOfType<PanelManager>();
                return m_Ins;
            }
        }

        [SerializeField] private List<PanelBase> m_Panels;

        private UnityAction m_OnBackButtonPressed;
        private PanelType m_PrevPanel;
        private PanelType m_CurrentPanel;

        public void OpenPanel(PanelType panelType, UnityAction backButtonAction)
        {
            m_Panels.ForEach(panel =>
            {
                if (panel.panelType != panelType)
                    panel.ClosePanel();
                else
                    panel.OpenPanel();
            });

            m_PrevPanel = m_CurrentPanel;
            m_CurrentPanel = panelType;
            m_OnBackButtonPressed = backButtonAction;
        }
    }
}
