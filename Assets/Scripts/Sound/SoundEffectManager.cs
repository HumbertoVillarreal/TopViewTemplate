using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SoundEffectManager : MonoBehaviour
{
    private static SoundEffectManager Instance;
    private static AudioSource audioSource;
    private static AudioSource randomPitchAudioSource;
    private static AudioSource voiceAudioSource;
    private static SoundEffectLibrary soundEffectLibrary;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private GameObject[] soundBars;


    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            AudioSource[] audioSources = GetComponents<AudioSource>();
            audioSource = audioSources[0];
            randomPitchAudioSource = audioSources[1];
            voiceAudioSource = audioSources[2];
            soundEffectLibrary = GetComponent<SoundEffectLibrary>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void Play(string soundName, bool randomPitch = false)
    {
        AudioClip audioClip = soundEffectLibrary.GetRandromClip(soundName);
        if (audioClip != null) {
            if (randomPitch)
            {
                randomPitchAudioSource.pitch = Random.Range(1f, 1.5f);
                randomPitchAudioSource.PlayOneShot(audioClip);
            }
            else
            {
                audioSource.PlayOneShot(audioClip);
            }
                
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sfxSlider.onValueChanged.AddListener(delegate { OnValueChanged(); });
        OnValueChanged();
    }

    public static void SetVolume(float volume)
    {
        audioSource.volume = volume;
        randomPitchAudioSource.volume = volume;
        voiceAudioSource.volume = volume;
        BgMusicManager.instance.getAudioSource().volume = volume;

        //Round for bars
        float rounded = Mathf.Clamp(Mathf.Round((volume * 10) / 2f) * 2f, 0f, 10f);
        SetBars(rounded);
        //Debug.Log("Volume: "+ volume);
    }

    public void OnValueChanged()
    {
        SetVolume(sfxSlider.value);
    }

    public static void SetBars(float rounded)
    {
   
        int filledBars = (int)(rounded / 2f);


        for (int i = 0; i < Instance.soundBars.Length; i++)
        {
            Image img = Instance.soundBars[i].GetComponent<Image>();

            if (i < filledBars)
                img.color = Color.green;   // filled
            else
                img.color = Color.white;    // empty
        }
    }


    public static void PlayVoice(AudioClip audioClip, float pitch = 1f)
    {
        voiceAudioSource.pitch = pitch;
        voiceAudioSource.PlayOneShot(audioClip);
    }

    public void IncreaseVolume()
    {
        sfxSlider.value = Mathf.Clamp(sfxSlider.value + .2f, 0f, 1f);
    }

    public void DecreaseVolume()
    {
        sfxSlider.value = Mathf.Clamp(sfxSlider.value - .2f, 0f, 1f);
    }
}
