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

        private PanelType m_PrevPanel = PanelType.None;
        private PanelType m_CurrentPanel = PanelType.MainPanel;

        public void OpenPanel(PanelType panelType)
        {
            m_Panels.ForEach(panel =>
            {
                if (panel.panelType == panelType)
                    panel.OpenPanel();
            });

            m_PrevPanel = m_CurrentPanel;
            m_CurrentPanel = panelType;

            if (m_PrevPanel != PanelType.None)
                ClosePanel(m_PrevPanel);

            BackButton.Ins.AddButtonEvents(() =>
            {
                ClosePanel(panelType);
                OpenPanel(m_PrevPanel);
            });
        }

        public void ClosePanel(PanelType panelType)
        {
            m_Panels.ForEach(panel =>
            {
                if (panel.panelType == panelType)
                {
                    panel.ClosePanel();
                    return;
                }
            });
        }
    }
}
