using UnityEngine;
using UnityEngine.UI;

public class MasEMas : Interaction
{

    public GameObject shopScreen;
    public Button ExitBtn;

    public override void Interact()
    {
        shopScreen.SetActive(true);
    }

    void Awake()
    {
        if (ExitBtn != null)
        ExitBtn.onClick.AddListener(Close);
    }

    void Close()
    {
        shopScreen.SetActive(false);
    }
}