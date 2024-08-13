using UnityEngine;
using Scripts.Player;
using Photon.Pun;
using Smooth;
using System.Collections;

namespace Scripts.Bullets
{
    [RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
    [RequireComponent(typeof(PhotonView), typeof(SmoothSyncPUN2))]
    public class BulletBase : MonoBehaviourPun
    {
        public int damage;
        public float bulletSpeed;
        public float lifeTime;
        public new Rigidbody2D rigidbody2D;

        public virtual IEnumerator Start()
        {
            yield return new WaitForSeconds(lifeTime);
            DestroyBullet();
        }

        public virtual void OnTriggerEnter2D(Collider2D collision)
        {
            
            if (collision.TryGetComponent<PlayerCollider>(out var player))
            {
                if (photonView.Owner == player.player.photonView.Owner) return;
                DestroyBullet();
            }
            else if (!collision.GetComponent<BulletBase>() && !collision.GetComponent<PlayerCollider>())
            {
                print(collision.name);
                DestroyBullet();
            }
        }

        public virtual void FireBullet(Vector2 fireDirection, Vector2 recoilValue)
        {
            var r = Random.Range(recoilValue.x, recoilValue.y);
            var recoil = new Vector2(r, r);
            rigidbody2D.AddForce(fireDirection + recoil, ForceMode2D.Impulse);
        }

        public void DestroyBullet()
        {
            Destroy(gameObject);
        }
    }
}
