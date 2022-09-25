using Game;
using Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private UIDocument document;
        public UIDocument Document { get { return document; } }

        [SerializeField] private MainMenu_Button_Start button_Start;
        public MainMenu_Button_Start Button_Start { get { return button_Start; } }

        [SerializeField] private MainMenu_Settings settings;
        public MainMenu_Settings Settings { get { return settings; } }

        [SerializeField] private MainMenu_Button_Exit button_Exit;
        public MainMenu_Button_Exit Button_Exit { get { return button_Exit; } }


        private void Start()
        {
            this.Button_Start.Initialize(this.Document.rootVisualElement);
            this.Settings.Initialize(this.Document.rootVisualElement);
            this.Button_Exit.Initialize(this.Document.rootVisualElement);
        }
    }

    [System.Serializable]
    public class MainMenu_Button_Start
    {
        [SerializeField] private string qName;
        public string QName { get { return qName; } }

        [SerializeField] private Button button;
        private Button Button { get { return button; } set { button = value; } }

        [SerializeField] private SceneLoaderProxy sceneLoader;
        public SceneLoaderProxy SceneLoader { get { return sceneLoader; } }


        internal void Initialize(VisualElement rootVisualElement)
        {
            this.Button = rootVisualElement.Q<Button>(this.QName);

            this.Button.clicked += this.Button_Clicked;
        }

        private void Button_Clicked()
        {
            this.SceneLoader.Load(SceneType.GameScene);
        }
    }

    [System.Serializable]
    public class MainMenu_Settings
    {
        [SerializeField] private string qName = "Container";
        public string QName { get { return qName; } }

        [SerializeField] private PlayerData data;
        public PlayerData Data { get { return data; } }


        [SerializeField] private MainMenu_Settings_Slider brightness;
        public MainMenu_Settings_Slider Brightness { get { return brightness; } }

        [SerializeField] private MainMenu_Settings_Slider mouseSensitivity;
        public MainMenu_Settings_Slider MouseSensitivity { get { return mouseSensitivity; } }


        internal void Initialize(VisualElement rootVisualElement)
        {
            this.Brightness.Initialize(rootVisualElement);
            this.Brightness.Register((x) => this.Data.Brightness = x);
            this.MouseSensitivity.Initialize(rootVisualElement);
            this.MouseSensitivity.Register((x) => this.Data.ViewSensitivity = x);
        }
    }

    [System.Serializable]
    public class MainMenu_Settings_Slider
    {
        [SerializeField] private string qName = "Slider";
        public string QName { get { return qName; } }

        [SerializeField] private Slider slider;
        private Slider Slider { get { return slider; } set { slider = value; } }

        Action<float> Action { get; set; }

        internal void Initialize(VisualElement rootVisualElement)
        {
            this.Slider = rootVisualElement.Q<Slider>(this.QName);
            this.Slider.RegisterValueChangedCallback(this.Changed);
        }

        private void Changed(ChangeEvent<float> value)
        {
            Debug.Log("Change");
            this.Action?.Invoke(value.newValue);
        }

        public void Register(Action<float> action)
        {
            this.Action = action;
        }
    }


    [System.Serializable]
    public class MainMenu_Button_Exit
    {
        [SerializeField] private string qName;
        public string QName { get { return qName; } }

        [SerializeField] private Button button;
        private Button Button { get { return button; } set { button = value; } }



        internal void Initialize(VisualElement rootVisualElement)
        {
            this.Button = rootVisualElement.Q<Button>(this.QName);

            this.Button.clicked += this.Button_Clicked;
        }

        private void Button_Clicked()
        {
            Application.Quit();
        }
    }
}
