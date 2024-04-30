using UnityEngine;

namespace Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Settings
        [Header("Settings")]
        [SerializeField] private float m_MovementSpeed;
        [SerializeField] private float m_JumpForce;
        [SerializeField] private float m_AirMovementSpeedDecreaseMultiplier;

        [Space]
        [SerializeField] private LayerMask m_GroundLayer;
        [SerializeField] private float m_RayDistance;
        #endregion

        #region Components
        private Rigidbody2D m_Rigidbody2D;
        private PlayerInput m_PlayerInput;
        #endregion

        #region Variables
        private bool m_CanHandleInput;
        private bool m_CanJump;
        private bool m_IsGrounded;
        #endregion

        #region Movement Directions
        private int m_RightDirection = 1;
        private int m_LeftDirection = -1;
        #endregion

        private void Start()
        {
            GetComponents();

            m_CanHandleInput = true;
        }

        private void Update()
        {
            if (!m_CanHandleInput) return;

            HandleInput();
            HandleGrounding();
        }

        public void SetHandlingInput(bool canHandleInput)
        {
            m_CanHandleInput = canHandleInput;
        }

        private void HandleGrounding()
        {
            var hit = Physics2D.Raycast(transform.position, Vector2.down, m_RayDistance, m_GroundLayer);
            if (hit.collider != null && hit.collider.CompareTag("Ground"))
            {
                m_CanJump = true;
                m_IsGrounded = true;
            }
            else
            {
                m_CanJump= false;
                m_IsGrounded = false;
            }
        }

        private void HandleInput()
        {
            if (m_PlayerInput.RightKey)
                Move(m_RightDirection);
            else if (m_PlayerInput.LeftKey)
                Move(m_LeftDirection);

            if (m_PlayerInput.JumpKey)
                Jump();

            if (m_PlayerInput.ThrowKey)
                Debug.Log("Throw");
        }

        private void Move(float direction)
        {
            if (!m_IsGrounded)
                direction /= m_AirMovementSpeedDecreaseMultiplier;

            transform.Translate(Vector3.right * direction * m_MovementSpeed * Time.deltaTime);
        }

        private void Jump()
        {
            if (m_CanJump && m_IsGrounded)
                m_Rigidbody2D.velocity = Vector3.up * m_JumpForce;
        }

        private void GetComponents()
        {
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_PlayerInput = GetComponent<PlayerInput>();
        }
    }
}
