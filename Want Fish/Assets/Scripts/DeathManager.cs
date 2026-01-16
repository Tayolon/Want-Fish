using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathManager : MonoBehaviour
{
    public static DeathManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TriggerDeath()
    {
        Debug.Log("kill [DeathManager]");
        Time.timeScale = 0f;

        UIManager.Instance?.ShowDeath();
    }

    public void GoToHub()
    {
        Time.timeScale = 1f;
        RunManager.Instance.EndRun();
        SceneManager.LoadScene("FishingHub");
    }
}
