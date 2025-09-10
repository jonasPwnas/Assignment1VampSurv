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
        [SerializeField] private float gravityIntensity = 1;
        [SerializeField] private Camera playerCamera;
        
        //Privates
        private MovementInputHandler _movementInputHandler;
        private CharacterController _characterController;
        private Vector3 m_playerVelocity;
        private bool m_groundedPlayer;
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
                //Bla bla bla
            }
            
        }
    }
}
