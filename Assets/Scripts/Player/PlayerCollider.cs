using UnityEngine;
using Scripts.Enums;

namespace Scripts.Player
{
    public class PlayerCollider : MonoBehaviour
    {
        public bool isLocalPlayer;
        public BodyPart bodyPart;
        public PlayerBase player;

        private void Start()
        {
            isLocalPlayer = true;
        }
    }
}
