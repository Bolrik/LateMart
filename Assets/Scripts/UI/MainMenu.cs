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

        [SerializeField] private MainMenut_ButtonStart button_Start;
        public MainMenut_ButtonStart Button_Start { get { return button_Start; } }


        private void Start()
        {
            this.Button_Start.Initialize(this.Document.rootVisualElement);
        }
    }

    [System.Serializable]
    public class MainMenut_ButtonStart
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
            Debug.Log("Geht");
        }
    }
}
