using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Desertenemy : MonoBehaviour
{
    public NavMeshAgent Enemy;
    public Transform player;
    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;


    //Patrolling 
    public Vector3 move;
    bool isMoving;
    public float MoveRange;

    //Attack
    public float attacktime;
    bool isAttacking;

    //Move to player and attack
    public float visiblityRange;
    public float attackRange;
    bool isVisible;
    bool isInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("PacManPlayer").transform;
        Enemy = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        isVisible = Physics.CheckSphere(transform.position, visiblityRange, whatIsPlayer);
        isInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (!isVisible && !isInAttackRange)
        {
            Patroling();
        }
        if (isVisible && !isInAttackRange)
        {
            ChasePlayer();
        }
        if (isVisible && isInAttackRange)
        {
            AttackPlayer();
        }

    }
    private void Patroling()
    {
        if (!isMoving)
        {
            Searchmove();
        }
        if (isMoving)
        {
            Enemy.SetDestination(move);
        }
        Vector3 distanceTomove = transform.position - move;
        //Distance reached
        if (distanceTomove.magnitude < 1f)
            isMoving = false;
    }
    private void Searchmove()
    {
        float randomZ = Random.Range(-MoveRange, MoveRange);
        float randomX = Random.Range(-MoveRange, MoveRange);

        move = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(move, -transform.up, 2f, whatIsGround))
            isMoving = true;
    }
    private void ChasePlayer()
    {
        Enemy.SetDestination(player.position);
    }
    private void AttackPlayer()
    {
        Enemy.SetDestination(transform.position);
        transform.LookAt(player);
        if (isAttacking)
        {
            Destroy(player);
        }
    }


}
