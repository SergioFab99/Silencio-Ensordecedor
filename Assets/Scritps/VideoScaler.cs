using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoScaler : MonoBehaviour
{
    public RawImage rawImage;  // El RawImage donde se reproducirá el video
    public VideoPlayer videoPlayer; // El VideoPlayer que está reproduciendo el video

    void Start()
    {
        // Asegúrate de que el RawImage ocupe toda la pantalla
        ForceScaleToScreen();
    }

    void ForceScaleToScreen()
    {
        // Establecer el tamaño del RawImage al tamaño de la pantalla
        rawImage.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);

        // Asegúrate de que el VideoPlayer utilice la Render Texture correctamente
        if (videoPlayer != null)
        {
            // Ajustar el tamaño de la textura para que ocupe toda la pantalla
            videoPlayer.renderMode = VideoRenderMode.APIOnly; // Usar API para la textura
            videoPlayer.targetTexture = null; // Desactivar la Render Texture

            // Deformar el video para que ocupe todo el espacio sin mantener el aspecto
            rawImage.texture = videoPlayer.texture;
        }
    }
}
