using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
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
        
        InitializeGame();
    }
    
    private void InitializeGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Loadlevel(string levelName)
    {
        try
        {
            SceneManager.LoadScene(levelName);
        }
        catch
        {
            Debug.LogError($"Fail trying open level {levelName}");
        }
    }
    
    
    public void QuitGame()
    {
        Debug.Log("Quit game");
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
