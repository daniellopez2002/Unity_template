
using UnityEngine;

public class Door: Interactable
{
    [Header("Door settings")]
    [SerializeField] public bool IsOpen = false;
    [SerializeField] private AudioClip _SFXWrong;
    
    [Header("Door References")]
    [SerializeField] public Collectable KeyCollectable;

    private PlayerController controller;

    private void Start()
    {
        controller = PlayerController.Instance;
    }

    public override void Interact()
    {
        if(IsOpen)
        {
            animator.SetTrigger(animationTriggerName);
            return;
        }

        if (controller.PlayerInventory.CurrentItem == KeyCollectable)
        {
            animator.SetTrigger(animationTriggerName);
            IsOpen = true;
        }
        else
        {
            animator.SetTrigger("Wrong");
        }
    }

    //This method is called in the open door animation
    public void PlayDoorSFX()
    {
        _audioSource.resource = _sfxSource;
        _audioSource.Play();
    }

    public void PlayWrongSFX()
    {
        _audioSource.resource = _SFXWrong;
        _audioSource.Play();
    }
}