using UnityEngine;
using UnityEngine.UI;

public class MasEMas : Interaction
{

    public GameObject shopScreen;
    public Button ExitBtn;

    public override void Interact()
    {
        shopScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    void Awake()
    {
        if (ExitBtn != null)
            ExitBtn.onClick.AddListener(Close);
            
    }

    void Close()
    {
        shopScreen.SetActive(false);
        Time.timeScale = 1f;
    }
}