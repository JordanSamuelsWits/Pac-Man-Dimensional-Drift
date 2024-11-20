/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform player;

    private NavMeshAgent enemy;


    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();    
    }

    // Update is called once per frame
    void Update()
    {
        EnemyMove();
    }

    void EnemyMove()
    {
        enemy.destination = player.position;
    }
}
*/

using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform player;           
    public float detectionRadius = 10f; 

    private NavMeshAgent enemy;
    private bool isChasing = false;    

    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        EnemyMove();
    }

    void EnemyMove()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius && !isChasing)
        {
            isChasing = true;
            enemy.SetDestination(player.position);
        }

        else if (distanceToPlayer > detectionRadius && isChasing)
        {
            isChasing = false;
            enemy.ResetPath(); 
        }

        if (isChasing)
        {
            enemy.SetDestination(player.position);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
