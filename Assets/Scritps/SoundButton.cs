using UnityEngine;
using UnityEngine.EventSystems;

public class SoundButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AudioClip hoverSound;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    void Start()
    {
        if (hoverSound == null)
        {
            Debug.LogWarning("No se ha asignado un sonido a hoverSound. Asegúrate de asignarlo en el Inspector.");
        }
        else
        {
            // Precarga el clip en memoria para evitar latencia
            hoverSound.LoadAudioData();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
