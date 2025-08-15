using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CambioEscena : MonoBehaviour
{
    public string nombreSiguienteEscena;
    private VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
        }
        else
        {
            Debug.LogError("No se encontró un componente VideoPlayer en este objeto.");
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        if (!string.IsNullOrEmpty(nombreSiguienteEscena))
        {
            SceneManager.LoadScene(nombreSiguienteEscena);
        }
        else
        {
            Debug.LogError("No se ha asignado el nombre de la siguiente escena en CambioEscena.");
        }
    }
}
