using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonEscena : MonoBehaviour
{
    // Método para cargar la escena de juego
    public void CambiarEscenaJugar()
    {
        string nombreEscena = "Transicion1"; // Asigna el nombre de la escena de juego
        if (!string.IsNullOrEmpty(nombreEscena))
        {
            SceneManager.LoadScene(nombreEscena);
        }
    }

    // Método para salir del juego
    public void SalirDelJuego()
    {
        // Si estamos en el editor, detenemos la ejecución
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // Si estamos en la versión final, cerramos la aplicación
        Application.Quit();
        #endif
    }
}
