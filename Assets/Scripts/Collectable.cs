using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Collectable : Interactable
{
    [Header("Collectable setting")]
    [SerializeField] public string CollectableInfo = "This is new thing";
    [SerializeField] public GameObject InspectModelPrefab;

    [Header("Flags")]
    [SerializeField] public bool IsBig = false;
    [SerializeField] public bool IsMedium = false;
    [SerializeField] public bool IsSmall = false;

    private Rigidbody _rb;

    private void Awake()
    {   
        _rb = GetComponent<Rigidbody>();
    }

    public override void Interact()
    {
        if (!IsEneable) return;

        Debug.Log($"[Collectable] Opening inspection for {gameObject.name}");
        base.Interact();

        PlayerController.Instance.PlayerHUD.DisplayInpection(true, this);
    }
    public void EnableRigidBody(bool enable)
    {
        if(enable)
        {
            _rb.isKinematic = false;
            _rb.useGravity = true;
        }
        else
        {
            _rb.isKinematic = true;
            _rb.useGravity = false;
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }
    }
    public void Drop(Transform playerTransform)
    {
        if (transform.parent != null)
            transform.SetParent(null);

        EnableRigidBody(true);

        transform.position = playerTransform.position + playerTransform.forward * 0.7f + Vector3.up * 0.5f;

        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        float throwForce = 4f;
        _rb.AddForce(playerTransform.forward * throwForce, ForceMode.Impulse);
        _rb.AddTorque(Random.insideUnitSphere * 3f, ForceMode.Impulse);

        Debug.Log($"[Collectable] Dropped {name} with forward impulse");
    }
}
