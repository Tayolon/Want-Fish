using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float baseScaleX = 2.5f;

    public AudioClip footstepSound;
    public float footstepInterval = 0.4f;
    public float footstepVolume = 0.6f;

    private float footstepTimer;
    private Interaction currentInteraction;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.E) && currentInteraction != null)
        {
            currentInteraction.Interact();
        }
    }

    void Move()
    {
        float move = 0f;

        if (Input.GetKey(KeyCode.A))
            move = -1f;
        else if (Input.GetKey(KeyCode.D))
            move = 1f;

        // Move player
        transform.Translate(Vector2.right * move * moveSpeed * Time.deltaTime);

        // Animation
        bool isWalking = move != 0;
        animator.SetBool("isWalking", isWalking);

        // Footstep sound
        if (isWalking)
        {
            footstepTimer -= Time.deltaTime;

            if (footstepTimer <= 0f)
            {
                AudioManager.instance.PlaySFX(footstepSound, footstepVolume);
                footstepTimer = footstepInterval;
            }
        }
        else
        {
            footstepTimer = 0f;
        }

        // Flip character
        if (move != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = move > 0 ? baseScaleX : -baseScaleX;
            transform.localScale = scale;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Interaction interaction = other.GetComponentInParent<Interaction>();
        if (interaction != null)
            currentInteraction = interaction;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Interaction interaction = other.GetComponentInParent<Interaction>();
        if (interaction != null && interaction == currentInteraction)
            currentInteraction = null;
    }
}
