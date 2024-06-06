using Scripts.Enums;
using UnityEngine;

namespace Scripts.UI
{
    public class PanelBase : MonoBehaviour
    {
        public PanelType panelType;

        private CanvasGroup m_CanvasGroup;

        private void Start()
        {
            m_CanvasGroup = GetComponent<CanvasGroup>();
        }

        public void OpenPanel()
        {
            m_CanvasGroup.alpha = 1.0f;
        }

        public void ClosePanel()
        {
            m_CanvasGroup.alpha = 0.0f;
        }
    }
}
