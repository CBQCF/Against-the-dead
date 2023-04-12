using Mirror;

public class Stats : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnDamageTaken))]
    public int health;
    private void OnDamageTaken(int oldValue, int newValue)
    {
        if (newValue <= 0)
        {
            NetworkServer.Destroy(this.gameObject);
        }
    }

    [Command(requiresAuthority = false)]
    public void AddHealth(int damage)
    {
        health += damage;
    }
}
