using System.Collections;
using UnityEngine;

public class PlayerParryFeedback : MonoBehaviour
{
    [SerializeField] private SpriteRenderer glowRenderer;

    [Header("Glow Settings")]
    [SerializeField] private float fadeDuration = 0.2f;
    [SerializeField] private float glowAlpha = 0.5f;

    [Header("Colours")]
    [SerializeField] private Color normalParryColor = new Color(1f, 0.2f, 0.65f);
    [SerializeField] private Color successfulParryColor = Color.white;

    private Coroutine glowCoroutine;

    public void PlayParryEffect(bool successful)
    {
        if (glowCoroutine != null)
            StopCoroutine(glowCoroutine);

        Color effectColor = successful
            ? successfulParryColor
            : normalParryColor;

        glowCoroutine = StartCoroutine(FadeGlow(effectColor));
    }

    private IEnumerator FadeGlow(Color effectColor)
    {
        glowRenderer.gameObject.SetActive(true);

        float timer = 0f;

        // Fade in
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            float alpha = Mathf.Lerp(
                0f, glowAlpha, timer / fadeDuration
            );

            glowRenderer.color = new Color(
                effectColor.r,
                effectColor.g,
                effectColor.b,
                alpha
            );

            yield return null;
        }

        // Fade out
        timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            float alpha = Mathf.Lerp(
                glowAlpha, 0f, timer / fadeDuration
            );

            glowRenderer.color = new Color(
                effectColor.r,
                effectColor.g,
                effectColor.b,
                alpha
            );

            yield return null;
        }

        glowRenderer.gameObject.SetActive(false);
        glowCoroutine = null;
    }
}