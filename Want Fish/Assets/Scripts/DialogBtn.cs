using UnityEngine;
using UnityEngine.UI;

public class DialogBtn : MonoBehaviour
{
    public GameObject targetObject;
    public Button btn;

    void Awake()
    {
        btn.onClick.AddListener(Toggle);
    }

    void Toggle()
    {
        targetObject.SetActive(!targetObject.activeSelf);
    }
}
