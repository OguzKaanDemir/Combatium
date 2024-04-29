using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Data
{
    [CreateAssetMenu(fileName = "Player Settings", menuName = "Datas/ Player Settings")]
    public class PlayerSettings : ScriptableObject
    {
        public KeyCode 
            rightKey, leftKey, jumpKey, throwKey, shootKey;
    }
}
