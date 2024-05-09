using UnityEngine;
using Scripts.Player;
using Scripts.Bullets;
using Scripts.Interfaces;
using System.Collections;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Scripts.Weapons
{
    [RequireComponent(typeof(MouseRotationer))]
    [RequireComponent(typeof(CapsuleCollider2D), typeof(BoxCollider2D), typeof(Rigidbody2D))]
    public class WeaponBase : MonoBehaviour, ICollectable
    {
        [field: SerializeField] public bool IsCollectable { get; set; }

        public bool canShoot;
        public bool isReloading;

        public float shootRate;
        public Vector2 recoilValue;
        //public float hitValue;

        public float reloadRate;
        public int maxBulletCount;
        public int spareBulletsCount;
        public int currentBulletCount;

        public Vector3 throwForce;

        public new Rigidbody2D rigidbody2D;

        public BulletBase bulletPrefab;
        [Range(1, 5)] public int bulletCountOnShoot;
        [ShowIf("@bulletCountOnShoot == 1")] public Transform bulletSpawnPoint;
        [ShowIf("@bulletCountOnShoot > 1")] public List<Transform> bulletSpawnPoints;

        public ParticleSystem shootParticle;

        [HideInInspector] public PlayerBase player;
        [HideInInspector] public PlayerInput playerInput;
        [HideInInspector] public MouseRotationer mouseRotationer;

        public virtual void Start()
        {
            IsCollectable = true;
            mouseRotationer = GetComponent<MouseRotationer>();
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
            {
                if (bulletCountOnShoot == 1)
                {
                    var gunDirection = transform.right;
                    var spawnPosition = bulletSpawnPoint.position;
                    var spawnRotation = Quaternion.LookRotation(Vector3.forward, gunDirection);

                    SpawnBullet(gunDirection, spawnPosition, spawnRotation);
                }
                else if (bulletCountOnShoot > 1)
                {
                    foreach (var spawnPoint in bulletSpawnPoints)
                    {
                        var pointDirection = spawnPoint.right;
                        var spawnPosition = spawnPoint.position;
                        var spawnRotation = Quaternion.LookRotation(Vector3.forward, pointDirection);

                        SpawnBullet(pointDirection, spawnPosition, spawnRotation);
                    }
                }
            }

            if (shootParticle)
                shootParticle.Play();

            currentBulletCount--;

            yield return new WaitForSeconds(shootRate);

            canShoot = true;
        }

        public virtual void SpawnBullet(Vector3 direction, Vector3 spawnPosition, Quaternion spawnRotation)
        {
            var newBullet = Instantiate(bulletPrefab, spawnPosition, spawnRotation);

            var fireDirection = direction * newBullet.bulletSpeed;
            newBullet.FireBullet(fireDirection, recoilValue);
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
            mouseRotationer.enabled = false;
            transform.position = Vector3.zero;

            yield return new WaitForSeconds(1);

            IsCollectable = true;
        }

        public virtual void Collect()
        {
            IsCollectable = false;
            canShoot = true;
            mouseRotationer.enabled = true;
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
