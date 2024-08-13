using Photon.Pun;
using Scripts.UI;
using UnityEngine;
using Scripts.Player;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Scripts.Helpers;

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

        [SerializeField] private GameObject m_PlayerPrefab;
        [SerializeField] private GameObject m_MapPrefab;
        [SerializeField] private CreateRoomPanel m_CreateRoomPanel;
        [SerializeField] private JoinByRoomListPanel m_JoinByRoomListPanel;

        private List<RoomInfo> m_RoomList = new();

        private Map.Map m_Map;

        private void Awake()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            Debug.LogError("Connected To Master...");

            PhotonNetwork.JoinLobby(TypedLobby.Default);
        }

        public override void OnJoinedLobby()
        {
            Debug.LogError("Joined Lobby");

            PanelManager.Ins.OpenPanel(Enums.PanelType.MainPanel);
        }

        public override void OnJoinedRoom()
        {
            Debug.LogError("Joined Room");
            PanelManager.Ins.CloseAllPanels();

            if (PhotonNetwork.IsMasterClient)
            {
                var map = PhotonNetwork.Instantiate(m_MapPrefab.name, Vector3.zero, Quaternion.identity);
            }

            m_Map = FindObjectOfType<Map.Map>();

            GameManager.Ins.onGameStart += () =>
                {
                    m_Map = FindObjectOfType<Map.Map>();
                };
            GameManager.Ins.onGameStart += SpawnPlayer;
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                GameManager.Ins.StartGame();
            }
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.LogError("Rooms Updated");

            foreach (RoomInfo room in roomList)
            {
                if (room.RemovedFromList)
                {
                    Debug.LogError("-" + room.Name);
                    m_RoomList.Remove(room);
                }
                else
                {
                    Debug.LogError("+" + room.Name);
                    m_RoomList.Add(room);
                }
            }
            m_JoinByRoomListPanel.ResetUI(m_RoomList);
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            base.OnCreateRoomFailed(returnCode, message);
            Debug.LogError(message);

            m_CreateRoomPanel.ResetCreateRoomButton();
        }

        public bool CreateRoom(string roomName, RoomOptions roomOptions)
        {
            if (m_RoomList.Exists(room => room.Name == roomName)) return false;

            PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
            return true;
        }

        public bool JoinRoom(string roomName, string roomPassword = "")
        {
            var room = m_RoomList.Find(room => room.Name == roomName);

            if (room != null)
            {
                if (room.PlayerCount >= room.MaxPlayers)
                {
                    Debug.LogError("Max player count reached");
                    return false;
                }
                else if (room.CustomProperties.ContainsKey(KeyHelper.PASSWORD_KEY))
                {
                    if ((string)room.CustomProperties[KeyHelper.PASSWORD_KEY] == roomPassword)
                    {
                        PhotonNetwork.JoinRoom(roomName);
                        Debug.LogError("Success");
                        return true;
                    }
                    else
                    {
                        Debug.LogError("Wrong Password");
                        return false;
                    }
                }
                else
                {
                    PhotonNetwork.JoinRoom(roomName);
                    Debug.LogError("Success");
                    return true;
                }
            }
            Debug.LogError("Room cant found");
            return false;
        }

        private void SpawnPlayer()
        {
            StartCoroutine(SpawnPlayerCrt());
        }

        private IEnumerator SpawnPlayerCrt()
        {
            yield return new WaitForSeconds(.5f);
            var player = PhotonNetwork.Instantiate(m_PlayerPrefab.name, m_Map.GetPlayerSpawnPosition().position, Quaternion.identity);
            player.GetComponent<PlayerSetter>().enabled = true;
        }
    }
}
