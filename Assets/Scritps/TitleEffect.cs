using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(TMP_Text))]
public class TitleEffect : MonoBehaviour
{
    [Header("Parámetros")]
    public float interval = 1f;        // Cada cuántos segundos ocurre el glitch
    public float skewFactor = 1.0f;    // Magnitud de inclinación
    public float returnDuration = 0.15f; // Tiempo que tarda en volver a la normalidad

    private TMP_Text tmp;
    private TMP_MeshInfo[] cachedMeshInfo;

    void Awake()
    {
        tmp = GetComponent<TMP_Text>();
    }

    void OnEnable()
    {
        StartCoroutine(GlitchLoop());
    }

    IEnumerator GlitchLoop()
    {
        while (true)
        {
            // Espera hasta el siguiente disparo
            yield return new WaitForSeconds(interval);

            // Cachea la malla original
            tmp.ForceMeshUpdate();
            cachedMeshInfo = tmp.textInfo.CopyMeshInfoVertexData();

            TMP_TextInfo textInfo = tmp.textInfo;

            // Aplica la inclinación (skew) inicial a todos los caracteres visibles
            for (int i = 0; i < textInfo.characterCount; i++)
            {
                if (!textInfo.characterInfo[i].isVisible)
                    continue;

                int matIndex = textInfo.characterInfo[i].materialReferenceIndex;
                int vertIndex = textInfo.characterInfo[i].vertexIndex;

                Vector3[] sourceVertices = cachedMeshInfo[matIndex].vertices;
                Vector3[] destVertices   = textInfo.meshInfo[matIndex].vertices;

                for (int j = 0; j < 4; j++)
                {
                    Vector3 orig = sourceVertices[vertIndex + j];
                    float y = orig.y;                        // Altura del vértice
                    destVertices[vertIndex + j] = orig + new Vector3(y * skewFactor, 0, 0);
                }
            }

            tmp.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);

            // Espera un frame y restaura la malla original
            yield return null;
            // Restaurar manualmente los vértices originales
            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                Vector3[] src = cachedMeshInfo[i].vertices;
                Vector3[] dst = textInfo.meshInfo[i].vertices;
                for (int j = 0; j < src.Length; j++)
                {
                    dst[j] = src[j];
                }
            }
            tmp.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
        }
    }
}
