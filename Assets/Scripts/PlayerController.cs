using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [Header("Player references")]
    [SerializeField]
    public PlayerHUD PlayerHUD;
    [SerializeField]
    public PlayerInventory PlayerInventory;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 4f;
    [SerializeField] private float gravity = -20f;

    [Header("Stamina")]
    [SerializeField] private float maxStamina = 5f;
    [SerializeField] private float staminaDrainPerSecond = 1.2f;
    [SerializeField] private float staminaRegenPerSecond = 0.8f;
    [SerializeField] private float staminaRegenDelay = 1.5f;

    [Header("Camera")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float mouseSensitivity = 0.5f;
    [SerializeField] private float verticalClamp = 80f;

    [SerializeField] private CameraEffects _cameraEffects;

    [Header("Interaction")]
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private LayerMask interactionMask;

    private CharacterController _controller;
    private float _cameraPitch = 0f;
    private Vector3 _velocity;
    private bool _isGrounded;
    private Transform _initialTransform;

    private float _currentStamina;
    private float _regenTimer;

    private bool _isDead = false;
    private bool _isEndGame = false;
    private bool _isPaused = false;

    private InputAction _moveAction;
    private InputAction _lookAction;
    private InputAction _sprintAction;
    private InputAction _interactionAction;
    private InputAction _changeinventoryAction;
    private InputAction _dropItemAction;
    private InputAction _pauseAction;
    private InputAction _shootAction;


    private void Awake()
    {
        Instance = this;

        _isEndGame = false;
        _isDead = false;
        _isPaused = false;

        _moveAction = InputSystem.actions.FindAction("Move");
        _lookAction = InputSystem.actions.FindAction("Look");
        _sprintAction = InputSystem.actions.FindAction("Sprint");
        _interactionAction = InputSystem.actions.FindAction("Interact");
        _changeinventoryAction = InputSystem.actions.FindAction("ChangeItemInventory");
        _dropItemAction = InputSystem.actions.FindAction("DropItem");
        _shootAction = InputSystem.actions.FindAction("Attack");
        _pauseAction = InputSystem.actions.FindAction("Pause");

        _lookAction.started += context =>
        {
            HandleLook(context.action.ReadValue<Vector2>());
        };
        _dropItemAction.started += context => PlayerInventory.DropCurrentItem();
        _shootAction.started += context => HandleShootAction();
        _pauseAction.started += context => HandlePause();
    }

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        PlayerInventory = GetComponent<PlayerInventory>();
        _initialTransform = transform;
        _currentStamina = maxStamina;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if(_isPaused || _isEndGame || _isDead) return;

        HandleMovement();
        HandleStamina();
        CheckForInteractable();
        HandleChangeInventory(_changeinventoryAction.ReadValue<Vector2>());
    }

    private void HandleShootAction()
    {
        if (_isPaused || _isEndGame || _isDead) return;

        if (PlayerInventory.CurrentItem is FlashLight flashLight)
        {
            flashLight.SwitchLight();
        }
    }

    public void HandlePause()
    {
        if (_isEndGame || _isDead) return;

        _isPaused = !_isPaused;
        GameManager.Instance.IsPause = _isPaused;

        if (_isPaused)
        {
            PlayerHUD.DisplayPauseMenu(true);
        }else
        {
            PlayerHUD.DisplayPointer(true);
        }
        
    }
    private void HandleLook(Vector2 direction)
    {
        if (InspectionManager.Instance.Inspecting || _isPaused || _isEndGame || _isDead) return;

        _cameraPitch -= direction.y;
        _cameraPitch = Mathf.Clamp(_cameraPitch, -verticalClamp, verticalClamp);
        cameraTransform.localRotation = Quaternion.Euler(_cameraPitch, 0f, 0f);

        transform.Rotate(Vector3.up * direction.x);
    }
    private void HandleChangeInventory(Vector2 direction)
    {
        double value = direction.y;

        if (value > 0)
        {
            Debug.Log("[PlayerController] changing inventory +");
            PlayerInventory.NextItem();
        }
        else if (value < 0)
        {
            Debug.Log("[PlayerController] changing inventory -");
            PlayerInventory.PrevItem();
        }
        
    }

    private void HandleMovement()
    {
        _isGrounded = _controller.isGrounded;

        if (_isGrounded && _velocity.y < 0)
            _velocity.y = -2f;

        Vector2 moveValue = _moveAction.ReadValue<Vector2>();
        bool wantsToRun = _sprintAction.IsPressed();

        bool canRun = _currentStamina > 0f && moveValue.y > 0f;
        bool isRunning = wantsToRun && canRun;

        float speed = isRunning ? runSpeed : walkSpeed;

        Vector3 move = transform.right * moveValue.x + transform.forward * moveValue.y;
        _controller.Move(move * speed * Time.deltaTime);

        _velocity.y += gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);

        if (isRunning)
        {
            _currentStamina -= staminaDrainPerSecond * Time.deltaTime;
            if (_currentStamina < 0f)
                _currentStamina = 0f;

            _regenTimer = 0f;
        }
        else
        {
            _regenTimer += Time.deltaTime;
        }

    }

    private void HandleStamina()
    {
        if (_regenTimer >= staminaRegenDelay && _currentStamina < maxStamina)
        {
            _currentStamina += staminaRegenPerSecond * Time.deltaTime;
            if (_currentStamina > maxStamina)
                _currentStamina = maxStamina;
        }
    }

    public float GetStaminaNormalized()
    {
        return _currentStamina / maxStamina;
    }

    private void CheckForInteractable()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

        Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.red);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactionMask))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable)
            {
                PlayerHUD.EnablePointerText();
                switch (interactable)
                {
                    case Collectable:
                        PlayerHUD.ChangePointerText("Press 'E' to inspect");
                        break;
                    case Door d:
                        if (PlayerInventory.Objects.Contains(d.KeyCollectable)  || d.IsOpen)
                            PlayerHUD.ChangePointerText("Press 'E' to open");
                        else 
                            PlayerHUD.ChangePointerText("Find the key");
                        break;
                    default:
                        PlayerHUD.ChangePointerText("Press 'E' to interact");
                        break;
                }
                if(_interactionAction.IsPressed()) interactable.Interact();
            }
            else PlayerHUD.DisablePointerText();
        }
        else
        {
            PlayerHUD.DisablePointerText();
        }
    }

    public void Kill()
    {
        _isDead = true;
        PlayerHUD.DisplayGameOver(true);
    }

    public void EndGame()
    {
        _isEndGame = true;
        PlayerHUD.DisplayEndGame(true);
    }
}
