using System;
using Input;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations;

namespace Player
{ 
    public class PlayerMovement : MonoBehaviour
    {
        //Animation parameters
        private static readonly int Slash1 = Animator.StringToHash("Slash1");
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Sprint = Animator.StringToHash("Sprint");

        //Editor editable
        [SerializeField] private float walkSpeed = 6f;
        [SerializeField] private float sprintSpeed = 9f;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private float jumpHeight = 1f;
        [SerializeField] private float gravityIntensity = -9f;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private Animator _animator;
        [SerializeField] private float AttackWaitTime = 1f;
        
        //Privates
        private MovementInputHandler _movementInputHandler;
        private CharacterController _characterController;
        private Vector3 m_playerVelocity;
        private bool m_groundedPlayer;
        private bool m_isAttacking = false;
        private float m_accumulatedAttackTime = 0f;
        private float m_attackTimer;
        private float m_movementMultiplier = 0.5f;
        
        [HideInInspector] public Vector3 _moveVelocity;
        
        private void OnEnable()
        {
            print("OnEnable");
        }

        private void Awake()
        {
            print("OnAwake");
        }

        void Start()
        {
            print("OnStart");
            _movementInputHandler = GetComponent<MovementInputHandler>();
            _characterController = GetComponent<CharacterController>();
        }
        
        void Update()
        {
            Movement();
            Attack();
        }

        void Attack()
        {
            if (_movementInputHandler.attackAction.IsPressed() && !m_isAttacking)
            {
                _animator.applyRootMotion = true;
                m_isAttacking = true;
                _animator.SetTrigger("Slash1");
                _animator.SetBool("Attacking", true);
                m_accumulatedAttackTime = Time.time;
                m_attackTimer = m_accumulatedAttackTime + AttackWaitTime;
            }

            if (m_isAttacking)
            {
                m_accumulatedAttackTime += Time.deltaTime;
                if (m_accumulatedAttackTime >= m_attackTimer)
                {
                    _animator.applyRootMotion = false;
                    
                    m_isAttacking = false;
                    _animator.SetBool("Attacking", false);
                    print("NOT ATTACKING!!!!!!");
                }
            }
        }
        void Movement()
        {
            if(m_isAttacking)
                return;
            
            Vector2 input = _movementInputHandler.moveAction.ReadValue<Vector2>();
            m_groundedPlayer = _characterController.isGrounded;
            
            if (m_groundedPlayer && m_playerVelocity.y < 0f)
            {
                m_playerVelocity.y = 0f;
            }
            
            float horizontalInput = input.x;
            float verticalInput = input.y;
            
            Vector3 movementInput = Quaternion.Euler(0, playerCamera.transform.eulerAngles.y, 0) * new Vector3(horizontalInput, 0, verticalInput);
            Vector3 movementDirection = movementInput.normalized;
            
            float speed = walkSpeed;
            if (_movementInputHandler.sprintAction.IsPressed())
            {
                m_movementMultiplier = 1f;
            }
            else
            {
                m_movementMultiplier = 0.5f;
            }
            
            _moveVelocity = movementDirection * (speed * Time.deltaTime);
            _animator.SetFloat("Speed", _moveVelocity.normalized.magnitude * m_movementMultiplier, 0.05f, Time.deltaTime);
            
            if (movementDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            if (_movementInputHandler.jumpAction.IsPressed() && m_groundedPlayer)
            {
                m_playerVelocity.y = Mathf.Sqrt(jumpHeight * -0.1f * gravityIntensity); //What should I call the magic number?
            }
            
            m_playerVelocity.y += gravityIntensity * Time.deltaTime;
            //_characterController.Move(m_playerVelocity * Time.deltaTime);
        }
        
        private void OnAnimatorMove()
        {
            //Animator movement
            Vector3 velocity = _animator.deltaPosition;
            velocity.y = m_playerVelocity.y;
            _characterController.Move(velocity);
        }
    }
}
