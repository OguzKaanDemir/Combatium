using UnityEngine;
using Scripts.Data;

namespace Scripts.Player
{
    public class PlayerInput : MonoBehaviour
    {
        #region Keys
        public bool RightKey
        {
            get =>
                Input.GetKey(m_PlayerSettings.rightKey);
        }

        public bool LeftKey
        {
            get =>
                Input.GetKey(m_PlayerSettings.leftKey);
        }

        public bool JumpKey
        {
            get =>
                Input.GetKey(m_PlayerSettings.jumpKey);
        }

        public bool ThrowKey
        {
            get =>
                Input.GetKey(m_PlayerSettings.throwKey);
        }

        public bool ShootKey
        {
            get =>
                Input.GetKey(m_PlayerSettings.shootKey);
        }

        public bool ReloadKey
        {
            get =>
                Input.GetKey(m_PlayerSettings.reloadKey);
        }
        #endregion

        #region Data
        private PlayerSettings m_PlayerSettings;
        #endregion

        private void Start()
        {
            m_PlayerSettings = Resources.Load<PlayerSettings>("Datas/Player Settings");
        }
    }
}
 