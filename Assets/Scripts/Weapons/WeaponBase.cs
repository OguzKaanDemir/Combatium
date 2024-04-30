using UnityEngine;
using Scripts.Bullets;

namespace Scripts.Weapons
{
    public class WeaponBase : MonoBehaviour
    {
        public float shootRate;
        public float recoilValue;
        public float hitValue;

        public int magazineCount;
        public int bulletCount;

        public BulletBase bulletPrefab;

        public virtual void Start()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void OnEnable()
        {

        }

        public virtual void OnDisable()
        {

        }

        public virtual void OnTriggerEnter2D(Collider2D collision)
        {

        }

        public virtual void Shoot()
        {

        }

        public virtual void Throw()
        {

        }

        public virtual void Take()
        {

        }
    }
}
