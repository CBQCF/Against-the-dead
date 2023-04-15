using System;
using Mirror;

public class Stats : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnDamageTaken))]
    public int health;

    public HealthBar healthBar;

    private void OnDamageTaken(int oldValue, int newValue)
    {
        if (healthBar is not null) healthBar.SetHealth(newValue);
        if (newValue <= 0)
        {
            NetworkServer.Destroy(this.gameObject);
        }
    }
    
    public void AddHealth(int damage)
    {
        health += damage;
        if (healthBar is not null) healthBar.SetHealth(health);
        if (health <= 0)
        {
            NetworkServer.Destroy(this.gameObject);
        }
    }
}