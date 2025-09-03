using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class MovementInputHandler : MonoBehaviour
    {
        [HideInInspector] public InputAction moveAction;
        [HideInInspector] public InputAction jumpAction;

        private void Start()
        {
            moveAction = InputSystem.actions.FindAction("Move");
            jumpAction = InputSystem.actions.FindAction("Jump");
        }
    }
}
