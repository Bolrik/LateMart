using Input;
using Misc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody rigidbody;
        public Rigidbody Rigidbody { get { return rigidbody; } }

        [SerializeField] private InputManager input;
        public InputManager Input { get { return input; } }


        [SerializeField] private float moveSpeed;
        public float MoveSpeed { get { return moveSpeed; } }


        [SerializeField] private PlayerData data;
        public PlayerData Data { get { return data; } }





        [SerializeField] private PlayerViewController viewController;
        public PlayerViewController ViewController { get { return viewController; } }

        [SerializeField] private FlashlightController flashlight;
        public FlashlightController Flashlight { get { return flashlight; } }



        // Update is called once per frame
        void Update()
        {
            Vector2 input = this.Input.View.GetVector2() * this.Data.ViewSensitivity;

            this.ViewController.Update(input);

            input = this.Input.Move.GetVector2();
            this.Move(input);

            this.Flashlight.FlashlightLerpTransform.Mode = input.magnitude > .1f ? LerpTransformMode.Instant : LerpTransformMode.Lerp;
        }

        void Move(Vector2 moveInput)
        {
            Vector3 move = this.transform.forward * moveInput.y * this.MoveSpeed;
            Vector3 strafe = this.transform.right * moveInput.x * this.MoveSpeed;

            this.Rigidbody.velocity = Vector3.Lerp(this.Rigidbody.velocity, move + strafe, 1 / .3f * Time.deltaTime);
        }
    }

    [System.Serializable]
    public class PlayerViewController
    {
        [Header("References")]
        [SerializeField] private PlayerController player;
        public PlayerController Player { get { return player; } }

        [SerializeField] private Transform transform;
        public Transform Transform { get { return transform; } }

        [SerializeField] private Transform head;
        public Transform Head { get { return head; } }

        [Header("Settings")]
        [SerializeField] private float rotationSpeedX;
        public float RotationSpeedX { get { return rotationSpeedX; } }

        [SerializeField] private float rotationSpeedY;
        public float RotationSpeedY { get { return rotationSpeedY; } }

        [SerializeField] private Vector2 viewConstraint;
        public Vector2 ViewConstraint { get { return viewConstraint; } }

        [Header("Info")]
        [SerializeField] private float x;
        private float X { get { return x; } set { x = value; } }

        [SerializeField] private float y;
        private float Y { get { return y; } set { y = value; } }





        public void Update(Vector2 viewInput)
        {
            Vector3 rotation = this.Transform.transform.localEulerAngles;
            this.Y += viewInput.x * this.RotationSpeedY * Time.deltaTime;
            rotation.y = this.Y;
            this.Transform.transform.localEulerAngles = rotation;

            rotation = this.Head.transform.localEulerAngles;
            this.X -= viewInput.y * this.RotationSpeedX * Time.deltaTime;
            this.X = Mathf.Clamp(this.X, this.ViewConstraint.x, this.ViewConstraint.y);
            rotation.x = this.X;
            this.Head.transform.localEulerAngles = rotation;
        }
    }

    [System.Serializable]
    public class FlashlightController
    {
        [SerializeField] private LerpTransform flashlightLerpTransform;
        public LerpTransform FlashlightLerpTransform { get { return flashlightLerpTransform; } }

        [SerializeField] private GameObject flashlight;
        public GameObject gameObject { get { return flashlight; } }

    }
}