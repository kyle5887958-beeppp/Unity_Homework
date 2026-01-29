using UnityEngine;

public class AudioSetting : MonoBehaviour
{
    private float awakeVolume = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioListener.volume = awakeVolume;
    }

    // Update is called once per frame
}
