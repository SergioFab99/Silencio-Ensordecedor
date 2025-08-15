using UnityEngine;

public class AudioController : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private float delayTime = 10f;
    [SerializeField] private float volume = 1f;
    [SerializeField] private bool loop = false;
    
    [Header("Debug Info")]
    [SerializeField] private bool isPlaying = false;
    [SerializeField] private float timeRemaining;
    
    private AudioSource audioSource;
    
    void Awake()
    {
        if (audioClip == null)
        {
            Debug.LogError($"No se ha asignado AudioClip en {gameObject.name}. Arrastra tu MP3 al campo 'Audio Clip'.");
            return;
        }
        
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = null;
        audioSource.playOnAwake = false;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.Stop();
        audioSource.enabled = false;
        
        Debug.Log($"AudioController configurado con: {audioClip.name}. Reproducción en {delayTime} segundos.");
    }
    
    void Start()
    {
        if (audioClip != null)
        {
            Invoke(nameof(PlayAudio), delayTime);
            timeRemaining = delayTime;
        }
    }
    
    void Update()
    {
        if (!isPlaying && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
            }
        }
        
        if (isPlaying && audioSource != null && !audioSource.isPlaying && !loop)
        {
            isPlaying = false;
        }
    }
    
    private void PlayAudio()
    {
        if (audioSource != null && audioClip != null)
        {
            audioSource.enabled = true;
            audioSource.clip = audioClip;
            audioSource.Play();
            isPlaying = true;
            timeRemaining = 0;
            Debug.Log($"Reproduciendo: {audioClip.name}");
        }
        else
        {
            Debug.LogWarning($"No se pudo reproducir audio en {gameObject.name}");
        }
    }
    
    public void RestartDelayedPlayback()
    {
        CancelInvoke(nameof(PlayAudio));
        
        if (audioSource != null)
        {
            audioSource.Stop();
        }
        
        isPlaying = false;
        timeRemaining = delayTime;
        
        Invoke(nameof(PlayAudio), delayTime);
        
        Debug.Log($"Reproducción reiniciada. Nuevo delay: {delayTime} segundos.");
    }
    
    public void CancelDelayedPlayback()
    {
        CancelInvoke(nameof(PlayAudio));
        isPlaying = false;
        timeRemaining = 0;
        
        if (audioSource != null)
        {
            audioSource.Stop();
        }
        
        Debug.Log("Reproducción cancelada.");
    }
    
    public void StopAudio()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
        isPlaying = false;
        Debug.Log("Audio detenido.");
    }
    
    public void SetAudioClip(AudioClip newAudioClip)
    {
        audioClip = newAudioClip;
        if (audioSource != null)
        {
            audioSource.clip = audioClip;
        }
        Debug.Log($"AudioClip cambiado a: {newAudioClip.name}");
    }
    
    public void SetDelayTime(float newDelayTime)
    {
        delayTime = Mathf.Max(0, newDelayTime);
        Debug.Log($"Nuevo delay time: {delayTime} segundos");
    }
    
    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume);
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }
    
    void OnDisable()
    {
        CancelInvoke(nameof(PlayAudio));
        if (audioSource != null)
        {
            audioSource.Stop();
        }
        isPlaying = false;
    }
    
    void OnDestroy()
    {
        CancelInvoke(nameof(PlayAudio));
        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.clip = null;
        }
    }
}