using UnityEngine;
using UnityEngine.UIElements;

namespace Player
{
    public class ScreenOverlayController : MonoBehaviour
    {
        [SerializeField] private UIDocument document;
        public UIDocument Document { get { return document; } }

        [SerializeField] private Player_Image_BloodOverlay image_BloodOverlay;
        public Player_Image_BloodOverlay Image_BloodOverlay { get { return image_BloodOverlay; } }

        [SerializeField] private Player_Element_Sprint element_Sprint;
        public Player_Element_Sprint Element_Sprint { get { return element_Sprint; } }


        private void Start()
        {
            Debug.Log(this.Document.rootVisualElement);
            this.Image_BloodOverlay.Initialize(this.Document.rootVisualElement);
            this.Element_Sprint.Initialize(this.Document.rootVisualElement);
        }
    }

    [System.Serializable]
    public class Player_Image_BloodOverlay
    {
        [SerializeField] private string qName = "Image_BloodOverlay";
        public string QName { get { return qName; } }

        [SerializeField] private VisualElement image;
        public VisualElement Image { get { return image; } private set { this.image = value; } }


        internal void Initialize(VisualElement rootVisualElement)
        {
            this.Image = rootVisualElement.Q<VisualElement>(this.QName);

            this.SetOpacity(0);
        }

        public void SetOpacity(float value)
        {
            this.Image.style.opacity = value;
        }
    }

    [System.Serializable]
    public class Player_Element_Sprint
    {
        [SerializeField] private string qName = "Element_Sprint";
        public string QName { get { return qName; } }

        [SerializeField] private ProgressBar progressBar;
        public ProgressBar ProgressBar { get { return progressBar; } private set { this.progressBar = value; } }


        internal void Initialize(VisualElement rootVisualElement)
        {
            this.ProgressBar = rootVisualElement.Q<ProgressBar>(this.QName);

            this.SetValue(100);
            this.SetOpacity(0);
        }

        public void SetValue(float value)
        {
            this.ProgressBar.value = value;
        }

        public void SetOpacity(float value)
        {
            this.ProgressBar.style.opacity = value;
        }
    }

}