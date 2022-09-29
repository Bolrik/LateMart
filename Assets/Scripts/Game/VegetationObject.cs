using UnityEngine;

namespace Game
{
    public class VegetationObject : MonoBehaviour
    {
        [SerializeField] private GameObject root;
        public GameObject Root { get { return root; } }

        public void Remove()
        {
            Debug.Log($"Removing {this.Root}");

            GameObject.Destroy(this.Root);
        }
    }
}