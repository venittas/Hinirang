using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    public AudioSource audioSource;
    public AudioClip titleMusic;
    public AudioClip introMusic;
    public AudioClip rushMusic;
    public AudioClip hinirangMusic;

    public enum MusicTrack { Title, Intro, Rush, Hinirang, None }
    private MusicTrack currentTrack = MusicTrack.None;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayTrack(MusicTrack track, bool forceRestart = false)
    {
        if (currentTrack == track && !forceRestart) return;

        currentTrack = track;
        AudioClip clip = track switch
        {
            MusicTrack.Title => titleMusic,
            MusicTrack.Intro => introMusic,
            MusicTrack.Rush => rushMusic,
            MusicTrack.Hinirang => hinirangMusic,
            _ => null
        };

        if (clip == null) return;
        StartCoroutine(CrossFade(clip));
    }

    public void StopMusic()
    {
        StartCoroutine(FadeOut());
        currentTrack = MusicTrack.None;
    }

    private IEnumerator CrossFade(AudioClip newClip, float fadeDuration = 1f)
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.clip = newClip;
        audioSource.loop = true;
        audioSource.Play();

        while (audioSource.volume < startVolume)
        {
            audioSource.volume += startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
        audioSource.volume = startVolume;
    }

    private IEnumerator FadeOut(float fadeDuration = 1f)
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
        audioSource.Stop();
    }
}