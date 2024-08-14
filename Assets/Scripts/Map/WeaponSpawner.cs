using Photon.Pun;
using UnityEngine;
using Scripts.Enums;
using Scripts.Weapons;
using Scripts.Managers;
using Scripts.Interfaces;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Scripts.Map
{
    public class WeaponSpawner : MonoBehaviour, IStartGame, IEndGame
    {
        [SerializeField] private Data.Weapons m_Weapons;
        [SerializeField] private Map m_Map;

        [SerializeField] private bool m_IsSpecifiedWeaponsMap;
        [SerializeField, ShowIf(nameof(m_IsSpecifiedWeaponsMap))] private List<WeaponType> m_WeaponTypes;
        [SerializeField] private Vector2 m_SpawnDuration;
        [SerializeField] private int m_StartWeaponCount;


        private void Start()
        {
            StartGame();
            EndGame();
        }

        public void StartGame()
        {
            GameManager.Ins.onGameStart += () =>
                {
                    var rTime = Random.Range(m_SpawnDuration.x, m_SpawnDuration.y);

                    for (int i = 0; i < m_StartWeaponCount; i++)
                        SpawnWeapon();

                    InvokeRepeating(nameof(SpawnWeapon), 3, rTime);
                };
        }

        public void EndGame()
        {
            GameManager.Ins.onGameEnd += () => CancelInvoke(nameof(SpawnWeapon));
        }

        private void SpawnWeapon()
        {
            if (!PhotonNetwork.IsMasterClient) return;

            var weaponType = (WeaponType)Random.Range(0, m_WeaponTypes.Count);
            WeaponBase targetWeapon = null;

            if (m_IsSpecifiedWeaponsMap)
                targetWeapon = m_Weapons.GetRandomWeaponByType(weaponType);
            else
                targetWeapon = m_Weapons.GetRandomWeapon();

            var weapon = PhotonNetwork.Instantiate(targetWeapon.name, m_Map.GetWeaponSpawnPosition().position, Quaternion.identity);
        }
    }
}
