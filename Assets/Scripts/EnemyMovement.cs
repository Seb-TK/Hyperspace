using System;
using System.Collections;
using NUnit.Framework.Constraints;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //add velocity to allow for some sliding and such 
    // for a bit more dynamic ness
    //ADD TIME.DELTATIME to the velocity
    [SerializeField] private float movementSpeed;
    [SerializeField] private float movementSpeedZ;
    [SerializeField] private float movementBurstsRate;
    [SerializeField] private float movementBurstsAmount;
    [SerializeField] private EnemyDataObj.AiTypes AiType;
    [SerializeField] private float hoverDistance;
    [SerializeField] private float chargeRate;
    [SerializeField] private float chargePower;
    [SerializeField] private float chargePauseLength;
    [SerializeField] private float movementAcrossZLength;
    [SerializeField] private float movementRandMultiplier;
    private float rotationSpeed;
    private GameObject player;
    private Vector3 playerPosition;
    private EnemyDataObj data;
    private Vector3 directionToPlayer;
    private WorldPlane worldPlane;
    private Vector3 velocity;
    [SerializeField] private float velocityDecayRate;
    [SerializeField] private float VelocityDecayRateZ;
    [SerializeField] private float hoverDistanceBuffer;
    private Vector3 velocityDecay;
    private float maxVelocity;
    private float maxVelocityZ;
    private float rotationalVelocity;
    private float rotationalVelocityDecay;
    private float rotationalVelocityClamp;
    private float chargeTimer;
    private float currentChargeTimer;
    
    void Awake()
    {
        player = FindAnyObjectByType<PlayerMovement>().gameObject;
    }
    void Start()
    {
        movementRandMultiplier = UnityEngine.Random.Range(movementRandMultiplier, 1);
        worldPlane = FindAnyObjectByType<WorldPlane>();
        data = GetComponent<EnemyData>().EnemyDataContainer;
        velocityDecayRate = data.velocityDecayRate;
        VelocityDecayRateZ = data.velocityDecayRateZ;
        velocityDecay = new Vector3 
        (velocityDecayRate,
        velocityDecayRate,
        VelocityDecayRateZ
        );
        movementSpeed = data.movementSpeed * movementRandMultiplier;
        movementSpeedZ = data.movementSpeedZ * movementRandMultiplier;
        maxVelocity = data.maxVelocity * movementRandMultiplier;
        maxVelocityZ = data.maxVelocityZ * movementRandMultiplier;
        rotationSpeed = data.rotationSpeed * movementRandMultiplier;
        rotationalVelocityClamp = data.rotationalVelocityClamp * movementRandMultiplier;
        rotationalVelocityDecay = data.rotationalVelocityDecay * movementRandMultiplier;
        movementBurstsRate = data.movementBurstsRate * movementRandMultiplier;
        movementBurstsAmount = data.movementBurstsAmount * movementRandMultiplier;
        hoverDistance = data.hoverDistance * movementRandMultiplier;
        chargePower = data.chargePower;
        chargeRate = data.chargeRate;
        chargePauseLength = data.chargePauseLength;
        AiType = data.aiType;
        PointAtPlayer();
        RunSetupFunctions();
    }

    void Update()
    {
        playerPosition = player.transform.position;
        directionToPlayer = (playerPosition - transform.position).normalized;
        RunAi();
        ApplyVelocity();
        ApplyRotationalVelocity();
    }

    private void RunAi()
    {
        if(AiType == EnemyDataObj.AiTypes.Ranger)
        {
            RangerAi();
        } else if(AiType == EnemyDataObj.AiTypes.Charger)
        {
            ChargerAi();
        }
        if(AiType == EnemyDataObj.AiTypes.Drifter)
        {
            DrifterAi();
        }
    }

    private void RunSetupFunctions()
    {
        if(AiType == EnemyDataObj.AiTypes.Ranger)
        {
            StartCoroutine(RandomMovementBursts());
        } else if(AiType == EnemyDataObj.AiTypes.Charger)
        {
            chargeTimer = Time.time + chargeRate;
            currentChargeTimer = chargeTimer * movementRandMultiplier;
            StartCoroutine(RandomMovementBursts());
        }
    }

    private void PointAtPlayer()
    {
        Vector3 thisPlayerPosition = player.transform.position;
        Vector3 thisDirectionToPlayer = (thisPlayerPosition - transform.position).normalized;
        thisDirectionToPlayer.z = 0;
        transform.up = thisDirectionToPlayer;
    }

    private void ApplyVelocity()
    {
        velocity.z = Mathf.Clamp(velocity.z, -maxVelocityZ, maxVelocityZ);
        velocity = Vector3.ClampMagnitude(velocity, maxVelocity);
        transform.position += velocity * Time.deltaTime;
        velocity = Vector3.Lerp(velocity, Vector3.zero, velocityDecayRate * Time.deltaTime);
    }

    private void ApplyRotationalVelocity()
    {
        rotationalVelocity = Mathf.Clamp(rotationalVelocity, -rotationalVelocityClamp, rotationalVelocityClamp);
        Quaternion rotationAmount = Quaternion.Euler(0,0,rotationalVelocity * rotationSpeed * Time.deltaTime);
        transform.rotation *= rotationAmount;
        rotationalVelocity = Mathf.Lerp(rotationalVelocity, 0, rotationalVelocityDecay * Time.deltaTime);
    }

    private void RangerAi()
    {
        TurnTowardsPlayer();
        HoverAtDistance(hoverDistance);
        MoveAcrossZ();
    }
    IEnumerator RandomMovementBursts()
    {
        while (true)
        {
            float waitTime = UnityEngine.Random.Range(movementBurstsRate / 2, movementBurstsRate * 2);
            yield return new WaitForSeconds(waitTime);
            Vector3 randomDirection = UnityEngine.Random.insideUnitCircle.normalized;
            float burstAmount = UnityEngine.Random.Range(movementBurstsAmount / 2, movementBurstsAmount * 2);
            velocity += randomDirection * burstAmount;
        }
    }

    private void ChargerAi()
    {
        if(Time.time > currentChargeTimer)
            {
                velocity += Vector3.forward * chargePower;
                chargeTimer = Time.time + chargeRate;
                currentChargeTimer = chargeTimer * movementRandMultiplier;
            }
    }

    private void HoverAtDistance(float distance)
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 thisPosition = transform.position;

        float distanceToPlayer = Vector3.Distance(
            playerPosition,
            thisPosition
        );
        hoverDistanceBuffer = 1f;
        if(Mathf.Round(distanceToPlayer) >= distance + hoverDistanceBuffer)
        {
            velocity += directionToPlayer * movementSpeed * Time.deltaTime;
        } else if(Mathf.Round(distanceToPlayer) <= distance - hoverDistanceBuffer)
        {
            velocity -= directionToPlayer * movementSpeed * Time.deltaTime;
        }
    }

        private void MoveAcrossZ()
        {
            if(worldPlane.dimensionLocation > transform.position.z)
            {
                velocity.z += movementSpeedZ * Time.deltaTime;
            } else if (worldPlane.dimensionLocation < transform.position.z)
            {
                velocity.z -= movementSpeedZ * Time.deltaTime;
            }
        }

    void TurnTowardsPlayer()
    {
        Vector2 directionToPlayer = playerPosition - transform.position;
        float angle = Vector3.SignedAngle(transform.up, directionToPlayer, Vector3.forward);
        rotationalVelocity += angle;
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
