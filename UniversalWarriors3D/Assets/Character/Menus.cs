using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
    [Header("All Menu's")]
    public GameObject pauseMenuUI;
    public GameObject endGameMenuUI;
    
    

    public static bool GameIsPause = false;
    private LocomotionInputAction _locomotionInputAction;
    private LocomotionInputAction.LocomotionActions _inputActions;
    
    
    
    

    private void Awake()
    {
        _locomotionInputAction = new LocomotionInputAction();
        _locomotionInputAction.Locomotion.Enable();
        _inputActions = _locomotionInputAction.Locomotion;
        
    }

    private void Update()
    {
        if (_inputActions.Pause.WasPressedThisFrame())
        {
            if (GameIsPause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false); 
        Time.timeScale = 1f;
        GameIsPause = false;
    }

    public void Restart()
    {
        SceneManager.LoadScene("The Bronx");
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("PlayGround");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quiting");
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPause = true;
    }

}
