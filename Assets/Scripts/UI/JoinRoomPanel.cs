using Scripts.Helpers;
using Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class JoinRoomPanel : PanelBase
    {
        [SerializeField] private Button m_JoinWithRoomPropertiesPanelButton;
        [SerializeField] private Button m_JoinWithRoomListPanelButton;

        public override void Start()
        {
            SetElements();
        }

        private void SetElements()
        {
            m_JoinWithRoomPropertiesPanelButton.SetButtonOnClick(() => PanelManager.Ins.OpenPanel(Enums.PanelType.JoinByRoomPropertiesPanel));
            m_JoinWithRoomListPanelButton.SetButtonOnClick(() => PanelManager.Ins.OpenPanel(Enums.PanelType.JoinByRoomListPanel));
        }
    }
}
