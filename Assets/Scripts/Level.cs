

using UnityEngine;

public class Level:MonoBehaviour
{
    [Header("Level setting")]
    [SerializeField] private AudioClip _mainMusic;

    private void Start()
    {
        PlayMusic(_mainMusic);
    }
    public void PlaySFX(AudioClip clip)
    {
        AudioManager.Instance.PlaySFX(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        AudioManager.Instance.PlayMusic(clip);
    }

    public void StopMusic()
    {
        AudioManager.Instance.PlayMusic(null);
    }
}