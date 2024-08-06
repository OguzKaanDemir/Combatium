using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Scripts.Helpers;
using Scripts.Managers;

namespace Scripts.UI
{
    public class JoinByRoomPropertiesPanel : PanelBase
    {
        [SerializeField] private TMP_InputField m_RoomNameField;
        [SerializeField] private TMP_InputField m_RoomPasswordField;
        [SerializeField] private Button m_JoinButton;

        private string m_RoomName;
        private string m_RoomPassword;

        public override void Start()
        {
            SetElements();
        }

        private void SetElements()
        {
            m_RoomNameField.SetTMPInputFieldOnValueChanged(SetRoomName);
            m_RoomPasswordField.SetTMPInputFieldOnValueChanged(SetRoomPassword);
            m_JoinButton.SetButtonOnClick(JoinRoom);
        }

        private void SetRoomName(string roomName)
            => m_RoomName = roomName;

        private void SetRoomPassword(string roomPassword)
            => m_RoomPassword = roomPassword;

        private void JoinRoom()
        {
            if (string.IsNullOrEmpty(m_RoomName)) return;

            m_JoinButton.enabled = false;

            if (!NetworkManager.Ins.JoinRoom(m_RoomName, m_RoomPassword))
            {
                m_JoinButton.interactable = true;
            }
        }
    }
}
