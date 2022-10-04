using Game;
using Game.Targets;
using Input;
using Misc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UIElements;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody rigidbody;
        public Rigidbody Rigidbody { get { return rigidbody; } }

        [SerializeField] private InputManager input;
        public InputManager Input { get { return input; } }

        [SerializeField] private ResetAnchor anchor;
        public ResetAnchor Anchor { get { return anchor; } }


        [SerializeField] private float speed;
        public float Speed { get { return speed; } }

        [SerializeField] private float sprintSpeed;
        public float SprintSpeed { get { return sprintSpeed; } }

        [SerializeField] private float acceleration;
        public float Acceleration { get { return acceleration; } }

        private Vector2 MoveInput { get; set; }




        [SerializeField] private PlayerData data;
        public PlayerData Data { get { return data; } }

        [SerializeField] private Target currentTarget;
        public Target CurrentTarget { get { return currentTarget; } private set { this.currentTarget = value; } }




        [SerializeField] private PlayerViewController viewController;
        public PlayerViewController ViewController { get { return viewController; } }

        [SerializeField] private FlashlightController flashlight;
        public FlashlightController Flashlight { get { return flashlight; } }

        [SerializeField] private PlayerUIController uiController;
        public PlayerUIController UIController { get { return uiController; } }

        [SerializeField] private ScreenOverlayController screenOverlayController;
        public ScreenOverlayController ScreenOverlayController { get { return screenOverlayController; } }

        [SerializeField] private SceneLoaderProxy sceneLoader;
        public SceneLoaderProxy SceneLoader { get { return sceneLoader; } }

        [SerializeField] private AudioSource audio;
        public AudioSource Audio { get { return audio; } }

        [SerializeField] private AudioClip hurtClip;
        public AudioClip HurtClip { get { return hurtClip; } }

        [SerializeField] private AudioSource areaAudio;
        public AudioSource AreaAudio { get { return areaAudio; } }

        [SerializeField] private AudioClip[] areaClips;
        public AudioClip[] AreaClips { get { return areaClips; } }




        float Sprint { get; set; } = 100;
        bool IsSprint { get; set;}
        float SprintCooldown { get; set; }

        int HitPoint { get; set; }
        int HitPointMax { get; set; } = 3;

        float AreaSoundTime { get; set; }

        private void Awake()
        {
            this.HitPoint = this.HitPointMax;
        }
        // Update is called once per frame
        void Update()
        {
            this.AreaSoundTime += Time.deltaTime;
            if (this.AreaSoundTime > 30)
            {
                this.PlayRandomAreaSound();
            }

            this.UpdateMoveInput();
            this.UpdateSprint();
            this.UpdateMoveAndView();
            this.UpdateFlashlight();

            this.CheckForTarget();
            string interactionText = null;

            if (this.CurrentTarget != null)
            {
                interactionText = DialogCatalogue.Get(this.Data.Language, DialogCatalogue.DialogLine.Player_Interaction).Aggregate("", (a, b) => a + b);
                if (this.Input.Action.WasPressed)
                {
                    this.CurrentTarget.Activate(this);
                    this.CurrentTarget = null;
                }
            }

            this.UIController.Label_Interaction.SetText(interactionText);
        }

        private void PlayRandomAreaSound()
        {
            this.AreaSoundTime = UnityEngine.Random.Range(0, 8);

            this.AreaAudio.PlayOneShot(this.AreaClips[UnityEngine.Random.Range(0, this.AreaClips.Length)]);
        }

        public void Win()
        {
            this.Anchor.enabled = false;
            this.UIController.AddDialog(new DialogItem(new[] { "GOOD JOB, you can leave now" }, 5, .2f));
        }

        public void Damage()
        {
            this.HitPoint -= 1;
            this.Audio.PlayOneShot(this.HurtClip);

            float opacity = 1f - ((0f + this.HitPoint) / this.HitPointMax);
            this.ScreenOverlayController.Image_BloodOverlay.SetOpacity(opacity);

            if (this.HitPoint <= 0)
                this.SceneLoader.Load(SceneType.MenuScene);
        }

        private void UpdateFlashlight()
        {
            this.Flashlight.SetUpdateType(this.Rigidbody.velocity.magnitude > .1f ? FlashlightUpdateType.Set : FlashlightUpdateType.Lerp);
            this.Flashlight.Update();
        }

        private void UpdateSprint()
        {
            bool hasMoveInput = this.MoveInput != Vector2.zero;

            if (hasMoveInput && this.Input.Sprint.IsPressed && this.Sprint > 0)
            {
                this.IsSprint = true;
                this.SprintCooldown = 2;
                this.Sprint = (this.Sprint - Time.deltaTime * 10f).ClampMin(0);
            }
            else
            {
                bool above = this.SprintCooldown > 0;
                this.SprintCooldown = (this.SprintCooldown -= Time.deltaTime).ClampMin(0);

                if (this.SprintCooldown == 0)
                {
                    if (above)
                        this.Sprint += 15;

                    this.Sprint = (this.Sprint + Time.deltaTime * 20).ClampMax(100);
                }
                
                this.IsSprint = false;
            }

            this.ScreenOverlayController.Element_Sprint.SetValue(this.Sprint);

            if (this.Sprint < 100)
            {
                this.ScreenOverlayController.Element_Sprint.SetOpacity(1);
            }
            else
            {
                this.ScreenOverlayController.Element_Sprint.SetOpacity(0);
            }
        }
        
        private void UpdateMoveInput()
        {
            this.MoveInput = this.Input.Move.GetVector2();
        }

        private void UpdateMoveAndView()
        {
            Vector3 move = this.transform.forward * this.MoveInput.y + this.transform.right * this.MoveInput.x;

            float speed = this.IsSprint ? this.SprintSpeed : this.Speed;

            this.Rigidbody.velocity = Vector3.MoveTowards(this.Rigidbody.velocity, move * speed, Time.deltaTime * this.Acceleration);

            this.ViewController.Update(this.Input.View.GetVector2() * this.Data.ViewSensitivity * Time.deltaTime);
        }

        private void CheckForTarget()
        {
            this.CurrentTarget = null;

            var hits = Physics.RaycastAll(
                new Ray(this.ViewController.Head.position, this.ViewController.Head.forward), this.Data.InteractionDistance, 
                Physics.AllLayers, QueryTriggerInteraction.Collide);

            foreach (var hit in hits)
            {
                if (hit.collider.GetComponent<Target>() is Target target)
                {
                    this.CurrentTarget = target;
                    return;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(new Ray(this.ViewController.Head.position, this.ViewController.Head.forward));
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
            this.Y += viewInput.x * this.RotationSpeedY;
            this.X -= viewInput.y * this.RotationSpeedX;

            this.X = Mathf.Clamp(this.X, this.ViewConstraint.x, this.ViewConstraint.y);

            this.Transform.transform.localRotation = Quaternion.Euler(0f, this.Y, 0f);
            this.Head.transform.localRotation = Quaternion.Euler(this.X, 0f, 0f);

            //Vector3 rotation = this.Transform.transform.localEulerAngles;

            //this.Y += viewInput.x * this.RotationSpeedY;
            //rotation.y = this.Y;
            //this.Transform.transform.localEulerAngles = rotation;

            //rotation = this.Head.transform.localEulerAngles;
            //this.X -= viewInput.y * this.RotationSpeedX;
            //this.X = Mathf.Clamp(this.X, this.ViewConstraint.x, this.ViewConstraint.y);
            //rotation.x = this.X;


            //this.Head.transform.localEulerAngles = rotation;
        }
    }

    [System.Serializable]
    public class FlashlightController
    {
        [SerializeField] private LerpTransform flashlightLerpTransform;
        public LerpTransform FlashlightLerpTransform { get { return flashlightLerpTransform; } }

        [SerializeField] private GameObject flashlight;
        public GameObject gameObject { get { return flashlight; } }

        [SerializeField] private float lerpTime = .1f;
        public float LerpTime { get { return lerpTime; } }

        [SerializeField] private float setLerpTime = .98f;
        public float SetLerpTime { get { return setLerpTime; } }


        public FlashlightUpdateType UpdateType { get; private set; }


        public void SetUpdateType(FlashlightUpdateType type)
        {
            this.UpdateType = type;
        }

        public void Update()
        {
            this.FlashlightLerpTransform.SetLerpTime((this.UpdateType == FlashlightUpdateType.Lerp) ? this.LerpTime : this.SetLerpTime);
            this.FlashlightLerpTransform.ManualUpdate();
        }
    }

    public static class DialogCatalogue
    {
        public static Dictionary<SystemLanguage, Dictionary<DialogLine, string[]>> DialogSystem { get; private set; } 


        static DialogCatalogue()
        {
            DialogSystem = new Dictionary<SystemLanguage, Dictionary<DialogLine, string[]>>();
            DialogSystem.Add(SystemLanguage.German, new Dictionary<DialogLine, string[]>());
            DialogSystem.Add(SystemLanguage.English, new Dictionary<DialogLine, string[]>());

            DialogSystem[SystemLanguage.German].Add(DialogLine.Player_Interaction, new[] { "Drücke [E] zum benutzen" });
            DialogSystem[SystemLanguage.English].Add(DialogLine.Player_Interaction, new[] { "Press [E] to use" });

            DialogSystem[SystemLanguage.German].Add(DialogLine.Player_Target_Collected, new[] { "{0} / {1}" });
            DialogSystem[SystemLanguage.English].Add(DialogLine.Player_Target_Collected, new[] { "{0} / {1}" });
        }

        public enum DialogLine
        {
            Player_Interaction,
            Player_Target_Collected
        }

        public static string[] Get(SystemLanguage language, DialogLine line, params string[] details)
        {
            if (!DialogSystem.ContainsKey(language))
                return new string[0];

            if (!DialogSystem[language].ContainsKey(line))
                return new string[0];

            details = details ?? new string[0];

            string[] toReturn = DialogSystem[language][line].ToArray();

            Regex regex = new Regex(@"\{\d+\}");
            for (int i = 0; i < toReturn.Length; i++)
            {
                string currentLine = toReturn[i];

                while(regex.IsMatch(currentLine))
                {
                    Match match = regex.Match(currentLine);
                    string detailsIndexString = match.Value.Trim('{', '}');

                    if (int.TryParse(detailsIndexString, out int detailsIndex) && details.Length > detailsIndex)
                    {
                        currentLine = currentLine.Replace(match.Value, details[detailsIndex].Replace("{", ""));
                    }
                    else
                    {
                        currentLine = currentLine.Replace(match.Value, "");
                    }
                }

                toReturn[i] = currentLine;
            }

            return toReturn;
        }
    }


    public enum FlashlightUpdateType
    {
        Lerp,
        Set
    }
}