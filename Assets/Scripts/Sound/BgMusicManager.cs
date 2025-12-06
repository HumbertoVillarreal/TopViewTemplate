using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BgMusicManager : MonoBehaviour
{

    public static BgMusicManager instance;
    private AudioSource audioSource;
    [SerializeField] public float fadeTime = 1f;
    [SerializeField] BgMusicLibrary library;


    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;

    }

    public void PlayMusic(AudioClip clip)
    {
        if(audioSource.clip == clip)
        {
            return;
        }

        StartCoroutine(FadeTo(clip));
    }

    public void PlayMusicByAreaName(string name)
    {
        AudioClip clip = library.GetClip(name);

        Debug.Log(clip.name);

        if (clip == null) { return; }

        if (audioSource.clip == clip)
        {
            return;
        }

        StartCoroutine(FadeTo(clip));
    }

    public IEnumerator FadeTo(AudioClip newClip)
    {

        float startVolume = audioSource.volume;

        for (float t = 0; t < fadeTime; t += Time.deltaTime) {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeTime);
            yield return null;
        }


        audioSource.clip = newClip;
        audioSource.Play();


        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0f, startVolume, t / fadeTime);
            yield return null;
        }

    }

    public AudioSource getAudioSource()
    {
        return audioSource;
    }


}
