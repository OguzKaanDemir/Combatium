using UnityEngine;
using Scripts.Player;

namespace Scripts.Bullets
{
    [RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
    public class BulletBase : MonoBehaviour
    {
        public int damage;
        public float bulletSpeed;
        public float lifeTime;
        public new Rigidbody2D rigidbody2D;

        public virtual void Start()
        {
            DestroyBulletByTime(lifeTime);
        }

        public virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<PlayerCollider>(out var player))
            {
                if (!player.isLocalPlayer)
                {
                    // damage actions
                    Destroy(gameObject);
                }
            }
            else if (!collision.GetComponent<BulletBase>())
            {
                Destroy(gameObject);
            }
        }

        public virtual void FireBullet(Vector2 fireDirection, Vector2 recoilValue)
        {
            var r = Random.Range(recoilValue.x, recoilValue.y);
            var recoil = new Vector2(r, r);
            rigidbody2D.AddForce(fireDirection + recoil, ForceMode2D.Impulse);
        }

        public void DestroyBulletByTime(float time)
        {
            Destroy(gameObject, time);
        }
    }
}
