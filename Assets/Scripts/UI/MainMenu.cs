using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("level reference")]
    [SerializeField] private string _firstLevelName = "NN";

    [Header("Buttons")]
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _quitButton;

    private void Awake()
    {
        _playButton.onClick.AddListener(() => GameManager.Instance.Loadlevel(_firstLevelName));
        _quitButton.onClick.AddListener(() => GameManager.Instance.QuitGame());
    }
}
