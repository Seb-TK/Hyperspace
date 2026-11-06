using System.Collections;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public float Health;
    private float iFramesEnd;
    public float invincibilityLength;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>().GetComponent<GameManager>();
    }

    public void OnHit(float damage)
    {
        Health -= damage;
        StartCoroutine(DamageAnimation(damage));
    }

    IEnumerator DamageAnimation(float damage)
    {
        if (Time.time > iFramesEnd)
        {
            iFramesEnd = Time.time + invincibilityLength;
        } else { 
            yield break;
        }
        Health -= damage;
        PlayerAnimationsManager playerAnimationsManager = FindAnyObjectByType<PlayerAnimationsManager>().GetComponent<PlayerAnimationsManager>();
        playerAnimationsManager.GetHit();
        if(Health <= 0f)
        {
            Die();
            yield break;
        }
    }

    private void Die()
    {
        gameManager.OnPlayerDeath();
    }
}
