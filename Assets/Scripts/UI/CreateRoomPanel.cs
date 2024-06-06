using Fusion;
using Scripts.Managers;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Scripts.UI
{
    public class CreateRoomPanel : MonoBehaviour
    {
        [SerializeField] private Button m_CreateRoomButton;

        void Start()
        {
        
        }

        void Update()
        {
        
        }

        public bool CreateRoom(string roomName, string roomPassword, int maxPlayers, bool isVisible)
        {
            var sessionProperties = new Dictionary<string, SessionProperty>()
            {
                {"Password", roomPassword },
            };

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
    }
}
