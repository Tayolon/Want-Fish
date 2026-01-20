using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI dialogText;
    public GameObject dialogBox;
    public float autoCloseTime = 13f; // seconds

    private string[] lines;
    private int index;
    private bool isActive;
    private float timer;

    void Update()
    {
        if (!isActive) return;

        timer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.E))
        {
            index++;
            timer = autoCloseTime; // reset timer

            if (index >= lines.Length)
            {
                CloseDialog();
            }
            else
            {
                dialogText.text = lines[index];
            }
        }

        if (timer <= 0f)
        {
            CloseDialog();
        }
    }

    public void StartDialog(string[] dialogLines)
    {
        lines = dialogLines;
        index = 0;
        timer = autoCloseTime;
        isActive = true;

        dialogBox.SetActive(true);
        dialogText.text = lines[index];
    }

    void CloseDialog()
    {
        dialogBox.SetActive(false);
        isActive = false;
    }
}
