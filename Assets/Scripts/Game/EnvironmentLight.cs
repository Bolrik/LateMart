using Player;
using UnityEngine;

public class EnvironmentLight : MonoBehaviour
{
    [SerializeField] private PlayerData data;
    public PlayerData Data { get { return data; } }

    [SerializeField] private Light light;
    public Light Light { get { return light; } }


    [SerializeField] private bool autoUpdate;
    public bool AutoUpdate { get { return autoUpdate; } }



    private void Start()
    {
        this.Light.intensity = this.Data.Brightness;
    }

    private void Update()
    {
        if (!this.AutoUpdate)
            return;

        this.Light.intensity = this.Data.Brightness;
    }
}