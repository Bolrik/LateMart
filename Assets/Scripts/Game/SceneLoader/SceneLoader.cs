using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Game
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private SceneLoaderProxy proxy;
        public SceneLoaderProxy Proxy { get { return proxy; } }

        [SerializeField] private UIDocument document;
        public UIDocument Document { get { return document; } }

        ProgressBar Progress { get; set; }

        private void Start()
        {
            this.Progress = this.Document.rootVisualElement.Q<ProgressBar>("LoadingBar");

            this.StartCoroutine(this.LoadScene());
        }

        IEnumerator LoadScene()
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(this.Proxy.BuildIndex);

            Debug.Log($"Now Loading Scene {this.Proxy.BuildIndex}");

            while (!operation.isDone)
            {
                float progress = (operation.progress / .9f).Clamp(0, 1);
                this.Progress.value = progress;

                yield return null;
            }

            Debug.Log($"Done Loading Scene {this.Proxy.BuildIndex}");
        }
    }
}
