using UnityEngine;

public class MusicBg : MonoBehaviour
{
    public static MusicBg Instance;

    public AudioClip BgSound;
    public AudioClip winSound;
    public float BgVolume = 0.6f;

    private AudioSource BgAudio;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        EnsureAudioSource();

        if (BgSound != null)
        {
            BgAudio.clip = BgSound;
            BgAudio.volume = BgVolume;
            BgAudio.loop = true;
            BgAudio.Play();
        }
        else
        {
            Debug.LogWarning("MusicBg: BgSound is not assigned.");
        }
    }

    private void EnsureAudioSource()
    {
        if (BgAudio == null)
        {
            BgAudio = GetComponent<AudioSource>();

            if (BgAudio == null)
            {
                BgAudio = gameObject.AddComponent<AudioSource>();
            }
        }
    }

    public void WinSound()
    {
        EnsureAudioSource();

        if (winSound == null)
        {
            Debug.LogWarning("MusicBg: winSound is not assigned.");
            return;
        }

        BgAudio.ignoreListenerPause = true;
        BgAudio.clip = winSound;
        BgAudio.volume = 6.7f;
        BgAudio.loop = false;
        BgAudio.Play();
    }

    public void FunPolice()
    {
        EnsureAudioSource();

        if (BgSound != null)
        {
            BgAudio.clip = BgSound;
            BgAudio.volume = BgVolume;
            BgAudio.loop = true;
            BgAudio.Play();
        }
        else
        {
            Debug.LogWarning("MusicBg: BgSound is not assigned.");
        }
    }
}
