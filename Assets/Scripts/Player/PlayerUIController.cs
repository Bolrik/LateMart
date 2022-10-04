using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Player
{
    public class PlayerUIController : MonoBehaviour
    {
        [SerializeField] private UIDocument document;
        public UIDocument Document { get { return document; } }

        [SerializeField] private Player_Label_Interaction label_Interaction;
        public Player_Label_Interaction Label_Interaction { get { return label_Interaction; } }

        [SerializeField] private Player_Label_MonoDialog label_MonoDialog;
        public Player_Label_MonoDialog Label_MonoDialog { get { return label_MonoDialog; } }

        Queue<DialogItem> DialogQueue { get; set; } = new Queue<DialogItem>();
        DialogItem CurrentDialogItem { get; set; }

        private void Start()
        {
            this.Label_Interaction.Initialize(this.Document.rootVisualElement);
            this.Label_MonoDialog.Initialize(this.Document.rootVisualElement);
        }

        private void Update()
        {
            if (this.CurrentDialogItem != null)
            {
                if (this.CurrentDialogItem.IsDone)
                    this.CurrentDialogItem = this.DialogQueue.Count > 0 ? this.DialogQueue.Dequeue() : null;
                else
                    this.Label_MonoDialog.SetText(this.CurrentDialogItem.Advance(Time.deltaTime));
            }
            else
            {
                this.CurrentDialogItem = this.DialogQueue.Count > 0 ? this.DialogQueue.Dequeue() : null;
                this.Label_MonoDialog.SetText("");
            }
        }

        public void AddDialog(DialogItem item)
        {
            this.DialogQueue.Enqueue(item);
        }
    }


    public class DialogItem
    {
        public string[] Lines { get; private set; }
        public float StayTime { get; private set; }
        public float CharTime { get; private set; }


        private float StayTimeTime { get; set; }
        private float CharTimeTime { get; set; }

        private bool IsStay { get; set; }
        private string ToReturn { get; set; }

        public bool IsDone { get => this.IsStay && this.StayTimeTime >= this.StayTime; }

        private int LineIndex { get; set; }
        private int CharIndex { get; set; }



        public DialogItem(string[] lines, float stayTime, float charTime)
        {
            this.Lines = lines;
            this.StayTime = stayTime;
            this.CharTime = charTime;
        }


        public string Advance(float deltaTime)
        {
            if (this.IsStay)
            {
                this.StayTimeTime += deltaTime;
                return ToReturn;
            }

            this.CharTimeTime += deltaTime;

            if (this.CharTimeTime < this.CharTime)
                return this.ToReturn;
            
            this.CharTimeTime = 0;
            string line = this.Lines[this.LineIndex];
            this.ToReturn += line[this.CharIndex];

            this.CharIndex++;

            if (this.CharIndex >= line.Length)
            {
                this.CharIndex = 0;
                this.LineIndex++;

                if (this.LineIndex >= this.Lines.Length)
                    this.IsStay = true;
                else
                    this.ToReturn += "\n";
            }

            return this.ToReturn;
        }
    }


    [System.Serializable]
    public class Player_Label_Interaction
    {
        [SerializeField] private string qName = "Label_Interaction";
        public string QName { get { return qName; } }

        [SerializeField] private Label label;
        public Label Label { get { return label; } private set { this.label = value; } }


        internal void Initialize(VisualElement rootVisualElement)
        {
            this.Label = rootVisualElement.Q<Label>(this.QName);

            this.SetText("");
        }

        public void SetText(string value)
        {
            this.Label.text = value;
        }
    }

    [System.Serializable]
    public class Player_Label_MonoDialog
    {
        [SerializeField] private string qName = "Label_MonoDialog";
        public string QName { get { return qName; } }

        [SerializeField] private Label label;
        public Label Label { get { return label; } private set { this.label = value; } }


        internal void Initialize(VisualElement rootVisualElement)
        {
            this.Label = rootVisualElement.Q<Label>(this.QName);

            this.SetText("");
        }

        public void SetText(string value)
        {
            this.Label.text = value;
        }
    }

}