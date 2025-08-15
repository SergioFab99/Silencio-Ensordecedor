using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class GlitchEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Hover Effect Settings")]
    public Color hoverColor = Color.cyan;
    public float hoverScale = 1.2f;
    public float transitionSpeed = 8f;

    [Header("Glitch Effect Settings")]
    public float glitchIntensity = 0.05f;
    public float glitchSpeed = 10f;
    public float glitchDuration = 0.1f;

    private TextMeshProUGUI buttonText;
    private Color originalColor;
    private Vector3 originalScale;
    private string originalText;
    private bool isHovered = false;

    void Start()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            originalColor = buttonText.color;
            originalScale = buttonText.transform.localScale;
            originalText = buttonText.text;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    RestoreOriginalText();
    }

    void Update()
    {
        if (buttonText == null) return;

        Color targetColor = isHovered ? hoverColor : originalColor;
        buttonText.color = Color.Lerp(buttonText.color, targetColor, Time.deltaTime * transitionSpeed);

        Vector3 targetScale = isHovered ? originalScale * hoverScale : originalScale;
        buttonText.transform.localScale = Vector3.Lerp(buttonText.transform.localScale, targetScale, Time.deltaTime * transitionSpeed);

        if (isHovered)
        {
            ApplyGlitchEffect();
        }
    }

    void ApplyGlitchEffect()
    {
        string glitchedText = originalText;

        char[] chars = originalText.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
        {
            if (Random.value < 0.5f)
            {
                chars[i] = (char)Random.Range(33, 126);
            }
        }
        glitchedText = new string(chars);
        buttonText.text = glitchedText;
    }

    void RestoreOriginalText()
    {
    buttonText.text = originalText;
    buttonText.color = originalColor;
    buttonText.transform.localScale = originalScale;
    }
}
