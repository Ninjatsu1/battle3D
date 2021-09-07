using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    public float lookRadius = 10f;
    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private float speed = 0;
    private float acceleration;
    public float deaceleration = 0.5f;
    private bool destinationReached = false;
    private bool attacking = false;

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
        }
        if (distance < agent.stoppingDistance)
        {
            attacking = true;
            StopRun();
            animator.SetBool(attackHash, attacking);
            Debug.Log("Stopped");
        }
    }

    //Animate run animation
    private void Run()
    {
        attacking = false;
        animator.SetBool(attackHash, attacking);
        animator.SetFloat(speedHash, speed);
     
    }
    private void StopRun()
    {
        animator.SetFloat(speedHash, 0);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
