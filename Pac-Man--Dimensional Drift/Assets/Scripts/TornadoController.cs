using UnityEngine;

public class TornadoController : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float detectionRadius = 20f; // Radius within which the tornado follows the player
    public float followSpeed = 5f; // Speed at which the tornado follows the player
    public float randomMovementSpeed = 2f; // Speed of random erratic movement
    public float randomnessIntensity = 3f; // Intensity of erratic movement
    public float floorHeight = 0f;

    private Vector3 randomDirection;
    private float changeDirectionTimer = 0f;
    private float changeDirectionInterval = 1f;

    void Start()
    {
        GenerateRandomDirection();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            FollowPlayer(distanceToPlayer);
        }
        else
        {
            MoveErratically();
        }
    }

    void FollowPlayer(float distanceToPlayer)
    {
        Vector3 randomOffset = new Vector3(
            Mathf.PerlinNoise(Time.time * randomnessIntensity, 0) - 0.5f,
            0,
            Mathf.PerlinNoise(0, Time.time * randomnessIntensity) - 0.5f
        ) * randomnessIntensity;

        Vector3 targetPosition = player.position + randomOffset;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }

    void MoveErratically()
    {
        if (Time.time > changeDirectionTimer)
        {
            GenerateRandomDirection();
            changeDirectionTimer = Time.time + changeDirectionInterval;
        }

        transform.position += randomDirection * randomMovementSpeed * Time.deltaTime;
    }

    void GenerateRandomDirection()
    {
        randomDirection = new Vector3(
            Random.Range(-1f, 1f),
            0,
            Random.Range(-1f, 1f)
        ).normalized;
    }

    void StayOnFloor()
    {
        transform.position = new Vector3(transform.position.x, floorHeight, transform.position.z);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
