using UnityEngine;
using TMPro;

public class TextIntro3 : MonoBehaviour
{
    private TextMeshProUGUI textoMesh;
    private float velocidadRevelado = 80f;
    private float velocidadDesplazamiento = 10f;
    private float posicionReveladoActual;
    private string textoObjetivo = "As adults, we are afraid of failing, of losing a job or the love of our lives.";
    private float tiempoEspera = 20f;
    private bool iniciado = false;
    private float tiempoDesdeAparicion = 0f;
    private bool haciendoFadeOut = false;
    private float duracionFadeOut = 2f;
    private float tiempoFadeOut = 0f;

    void Start()
    {
        textoMesh = GetComponent<TextMeshProUGUI>();
        textoMesh.text = textoObjetivo;
        textoMesh.ForceMeshUpdate();
        TMP_TextInfo infoTexto = textoMesh.textInfo;
        if (infoTexto.characterCount > 0)
        {
            float primerCaracterX = infoTexto.characterInfo[0].bottomLeft.x;
            posicionReveladoActual = primerCaracterX - 100f;
        }
        else
        {
            posicionReveladoActual = -100f;
        }
        for (int i = 0; i < infoTexto.characterCount; i++)
        {
            CambiarAlfaCaracter(i, 0f);
        }
        textoMesh.enabled = false;
    }

    void Update()
    {
        if (!iniciado)
        {
            tiempoEspera -= Time.deltaTime;
            if (tiempoEspera <= 0f)
            {
                textoMesh.enabled = true;
                iniciado = true;
            }
            else
            {
                return;
            }
        }
        if (iniciado && !haciendoFadeOut)
        {
            tiempoDesdeAparicion += Time.deltaTime;
            if (tiempoDesdeAparicion >= 10f)
            {
                haciendoFadeOut = true;
                tiempoFadeOut = 0f;
            }
        }
        if (haciendoFadeOut)
        {
            FadeOutTexto();
        }
        else
        {
            RevelarTexto();
        }
        DesplazarTexto();
    }

    void FadeOutTexto()
    {
        TMP_TextInfo infoTexto = textoMesh.textInfo;
        if (infoTexto.characterCount == 0) return;
        tiempoFadeOut += Time.deltaTime;
        float alpha = Mathf.Clamp01(1f - (tiempoFadeOut / duracionFadeOut));
        for (int i = 0; i < infoTexto.characterCount; i++)
        {
            CambiarAlfaCaracter(i, alpha);
        }
        textoMesh.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
        if (alpha <= 0f)
        {
            textoMesh.enabled = false;
        }
    }

    void RevelarTexto()
    {
        TMP_TextInfo infoTexto = textoMesh.textInfo;
        if (infoTexto.characterCount == 0) return;

        posicionReveladoActual += velocidadRevelado * Time.deltaTime;

        for (int i = 0; i < infoTexto.characterCount; i++)
        {
            if (!infoTexto.characterInfo[i].isVisible) continue;
            TMP_CharacterInfo infoCaracter = infoTexto.characterInfo[i];
            float caracterX = infoCaracter.bottomLeft.x;
            float desvanecer = Mathf.Clamp01((posicionReveladoActual - caracterX) / 50f);
            CambiarAlfaCaracter(i, desvanecer);
        }

        textoMesh.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }

    void CambiarAlfaCaracter(int indiceCaracter, float alfa)
    {
        TMP_TextInfo infoTexto = textoMesh.textInfo;
        if (indiceCaracter >= infoTexto.characterCount || !infoTexto.characterInfo[indiceCaracter].isVisible) return;
        TMP_MeshInfo infoMalla = infoTexto.meshInfo[infoTexto.characterInfo[indiceCaracter].materialReferenceIndex];
        int indiceVertice = infoTexto.characterInfo[indiceCaracter].vertexIndex;
        for (int j = 0; j < 4; j++)
        {
            int indice = indiceVertice + j;
            Color32 color = infoMalla.colors32[indice];
            color.a = (byte)(alfa * 255);
            infoMalla.colors32[indice] = color;
        }
    }

    void DesplazarTexto()
    {
        Vector3 posicionActual = transform.position;
        posicionActual.x -= velocidadDesplazamiento * Time.deltaTime;
        transform.position = posicionActual;
    }
}