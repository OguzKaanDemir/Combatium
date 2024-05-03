using UnityEngine;
using Scripts.Player;
using Scripts.Bullets;
using Scripts.Interfaces;
using System.Collections;

namespace Scripts.Weapons
{
    public class WeaponBase : MonoBehaviour, ICollectable
    {
        [field: SerializeField] public bool IsCollectable { get; set; }

        public bool canShoot;
        public bool isReloading;

        public float shootRate;
        public float recoilValue;
        public float hitValue;

        public float reloadRate;
        public int maxBulletCount;
        public int spareBulletsCount;
        public int currentBulletCount;

        public new Rigidbody2D rigidbody2D;

        public BulletBase bulletPrefab;

        public ParticleSystem shootParticle;

        [HideInInspector] public PlayerInput playerInput;

        public virtual void Start()
        {
            IsCollectable = true;
        }

        public virtual void Update()
        {
            if (!IsCollectable && playerInput)
            {
                if (playerInput.ShootKey)
                    Shoot();
                else if (playerInput.ReloadKey)
                    Reload();
            }
        }

        public virtual void OnEnable()
        {

        }

        public virtual void OnDisable()
        {

        }

        public virtual void OnTriggerEnter2D(Collider2D collider)
        {
            print(collider.name);
            if (IsCollectable && collider.TryGetComponent<PlayerBase>(out var player))
            {
                print("ss");
                if (player.CollectWeapon(this))
                {
                    print("aa");
                    Collect();
                }
            }
        }

        public virtual void Shoot()
        {
            if (!(!canShoot || currentBulletCount <= 0))
                StartCoroutine(ShootCrt());
        }

        public virtual IEnumerator ShootCrt()
        {
            canShoot = false;

            if (bulletPrefab)
                bulletPrefab.SpawnBullet();

            if (shootParticle)
                shootParticle.Play();

            currentBulletCount--;

            yield return new WaitForSeconds(shootRate);

            canShoot = true;
        }

        public virtual void Reload()
        {
            if (!isReloading && currentBulletCount != maxBulletCount && spareBulletsCount > 0)
                StartCoroutine(ReloadCrt());
        }

        public virtual IEnumerator ReloadCrt()
        {
            canShoot = false;
            isReloading = true;

            yield return new WaitForSeconds(reloadRate);

            var diff = maxBulletCount - currentBulletCount;
            if (diff > spareBulletsCount)
            {
                currentBulletCount += spareBulletsCount;
                spareBulletsCount = 0;
            }
            else
            {
                currentBulletCount = maxBulletCount;
                spareBulletsCount -= diff;
            }

            canShoot = true;
            isReloading = false;
        }

        public virtual void Throw()
        {
            transform.parent = null;
            rigidbody2D.isKinematic = false;
            // more
        }

        public virtual void Collect()
        {
            IsCollectable = false;
            canShoot = true;
        }

        public void SetPlayerInputComponent(PlayerInput input)
        {
            playerInput = input;
        }

        public Rigidbody2D GetRigidbody2D()
        {
            return GetComponent<Rigidbody2D>();
        }
    }
}
