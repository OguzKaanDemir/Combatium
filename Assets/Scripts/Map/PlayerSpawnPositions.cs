using Photon.Pun;
using UnityEngine;
using System.Collections.Generic;

namespace Scripts.Map
{
    [RequireComponent(typeof(PhotonView))]
    public class PlayerSpawnPositions : MonoBehaviourPun
    {
        [SerializeField] private List<Transform> m_SpawnPositions;

        private List<int> m_GottenPositions = new();
        private int m_LastIndex;

        public Transform GetPosition()
        {
            return m_SpawnPositions[GetAvailableIndex()];
        }

        [PunRPC]
        private void SetGottenPositions(int index)
        {
            m_GottenPositions.Add(index);
            m_LastIndex = index;

            if (m_SpawnPositions.Count == m_GottenPositions.Count)
                m_GottenPositions.Clear();

        }

        private int GetAvailableIndex()
        {
            int rnd;

            do
            {
                rnd = Random.Range(0, m_SpawnPositions.Count);
            } while (rnd == m_LastIndex || m_GottenPositions.Contains(rnd));
            photonView.RPC(nameof(SetGottenPositions), RpcTarget.AllBuffered, rnd);

            return rnd;
        }
    }
}
