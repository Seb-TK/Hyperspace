using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //add velocity to allow for some sliding and such 
    // for a bit more dynamic ness
    //ADD TIME.DELTATIME to the velocity
    [SerializeField] private float movementSpeed;
    [SerializeField] private float movementSpeedZ;
    [SerializeField] private float movementRandomness;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private EnemyDataScriptableObject.AiTypes AiType;
    [SerializeField] private float hoverDistance;
    [SerializeField] private float chargeRate;
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float chargeDistance;
    [SerializeField] private float chargePauseLength;
    private float chargeTimer;
    private GameObject player;
    private Vector3 playerPosition;
    private EnemyDataScriptableObject data;
    private Vector3 directionToPlayer;

    void Awake()
    {
        player = FindAnyObjectByType<PlayerMovement>().gameObject;
    }
    void Start()
    {
        data = GetComponent<EnemyData>().EnemyDataContainer;
        movementSpeed = data.movementSpeed;
        rotationSpeed = data.rotationSpeed;
        movementRandomness = data.movementRandomness;
        AiType = data.aiType;
        hoverDistance = data.hoverDistance;
        chargeDistance = data.chargeDistance;
        chargeSpeed = data.chargeSpeed;
        chargeRate = data.chargeRate;
        chargePauseLength = data.chargePauseLength;
        chargeTimer = Time.deltaTime + chargeRate;
        if(AiType == EnemyDataScriptableObject.AiTypes.Charger)
        {
            StartCoroutine(ChargerAi());
        }
    }

    void Update()
    {
        playerPosition = player.transform.position;
        directionToPlayer = (playerPosition - transform.position).normalized;
        if(AiType == EnemyDataScriptableObject.AiTypes.Ranger)
        {
            RangerAi();
        } else if(AiType == EnemyDataScriptableObject.AiTypes.Charger)
        {
            ChargerAi();
        } else if(AiType == EnemyDataScriptableObject.AiTypes.Drifter)
        {
            DrifterAi();
        }
    }

    public void RangerAi()
    {
        TurnTowardsPlayer();
        HoverAtDistance(hoverDistance);
    }

    IEnumerator ChargerAi()
    {
        while (true)
        {
            if(Time.time > chargeTimer)
            {
                float chargePauseLengthTimer = Time.time + chargePauseLength;
                while(Time.time < chargePauseLengthTimer)
                {
                    yield return null;
                }
                Vector3 currentPosition = transform.position;
                Vector3 targetPosition = transform.position += transform.right * chargeDistance;
                float t = 0f;
                while(t <= 1f)
                {
                    t += chargeSpeed;
                    //experiment with an animation curve for better easing
                    float easedT = Mathf.SmoothStep(0,1,t);
                    transform.position = Vector3.Lerp(currentPosition, targetPosition, easedT);
                    yield return null;
                }
                chargeTimer = Time.time + chargeRate;
            }
            RangerAi();
            yield return null;
        }
    }

    private void HoverAtDistance(float distance)
    {
        Vector3 playerPosition = player.transform.position;
        playerPosition.z = 0;
        Vector3 thisPosition = transform.position;
        thisPosition.z = 0;

        float distanceToPlayer = Vector3.Distance(
            playerPosition,
            thisPosition
        );
        float hoverDistanceBuffer = 2f;
        if(Mathf.Round(distanceToPlayer) >= distance)
        {
            thisPosition += directionToPlayer * movementSpeed * Time.deltaTime;
        } else if(Mathf.Round(distanceToPlayer) <= distance - hoverDistanceBuffer)
        {
            thisPosition -= directionToPlayer * movementSpeed * Time.deltaTime;
        }
        thisPosition.z = transform.position.z;
        transform.position = thisPosition;

        if(transform.position.z <= player.transform.position.z)
        {
            transform.position += Vector3.forward * movementSpeedZ * Time.deltaTime;
        } else if(transform.position.z >= player.transform.position.z + 0.05f)
        {
            transform.position -= Vector3.forward * movementSpeedZ * Time.deltaTime;
        }
    }

    void TurnTowardsPlayer()
    {
        Vector2 directionToPlayer = playerPosition - transform.position;
        float targetAngle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(0,0,targetAngle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    // add a failsafe so that drifters dont drift
    //out of bounds on the z axis, maybe a tp?
    void DrifterAi() 
    {
        float t = Time.deltaTime * rotationSpeed;
        Vector3 direction = new Vector3(
            Mathf.PerlinNoise(t, 0),
            Mathf.PerlinNoise(t, 0),
            Mathf.PerlinNoise(t, 0)
        );
        direction.Normalize();
        transform.position += direction * movementSpeed * Time.deltaTime;

        WorldPlane worldPlane = FindAnyObjectByType<WorldPlane>();
        float dimensionRange = worldPlane.dimensionRange;
        Vector3 newPos = transform.position;
        if(transform.position.z > dimensionRange)
        {
            newPos.z = -dimensionRange;
        }
        if(transform.position.z < -dimensionRange)
        {
            newPos.z = dimensionRange;
        }
        transform.position = newPos;
    }

}
