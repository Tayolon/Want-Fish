using UnityEngine;

public class fishing : MonoBehaviour
{
    public CastingController casting;


    public AudioClip castingsound;
    public float Volume = 0.4f;

    private AudioSource audioSource;
    private bool wasCasting;

    void Start()
    {
        // Create / get AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
    }
    void Update()
    {
        // 🔒 LOCK TOTAL INPUT
        if (casting.IsBusy())
            return;

        if (wasCasting && !casting.IsCasting())
        {
            PlayCastingSound(); // 🔊 plays AFTER casting
        }

        wasCasting = casting.IsCasting();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!casting.IsCasting())
                casting.StartCasting();
            else
                casting.StopCasting();
        }
    }

    void PlayCastingSound()
    {
        if (castingsound != null)
            audioSource.PlayOneShot(castingsound, Volume);
    }

}
