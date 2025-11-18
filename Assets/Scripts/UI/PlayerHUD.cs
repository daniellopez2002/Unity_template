using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("Pointer")]
    [SerializeField] private GameObject _pointer;
    [SerializeField] private TMP_Text _pointerText;
    [SerializeField] private RawImage _pointerImage;

    [Header("Pause Menu")]
    [SerializeField] private GameObject _pauseMenu;

    [Header("Inspection")]
    [SerializeField] private GameObject _inspectionMenu;

    [Header("Game Over")]
    [SerializeField] private GameObject _gameOverInterface;

    [Header("End Game")]
    [SerializeField] private GameObject _endGameInterface;
    
    
    private Inspection _inspectionUI;

    private InputAction _pauseAction;

    private void Awake()
    {
        DisablePointerText();
        DisplayPointer(true);
    }

    private void Start()
    {
        _inspectionUI = _inspectionMenu.GetComponent<Inspection>();
    }
    public void DisplayPointer(bool enable)
    {
        _pointer.SetActive(enable);

        if (enable)
        {
            Cursor.lockState = CursorLockMode.Locked;
            _pauseMenu.SetActive(false);
            _inspectionMenu.SetActive(false);
            _endGameInterface.SetActive(false);
            _gameOverInterface.SetActive(false);
        }
    }
    public  void DisplayPauseMenu(bool enable)
    {
        _pauseMenu.SetActive(enable);

        if(enable)
        {
            Cursor.lockState = CursorLockMode.None;
            _pointer.SetActive(false);
            _inspectionMenu.SetActive(false);
            _endGameInterface.SetActive(false);
            _gameOverInterface.SetActive(false);
        }
    }
    public void DisplayInpection(bool enable, Collectable collectableObject = null)
    {
        _inspectionMenu.SetActive(enable);

        if (enable)
        {
            _pointer.SetActive(false);
            _pauseMenu.SetActive(false);
            _endGameInterface.SetActive(false);
            _gameOverInterface.SetActive(false);
            if (collectableObject != null)
            {
                InspectionManager.Instance.StartInspection(collectableObject);
                _inspectionUI.SetText(collectableObject.CollectableInfo);
            }

        }
    }

    public void DisplayGameOver(bool enable)
    {
        _gameOverInterface.SetActive(enable);

        if (enable)
        {
            Cursor.lockState = CursorLockMode.None;
            _pointer.SetActive(false);
            _pauseMenu.SetActive(false);
            _inspectionMenu.SetActive(false);
            _endGameInterface.SetActive(false);
        }
    }

    public void DisplayEndGame(bool enable)
    {
        _endGameInterface.SetActive(enable);

        if(enable)
        {
            Cursor.lockState = CursorLockMode.None;
            _pointer.SetActive(false);
            _pauseMenu.SetActive(false);
            _inspectionMenu.SetActive(false);
            _gameOverInterface.SetActive(false);
        }
    }

    public void ChangePointerText(string text)
    {
        _pointerImage.enabled = true;
        _pointerText.text = text;
    }

    public void EnablePointerText()
    {
        _pointerText.enabled = true;
    }

    public void DisablePointerText()
    {
        _pointerText.enabled = false;
    }

    public void RestartLevel()
    {
        GameManager.Instance.Loadlevel("MainLevel");
    }

    public void BackToMenu()
    {
        GameManager.Instance.Loadlevel("MainMenu");
    }
}
