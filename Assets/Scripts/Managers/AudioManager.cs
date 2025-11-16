using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioManager Instance;

    [SerializeField] private float generalVolume = 100.0f;
    [SerializeField] private float musicVolume = 100.0f;
    [SerializeField] private float sfxVolume = 100.0f;
    public float GeneralVolume
    {
        get => generalVolume;
        private set => generalVolume = value;
    }
    public float MusicVolume
    {
        get => musicVolume;
        private set => musicVolume = value;
    }
    public float SFXVolume
    {
        get => sfxVolume;
        private set => sfxVolume = value;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {

            Destroy(gameObject);
        }
    }

    public void SetGeneralVolume(float newVolume)
    {

    }
    public void SetMusicVolume(float newVolume)
    {

    }

    public void SetSFXVolume(float newVolume)
    {

    }

    public void PlayMusic(AudioSource source, bool loop = true)
    {
        source.loop = loop;
    }

    public void PlaySFX(AudioSource source)
    {
        source.loop = false;
    }
}
