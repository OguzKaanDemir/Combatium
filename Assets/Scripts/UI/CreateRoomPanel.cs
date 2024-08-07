using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Scripts.Helpers;
using Photon.Realtime;
using Scripts.Managers;
using ExitGames.Client.Photon;
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

            List<string> customPropertiesForLobby = new()
            {
                KeyHelper.ISVISIBLE_KEY
            };

            var roomOptions = new RoomOptions()
            {
                IsVisible = true,
                IsOpen = true,
                MaxPlayers = maxPlayers
            };

            var customProperties = new Hashtable
            {
                { KeyHelper.ISVISIBLE_KEY, isVisible }
            };

            if (!string.IsNullOrEmpty(roomPassword))
            {
                customProperties.Add(KeyHelper.PASSWORD_KEY, roomPassword);
                customPropertiesForLobby.Add(KeyHelper.PASSWORD_KEY);
            }

            roomOptions.CustomRoomProperties = customProperties;
            roomOptions.CustomRoomPropertiesForLobby = customPropertiesForLobby.ToArray();

            if (NetworkManager.Ins.CreateRoom(roomName, roomOptions))
                m_CreateRoomButton.interactable = false;
            else
                m_CreateRoomButton.interactable = true;
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
