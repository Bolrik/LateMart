using Enemy;
using Input;
using Player;
using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private InputManager input;
        public InputManager Input { get { return input; } }

        [SerializeField] private SceneLoaderProxy sceneLoader;
        public SceneLoaderProxy SceneLoader { get { return sceneLoader; } }

        [SerializeField] private Eyenstein enemy;
        public Eyenstein Enemy { get { return enemy; } }

        [SerializeField] private PlayerController player;
        public PlayerController Player { get { return player; } }


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