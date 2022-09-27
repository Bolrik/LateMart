using Input;
using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private InputManager input;
        public InputManager Input { get { return input; } }

        [SerializeField] private SceneLoaderProxy sceneLoader;
        public SceneLoaderProxy SceneLoader { get { return sceneLoader; } }

        private void Update()
        {
            if (this.Input.Back.IsPressed)
            {
                this.SceneLoader.Load(SceneType.MenuScene);
                return;
            }
        }
    }
}