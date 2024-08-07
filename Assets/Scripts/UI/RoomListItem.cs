using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Scripts.Helpers;
using Scripts.Managers;

namespace Scripts.UI
{
    public class RoomListItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_RoomNameText;
        [SerializeField] private TMP_Text m_RoomPlayersText;

        private Button m_JoinButton;

        public void SetItem(RoomInfo room)
        {
            var roomName = room.Name;
            var roomMaxPlayerCount = room.MaxPlayers;
            var roomCurrnetPlayers = room.PlayerCount;

            m_RoomNameText.text = roomName;

            m_RoomPlayersText.text = $"{roomCurrnetPlayers}/{roomMaxPlayerCount}";

            m_JoinButton = GetComponent<Button>();
            m_JoinButton.SetButtonOnClick(() => JoinRoom(roomName));
        }

        private void JoinRoom(string roomName)
        {
            NetworkManager.Ins.JoinRoom(roomName);
        }
    }
}
