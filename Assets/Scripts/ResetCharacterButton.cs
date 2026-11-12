using UnityEngine;

public class ResetCharacterButton : MonoBehaviour
{
    HealthManager healthManager;

    void Start()
    {
        healthManager = FindAnyObjectByType<HealthManager>();
    }

    public void OnResetCharacterButtonPressed()
    {
        healthManager.Health = 0;
    }
}
