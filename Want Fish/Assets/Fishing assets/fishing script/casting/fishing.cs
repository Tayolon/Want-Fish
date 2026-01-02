using UnityEngine;

public class fishing : MonoBehaviour
{
    public CastingController casting;

void Update()
{
    // 🔒 LOCK TOTAL INPUT
    if (casting.IsBusy())
        return;

    if (Input.GetKeyDown(KeyCode.E))
    {
        if (!casting.IsCasting())
            casting.StartCasting();
        else
            casting.StopCasting();
    }
}

}
