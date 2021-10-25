using UnityEngine.AI;
using UnityEngine;
using System.Collections;
using System;

public class EnemyController : MonoBehaviour
{
    //Käytä coroutinea hyökkäys cooldownissa
    //Hyökkäys suoritettu bool
    [Header("Character settings")]
    public float lookRadius = 10f;
    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private float speed = 0;
    private float acceleration;
    public float deaceleration = 0.5f;
    //private bool attacking = false;

    [Header("Attack weapon settings")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask playerLayer;
    private bool playerHit = false;
    public int baseDamage = 3;
    public float attackCooldown;
    private bool attacked; //If needed -> make public method that returns value
    public float cooldown = 2;
    public float cooldownTimer = 0;

    private IEnumerator coroutine;    
    //Animation hashes
    private int speedHash;
    private int attackHash;
    public bool Attacked => attacked;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        speed = agent.speed;
        acceleration = agent.acceleration;
        animator = GetComponent<Animator>();
        speedHash = Animator.StringToHash("Speed");
        attackHash = Animator.StringToHash("Attack");
    }

    // Update is called once per frame
    public void Move()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance <= lookRadius)
        {
            Run();
            agent.SetDestination(player.position);
            animator.ResetTrigger(attackHash);

        }

    }

    //Start one attack
    public IEnumerator StartAttack()
    {
        animator.SetTrigger(attackHash);
        DetectHit();
        yield return new WaitForSeconds(1);
        attacked = true;
    }

    //Detect if player was hit
    public void DetectHit()
    {
        Collider[] hitPlayers = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayer);
        for (int i = 0; i < hitPlayers.Length; i++)
        {
            if (hitPlayers[i].CompareTag("Player"))
            {
                playerHit = true;
                PlayerStats playerStats = hitPlayers[i].GetComponent<PlayerStats>();
                if (playerStats != null)
                {
                    if (playerHit)
                    {
                        Debug.Log("Player damaged");
                        playerStats.TakeDamege(baseDamage); 
                    }
                    playerHit = false;
                }
            }
        }
        
    }
    //Animate run animation
    public void Run()
    {
        animator.ResetTrigger(attackHash);
        animator.SetFloat(speedHash, speed);

    }
    public void StopRun()
    {
        animator.SetFloat(speedHash, 0);
    }

    //Look radius
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    //Start attack
    public void Attack()
    {
        StartCoroutine(StartAttack());
    }

    //Negate attack boolean
    public void NegateAttacked() => attacked = false;
}
