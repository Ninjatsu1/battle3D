using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 20;
    public int currentHealth;

    private void Update()
    {
        
    }

    //Damages player
    public void TakeDamege(int damage)
    {
        currentHealth -= damage;
    }

    //Player dies
    public void PlayerDeath()
    {
        if (currentHealth <= 0)
        {
            Debug.Log("Die");
        }
    }
}
