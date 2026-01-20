using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HubSetting : MonoBehaviour
{
    public Button MainMenubtn;
    public Button Quitbtn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MainMenubtn.onClick.AddListener(MainMenu);
        Quitbtn.onClick.AddListener(Quit);
    }

    void Quit()
    {
        SaveManager.Instance.SaveAll();
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    void MainMenu()
    {
        SaveManager.Instance.SaveAll();
        SceneManager.LoadScene("MainMenu");
    }
}
