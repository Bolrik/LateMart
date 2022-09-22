using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Scene Loader Catalog", menuName = "Game/Data/Scenes/new Scene Loader Catalog")]
    public class SceneLoaderCatalog : ScriptableObject
    {
        [SerializeField] private SceneInfo[] scenes;
        public SceneInfo[] Scenes
        {
            get { return scenes; }
        }
    }

    [System.Serializable]
    public class SceneInfo
    {
        [SerializeField] private int buildIndex;
        public int BuildIndex { get { return buildIndex; } }

        [SerializeField] private SceneType type;
        public SceneType Type { get { return type; } }
    }

    public enum SceneType
    {
        MenuScene,
        GameScene
    }
}