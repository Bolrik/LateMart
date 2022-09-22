using Game;
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

        [SerializeField] private MainMenut_Button_Start button_Start;
        public MainMenut_Button_Start Button_Start { get { return button_Start; } }

        [SerializeField] private MainMenut_Button_Exit button_Exit;
        public MainMenut_Button_Exit Button_Exit { get { return button_Exit; } }


        private void Start()
        {
            this.Button_Start.Initialize(this.Document.rootVisualElement);
            this.Button_Exit.Initialize(this.Document.rootVisualElement);
        }
    }

    [System.Serializable]
    public class MainMenut_Button_Start
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
    public class MainMenut_Button_Exit
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
