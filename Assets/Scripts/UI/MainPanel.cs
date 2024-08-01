using UnityEngine;
using Scripts.Enums;
using UnityEngine.UI;
using Scripts.Helpers;
using Scripts.Managers;

namespace Scripts.UI
{
    public class MainPanel : PanelBase
    {
        [SerializeField] private Button m_CreateRoomPanelButton;
        [SerializeField] private Button m_JoinRoomPanelButton;

        public override void Start()
        {
            SetButtons();
        }

        private void SetButtons()
        {
            CreateRoomPanelButton();
            JoinRoomPanelButton();
        }

        private void CreateRoomPanelButton()
        {
            m_CreateRoomPanelButton.SetButtonOnClick(() => PanelManager.Ins.OpenPanel(PanelType.CreateRoomPanel));
        }

        private void JoinRoomPanelButton()
        {
            m_JoinRoomPanelButton.SetButtonOnClick(() => PanelManager.Ins.OpenPanel(PanelType.JoinRoomPanel));
        }
    }
}
