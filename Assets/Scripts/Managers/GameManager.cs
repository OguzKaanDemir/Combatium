using Photon.Pun;
using Scripts.Interfaces;
using UnityEngine.Events;

namespace Scripts.Managers
{
    public class GameManager : MonoBehaviourPun, IStartGame, IEndGame
    {
        private static GameManager m_Ins;
        public static GameManager Ins
        {
            get
            {
                if(!m_Ins)
                    m_Ins = FindObjectOfType<GameManager>();
                return m_Ins;
            }
        }

        public UnityAction onGameStart;
        public UnityAction onGameEnd;

        public void StartGame()
        {
            photonView.RPC(nameof(StartGameRPC), RpcTarget.AllBuffered);
        }

        [PunRPC]
        private void StartGameRPC()
        {
            onGameStart?.Invoke();
        }

        public void EndGame()
        {
            photonView.RPC(nameof(EndGameRPC), RpcTarget.AllBuffered);
        }

        [PunRPC]
        private void EndGameRPC()
        {
            onGameEnd?.Invoke();
        }
    }
}
