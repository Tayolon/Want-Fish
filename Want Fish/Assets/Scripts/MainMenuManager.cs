using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public Button StartNewBtn;
    public Button quitBtn;
    public Button ContinueBtn;
    public GameObject warnign;
    public Button warnConfirm;
    public Button warnBack;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartNewBtn.onClick.AddListener(Warn);
        quitBtn.onClick.AddListener(Quit);
        ContinueBtn.onClick.AddListener(ContinueGame);
        warnConfirm.onClick.AddListener(NewGame);
        warnBack.onClick.AddListener(WarnCoward);
    }

    public void NewGame()
    {
        SaveManager.Instance.FullReset();
        PermanentUpgradeManager.Instance.FullReset();
        CurrencyManager.Instance.fishCoin = 0;
        SceneManager.LoadScene("FishingHub");
    }

    public void ContinueGame()
    {
        SaveManager.Instance.LoadAll();
        SceneManager.LoadScene("FishingHub");
    }

    public void Quit()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    void Warn()
    {
        warnign.SetActive(true);
    }

    void WarnCoward()
    {
        warnign.SetActive(false);
    }
}
