using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextEffect : MonoBehaviour
{
    public float duracionFadeIn = 2f; // tiempo que dura el fade
    private TextMeshProUGUI textoComponent;
    private bool fadeCompletado = false;

    void Awake()
    {
        textoComponent = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        if (!fadeCompletado)
        {
            StopAllCoroutines();
            StartCoroutine(EfectoFadeIn());
        }
    }

    private IEnumerator EfectoFadeIn()
    {
        textoComponent.alpha = 0f;
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracionFadeIn)
        {
            float alfaActual = Mathf.Clamp01(tiempoTranscurrido / duracionFadeIn);
            textoComponent.alpha = alfaActual;
            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }

        textoComponent.alpha = 1f;
        // Mantener el texto visible para siempre
        while (true)
        {
            textoComponent.alpha = 1f;
            yield return null;
        }
    }
}
