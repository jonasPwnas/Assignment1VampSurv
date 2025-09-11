using System;
using Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{ 
    public class PlayerMovement : MonoBehaviour
    {
        //Editor editable
        [SerializeField] private float movementSpeed = 10f;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private float jumpHeight = 1f;
        [SerializeField] private float gravityIntensity = -9f;
        [SerializeField] private Camera playerCamera;
        
        //Privates
        private MovementInputHandler _movementInputHandler;
        private CharacterController _characterController;
        private Vector3 m_playerVelocity;
        private bool m_groundedPlayer;
        
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
        }

        void Movement()
        {
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
            _moveVelocity = movementDirection * (movementSpeed * Time.deltaTime);
            
            _characterController.Move(_moveVelocity);

            if (movementDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            if (_movementInputHandler.jumpAction.IsPressed() && m_groundedPlayer)
            {
                m_playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityIntensity); //What should I call the magic number?
            }
            
            m_playerVelocity.y += gravityIntensity * Time.deltaTime;
            _characterController.Move(m_playerVelocity * Time.deltaTime);
        }
    }
}
