using TMPro;
using Fusion;
using UnityEngine;
using UnityEngine.UI;
using Scripts.Managers;
using System.Collections.Generic;

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
        [SerializeField] private Button m_BackButton;

        private string m_RoomName;
        private string m_RoomPassword;
        private bool m_IsVisibleRoom;
        private int m_MaxPlayers;

        public override void Start()
        {
            base.Start();
            SetElements();
        }

        public bool CreateRoom(string roomName, string roomPassword, int maxPlayers, bool isVisible)
        {
            if (string.IsNullOrEmpty(roomName)) return false;
            if (maxPlayers <= 0) maxPlayers = m_DefaultMaxPlayers;

            var sessionProperties = new Dictionary<string, SessionProperty>();
            if (!string.IsNullOrEmpty(roomPassword))
            {
                sessionProperties.Add("Password", roomPassword);
            }

            var gameConfig = new StartGameArgs()
            {
                GameMode = GameMode.AutoHostOrClient,
                SessionName = roomName,
                SessionProperties = sessionProperties,
                PlayerCount = maxPlayers,
                IsVisible = isVisible
            };

            var result = NetworkManager.Runner.StartGame(gameConfig).Result;

            if (result.ShutdownReason is not ShutdownReason.Ok)
                return false;

            return true;
        }

        private void SetElements()
        {
            m_RoomNameField.onValueChanged.AddListener(SetRoomName);
            m_RoomPasswordField.onValueChanged.AddListener(SetRoomPassword);
            m_IsVisibleRoomToggle.onValueChanged.AddListener(SetIsRoomVisible);
            m_MaxPlayerSlider.onValueChanged.AddListener(SetMaxPlayers);
            m_CreateRoomButton.onClick.AddListener(() => CreateRoom(m_RoomName, m_RoomPassword, m_MaxPlayers, m_IsVisibleRoom));
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
        }
    }
}
