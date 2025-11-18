using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] public bool IsEneable = true;

    [Header("Animator & Animation Settings")]
    [SerializeField] protected private Animator animator;
    [Tooltip("Name of the Animator parameter that will be activated on interaction (Trigger).")]
    [SerializeField] protected private string animationTriggerName = "Interact";
    
    [Header("Sound effect")]
    [Tooltip("If the object has sound effect need to add AudioSource component to work")]
    [SerializeField] protected private AudioClip _sfxSource;
    [SerializeField] protected private AudioSource _audioSource;

    private void Awake()
    {
        if (_audioSource != null)
        {
            _audioSource.loop = false;
        }
    }
    public virtual void Interact()
    {
        if (animator != null && !string.IsNullOrEmpty(animationTriggerName))
        {
            animator.SetTrigger(animationTriggerName);
        }

        if (_sfxSource != null && _audioSource != null)
        {
            _audioSource.resource = _sfxSource;
            _audioSource.Play();
        }
    }
}
