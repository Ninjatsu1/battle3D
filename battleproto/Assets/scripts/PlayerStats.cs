using UnityEngine;
using TMPro;
public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 20;
    public int currentHealth;
    public TextMeshProUGUI playerHealth;
    public Animator animator;
    [SerializeField]
    private bool dying;
    [SerializeField]
    private float fallingSpeed;
    public CharacterController cr;
    public float deacreseCollisionRate = 1f;
    public bool deacrece = false;
    private int deathHash;
    private void Start()
    {
        playerHealth.text = currentHealth.ToString();
        maxHealth = currentHealth;
        deathHash = Animator.StringToHash("Dying");
    }

    private void Update()
    {
        if (dying)
        {
            transform.Translate(Vector3.down * Time.deltaTime * fallingSpeed);
            
            if (deacrece)
            {
                cr.height -= deacreseCollisionRate * Time.deltaTime;

                if (cr.height <= 0.5)
                {
                    deacrece = false;

                }
            }
            
        }

    }

    //Damages player
    public void TakeDamege(int damage)
    {
        currentHealth -= damage;
        playerHealth.text = currentHealth.ToString();
        PlayerDeath();
    }

    //Player dies
    public void PlayerDeath()
    {
        if (currentHealth <= 0)
        {
            dying = true;
            deacrece = true;

            animator.SetBool(deathHash, true);
        }
    }
}
