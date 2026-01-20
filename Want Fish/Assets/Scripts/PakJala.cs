using UnityEngine;

public class PakJala : Interaction
{
    public GameObject targetObject;
    public float autoDisableTime = 5f;

    private bool isActive;

    void Start()
    {
        if (targetObject != null)
            targetObject.SetActive(false);
    }

    public override void Interact()
    {
        if (isActive || targetObject == null) return;

        targetObject.SetActive(true);
        isActive = true;

        if (autoDisableTime > 0f)
            Invoke(nameof(DisableObject), autoDisableTime);
    }

    void DisableObject()
    {
        targetObject.SetActive(false);
        isActive = false;
    }
}
