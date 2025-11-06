using System;
using UnityEngine;
using UnityEngine.UI;

public class SoundEffectManager : MonoBehaviour
{
    private static SoundEffectManager Instance;
    private static AudioSource audioSource;
    private static SoundEffectLibrary soundEffectLibrary;
    
    [SerializeField] private Slider sfxSlider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            soundEffectLibrary = GetComponent<SoundEffectLibrary>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        sfxSlider.onValueChanged.AddListener(delegate { OnValueChanged(); });
    }

    public static void PlaySoundEffect(string soundName)
    {
        if (Instance == null || soundEffectLibrary == null || audioSource == null)
        {
            Debug.LogWarning($"SoundEffectManager not ready. Cannot play: {soundName}");
            return;
        }
        
        AudioClip clip = soundEffectLibrary.GetRandomSoundEffect(soundName);
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"Sound effect '{soundName}' not found in library.");
        }
    }

    public static void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
    
    public void OnValueChanged()
    {
        SetVolume(sfxSlider.value);
    }

}
