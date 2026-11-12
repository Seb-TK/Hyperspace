using UnityEngine;

public class ExitButton : MonoBehaviour
{
    
    GameManager gameManager;
    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }
    public void OnExitPress()
    {
        gameManager.OnExitToMenu();
    }
}
