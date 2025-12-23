using UnityEngine;
using UnityEngine.SceneManagement;

public class Kapal : Interaction
{
    public string sceneTujuan = "GantiHari";

    public override void Interact()
    {
        Debug.Log("Menuju balik cakrawala...");
        SceneManager.LoadScene(sceneTujuan);
    }
}
