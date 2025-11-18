using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InspectionManager : MonoBehaviour
{
    public static InspectionManager Instance;

    [Header("Flags")]
    [SerializeField] public bool Inspecting = false;
    [SerializeField] private bool _isBig;
    [SerializeField] private bool _isMedium;
    [SerializeField] private bool _isSmall;

    public bool IsBig
    {
        get => _isBig;
        set
        {
            _isBig = value;
            if (value)
            {
                _isMedium = false;
                _isSmall = false;
            }
        }
    }
    public bool IsMedium
    {
        get => _isMedium;
        set
        {
            _isMedium = value;
            if (value)
            {
                _isBig = false;
                _isSmall = false;
            }
        }
    }
    public bool IsSmall
    {
        get => _isSmall;
        set
        {
            _isSmall = value;
            if (value)
            {
                _isBig = false;
                _isMedium = false;
            }
        }
    }


    [Header("3D Model Holder")]
    [SerializeField]
    private Transform modelContainer;
    [SerializeField]
    private FanRotation _rotation;

    [Header("Rotation Settings")]
    public float rotationSpeed = 80f;

    [Header("Position Settings")]
    [SerializeField]
    private Vector3 _containerPositionSmallObjects = Vector3.zero;
    [SerializeField]
    private Vector3 _containerPositionMediumObjects = Vector3.zero;
    [SerializeField]
    private Vector3 _containerPositionBigObjects = Vector3.zero;

    private GameObject _currentModel;
    private Collectable _currentCollectable;

    PlayerController _playerController;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        _playerController = PlayerController.Instance;
    }

    public void StartInspection(Collectable collectable)
    {
        Inspecting = true;
        _currentCollectable = collectable;

        _currentModel = Instantiate(collectable.InspectModelPrefab, modelContainer);
        _currentModel.transform.localPosition = Vector3.zero;

        _isBig = _currentCollectable.IsBig;
        _isMedium = _currentCollectable.IsMedium;
        _isSmall = _currentCollectable.IsSmall;

        _playerController.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        if(!Inspecting) return;
        HandleObjectSize();
    }

    private void HandleObjectSize()
    {
        _rotation.RotationSpeed = rotationSpeed;

        if (_isSmall) modelContainer.transform.localPosition = _containerPositionSmallObjects;
        if(_isMedium) modelContainer.transform.localPosition = _containerPositionMediumObjects;
        if (_isBig) modelContainer.transform.localPosition = _containerPositionBigObjects;
    }
    
    public void ConfirmPickup()
    {
        PlayerInventory inventory = _playerController.GetComponent<PlayerInventory>();
        inventory.AddItem(_currentCollectable);

        ExitInspection();
        _currentCollectable.gameObject.SetActive(false);
    }

    public void Reject()
    {
        ExitInspection();
        _currentCollectable.Drop(_playerController.transform);
    }

    public void ExitInspection()
    {
        Inspecting = false;

        _playerController.PlayerHUD.DisplayPointer(true);

        // Destruir modelo instanciado
        if (_currentModel != null)
            Destroy(_currentModel);

        // Restaurar control del jugador
        PlayerController.Instance.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
