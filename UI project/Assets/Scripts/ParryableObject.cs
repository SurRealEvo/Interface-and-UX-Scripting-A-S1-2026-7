using UnityEngine;

public class ParryableObject : MonoBehaviour
{
    public bool isParryable = true;

    private bool playerInRange;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player is in parry range!");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player left parry range!");
        }
    }
    public void TryParry(Rigidbody2D playerRb, float bounceForce)
    {
        if (!isParryable) return;

        playerRb.linearVelocity = new Vector2(
            playerRb.linearVelocity.x,
            bounceForce
        );

        Debug.Log("Successful parry!");
    }
}