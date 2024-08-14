using UnityEngine;

namespace Scripts.Weapons
{
    public class Kalashnikov : WeaponBase
    {
        public override void Update()
        {
            base.Update();
        }

        public override void OnTriggerEnter2D(Collider2D collider)
        {
            base.OnTriggerEnter2D(collider);
        }

        public override void SetIsCollectable(bool isCollectable)
        {
            base.SetIsCollectable(isCollectable);
        }
    }
}
