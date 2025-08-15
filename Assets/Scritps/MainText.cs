using UnityEngine;
using TMPro;
using System.Collections;

public class MainText : MonoBehaviour
{
    [Header("Text Appearance Settings")]
    public float fadeDuration = 3f;      // Duración de la transición de aparición
    public float moveSpeed = 0.5f;       // Velocidad del movimiento suave de letras
    public float distortionAmount = 0.1f; // Intensidad de distorsión (más suave)
    public float maxBlurAmount = 0.2f;   // Máxima cantidad de "desenfoque" aplicado a las letras
    public float maxScaleAmount = 1.1f;  // Tamaño máximo de escala durante la animación

    private TextMeshProUGUI textMesh;    // Componente de texto
    private Color initialColor;          // Color inicial del texto
    private Vector3 initialPosition;     // Posición inicial del texto
    private Vector3 finalPosition;       // Posición final del texto

    private bool isHovered = false;      // Para controlar si el mouse está sobre el texto
    private bool hasAnimationFinished = false; // Para asegurarnos que la animación solo ocurra una vez

    void Start()
    {
        // Obtener el componente TextMeshProUGUI
        textMesh = GetComponent<TextMeshProUGUI>();
        initialColor = textMesh.color;
        initialPosition = textMesh.transform.position;
        finalPosition = initialPosition;

        // Inicializar el color y la opacidad en 0 (transparente)
        textMesh.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0);
        textMesh.transform.localScale = Vector3.one;

        // Comenzar la animación de aparición
        StartCoroutine(FadeInAndDistort());
    }

    IEnumerator FadeInAndDistort()
    {
        float elapsedTime = 0f;
        float distortionTime = 0f;

        // Animación de aparición y distorsión
        while (elapsedTime < fadeDuration)
        {
            // Aumentar la opacidad de 0 a 1 (transición suave)
            float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            textMesh.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);

            // Pulsar el texto (efecto de escala suave)
            float scaleFactor = Mathf.Lerp(1f, maxScaleAmount, elapsedTime / fadeDuration);
            textMesh.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);

            // Mover las letras de manera suave (distorsión suave)
            Vector3 randomOffset = new Vector3(
                Mathf.Sin(distortionTime) * distortionAmount,
                Mathf.Cos(distortionTime) * distortionAmount,
                0
            );
            textMesh.transform.position = initialPosition + randomOffset;

            // Incrementar la intensidad del desenfoque (blur) de manera suave
            float blurAmount = Mathf.Lerp(0, maxBlurAmount, elapsedTime / fadeDuration);
            textMesh.fontMaterial.SetFloat("_OutlineSoftness", blurAmount); // Suavizar el borde

            elapsedTime += Time.deltaTime;
            distortionTime += Time.deltaTime * moveSpeed;

            yield return null;
        }

        // Después de que la animación termine, iniciamos el proceso de restauración
        hasAnimationFinished = true;
        StartCoroutine(RestoreText());
    }

    // Función que restaura gradualmente el texto a su forma original
    IEnumerator RestoreText()
    {
        float elapsedTime = 0f;
        float distortionTime = 0f;

        // Restaurar gradualmente la opacidad, escala, y desenfoque de forma inversa
        while (elapsedTime < fadeDuration)
        {
            // Reducir la opacidad de 1 a 1 (sin desaparecer, se mantiene visible)
            float alpha = Mathf.Lerp(1, 1, elapsedTime / fadeDuration); // Mantener en 1
            textMesh.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);

            // Reducir el tamaño gradualmente
            float scaleFactor = Mathf.Lerp(maxScaleAmount, 1f, elapsedTime / fadeDuration);
            textMesh.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);

            // Restaurar la distorsión gradualmente (mover las letras)
            Vector3 randomOffset = new Vector3(
                Mathf.Sin(distortionTime) * distortionAmount,
                Mathf.Cos(distortionTime) * distortionAmount,
                0
            );
            textMesh.transform.position = initialPosition + randomOffset;

            // Reducir el desenfoque gradualmente
            float blurAmount = Mathf.Lerp(maxBlurAmount, 0, elapsedTime / fadeDuration);
            textMesh.fontMaterial.SetFloat("_OutlineSoftness", blurAmount);

            elapsedTime += Time.deltaTime;
            distortionTime += Time.deltaTime * moveSpeed;

            yield return null;
        }

        // Asegurarse de que el texto quede completamente restaurado
        textMesh.color = new Color(initialColor.r, initialColor.g, initialColor.b, 1); // Mantener opaco
        textMesh.transform.localScale = Vector3.one;
        textMesh.fontMaterial.SetFloat("_OutlineSoftness", 0);
        textMesh.transform.position = initialPosition;

        // Asegurarse de que la animación no se ejecute más de una vez
        hasAnimationFinished = false;
    }
}
