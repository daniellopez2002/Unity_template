using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Volume settings")]
    [SerializeField] private float _generalVolume = 100.0f;
    [SerializeField] private float _musicVolume = 100.0f;
    [SerializeField] private float _sfxVolume = 100.0f;

    [Header("Mixer")]
    [SerializeField] private AudioMixer _audioMixer;

    [Header("Sources")]
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioSource _musicSource ;
    public float GeneralVolume
    {
        get => _generalVolume;
        private set => _generalVolume = value;
    }
    public float MusicVolume
    {
        get => _musicVolume;
        private set => _musicVolume = value;
    }
    public float SFXVolume
    {
        get => _sfxVolume;
        private set => _sfxVolume = value;
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
        GeneralVolume = newVolume;
        _audioMixer.SetFloat("Master", GeneralVolume);
    }
    public void SetMusicVolume(float newVolume)
    {
        MusicVolume = newVolume;
        _audioMixer.SetFloat("Music", MusicVolume);
    }

    public void SetSFXVolume(float newVolume)
    {
        SFXVolume = newVolume;
        _audioMixer.SetFloat("SFX", SFXVolume);
    }

    public void PlayMusic(AudioClip source, bool loop = true)
    {
        if(source == null)
        {
            _musicSource.resource = null;
            _musicSource.mute = true;
            return;
        }
        _musicSource.mute = false;
        _musicSource.loop = loop;
        _musicSource.resource = source;
        _musicSource.Play();
    }

    public void PlaySFX(AudioClip source)
    {
        _sfxSource.resource = source;
        _sfxSource.Play();
    }
}
