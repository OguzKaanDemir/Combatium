using UnityEngine;
using Scripts.Enums;
using DG.Tweening;

namespace Scripts.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class PanelBase : MonoBehaviour
    {
        public PanelType panelType;

        private CanvasGroup m_CanvasGroup;
        private float m_AnimationSpeed = 0.075f;


        public virtual void Start()
        {

        }

        public virtual bool OpenPanel()
        {
            return SetCanvasGroup(1.0f, true, true);
        }

        public virtual bool ClosePanel()
        {
            return SetCanvasGroup(0.0f, false, false);
        }

        public virtual bool SetCanvasGroup(float alpha, bool interactable, bool blocksRaycasts)
        {
            if (!m_CanvasGroup) m_CanvasGroup = GetComponent<CanvasGroup>();
            if (m_CanvasGroup.alpha == alpha) return false;

            m_CanvasGroup.DOFade(alpha, m_AnimationSpeed);
            m_CanvasGroup.interactable = interactable;
            m_CanvasGroup.blocksRaycasts = blocksRaycasts;

            return true;
        }
    }
}
