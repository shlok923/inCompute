using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("-----SOURCES-----")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("-----CLIPS-----")]
    public AudioClip background;
    // add all the clips needed here as audio clips

    private void Start()
    {
        PlayMusic(background);
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.clip = clip;
        SFXSource.Play();
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }
}