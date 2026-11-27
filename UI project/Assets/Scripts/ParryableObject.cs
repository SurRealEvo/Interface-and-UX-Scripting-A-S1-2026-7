using System.Collections;
using UnityEngine;

public class ParryableObject : MonoBehaviour
{
    [Header("Parry Settings")]
    public bool isParryable = true;
    [SerializeField] private float reactivationTime = 0.5f;

    [Header("Visual Feedback")]
    [SerializeField] private Color normalColor = Color.magenta;
    [SerializeField] private Color successfulParryColor = Color.white;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = normalColor;
    }

    public void TryParry(Rigidbody2D playerRb, float bounceForce)
    {
        if (!isParryable) return;

        isParryable = false;

        playerRb.linearVelocity = new Vector2(
            playerRb.linearVelocity.x,
            bounceForce
        );

        spriteRenderer.color = successfulParryColor;

        Debug.Log("Successful parry!");

        StartCoroutine(ReactivateParry());
    }

    private IEnumerator ReactivateParry()
    {
        yield return new WaitForSeconds(reactivationTime);

        isParryable = true;
        spriteRenderer.color = normalColor;
    }
}