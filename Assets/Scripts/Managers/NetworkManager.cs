using TMPro;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Managers
{
    [RequireComponent(typeof(NetworkRunner))]
    public class NetworkManager : MonoBehaviour
    {
        [SerializeField] private TMP_InputField m_RoomNameField;
        [SerializeField] private TMP_InputField m_RoomPasswordField;
        [SerializeField] private Slider m_MaxPlayerSlider;
        [SerializeField] private TMP_Text m_MaxPlayerText;
        [SerializeField] private Toggle m_IsVisibleRoomToggle;
        [SerializeField] private Button m_CreateRoomButton;
        [SerializeField] private Button m_BackButton;

        private NetworkRunner m_Runner;

        private string m_RoomName;
        private string m_RoomPassword;
        private bool m_IsVisibleRoom;
        private int m_MaxPlayers;

        private void Start()
        {
            m_Runner = GetComponent<NetworkRunner>();
            SetElements();
        }

        private void SetElements()
        {
            m_RoomNameField.onValueChanged.AddListener(SetRoomName);
            m_RoomPasswordField.onValueChanged.AddListener(SetRoomPassword);
            m_IsVisibleRoomToggle.onValueChanged.AddListener(SetIsRoomVisible);
            m_MaxPlayerSlider.onValueChanged.AddListener(SetMaxPlayers);
            m_CreateRoomButton.onClick.AddListener(CreateRoom);
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

        private void CreateRoom()
        {
            var gameConfig = new StartGameArgs()
            {
                GameMode = GameMode.AutoHostOrClient,

            };
            m_Runner.StartGame(gameConfig);
        }
    }
}
