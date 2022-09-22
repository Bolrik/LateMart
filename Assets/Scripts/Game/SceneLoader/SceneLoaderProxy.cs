using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    [CreateAssetMenu(fileName = "Scene Loader Proxy", menuName = "Game/Data/Scenes/new Scene Loader Proxy")]
    public class SceneLoaderProxy : ScriptableObject
    {
        [SerializeField] private int buildIndex;
        public int BuildIndex { get { return buildIndex; } private set { this.buildIndex = value; } }

        [SerializeField] private SceneLoaderCatalog catalog;
        public SceneLoaderCatalog Catalog { get { return catalog; } }


        [SerializeField] private int loadingSceneBuildIndex;
        public int LoadingSceneBuildIndex { get { return loadingSceneBuildIndex; } }


        public void Load(SceneType type)
        {
            SceneInfo info = this.Catalog.Scenes.FirstOrDefault(sceneInfo => sceneInfo.Type == type);
            
            if (info == null)
                return;

            this.BuildIndex = info.BuildIndex;

            SceneManager.LoadScene(this.LoadingSceneBuildIndex);
        }
    }
}
