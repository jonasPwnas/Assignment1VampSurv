using System;
using Input;
using Tiny;
using UnityEngine;

namespace Player
{ 
    public class PlayerMovement : MonoBehaviour
    {
        //Animator parameters
        private static readonly int NormalAttack = Animator.StringToHash("Attack");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int IsGrounded = Animator.StringToHash("isGrounded");
        private static readonly int GravityIntensity = Animator.StringToHash("GravityIntensity");
        private static readonly int AttackMomentum = Animator.StringToHash("AttackMomentum");
        private static readonly int AttackTag = Animator.StringToHash("Attack");
        private static readonly int JumpTag = Animator.StringToHash("Jump");

        //Editor editable
        [Tooltip("The higher the value the higher we jump")] [SerializeField] private float jumpIntensity = 1.5f;
        [Tooltip("How fast we can jump after landing, in seconds")] [SerializeField] private float jumpFrequency = 0.4f;
        [Tooltip("Downward force intensity")] [SerializeField] private float gravityIntensity = -6f;
        [Tooltip("How fast the player rotates")] [SerializeField] private float rotationSpeed = 10f;
        [Tooltip("The camera used by the player")] [SerializeField] private Camera playerCamera;
        [SerializeField] Trail trail;
        
        //Privates
        private Animator m_animator;
        private MovementInputHandler m_movementInputHandler;
        private CharacterController m_characterController;
        private Vector3 m_playerVelocity;
        private bool m_groundedPlayer;
        private bool m_allowJump = true;
        private bool m_canAttack = true;
        private bool m_inputWindowOpen;
        private float m_accumulatedAttackTime;
        private float m_attackTimer;
        private float m_movementMultiplier = 0.5f;
        private float m_jumpTimer;
        
        //Read-only
        private readonly float m_attackInputFrequency = 0.25f;
        
        //Publics
        [HideInInspector] public Vector3 _moveVelocity;

        void Start()
        {
            m_movementInputHandler = GetComponent<MovementInputHandler>();
            m_characterController = GetComponent<CharacterController>();
            m_animator = GetComponent<Animator>();
            trail.enabled = false;
        }
        
        void Update()
        {
            Attack();
            Movement();
        }

        void Attack()
        {
            if (m_canAttack && m_inputWindowOpen && m_movementInputHandler.attackAction.IsPressed())
            {
                m_animator.SetTrigger(NormalAttack);
                m_accumulatedAttackTime = Time.time;
                m_attackTimer = m_accumulatedAttackTime + m_attackInputFrequency;
                m_inputWindowOpen = false;
                m_canAttack = false;
            }
            
            if (m_canAttack && m_movementInputHandler.attackAction.IsPressed())
            {
                m_animator.SetTrigger(NormalAttack);
                m_accumulatedAttackTime = Time.time;
                m_attackTimer = m_accumulatedAttackTime + m_attackInputFrequency;
                m_canAttack = false;
            }
            
            if (!m_canAttack)
            {
                m_accumulatedAttackTime += Time.deltaTime;
                if (m_accumulatedAttackTime >= m_attackTimer)
                {
                    m_canAttack = true;
                    m_attackTimer = 0f;
                    m_animator.ResetTrigger(NormalAttack);
                }
            }
        }
        
        void Movement()
        {
            m_groundedPlayer = m_characterController.isGrounded;

            if (m_groundedPlayer && !m_allowJump)
            {
                m_jumpTimer += Time.deltaTime;
                if(m_jumpTimer >= jumpFrequency)
                {
                    m_allowJump = true;
                }
            }

            if (!m_animator.IsInTransition(0))
            {
                if(m_animator.GetCurrentAnimatorStateInfo(0).tagHash == AttackTag)
                    return;
            }
            
            
            Vector2 input = m_movementInputHandler.moveAction.ReadValue<Vector2>();
            float horizontalInput = input.x;
            float verticalInput = input.y;
            Vector3 movementInput = Quaternion.Euler(0, playerCamera.transform.eulerAngles.y, 0) * new Vector3(horizontalInput, 0, verticalInput);
            Vector3 movementDirection = movementInput.normalized;
            
            if (m_movementInputHandler.sprintAction.IsPressed())
            {
                m_movementMultiplier = 1f;
            }
            else
            {
                m_movementMultiplier = 0.5f;
            }
            
            _moveVelocity = movementDirection * Time.deltaTime;
            m_animator.SetFloat(Speed, _moveVelocity.normalized.magnitude * m_movementMultiplier, 0.05f, Time.deltaTime);
            
            if (movementDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            
            if (m_groundedPlayer && m_playerVelocity.y < 0)
            {
                m_playerVelocity.y = 0f;
                m_animator.SetBool(IsGrounded, m_groundedPlayer);
            }
            
            if (m_allowJump && m_groundedPlayer&& m_movementInputHandler.jumpAction.IsPressed())
            {
                m_allowJump = false;
                m_jumpTimer = 0f;
                m_animator.SetTrigger(Jump);
            }
            
            if (m_animator.GetCurrentAnimatorStateInfo(0).tagHash != JumpTag)
            {
               m_playerVelocity.y += gravityIntensity * Time.deltaTime;
               m_characterController.Move(m_playerVelocity * Time.deltaTime); 
            }
            
            if (m_animator.GetCurrentAnimatorStateInfo(0).tagHash != AttackTag)
            {
                trail.enabled = false;
            }
        }

        private void OnAnimatorMove() //Animator movement - root movement
        {
            Vector3 velocity = m_animator.deltaPosition;
            if (m_animator.GetCurrentAnimatorStateInfo(0).tagHash == JumpTag)
            {
                gravityIntensity = m_animator.GetFloat(GravityIntensity);
                velocity.y += gravityIntensity * Time.deltaTime;
            }
            else
            {
                gravityIntensity = -8f;
            }

            if (m_animator.GetCurrentAnimatorStateInfo(0).tagHash == AttackTag)
            {
                velocity += transform.forward * m_animator.GetFloat(AttackMomentum) * Time.deltaTime;
                velocity.y += gravityIntensity * Time.deltaTime;
                trail.enabled = true;
            }
            
            m_characterController.Move(velocity);
        }
        
        //Called from animation events
        public void InputStart()
        {
            print("InputStart called from animation event");
            m_inputWindowOpen = true;
        }
        
        public void InputEnd()
        {
            print("InputEnd called from animation event");
            m_inputWindowOpen = false;
        }
    }
}