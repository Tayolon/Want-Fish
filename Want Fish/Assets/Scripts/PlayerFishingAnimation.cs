using UnityEngine;

public class PlayerFishingAnimation : MonoBehaviour
{
    public static PlayerFishingAnimation Instance;
    private Animator anim;

    void Awake()
    {
        Instance = this;
        anim = GetComponentInChildren<Animator>();
    }

    // One-shot casting animation
    public void PlayCastingOnce()
    {
        anim.ResetTrigger("Cast");
        anim.SetTrigger("Cast");
    }

    // Loop reeling
    public void StartReeling()
    {
        anim.SetBool("IsReeling", true);
    }

    // Stop reeling → Idle
    public void StopReeling()
    {
        anim.SetBool("IsReeling", false);
    }
}
