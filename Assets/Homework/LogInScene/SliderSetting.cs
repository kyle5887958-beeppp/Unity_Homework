using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class SliderSetting : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private float awakeVolume = 0.5f;
    private void Awake()
    {
        slider.value = awakeVolume;
        AudioListener.volume = awakeVolume;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is create
    void Start()
    {
        slider.onValueChanged.AddListener((value) =>
        {
            AudioListener.volume = value;
        });
    }
}
