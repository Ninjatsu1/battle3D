using UnityEngine;
using UnityEngine.AI;
public class EnemyStateMachine : MonoBehaviour
{
    [SerializeField]
    GameObject _player;

    private StateMachine _stateMachine;
    //Character settings
    [Header("Character settings")]
    public float lookRadius = 10f;
    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private float speed = 0;
    private float acceleration;
    public float deaceleration = 0.5f;
    public EnemyController enemyController;
    //Weapon settings
    [Header("Attack weapon settings")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask playerLayer;
    private bool playerHit = false;
    public int baseDamage = 3;

    //Animation hashes
    private int speedHash;
    private int attackHash;

    private void Awake()
    {
        player = GameManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        speed = agent.speed;
        acceleration = agent.acceleration;
        animator = GetComponent<Animator>();

        _stateMachine = new StateMachine();
        var idle = new Idle(animator);
        var moving = new Moving(animator, enemyController);
        var rangedAttack = new RangedAttack();
        var meleeAttack = new MeleeAttack(animator, enemyController);

        //State machine values
        _stateMachine.AddState(idle);
        _stateMachine.AddState(rangedAttack);
        _stateMachine.AddState(meleeAttack);

        _stateMachine.AddTransition(idle, moving, () => Vector3.Distance(transform.position, _player.transform.position) <= 10);

        _stateMachine.AddTransition(moving, meleeAttack, () => Vector3.Distance(transform.position, _player.transform.position) <= agent.stoppingDistance);

        _stateMachine.AddTransition(moving, idle, () => Vector3.Distance(transform.position, _player.transform.position) <= 5);
        _stateMachine.AddTransition(meleeAttack, moving, () => enemyController.Attacked);

        _stateMachine.SetState(idle);
    }

    // Update is called once per frame
    private void Update()
    {
        _stateMachine.Tick();

    }
}

//Laita omaan tiedostoon t. Aleksi

public class Idle : IState
{
    private Animator animator;

    public Idle(Animator animator)
    {
        this.animator = animator;
    }

    public void OnEnter()
    {
        Debug.Log("Changed to idle");
    }

    public void OnExit()
    {

    }

    public void Tick()
    {
        Debug.Log("Idling");
        //Koodi tähän???
    }
}

public class Moving : IState
{
    private Animator animator;
    private EnemyController enemyController;
    public Moving(Animator animator, EnemyController enemyController)
    {
        this.animator = animator;
        this.enemyController = enemyController;
    }


    public void OnEnter()
    {
        Debug.Log("Changed to moving state");
    }

    public void OnExit()
    {
      
    }

    public void Tick()
    {
        enemyController.Move();
        Debug.Log("Moving...");
    }
}

public class RangedAttack : IState
{
    public void OnEnter()
    {
        //Debug.Log("attacking with bananas");
    }

    public void OnExit()
    {

    }

    public void Tick()
    {
       // Debug.Log("Throwing bananas");
    }
}

public class MeleeAttack : IState
{
    private Animator animator;
    private EnemyController enemyController;
    //Aseta ajastin/ attack cooldown
    public MeleeAttack(Animator animator, EnemyController enemyController)
    {
        this.animator = animator;
        this.enemyController = enemyController;
    }

    public void OnEnter()
    {
        enemyController.Attack();
    }

    public void OnExit()
    {
        enemyController.NegateAttacked();
    }

    public void Tick()
    {

        //Debug.Log("Attacking with knife");
    }
}