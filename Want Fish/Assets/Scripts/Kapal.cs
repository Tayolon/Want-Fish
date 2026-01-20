using UnityEngine;
using UnityEngine.SceneManagement;

public class Kapal : Interaction
{
    public string sceneTujuan = "MainGame";

    public override void Interact()
    {
        CurrencyManager.Instance.ResetRunData();
        MusicBg.Instance.FunPolice();
        Debug.Log("Menuju balik cakrawala...");
        SceneManager.LoadScene(sceneTujuan);
    }
}
