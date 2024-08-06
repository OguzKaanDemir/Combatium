using Photon.Pun;
using Photon.Realtime;
using Scripts.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Managers
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        private static NetworkManager m_Ins;
        public static NetworkManager Ins
        {
            get
            {
                if (!m_Ins)
                    m_Ins = FindObjectOfType<NetworkManager>();
                return m_Ins;
            }
        }

        [SerializeField] private CreateRoomPanel m_CreateRoomPanel;

        private List<RoomInfo> m_RoomList = new();


        private void Awake()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected To Master...");

            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("Joined Lobby");

            PanelManager.Ins.OpenPanel(Enums.PanelType.MainPanel);
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Joined Room");
            PanelManager.Ins.CloseAllPanels();
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            m_RoomList = roomList;
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            base.OnCreateRoomFailed(returnCode, message);
            m_CreateRoomPanel.ResetCreateRoomButton();
        }

        public void CreateRoom(string roomName, RoomOptions roomOptions)
        {
            PhotonNetwork.CreateRoom(roomName, roomOptions);
        }

        public bool JoinRoom(string roomName, string roomPassword)
        {
            var room = m_RoomList.Find(room => room.Name == roomName);

            if (room != null)
            {
                if (room.CustomProperties.ContainsKey("Password") && (string)room.CustomProperties["Password"] == roomPassword)
                {
                    PhotonNetwork.JoinRoom(roomName);
                    return true;
                }
                return false;
            }
            return false;
        }

        public List<RoomInfo> GetRoomList()
        {
            return m_RoomList;
        }
    }
}
