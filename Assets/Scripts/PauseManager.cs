using System;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject pauseTitle;
    private GameObject pauseObjInstance;
    public bool isPaused;
    private Action OnGamePaused;
    private Action OnGameUnpaused;
    void Awake()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    void OnEnable()
    {
        OnGamePaused += PauseGame;
        OnGameUnpaused += UnpauseGame;
    }
    void OnDisable()
    {
        OnGamePaused -= PauseGame;
        OnGameUnpaused -= UnpauseGame;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) | Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                isPaused = false;
                OnGameUnpaused?.Invoke();
            }
            else
            {
                isPaused = true;
                OnGamePaused?.Invoke();
            }
            
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        pauseObjInstance = Instantiate(pauseTitle, new Vector3(0,100,0), Quaternion.identity, pauseMenu.transform);
    }
    void UnpauseGame()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }
}
