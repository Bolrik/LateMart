using UnityEngine;

namespace Game
{
    public class VegetationObject : MonoBehaviour
    {
        [SerializeField] private GameObject root;
        public GameObject Root { get { return root; } }

        public void Remove()
        {
            GameObject.Destroy(this.Root);
        }
    }
}