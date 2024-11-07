using System.Collections;
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
