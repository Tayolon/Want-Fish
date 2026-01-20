using UnityEngine;

using UnityEngine.SceneManagement;

public class ChangeSceneBtn : MonoBehaviour
{
    public string scene;

    public void LoadScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(scene);
    }
}
