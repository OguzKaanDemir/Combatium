using UnityEngine;

namespace Scripts.Bullets
{
    [RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
    public class BulletBase : MonoBehaviour
    {
        public float bulletSpeed;
        public new Rigidbody2D rigidbody2D;

        public virtual void FireBullet(Vector2 fireDirection, Vector2 recoilValue)
        {
            var r = Random.Range(recoilValue.x, recoilValue.y);
            var recoil = new Vector2(r, r);
            rigidbody2D.AddForce(fireDirection + recoil, ForceMode2D.Impulse);
        }
    }
}
