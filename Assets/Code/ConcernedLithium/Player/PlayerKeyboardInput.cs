using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConcernedLithium
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerKeyboardInput : MonoBehaviour
    {
        public Vector2 MouseSensitivity = new Vector2(1.0f, 1.0f);
        public bool InvertMouseY = false;
        public bool InvertMouseX = false;

        private PlayerController _controller;

        private void Start() {
            _controller = GetComponent<PlayerController>();
        }

        private void Update() {
            // Get movement inputs.
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // Get look inputs.
            float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity.x * (InvertMouseX ? -1 : 1);
            float mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity.y * (InvertMouseY ? 1 : -1);

            // Get jump input.
            bool jump = Input.GetKeyDown(KeyCode.Space);

            // Get walk/run input.
            if (Input.GetKey(KeyCode.LeftShift)) {
                _controller.Run();
            } else {
                _controller.Walk();
            }

            Vector3 inputDirection = new Vector3(horizontal, 0, vertical).normalized;

            // Send inputs to controller.
            _controller.Rotate(mouseY, mouseX, 0);
            _controller.Move(inputDirection);
            if (jump) {
                _controller.Jump();
            }
        }
    }
}

