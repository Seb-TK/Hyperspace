using System;
using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //simple ai gives movement inputs like the player
    //simply keeps a certain distance from the player
    // moves towards or away to keep at around this distance
    [SerializeField] private float movementSpeed;
    [SerializeField] private float movementRandomness;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private EnemyDataScriptableObject.AiTypes AiType;
    [SerializeField] private float hoverDistance;
    private GameObject player;
    private Vector3 playerPosition;
    private EnemyDataScriptableObject data;

    void Awake()
    {
        player = FindAnyObjectByType<PlayerMovement>().gameObject;
    }
    void Start()
    {
        data = GetComponent<EnemyData>().EnemyDataScriptableObject;
        movementSpeed = data.movementSpeed;
        rotationSpeed = data.rotationSpeed;
        movementRandomness = data.movementRandomness;
        AiType = data.aiType;
        hoverDistance = data.hoverDistance;
    }

    void Update()
    {
        playerPosition = player.transform.position;
        if(AiType == EnemyDataScriptableObject.AiTypes.Ranger)
        {
        RangerAi();
        }
    }

    public void RangerAi()
    {
        TurnTowardsPlayer();
        HoverAtDistance(hoverDistance);
    }

    IEnumerator Charge(float chargeRange, float chargeSpeed)
    {
        float distanceTraveled = 0;
        while (distanceTraveled < chargeRange)
        {
            distanceTraveled++;
            transform.position+= transform.up * chargeSpeed;
            yield return null;  
        }
    }

    private void HoverAtDistance(float distance)
    {
        float distanceToPlayer = Vector3.Distance(
            player.transform.position,
            transform.position
        );
        Vector3 directionToPlayer = playerPosition - transform.position;
        float hoverDistanceBuffer = 2f;
        if(Mathf.Round(distanceToPlayer) >= distance)
        {
            transform.position += directionToPlayer * movementSpeed;
        } else if(Mathf.Round(distanceToPlayer) <= distance - hoverDistanceBuffer)
        {
            transform.position -= directionToPlayer * movementSpeed;
        }
    }

    void TurnTowardsPlayer()
    {
        Vector2 directionToPlayer = playerPosition - transform.position;
        float targetAngle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(0,0,targetAngle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void Drift()
    {
        float t = Time.deltaTime * rotationSpeed;
        Vector3 direction = new Vector3(
            Mathf.PerlinNoise(t, 0),
            Mathf.PerlinNoise(t, 0),
            Mathf.PerlinNoise(t, 0)
        );
        direction.Normalize();
        transform.position += direction * movementSpeed * Time.deltaTime;
    }

}
