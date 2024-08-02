using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Scripts.Helpers;
using Photon.Realtime;
using Scripts.Managers;
using ExitGames.Client.Photon;

namespace Scripts.UI
{
    public class CreateRoomPanel : PanelBase
    {
        [SerializeField] private int m_DefaultMaxPlayers = 2;

        [SerializeField] private TMP_InputField m_RoomNameField;
        [SerializeField] private TMP_InputField m_RoomPasswordField;
        [SerializeField] private Slider m_MaxPlayerSlider;
        [SerializeField] private TMP_Text m_MaxPlayerText;
        [SerializeField] private Toggle m_IsVisibleRoomToggle;
        [SerializeField] private Button m_CreateRoomButton;

        private string m_RoomName;
        private string m_RoomPassword;
        private bool m_IsVisibleRoom;
        private int m_MaxPlayers;

        public override void Start()
        {
            SetElements();
        }

        public void CreateRoom(string roomName, string roomPassword, int maxPlayers, bool isVisible)
        {
            if (string.IsNullOrEmpty(roomName)) return;
            if (maxPlayers <= 0) maxPlayers = m_DefaultMaxPlayers;

            var customProperties = new Hashtable();
            if (!string.IsNullOrEmpty(roomPassword))
                customProperties.Add("Password", roomPassword);

            var roomOptions = new RoomOptions()
            {
                IsVisible = isVisible,
                CustomRoomProperties = customProperties,
                MaxPlayers = maxPlayers
            };

            NetworkManager.Ins.CreateRoom(roomName, roomOptions);
            m_CreateRoomButton.interactable = false;
        }

        public void ResetCreateRoomButton()
        {
            m_CreateRoomButton.interactable = true;
        }

        private void SetElements()
        {
            m_RoomNameField.AddTMPInputFieldOnValueChanged(SetRoomName);
            m_RoomPasswordField.AddTMPInputFieldOnValueChanged(SetRoomPassword);
            m_IsVisibleRoomToggle.AddToggleOnValueChanged(SetIsRoomVisible);
            m_MaxPlayerSlider.AddSliderOnValueChanged(SetMaxPlayers);
            m_CreateRoomButton.AddButtonOnClick(() => CreateRoom(m_RoomName, m_RoomPassword, m_MaxPlayers, m_IsVisibleRoom));
        }

        private void SetRoomName(string name)
        {
            m_RoomName = name;
        }

        private void SetRoomPassword(string password)
        {
            m_RoomPassword = password;
        }

        private void SetIsRoomVisible(bool visible)
        {
            m_IsVisibleRoom = visible;
        }

        private void SetMaxPlayers(float maxPlayers)
        {
            m_MaxPlayers = (int)maxPlayers;
            m_MaxPlayerText.text = $"({maxPlayers})";
        }
    }
}
