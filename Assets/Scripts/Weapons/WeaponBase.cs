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

        public Vector3 throwForce;

        public new Rigidbody2D rigidbody2D;

        public BulletBase bulletPrefab;

        public ParticleSystem shootParticle;

        [HideInInspector] public PlayerBase player;
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
                else if (playerInput.ThrowKey)
                    Throw();
            }
        }

        public virtual void OnTriggerEnter2D(Collider2D collider)
        {
            if (IsCollectable && collider.TryGetComponent<PlayerBase>(out var player))
            {
                if (player.CollectWeapon(this))
                {
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
            if (!isReloading)
                StartCoroutine(ThrowCrt());
        }

        public virtual IEnumerator ThrowCrt()
        {
            transform.parent = null;
            rigidbody2D.isKinematic = false;

            canShoot = false;
            IsCollectable = false;

            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var playerPosition = playerInput.gameObject.transform.position;
            var throwDirection = mousePosition.x > playerPosition.x ? 1 : -1;
            var throwPosition = new Vector3(throwDirection * throwForce.x, throwForce.y, 0);

            rigidbody2D.AddForce(throwPosition, ForceMode2D.Impulse);

            player.ThrowWeapon();
            player = null;
            playerInput = null;

            yield return new WaitForSeconds(1);

            IsCollectable = true;
        }

        public virtual void Collect()
        {
            IsCollectable = false;
            canShoot = true;
        }

        public void SetPlayerComponents(PlayerBase player, PlayerInput input)
        {
            this.player = player;
            playerInput = input;
        }

        public Rigidbody2D GetRigidbody2D()
        {
            return GetComponent<Rigidbody2D>();
        }
    }
}
