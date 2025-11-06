

using System;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    private static MusicManager Instance;
    private AudioSource audioSource;
    public AudioClip[] songs;
    [SerializeField] private Slider musicSlider;

    [Range(0, 10)]
    public int inspectorSongIndex = 0;

    private int currentSongIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (songs != null && songs.Length > 0)
        {
            PlaySong(0);
        }

        musicSlider.onValueChanged.AddListener(delegate { SetVolume(musicSlider.value); });
    }

    public static void SetVolume(float volume)
    {
        Instance.audioSource.volume = volume;
    }

    public static void PlaySong(int index)
    {
        if (Instance.songs == null || Instance.songs.Length == 0) return;
        if (index < 0 || index >= Instance.songs.Length) return;
        Instance.currentSongIndex = index;
        Instance.audioSource.clip = Instance.songs[index];
        Instance.audioSource.Play();
    }

    public static void PlayNextSong()
    {
        if (Instance.songs == null || Instance.songs.Length == 0) return;
        int nextIndex = (Instance.currentSongIndex + 1) % Instance.songs.Length;
        PlaySong(nextIndex);
    }

    public static void PlayPreviousSong()
    {
        if (Instance.songs == null || Instance.songs.Length == 0) return;
        int prevIndex = (Instance.currentSongIndex - 1 + Instance.songs.Length) % Instance.songs.Length;
        PlaySong(prevIndex);
    }

    public static void PlayBackgroundMusic(bool resetSong, AudioClip audioClip = null)
    {
        if (audioClip != null)
        {
            Instance.audioSource.clip = audioClip;
            Instance.audioSource.Play();
        }
        else if (Instance.audioSource.clip != null)
        {
            if (resetSong)
            {
                Instance.audioSource.Stop();
            }

            Instance.audioSource.Play();
        }
    }

    public static void PauseBackgroundMusic()
    {
        Instance.audioSource.Pause();
    }

    public static class MusicIndex
    {
        public const int Placeholder = 0;
        public const int Menu = 1;
        public const int Level1 = 2;
        public const int Level2 = 3;
        public const int Level3 = 4;
        public const int Level4 = 5;
        public const int Level5 = 6;
        public const int Level6 = 7;
        public const int Level7 = 8;
        public const int Level8 = 9;
        public const int Level9 = 10;
        public const int Level10 = 11;
    }

    public static void PlayPlaceholder() => PlaySong(MusicIndex.Placeholder);
    public static void PlayMenuMusic() => PlaySong(MusicIndex.Menu);
    /// <summary>
    /// Level index is 1+level number (e.g., level 1 = index 2)
    /// </summary>
    /// <param name="level"></param>
    public static void PlayLevelMusic(int level)
    {
        int index = level + 1;
        PlaySong(index);
    }

#if UNITY_EDITOR
    [ContextMenu("Play Selected Song")]
    public void PlaySelectedSongFromInspector()
    {
        PlaySong(inspectorSongIndex);
    }
#endif
}