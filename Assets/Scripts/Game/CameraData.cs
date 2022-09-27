using Game;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraData", menuName = "Game/Data/Camera/new Camera Data")]
public class CameraData : ScriptableObject
{
    [SerializeField] private GameCamera camera;
    public GameCamera Camera { get { return camera; } set { this.camera = value; } }

}