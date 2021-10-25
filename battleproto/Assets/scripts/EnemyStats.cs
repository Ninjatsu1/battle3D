using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int maxHealth = 20;
    public int currentHealth;
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            EnemyDeath();
        }
    }
    //Damage enemy
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            EnemyDeath();
        }
    }

    //Enemy death
    public void EnemyDeath()
    {
        Destroy(gameObject, 1f);
    }
}
