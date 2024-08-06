using UnityEngine;

namespace Scripts.Map
{
    public class Map : MonoBehaviour
    {
        [SerializeField] private PlayerSpawnPositions m_PlayerSpawnPositions;
        [SerializeField] private WeaponSpawnPositions m_WeaponSpawnPositions;

        public Transform GetPlayerSpawnPosition()
            => m_PlayerSpawnPositions.GetPosition();

        public Transform GetWeaponSpawnPosition()
            => m_WeaponSpawnPositions.GetPosition();
    }
}
