using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    Transform playerTransform;
    NavMeshAgent agent;
    Animator animator;
    float chaseDistance = 10;
    float attackDistance = 2;
    float attackTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(agent.transform.position, playerTransform.transform.position);
        if(distance<chaseDistance && distance>attackDistance)
        {
            ChasePlayer();
        }
        else if(distance<attackDistance)
        {
            Attack();
        }
    }

    private void Attack()
    {
        attackTimer += Time.deltaTime;
        //animations
        animator.SetBool("IsAttacking", true);
        animator.SetBool("IsRunning", false);

        Vector3 lookAtPosition = playerTransform.position;
        lookAtPosition.y = transform.position.y;
        transform.LookAt(lookAtPosition);

        if(attackTimer>=1f)
        {
            playerTransform.gameObject.GetComponent<PlayerHealth>().TakeDamage(10);
            attackTimer = 0;
        }
    }

    void ChasePlayer()
    {
        animator.SetBool("IsRunning", true);
        animator.SetBool("IsAttacking", false);
        agent.SetDestination(playerTransform.transform.position);
    }
}
