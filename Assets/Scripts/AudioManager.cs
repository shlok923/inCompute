// using UnityEngine;
// using System.Collections;

// public class AudioManager : MonoBehaviour
// {
//     [Header("-----SOURCES-----")]
//     [SerializeField] private AudioSource musicSource;
//     [SerializeField] private AudioSource SFXSource;

//     [Header("-----CLIPS-----")]
//     public AudioClip background;
//     public AudioClip keyboardLvl;
//     public AudioClip GPULvl;
//     public AudioClip PowerLvl;
//     public AudioClip MotherboardLvl;

//     public AudioClip boundaryHit;
//     public AudioClip cardPick;
//     public AudioClip damage;
//     public AudioClip electricSpark;
//     public AudioClip fan;
//     public AudioClip fire;
//     public AudioClip flipCard;
//     public AudioClip floating;
//     public AudioClip itemEquip;
//     public AudioClip keyboard;
//     public AudioClip lightSwitch;
//     public AudioClip plugIn;
//     public AudioClip powerCharge;
//     public AudioClip shutDown;

//     private string currentLevelName = "main";
//     private Coroutine fadeCoroutine;

//     private void Start()
//     {
//         PlayMusic(background);
//     }

//     public void PlaySFX(AudioClip clip)
//     {
//         SFXSource.clip = clip;
//         SFXSource.Play();
//     }

//     public void PlayMusic(AudioClip clip)
//     {
//         if (musicSource.clip == clip && musicSource.isPlaying) return;
//         if (fadeCoroutine != null)
//         {
//             StopCoroutine(fadeCoroutine);
//         }
//         fadeCoroutine = StartCoroutine(FadeMusic(clip, 3f));
//     }

//     public void ChangeLevelMusic(string levelName, bool isLevelCleared)
//     {
//         if (!isLevelCleared && levelName == currentLevelName)
//         {
//             return;
//         }

//         currentLevelName = levelName;
//         switch (levelName)
//         {
//             case "KeyboardLevel":
//                 PlayMusic(keyboardLvl);
//                 break;
//             case "GPULevel":
//                 PlayMusic(GPULvl);
//                 break;
//             case "PowerLevel":
//                 PlayMusic(PowerLvl);
//                 break;
//             case "MotherboardLevel":
//                 PlayMusic(MotherboardLvl);
//                 break;
//             default:
//                 PlayMusic(background);
//                 break;
//         }
//     }

//     private IEnumerator FadeMusic(AudioClip newClip, float duration)
//     {
//         float currentVolume = musicSource.volume;
//         for (float t = 0; t < duration / 2; t += Time.deltaTime)
//         {
//             musicSource.volume = Mathf.Lerp(currentVolume, 0, t / (duration / 2));
//             yield return null;
//         }
//         musicSource.volume = 0;
//         musicSource.Stop();
//         musicSource.clip = newClip;
//         musicSource.loop = true;
//         musicSource.Play();
//         for (float t = 0; t < duration / 2; t += Time.deltaTime)
//         {
//             musicSource.volume = Mathf.Lerp(0, currentVolume, t / (duration / 2));
//             yield return null;
//         }
//         musicSource.volume = currentVolume;
//     }
// }

using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    [Header("-----SOURCES-----")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("-----CLIPS-----")]
    public AudioClip background;
    public AudioClip keyboardLvl;
    public AudioClip GPULvl;
    public AudioClip PowerLvl;
    public AudioClip MotherboardLvl;

    public AudioClip boundaryHit;
    public AudioClip cardPick;
    public AudioClip damage;
    public AudioClip electricSpark;
    public AudioClip fan;
    public AudioClip fire;
    public AudioClip flipCard;
    public AudioClip floating;
    public AudioClip itemEquip;
    public AudioClip keyboard;
    public AudioClip lightSwitch;
    public AudioClip plugIn;
    public AudioClip powerCharge;
    public AudioClip shutDown;
    public AudioClip scan;
    public AudioClip whoosh;
    public AudioClip whooshLvlSwitch;

    [Header("-----CARD SOUNDS-----")]
    public AudioClip cardPickup;
    public AudioClip freezeCardUse;
    public AudioClip lockCardUse;
    public AudioClip colorCardUse;

    private string currentLevelName = "main";
    private Coroutine fadeCoroutine;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic(background);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (SFXSource.clip == clip && SFXSource.isPlaying) return;
        SFXSource.clip = clip;
        SFXSource.Play();
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip && musicSource.isPlaying) return;
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeMusic(clip, 3f));
    }

    public void ChangeLevelMusic(string levelName)
    {
        if (levelName == currentLevelName) return;

        currentLevelName = levelName;
        AudioClip selectedMusic = levelName switch
        {
            "KeyboardLevel" => keyboardLvl,
            "GPULevel" => GPULvl,
            "PowerLevel" => PowerLvl,
            "MotherboardLevel" => MotherboardLvl,
            _ => background
        };

        PlayMusic(selectedMusic);
    }

    private IEnumerator FadeMusic(AudioClip newClip, float duration)
    {
        float currentVolume = musicSource.volume;
        for (float t = 0; t < duration / 2; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(currentVolume, 0, t / (duration / 2));
            yield return null;
        }
        musicSource.volume = 0;
        musicSource.Stop();
        musicSource.clip = newClip;
        musicSource.loop = true;
        musicSource.Play();
        for (float t = 0; t < duration / 2; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(0, currentVolume, t / (duration / 2));
            yield return null;
        }
        musicSource.volume = currentVolume;
    }
}