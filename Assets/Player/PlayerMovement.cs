using System;
using Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{ 
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float movementSpeed = 10f;
        
        //Privates
        private InputHandler _inputHandler;
        private CharacterController _characterController;
        private void OnEnable()
        {
            print("OnEnable");
        }

        private void Awake()
        {
            print("OnAwake");
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            print("OnStart");
            _inputHandler = GetComponent<InputHandler>();
            _characterController = GetComponent<CharacterController>();
        }
        
        // Update is called once per frame
        void Update()
        {
            Vector2 input = _inputHandler.moveAction.ReadValue<Vector2>();
            Vector3 movement = new Vector3(input.x, 0, input.y);
            _characterController.Move(movement * (movementSpeed * Time.deltaTime));
        }
    }
}
