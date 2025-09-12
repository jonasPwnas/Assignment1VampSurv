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
        //Animator parameters
        private static readonly int NormalAttack = Animator.StringToHash("Attack");
        private static readonly int Attacking = Animator.StringToHash("Attacking");
        private static readonly int Jump = Animator.StringToHash("Jump");

        //Editor editable
        [SerializeField] private float walkSpeed = 6f;
        [SerializeField] private float gravityIntensity = -6f;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private Camera playerCamera;
        //[SerializeField] private float sprintSpeed = 9f;
        //[SerializeField] private float attackWaitTime = 1f;
        
        //Privates
        private Animator m_animator;
        private MovementInputHandler _movementInputHandler;
        private CharacterController _characterController;
        private Vector3 m_playerVelocity;
        private bool m_groundedPlayer;
        private bool m_isAttacking;
        private float m_accumulatedAttackTime;
        private float m_attackTimer;
        private float m_movementMultiplier = 0.5f;
        private RuntimeAnimatorController m_animatorController;
        
        [HideInInspector] public Vector3 _moveVelocity;

        void Start()
        {
            _movementInputHandler = GetComponent<MovementInputHandler>();
            _characterController = GetComponent<CharacterController>();
            m_animator = GetComponent<Animator>();
            //m_animatorController = animator.runtimeAnimatorController;
        }
        
        void Update()
        {
            Movement();
            Attack();
        }

        void Attack()
        {
            m_groundedPlayer = _characterController.isGrounded;
            
            if (_movementInputHandler.attackAction.IsPressed() && !m_isAttacking)
            {
                m_animator.SetTrigger(NormalAttack);
                m_animator.SetBool(Attacking, true);
                m_accumulatedAttackTime = Time.time;
                m_attackTimer = m_accumulatedAttackTime + 1.4f; //Don't we just love magic numbers? (approximate anim clip length)
                m_isAttacking = true;
            }
            
            if (_movementInputHandler.jumpAction.IsPressed() && m_groundedPlayer && !m_isAttacking)
            {
                print("JUMP INPUT YAAAS");
                m_animator.SetTrigger(Jump);
                m_animator.SetBool(Attacking, true);
                m_accumulatedAttackTime = Time.time;
                m_attackTimer = m_accumulatedAttackTime + 2.1f; //Don't we just love magic numbers? (approximate anim clip length)
                m_isAttacking = true;
            }

            if (m_isAttacking)
            {
                m_accumulatedAttackTime += Time.deltaTime;
                if (m_accumulatedAttackTime >= m_attackTimer)
                {
                    m_animator.SetBool(Attacking, false);
                    m_isAttacking = false;
                    print("STOPPED ATTACKING!!!!!!");
                }
            }
        }
        void Movement()
        {
            if(m_isAttacking)
                return;
            
            Vector2 input = _movementInputHandler.moveAction.ReadValue<Vector2>();
            
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
            m_animator.SetFloat("Speed", _moveVelocity.normalized.magnitude * m_movementMultiplier, 0.05f, Time.deltaTime);
            
            if (movementDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            
        }
        
        private void OnAnimatorMove()
        {
            //Animator movement
            Vector3 velocity = m_animator.deltaPosition;
            if(!m_isAttacking)
                velocity.y += gravityIntensity * Time.deltaTime;
            
            _characterController.Move(velocity);
        }
    }
}
