using Smooth;
using Photon.Pun;
using UnityEngine;
using Scripts.Weapons;

namespace Scripts.Player
{
    [RequireComponent(typeof(PlayerInput), typeof(PlayerController), typeof(Rigidbody2D))]
    [RequireComponent(typeof(PhotonView), typeof(SmoothSyncPUN2))]
    public class PlayerBase : MonoBehaviourPun
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

            photonView.RPC(nameof(CollectWeaponRPC), RpcTarget.AllBuffered, weapon.photonView.ViewID);

            return true;
        }

        [PunRPC]
        public virtual void CollectWeaponRPC(int viewID)
        {
            m_CurrentWeapon = PhotonView.Find(viewID).GetComponent<WeaponBase>();
            m_CurrentWeapon.GetRigidbody2D().isKinematic = true;

            m_CurrentWeapon.transform.parent = m_WeaponParent;
            m_CurrentWeapon.transform.localPosition = Vector3.zero;
            m_CurrentWeapon.SetPlayerComponents(this, m_PlayerInput);
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
