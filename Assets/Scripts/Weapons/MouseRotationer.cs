using UnityEngine;

namespace Scripts.Weapons
{
    public class MouseRotationer : MonoBehaviour
    {
        void Update()
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            var direction = mousePos - transform.position;

            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            var rotation = mousePos.x > transform.position.x ?
                 Quaternion.Euler(0, 0, angle) : Quaternion.Euler(180, 0, -angle);

            transform.rotation = rotation;
        }
    }
}
