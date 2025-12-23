using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Interaction currentInteraction;

    void Update()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.F) && currentInteraction != null)
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

        transform.Translate(Vector2.right * move * moveSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Interaction interaction = other.GetComponentInParent<Interaction>();

        if (interaction != null)
        {
            currentInteraction = interaction;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Interaction interaction = other.GetComponentInParent<Interaction>();

        if (interaction != null && interaction == currentInteraction)
        {
            currentInteraction = null;
        }
    }
}