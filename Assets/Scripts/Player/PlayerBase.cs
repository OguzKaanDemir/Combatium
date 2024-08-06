using UnityEngine;
using Scripts.Weapons;
using Photon.Pun;

namespace Scripts.Player
{
    [RequireComponent(typeof(PlayerInput), typeof(PlayerController), typeof(Rigidbody2D))]
    [RequireComponent(typeof(PhotonView), typeof(PhotonTransformView), typeof(PhotonRigidbody2DView))]
    public class PlayerBase : MonoBehaviour
    {
        [SerializeField] private Transform m_WeaponParent;

        private PlayerInput m_PlayerInput;
        private PlayerController m_PlayerController;
        private Rigidbody2D m_Rigidbody2D;

        private WeaponBase m_CurrentWeapon;

        private void Start()
        {
            GetComponents();
            SetComponents();
        }

        public virtual bool CollectWeapon(WeaponBase weapon)
        {
            if (m_CurrentWeapon) return false;

            weapon.GetRigidbody2D().isKinematic = true;

            weapon.transform.parent = m_WeaponParent;
            weapon.transform.localPosition = Vector3.zero;
            weapon.SetPlayerComponents(this, m_PlayerInput);

            m_CurrentWeapon = weapon;

            return true;
        }

        public virtual void ThrowWeapon()
        {
            m_CurrentWeapon = null;
        }

        public virtual void GetComponents()
        {
            m_PlayerInput = GetComponent<PlayerInput>();
            m_PlayerController = GetComponent<PlayerController>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public virtual void SetComponents()
        {
            m_PlayerController.SetComponents(m_Rigidbody2D, m_PlayerInput);
        }

        public PlayerInput GetPlayerInputComponent()
        {
            return m_PlayerInput;
        }
    }
}
