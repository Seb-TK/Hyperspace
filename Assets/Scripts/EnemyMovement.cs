using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //simple ai gives movement inputs like the player
    //simply keeps a certain distance from the player
    // moves towards or away to keep at around this distance
    [SerializeField] private float movementSpeed;
    [SerializeField] private float movementRandomness;
    [SerializeField] private float rotationSpeed;
    private GameObject player;
    private Vector3 playerPosition;
    void Awake()
    {
        player = FindAnyObjectByType<PlayerMovement>().gameObject;
    }
    void Start()
    {
        EnemyDataScriptableObject data = GetComponent<EnemyData>().EnemyDataScriptableObject;
        movementSpeed = data.movementSpeed;
        rotationSpeed = data.rotationSpeed;
        movementRandomness = data.movementRandomness;
    }

    void Update()
    {
        playerPosition = player.transform.position;
        TurnTowardsPlayer();
        Move();
    }

    void TurnTowardsPlayer()
    {
        Vector2 directionToPlayer = playerPosition - transform.position;
        float targetAngle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(0,0,targetAngle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void Move()
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
