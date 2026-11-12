using System.Collections;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public float Health;
    public float MaxHealth;
    private float iFramesEnd;
    public float invincibilityLength;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>().GetComponent<GameManager>();
    }

    void OnEnable()
    {
        Card.CardAdded += HealthAdded;
    }

    void OnDisable()
    {
        Card.CardAdded -= HealthAdded;
    }

    public void HealthAdded(PlayerStats.StatCard card)
    {
        if(card.statType == CrdMngr.Stats.MaxHealth)
        {
            Health *= card.Value;
        }
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
