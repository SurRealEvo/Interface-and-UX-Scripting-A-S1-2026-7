using System.Collections;
using UnityEngine;

public class ParryableObject : MonoBehaviour
{
    [Header("Parry Settings")]
    public bool isParryable = true;
    [SerializeField] private float reactivationTime = 1f;

    [Header("Visual Feedback")]
    [SerializeField] private Color normalColor = new Color(0.55f, 0.15f, 0.35f);
    [SerializeField] private Color inRangeColor = new Color(1f, 0.2f, 0.65f);
    [SerializeField] private Color successfulParryColor = Color.white;

    [Header("Glow")]
    [SerializeField] private SpriteRenderer glowRenderer;

    private SpriteRenderer spriteRenderer;
    private bool playerInRange;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateVisualState();
    }

    public void SetPlayerInRange(bool inRange)
    {
        playerInRange = inRange;
        UpdateVisualState();
    }

    public bool TryParry(Rigidbody2D playerRb, float bounceForce)
    {
        if (!isParryable) return false;

        isParryable = false;

        playerRb.linearVelocity = new Vector2(
            playerRb.linearVelocity.x,
            bounceForce
        );

        UpdateVisualState();

        Debug.Log("Successful parry!");

        StartCoroutine(ReactivateParry());

        return true;
    }

    private void UpdateVisualState()
    {
        if (!isParryable)
        {
            spriteRenderer.color = successfulParryColor;
            glowRenderer.enabled = false;
        }
        else if (playerInRange)
        {
            spriteRenderer.color = inRangeColor;
            glowRenderer.enabled = true;
        }
        else
        {
            spriteRenderer.color = normalColor;
            glowRenderer.enabled = false;
        }
    }

    private IEnumerator ReactivateParry()
    {
        yield return new WaitForSeconds(reactivationTime);

        isParryable = true;
        UpdateVisualState();
    }
}