using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Elevator : MonoBehaviour
{
    [Header("Aniamtion components")]
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private string _animationTriggerParameter = "Interact";

    [Header("SFX")]
    [SerializeField]
    private AudioSource _sourceAudio;
    [SerializeField]
    private AudioClip _sfxDoorOpen;
    [SerializeField]
    private AudioClip _sfxDoorClose;
    [SerializeField]
    private AudioClip _sfxMovement;

    private bool _isUsed = false;
    private void Awake()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isUsed) return;
        if (other.CompareTag("Player"))
        {
            Debug.Log("El jugador entró al ascensor");
            _isUsed = true;
            other.gameObject.transform.SetParent(gameObject.transform);
            _animator.SetTrigger(_animationTriggerParameter);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isUsed = false;
            Debug.Log("El jugador salió del ascensor");
            other.gameObject.transform.SetParent(null);
        }
    }

    public void PlayCloseSFX()
    {
        _sourceAudio.resource = _sfxDoorClose;
        _sourceAudio.Play();
    }
}
