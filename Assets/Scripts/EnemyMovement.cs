using System;
using System.Collections;
using System.Linq;
using NUnit.Framework.Constraints;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class EnemyMovement : MonoBehaviour
{
    //add velocity to allow for some sliding and such 
    // for a bit more dynamic ness
    //ADD TIME.DELTATIME to the velocity
    [SerializeField] private float movementSpeed;
    [SerializeField] private float movementSpeedZ;
    [SerializeField] private float movementBurstsRate;
    [SerializeField] private float movementBurstsAmount;
    [SerializeField] private float hoverDistance;
    [SerializeField] private float chargeRate;
    [SerializeField] private float chargePower;
    [SerializeField] private float chargePauseLength;
    [SerializeField] private float chargeEndPauseLength;
    [SerializeField] private float movementAcrossZLength;
    [SerializeField] private float movementRandMultiplierInput;
    [SerializeField] public EnemyDataObj.AiTypes AiType;
    public float movementRandMultiplier;
    private float rotationSpeed;
    private GameObject player;
    private Vector3 playerPosition;
    private EnemyDataObj data;
    private Vector2 directionToPlayer;
    private WorldPlane worldPlane;
    private Vector3 velocity;
    [SerializeField] private float velocityDecayRate;
    [SerializeField] private float VelocityDecayRateZ;
    [SerializeField] private float hoverDistanceBuffer;
    [SerializeField] private float movementRandMultiplierInputZ;
    private float movementRandMultiplierZ;
    private float chargeSpeedDecayMultiplier;
    private float maxVelocity;
    public float maxVelocityZ;
    private float rotationalVelocity;
    private float rotationalVelocityDecay;
    private float rotationalVelocityClamp;
    private float driftSpeed;
    public float chargeTimer;
    public float currentChargeTimer;
    float driftOffset;
    public float timeLastMovedFromClosestEnemy;
    [SerializeField ]float moveFromCloseEnemyRate;
    [SerializeField ]float moveFromCloseEnemyAmount;
    
    void Awake()
    {
        player = FindAnyObjectByType<PlayerMovement>().gameObject;
    }
    void Start()
    {
        worldPlane = FindAnyObjectByType<WorldPlane>();
        data = GetComponent<EnemyData>().EnemyDataContainer;

        movementRandMultiplier = UnityEngine.Random.Range(movementRandMultiplierInput, 1);
        movementRandMultiplierInputZ = data.movementRandMultiplierInputZ;
        movementRandMultiplierZ = UnityEngine.Random.Range(movementRandMultiplierInputZ, 1);
        movementRandMultiplierZ *= movementRandMultiplier;
        velocityDecayRate = data.velocityDecayRate;
        VelocityDecayRateZ = data.velocityDecayRateZ;

        movementSpeed = data.movementSpeed * movementRandMultiplier;
        movementSpeedZ = data.movementSpeedZ * movementRandMultiplierZ;
        maxVelocity = data.maxVelocity * movementRandMultiplier;
        maxVelocityZ = data.maxVelocityZ * movementRandMultiplierZ;
        rotationSpeed = data.rotationSpeed * movementRandMultiplier;
        rotationalVelocityClamp = data.rotationalVelocityClamp * movementRandMultiplier;
        rotationalVelocityDecay = data.rotationalVelocityDecay * movementRandMultiplier;
        movementBurstsRate = data.movementBurstsRate * movementRandMultiplier;
        movementBurstsAmount = data.movementBurstsAmount * movementRandMultiplier;
        hoverDistance = data.hoverDistance * movementRandMultiplier;

        chargePower = data.chargePower;
        chargeRate = data.chargeRate;
        chargeSpeedDecayMultiplier = data.chargeSpeedDecayMultiplier;
        chargePauseLength = data.chargePauseLength;
        chargeEndPauseLength = data.chargeEndPauseLength;
        driftSpeed = data.driftSpeed;
        AiType = data.aiType;
        PointAtPlayer();
        RunStartFunctions();
    }
    
    private void RunStartFunctions()
    {
        if(AiType == EnemyDataObj.AiTypes.Ranger)
        {
            StartCoroutine(RandomMovementBursts());
        } else if(AiType == EnemyDataObj.AiTypes.Charger)
        {
            chargeTimer = Time.time + chargeRate;
            currentChargeTimer = chargeTimer * movementRandMultiplier;
            StartCoroutine(RandomMovementBursts());
            StartCoroutine(ChargerAi());
        } else if(AiType == EnemyDataObj.AiTypes.Drifter)
        {
            driftOffset = Random.Range(0f,1000f);
            float upperRotationSpeedRange = 8f;
            float lowerRotationSpeedRange = 3f;
            rotationSpeed = Random.Range(lowerRotationSpeedRange,upperRotationSpeedRange);
            if(Random.Range(0,1) == 1)
            {
                rotationSpeed *= -1;
            }
        }
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
        if(AiType == EnemyDataObj.AiTypes.Ranger | AiType == EnemyDataObj.AiTypes.Sniper)
        {
            RangerAi();
        } else if(AiType == EnemyDataObj.AiTypes.Drifter)
        {
            DrifterAi();
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
        velocity.z = Mathf.Lerp(velocity.z, 0, VelocityDecayRateZ * Time.deltaTime);
        float cacheVelocityZ = velocity.z;
        velocity = Vector3.Lerp(velocity, Vector3.zero, velocityDecayRate * Time.deltaTime);
        velocity.z = cacheVelocityZ;
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
            float waitTime = Random.Range(movementBurstsRate / 2, movementBurstsRate * 2);
            yield return new WaitForSeconds(waitTime);
            float burstAmount = Random.Range(movementBurstsAmount / 2, movementBurstsAmount * 2);
            Vector3 randomDirection = Random.insideUnitSphere.normalized;
            velocity += randomDirection * burstAmount;
        }
    }

    IEnumerator ChargerAi()
    {
        while (true)
        {
            if(Time.time > currentChargeTimer)
            {
                yield return StartCoroutine(Charge());
            }
            RangerAi();
            yield return null;
        }
        
    }

    IEnumerator Charge()
    {   
        float cacheVelocityDecay = velocityDecayRate;
        velocityDecayRate *= chargeSpeedDecayMultiplier;
        yield return new WaitForSeconds(chargePauseLength);
        velocity = transform.up * chargePower;
        float cacheMaxVelocity = maxVelocity;
        maxVelocity = chargePower;
        yield return new WaitForSeconds(chargeEndPauseLength);
        maxVelocity = cacheMaxVelocity;
        velocityDecayRate = cacheVelocityDecay;

        chargeTimer = Time.time + chargeRate * movementRandMultiplier;
        currentChargeTimer = chargeTimer;
    }

    private void HoverAtDistance(float distance)
    {
        Vector2 velocityChange = Vector2.zero;

        float distanceBetweenEnemies = 2f;
        LayerMask mask = LayerMask.GetMask("Enemy");
        Collider[] closeEnemies = Physics.OverlapSphere(transform.position, distanceBetweenEnemies, mask);
        int closestEnemyIndex = 0;
        float closestDistance = 0;
        try
        {
            closestDistance = Vector2.Distance(closeEnemies[0].gameObject.transform.position,transform.position);
        } catch (Exception){}
        for(int i = 0; i < closeEnemies.Length; i++)
        {
            float currentDistance = Vector2.Distance(closeEnemies[i].gameObject.transform.position,transform.position);
            if(currentDistance == 0)
            {
                i++;
                continue;
            }
            if(currentDistance < closestDistance)
            {
                closestEnemyIndex = i;
                closestDistance = currentDistance;
            }
        }
        
        if(closeEnemies.Length > 0 & Time.time > timeLastMovedFromClosestEnemy + moveFromCloseEnemyRate)
        {
            timeLastMovedFromClosestEnemy = Time.time;
            Vector3 dirVec3 = transform.position - closeEnemies[closestEnemyIndex].gameObject.transform.position;
            Vector2 dir = dirVec3;
            velocityChange += dir * movementSpeed * (moveFromCloseEnemyAmount - closestDistance);
        }
        Vector2 playerPosition = player.transform.position;
        Vector2 thisPosition = transform.position;

        float distanceToPlayer = Vector2.Distance(
            playerPosition,
            thisPosition
        );
        
        hoverDistanceBuffer = 1;
        if(Mathf.Round(distanceToPlayer) >= distance + hoverDistanceBuffer)
        {
            velocityChange += directionToPlayer * movementSpeed;
        } else if(Mathf.Round(distanceToPlayer) <= distance - hoverDistanceBuffer)
        {
            velocityChange -= directionToPlayer * movementSpeed;
        }
        Vector3 velocityChangeVec3 = velocityChange;
        velocity += velocityChangeVec3;

    }

        private void MoveAcrossZ()
        {
            float hoverDistanceBufferZ = 0.5f * movementRandMultiplierZ;
            float distance = Mathf.Abs(worldPlane.dimensionLocation - transform.position.z);
            if(distance < hoverDistanceBufferZ)
            {
                velocity.z -= Mathf.Lerp(0, velocity.z, 0.01f);
            } else if(transform.position.z < worldPlane.dimensionLocation)
            {
                velocity.z += movementSpeedZ;
            } else if (transform.position.z > worldPlane.dimensionLocation)
            {
                velocity.z -= movementSpeedZ;
            }
            
        }

    void TurnTowardsPlayer()
    {
        Vector2 directionToPlayer = playerPosition - transform.position;
        float angle = Vector3.SignedAngle(transform.up, directionToPlayer, Vector3.forward);
        rotationalVelocity += angle;
    }

    void DrifterAi()
    {
        float t = driftSpeed * Time.time + driftOffset;
        Vector3 direction = new Vector3(
            Mathf.PerlinNoise(t, 0),
            Mathf.PerlinNoise(t, 0),
            Mathf.PerlinNoise(t, 0)
        );
        direction.Normalize();
        velocity += direction * movementSpeed;
        velocity.z += direction.z * movementSpeedZ;
        rotationalVelocity = rotationSpeed;
    }

}
