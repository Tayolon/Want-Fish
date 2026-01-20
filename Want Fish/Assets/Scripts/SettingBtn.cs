using UnityEngine;
using UnityEngine.UI;

public class SettingBtn : MonoBehaviour
{
    public GameObject PlayerUI;
    public Button SettingButn;
    public GameObject UISetting;
    public Button ExitButton;

    

    void Awake()
    {
        if (ExitButton != null)
            ExitButton.onClick.AddListener(Close);

        if(SettingButn != null)
            SettingButn.onClick.AddListener(Open);
    }
    public void Open()
    {
        PlayerUI.SetActive(false);
        UISetting.SetActive(true);
        Time.timeScale = 0f;
        MusicBg.Instance.FunPolice();
    }

    void Close()
    {
        PlayerUI.SetActive(true);
        UISetting.SetActive(false);
        Time.timeScale = 1f;
    }
}
