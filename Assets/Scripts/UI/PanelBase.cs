using UnityEngine;
using Scripts.Enums;

namespace Scripts.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class PanelBase : MonoBehaviour
    {
        public PanelType panelType;

        private CanvasGroup m_CanvasGroup;

        public virtual void Start()
        {
            m_CanvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual bool OpenPanel()
        {
            return SetCanvasGroup(1.0f, true, true);
        }

        public virtual bool ClosePanel()
        {
            return SetCanvasGroup(0.0f, false, false);
        }

        private bool SetCanvasGroup(float alpha, bool interactable, bool blocksRaycasts)
        {
            if (m_CanvasGroup.alpha == alpha) return false;

            m_CanvasGroup.alpha = alpha;
            m_CanvasGroup.interactable = interactable;
            m_CanvasGroup.blocksRaycasts = blocksRaycasts;

            return true;
        }
    }
}
