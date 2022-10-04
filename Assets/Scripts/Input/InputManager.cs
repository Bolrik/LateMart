using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputManager : MonoBehaviour
    {
        private GameInput Input { get; set; }

        public InputState Move { get; private set; }
        public InputState Sprint { get; private set; }
        public InputState View { get; private set; }
        public InputButton Action { get; private set; }
        public InputButton AnyKey { get; private set; }
        public InputButton Back { get; private set; }

        InputStateUpdate InputStateUpdate { get; set; }

        private void Awake()
        {
            this.Input = new GameInput();
            this.Input.Enable();

            this.Move = this.Add(this.Input.Player.Move);
            this.Sprint = this.Add(this.Input.Player.Sprint);
            this.View = this.Add(this.Input.Player.View);

            this.Action = this.AddButton(this.Input.Player.Act);
            this.AnyKey = this.AddButton(this.Input.Player.KeyPress);

            this.Back = this.AddButton(this.Input.Player.Back);
        }

        private InputState Add(InputAction inputAction)
        {
            InputStateUpdate inputStateUpdate = this.InputStateUpdate;

            InputState toReturn = new InputState(inputAction, ref inputStateUpdate);

            this.InputStateUpdate = inputStateUpdate;

            return toReturn;
        }

        private InputButton AddButton(InputAction inputAction)
        {
            InputStateUpdate inputStateUpdate = this.InputStateUpdate;

            InputButton toReturn = new InputButton(inputAction, ref inputStateUpdate);

            this.InputStateUpdate = inputStateUpdate;

            return toReturn;
        }

        private void Update()
        {
            this.InputStateUpdate?.Invoke();
        }
    }

    public delegate void InputStateUpdate();


    public class InputState
    {
        public bool WasPressed { get => this.InputAction.WasPressedThisFrame(); }
        public bool WasReleased { get => this.InputAction.WasReleasedThisFrame(); }
        public bool IsPressed { get => this.InputAction.IsPressed(); }

        protected InputAction InputAction { get; private set; }

        public InputState(InputAction input, ref InputStateUpdate action)
        {
            this.InputAction = input;
        }

        public Vector2 GetVector2() => this.InputAction.ReadValue<Vector2>();

        public static implicit operator float(InputState inputState) => inputState.InputAction.ReadValue<float>();
    }

    public class InputButton : InputState
    {
        public float DownTime { get; private set; }
        public float UpTime { get; private set; }

        public InputButton(InputAction input, ref InputStateUpdate action)
            : base(input, ref action)
        {
            action += this.Update;
        }


        void Update()
        {
            if (this.IsPressed)
            {
                this.DownTime += Time.deltaTime;
                this.UpTime = 0;
            }
            else
            {
                this.DownTime = 0;
                this.UpTime += Time.deltaTime;
            }
        }
    }
}