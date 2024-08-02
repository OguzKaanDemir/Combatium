using UnityEngine;
using System.Linq;
using Scripts.Enums;
using Scripts.Weapons;

namespace Scripts.Data
{
    [CreateAssetMenu(fileName = "Weapons", menuName = "Datas/ Weapons")]
    public class Weapons : ScriptableObject
    {
        private static Weapons m_Ins;
        public static Weapons Ins
        {
            get
            {
                if (!m_Ins)
                    m_Ins = Resources.Load<Weapons>("Datas/Weapons");
                return m_Ins;
            }
        }

        public WeaponBase[] weapons;

        public WeaponBase GetRandomWeapon()
           => weapons[RandomIndex(0, weapons.Length)];

        public WeaponBase GetRandomWeaponByType(WeaponType type)
        {
            var filteredWeapons = weapons.Where(w => w.weaponType == type).ToArray();
            return filteredWeapons[RandomIndex(0, filteredWeapons.Length)];
        }

        private int RandomIndex(int min, int max)
            => Random.Range(min, max);
    }
}
