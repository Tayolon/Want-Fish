using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextTutor : MonoBehaviour
{
    [SerializeField] private GameObject textObject;
    [SerializeField] private float showTime = 2f;
    private Coroutine runningCoroutine;

    void Start()
    {
        ShowText();
    }
    public void ShowText()
    {
        if (runningCoroutine != null)
            StopCoroutine(runningCoroutine);

        runningCoroutine = StartCoroutine(ShowRoutine());
    }

    private IEnumerator ShowRoutine()
    {
        textObject.SetActive(true);
        yield return new WaitForSecondsRealtime(showTime);
        textObject.SetActive(false);
    }
}
