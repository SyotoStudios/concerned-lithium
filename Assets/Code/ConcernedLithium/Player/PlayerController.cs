using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConcernedLithium
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Components")]
        public Transform CamProxy;
        public CapsuleCollider Collider;
        public Rigidbody RB;

        [Header("Settings")]
        public float WalkSpeed = 24.0f;
        public float RunSpeed = 36.0f;
        public float JumpPower = 4.0f;
        public float FrictionMultiplier = 1.0f;
        public bool DoOverrideFriction = false;
        public float OverrideFriction = 10f;
        public Vector3 Gravity = Physics.gravity;
        public float GroundCheckThreshold = 0.1f;
        public float GroundCheckRadius = 0.5f;

        [Header("Debug Info")]
        public bool Grounded;
        public Vector3 Velocity;
        public float Speed;
        public float Friction;

        private Camera _cam;
        private bool _justHitGround;
        private float _camXRotation = 0.0f;

        public void Start() {
            // Locate the camera. Probably in Level Container.
            _cam = Camera.main;
        }

        public void Rotate(float x, float y, float z) {
            // Accumulate X axis rotation for camera.
            _camXRotation += x;
            // Update camera's rotation
            CamProxy.transform.localRotation = Quaternion.Euler(_camXRotation, 0, 0);

            // Rotate player along its Up axis.
            transform.Rotate(transform.up, y);
        }

        public void Move(Vector3 inputDirection) {
            // Normalize to move constant speed in all directions.
            inputDirection = inputDirection.normalized;

            // Accelerate in direction of input at speed.
            Velocity += transform.rotation * inputDirection * Speed * Time.deltaTime;
        }

        public void Run() {
            Speed = RunSpeed;
        }

        public void Walk() {
            Speed = WalkSpeed;
        }

        public void Jump() {
            // Add jump velocity if player can jump.
            if (Grounded) {
                Velocity.y += JumpPower;
            }
        }

        private void DoFriction() {
            // Ignore Y velocity to not affect gravity.
            Vector3 horizontalVelocity = new Vector3(Velocity.x, 0, Velocity.z);

            // Allow overriding a surface's friction.
            float friction = (DoOverrideFriction ? OverrideFriction : Friction) * FrictionMultiplier;

            // Apply the friction.
            Velocity -= horizontalVelocity * friction * Time.deltaTime;
        }

        private void DoGravity() {
            // Player is in the air, accelerate player with gravity.
            if (!Grounded) {
                Velocity += Gravity * Time.deltaTime;
            }

            // Check if the player just hit the ground this frame.
            if (_justHitGround) {
                // Clear the built-up velocity from gravity.
                Velocity.y = 0;
                // Reset flag.
                _justHitGround = false;
            }
        }

        private void Update() {
            SyncCamera();

            CheckGround();

            DoFriction();

            DoGravity();

            RB.velocity = Velocity;
        }

        // Move and rotate the camera to match the proxy.
        private void SyncCamera() {
            _cam.transform.position = CamProxy.position;
            _cam.transform.rotation = CamProxy.rotation;
        }

        private void CheckGround() {
            // Cast a sphere to just under the player's feet.
            Ray ray = new Ray(transform.position, -transform.up);
            RaycastHit hit;
            if (Physics.SphereCast(ray, GroundCheckRadius, out hit, Collider.bounds.extents.y - GroundCheckRadius + GroundCheckThreshold)) {
                if (!Grounded) {
                    // Get the surfaces friction.
                    // Set the flag for the player just now hitting the ground this frame.
                    Friction = hit.collider.material.dynamicFriction;
                    _justHitGround = true;
                }
                Grounded = true;
            } else {
                Grounded = false;
            }
        }
    }
}
