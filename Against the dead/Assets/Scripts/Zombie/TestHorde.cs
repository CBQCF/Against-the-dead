using System;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

public class TestHorde : NetworkBehaviour
{
    [SerializeField] private GameObject Zombie_normal;

    [SerializeField] private GameObject Zombie_crawler;

    [SerializeField] private Vector3 zoneSize;

    private float spawnTimer = 0f;
    private int spawnCount = 10;
    private int compteur = 0;

    public ServerInfo serverInfo;

    [SerializeField] public int zombieGroupSize;
    [SerializeField] public float groupSpawnDelay;
    private float groupSpawnTimer = 0f;
    public int maxHorde;
    private int nbHorde = 0;


    /// <summary>
    /// Get a random Vector3 position in the ZombieSpawn zone
    /// </summary>
    /// <returns></returns>
    private Vector3 randomSpawn()
    {
        return new Vector3(
            Random.Range(transform.position.x - zoneSize.x / 2, transform.position.x + zoneSize.x / 2),
            Random.Range(transform.position.y - zoneSize.y / 2, transform.position.y + zoneSize.y / 2),
            Random.Range(transform.position.z - zoneSize.z / 2, transform.position.z + zoneSize.z / 2)
        );
    }

    /// <summary>
    /// Spawn multiple zombies
    /// </summary>
    void SpawnZombies()
    {
        if (nbHorde < maxHorde)
        {
            spawnTimer += Time.deltaTime;
            groupSpawnTimer += Time.deltaTime;
            if (groupSpawnTimer >= groupSpawnDelay)
            {
                groupSpawnTimer = 0f;

                // Spawn the zombie group
                Vector3 coord = randomSpawn();
                GameObject boss = Instantiate(Zombie_crawler, coord, Quaternion.identity);
        
                ZombieCharacterControl zccc = boss.AddComponent<ZombieCharacterControl>();
                zccc.serverInfo = serverInfo;

                NetworkServer.Spawn(boss);
                for (int i = 1; i < zombieGroupSize - 1; i++)
                {
                    GameObject zomb = Instantiate(Zombie_normal, coord, Quaternion.identity);
        
                    ZombieCharacterControl zcc = zomb.AddComponent<ZombieCharacterControl>();
                    zcc.serverInfo = serverInfo;

                    NetworkServer.Spawn(zomb);
                }

                nbHorde += 1;
            }
            else if (spawnTimer >= 10f)
            {
                spawnTimer = 0f;

                // Spawn normal zombies
                Vector3 coord = randomSpawn();
                for (int i = 1; i < spawnCount; i++)
                {
                    GameObject zomb = Instantiate(Zombie_normal, coord, Quaternion.identity);
        
                    ZombieCharacterControl zcc = zomb.AddComponent<ZombieCharacterControl>();
                    zcc.serverInfo = serverInfo;

                    NetworkServer.Spawn(zomb);
                }

                nbHorde += 1;
            }  
        }
    }
    [Server]
    void Update()
    {
        SpawnZombies();
    }

    [Server]
    void Start()
    {
        serverInfo = FindObjectOfType<ServerInfo>();
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, zoneSize);
    }
}
