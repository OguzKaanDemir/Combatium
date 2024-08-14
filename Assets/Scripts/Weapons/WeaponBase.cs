using Photon.Pun;
using UnityEngine;
using Scripts.Enums;
using Scripts.Player;
using Scripts.Bullets;
using Scripts.Interfaces;
using System.Collections;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Scripts.Weapons
{
    [RequireComponent(typeof(MouseRotationer), typeof(PhotonView), typeof(PhotonTransformView))]
    [RequireComponent(typeof(CapsuleCollider2D), typeof(BoxCollider2D), typeof(Rigidbody2D))]
    public class WeaponBase : MonoBehaviourPun, ICollectable
    {
        [field: SerializeField] public bool IsCollectable { get; set; }

        public WeaponType weaponType;

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

        private CapsuleCollider2D m_Collider;

        public virtual IEnumerator Start()
        {
            m_Collider = GetComponent<CapsuleCollider2D>();
            mouseRotationer = GetComponent<MouseRotationer>();

            yield return new WaitForSeconds(0.5f);

            SetIsCollectable(true);
        }

        public virtual void Update()
        {
            if (!IsCollectable && playerInput)
            {
                transform.localPosition = Vector3.zero;

                if (playerInput.ShootKey)
                    photonView.RPC(nameof(Shoot), RpcTarget.AllBuffered);
                else if (playerInput.ReloadKey)
                    photonView.RPC(nameof(Reload), RpcTarget.AllBuffered);
                else if (playerInput.ThrowKey)
                    photonView.RPC(nameof(Throw), RpcTarget.AllBuffered);
            }
        }

        public virtual void OnTriggerEnter2D(Collider2D collider)
        {
            if (IsCollectable && collider.TryGetComponent<PlayerBase>(out var player))
            {
                if (player.CollectWeapon(this))
                {
                    photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
                    photonView.RPC(nameof(Collect), RpcTarget.AllBuffered);
                    mouseRotationer.enabled = true;
                }
            }
        }

        #region Shoot

        [PunRPC]
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
                    ShootActions(transform, bulletSpawnPoint);

                else if (bulletCountOnShoot > 1)
                {
                    foreach (var spawnPoint in bulletSpawnPoints)
                        ShootActions(spawnPoint, spawnPoint);
                }
            }

            if (shootParticle)
                shootParticle.Play();

            currentBulletCount--;

            yield return new WaitForSeconds(shootRate);

            canShoot = true;
        }

        public virtual void ShootActions(Transform directionTranfsorm, Transform spawnPointTransform)
        {
            if (player.photonView.Owner != PhotonNetwork.LocalPlayer) return;

            var properties = SetBulletProperties(directionTranfsorm, spawnPointTransform);

            var bullet = SpawnBullet(properties.spawnPosition, properties.spawnRotation);

            FireBullet(bullet, properties.direction);
        }

        public virtual (Vector3 direction, Vector3 spawnPosition, Quaternion spawnRotation) SetBulletProperties(Transform directionTranfsorm, Transform spawnPointTransform)
        {
            var pointDirection = directionTranfsorm.right;
            var spawnPosition = spawnPointTransform.position;
            var spawnRotation = Quaternion.LookRotation(Vector3.forward, pointDirection);

            return (pointDirection, spawnPosition, spawnRotation);
        }

        public virtual BulletBase SpawnBullet(Vector3 spawnPosition, Quaternion spawnRotation)
        {
            return PhotonNetwork.Instantiate(bulletPrefab.name, spawnPosition, spawnRotation).GetComponent<BulletBase>();
        }

        public virtual void FireBullet(BulletBase bullet, Vector3 direction)
        {
            var fireDirection = direction * bullet.bulletSpeed;
            bullet.FireBullet(fireDirection, recoilValue);
        }

        #endregion

        #region Reload

        [PunRPC]
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
        #endregion

        #region Throw
        [PunRPC]
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
            SetIsCollectable(false);

            if (photonView.Owner == PhotonNetwork.LocalPlayer)
                rigidbody2D.AddForce(CalculateThrowPosition(), ForceMode2D.Impulse);

            player.ThrowWeapon();
            player = null;
            playerInput = null;
            mouseRotationer.enabled = false;
            m_Collider.enabled = true;

            yield return new WaitForSeconds(2);

            SetIsCollectable(true);
        }

        public Vector3 CalculateThrowPosition()
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var playerPosition = playerInput.gameObject.transform.position;
            var throwDirection = mousePosition.x > playerPosition.x ? 1 : -1;
            return new Vector3(throwDirection * throwForce.x, throwForce.y, 0);
        }
        #endregion

        [PunRPC]
        public virtual void Collect()
        {
            IsCollectable = false;
            canShoot = true;
            m_Collider.enabled = false;
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

        public virtual void SetIsCollectable(bool isCollectable)
        {
            photonView.RPC(nameof(SetIsCollectableRPC), RpcTarget.AllBuffered, isCollectable);
        }

        [PunRPC]
        public virtual void SetIsCollectableRPC(bool isCollectable)
        {
            IsCollectable = isCollectable;
        }
    }
}
