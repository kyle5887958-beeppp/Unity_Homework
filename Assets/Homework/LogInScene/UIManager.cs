using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject settingPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        infoPanel.SetActive(false);
        settingPanel.SetActive(false);
    }

    // Update is called once per frame
    public void OpenInfo()
    {
        infoPanel.SetActive(true);
    }

    public void CloseInfo()
    {
        infoPanel.SetActive(false);
    }

    public void OpenSetting()
    {
        settingPanel.SetActive(true);
    }
    public void CloseSetting()
    {
        settingPanel.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
