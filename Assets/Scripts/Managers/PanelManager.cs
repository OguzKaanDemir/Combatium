using Scripts.UI;
using UnityEngine;
using Scripts.Enums;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

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
        private PanelType m_CurrentPanel = PanelType.None;

        private void Start()
        {
            CloseAllPanels();
            OpenPanel(PanelType.LoadingPanel, true);
        }

        public void OpenPanel(PanelType panelType, bool isIgnore = false)
        {
            var panel = m_Panels.First(p => p.panelType == panelType);

            panel.OpenPanel();

            m_PrevPanel = panel.parentPanelType;
            m_CurrentPanel = panelType;

            if (m_PrevPanel != PanelType.None)
                ClosePanel(m_PrevPanel);

            if (isIgnore || m_CurrentPanel == PanelType.LoadingPanel || m_PrevPanel == PanelType.LoadingPanel) return;

            BackButton.Ins.AddButtonEvents(() =>
            {
                ClosePanel(panelType);
                OpenPanel(m_PrevPanel, true);
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

        public void CloseAllPanels()
        {
            m_Panels.ForEach(panel =>
            {
                panel.ClosePanel();
            });

            BackButton.Ins.ClearButtonEvents();
        }
    }
}
