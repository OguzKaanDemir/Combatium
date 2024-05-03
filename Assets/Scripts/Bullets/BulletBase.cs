using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Bullets
{
    public class BulletBase : MonoBehaviour
    {
        public virtual void SpawnBullet()
        {
            print("spawned");
        }
    }
}
