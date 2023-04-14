using System;
using Mirror;

public class Stats : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnDamageTaken))]
    public int health;

    public HealthBar healthBar;

    public void Start()
    {
        healthBar.SetMaxHealth(health);
    }

    private void OnDamageTaken(int oldValue, int newValue)
    {
        healthBar.SetHealth(newValue);
        if (newValue <= 0)
        {
            NetworkServer.Destroy(this.gameObject);
        }
    }
    
    public void AddHealth(int damage)
    {
        health += damage;
        healthBar.SetHealth(health);
        if (health <= 0)
        {
            NetworkServer.Destroy(this.gameObject);
        }
    }
}