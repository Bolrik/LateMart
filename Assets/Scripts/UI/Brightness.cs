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
    public class Brightness : MonoBehaviour
    {
        [SerializeField] private UIDocument document;
        public UIDocument Document { get { return document; } }

        [SerializeField] private Brightness_Button_Confirm button_Confirm;
        public Brightness_Button_Confirm Button_Confirm { get { return button_Confirm; } }

        [SerializeField] private Slider slider;
        public Slider Slider { get { return slider; } private set { this.slider = value; } }

        [SerializeField] private string sliderName;
        public string SliderName { get { return sliderName; } }

        [SerializeField] private PlayerData data;
        public PlayerData Data { get { return data; } }




        [SerializeField] private EnvironmentLight light;
        public EnvironmentLight Light { get { return light; } }


        private void Start()
        {
            this.Button_Confirm.Initialize(this.Document.rootVisualElement);
            this.Slider = this.Document.rootVisualElement.Q<Slider>(this.SliderName);
        }

        private void Update()
        {
            this.Data.Brightness = this.Slider.value;
        }
    }

    [System.Serializable]
    public class Brightness_Button_Confirm
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
            this.SceneLoader.Load(SceneType.MenuScene);
        }
    }
}
