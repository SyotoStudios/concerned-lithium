using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConcernedLithium
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Settings")]
        public Camera PlayerCamera;
        public Transform CameraProxy;
        public CapsuleCollider Collider;
        public Rigidbody RB;
        public Vector3 Gravity = Physics.gravity;
        public float GroundCheckDistance = 2.0f;
        public float GroundCheckThreshold = 0.05f;
        public Vector2 MouseSensitivity = new Vector2(1.0f, 1.0f);
        public float WalkSpeed = 8.0f;
        public float SurfaceFrictionMultiplier = 1.0f;

        [Header("Debug Info")]
        public bool Grounded;
        public float SurfaceFriction;
        public Vector3 Velocity;
        public float LookRotationX;
        public float LookRotationZ;
        //public Quaternion CameraRotation;
        public Quaternion PlayerRotation;

        private float _sphereCastRadius = 0.5f;
        private float _horizontal;
        private float _vertical;
        private float _mouseX;
        private float _mouseY;

        private void Start() {
            if (!PlayerCamera) PlayerCamera = Camera.main;
            if (!RB) RB = GetComponent<Rigidbody>();
            _sphereCastRadius = Collider.radius;
        }

        private void Update() {
            CollectInputs();
            GroundCheck();

            // Calculate rotation
            LookRotationX += _mouseY;
            LookRotationX = Mathf.Clamp(LookRotationX, -80, 80);

            // Apply rotation
            transform.Rotate(transform.up, _mouseX);
            CameraProxy.transform.localRotation = Quaternion.Euler(LookRotationX, 0, 0);

            // Calculate movement direction
            Vector3 newVelocity = new Vector3(0, 0, 0);
            Vector3 inputVelocityDirection = new Vector3(_horizontal, 0, _vertical).normalized;
            Vector3 inputVelocity = inputVelocityDirection * WalkSpeed;

            newVelocity += transform.rotation * inputVelocity;

            // Calculate gravity
            if (!Grounded) {
                newVelocity += Gravity;
            }

            // Apply friction
            //newVelocity = newVelocity - (RB.velocity.normalized * (SurfaceFriction * SurfaceFrictionMultiplier));

            // Save velocity
            Velocity = newVelocity;

            // Update camera
            SyncCameraProxy();
        }

        private void FixedUpdate() {
            RB.velocity = Velocity;
        }

        private void CollectInputs() {
            _horizontal = Input.GetAxis("Horizontal");
            _vertical = Input.GetAxis("Vertical");

            _mouseX = Input.GetAxis("Mouse X") * MouseSensitivity.x;
            _mouseY = -Input.GetAxis("Mouse Y") * MouseSensitivity.y;

        }

        private void GroundCheck() {
            Vector3 castOrigin = Collider.bounds.center - new Vector3(0, (Collider.height / 2.0f) - _sphereCastRadius, 0);

            RaycastHit hitInfo;
            if (Physics.SphereCast(transform.position, _sphereCastRadius, Gravity.normalized, out hitInfo, GroundCheckDistance)) {
                if (hitInfo.distance <= GroundCheckThreshold + _sphereCastRadius) {
                    Grounded = true;
                    SurfaceFriction = hitInfo.collider.material.dynamicFriction;
                } else {
                    Grounded = false;
                    SurfaceFriction = 0.0f;
                }
            } else {
                Grounded = false;
                SurfaceFriction = 0.0f;
            }
        }

        private void SyncCameraProxy() {
            PlayerCamera.transform.position = CameraProxy.position;
            PlayerCamera.transform.rotation = CameraProxy.rotation;
        }
    }
}
