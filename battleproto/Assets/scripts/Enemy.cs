using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
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

    //Animation hashes
    private int speedHash;
    private int attackHash;
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
    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance <= lookRadius)
        {
            Run();
            agent.SetDestination(player.position);
            animator.ResetTrigger(attackHash);
        }
        if (distance < agent.stoppingDistance)
        {
            Attack();
            DetectHit();
        }
    }

    //Attack player
    private void Attack()
    {
        StopRun();
        animator.SetTrigger(attackHash);
    }

    //Detect if player was hit
    private void DetectHit()
    {
        Collider[] hitPlayers = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayer);
        foreach (Collider player in hitPlayers)
        {
            playerHit = true;
            if (playerHit)
            {
                Debug.Log("hit" + player.name);
            }
        }
        playerHit = false;
    }
    //Animate run animation
    private void Run()
    {
        animator.ResetTrigger(attackHash);
        animator.SetFloat(speedHash, speed);
     
    }
    private void StopRun()
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
}
