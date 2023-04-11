using System;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnZone : NetworkBehaviour
{
    [SerializeField] private GameObject Zombie_normal;
    
    [SerializeField] private GameObject Zombie_crawler;

    [SerializeField] private Vector3 zoneSize;

    private float spawnTimer = 0f;
    private int spawnCount = 10;
    private int compteur = 0;

    public ServerInfo serverInfo;


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
    /// Spawn a single zombie at random coordinates
    /// </summary>
    /// <param name="zombie"></param>
    void SpawnZombie(GameObject zombie)
    {
        Vector3 spawn = randomSpawn();
        GameObject zomb = Instantiate(zombie, spawn, Quaternion.identity);
        
        ZombieCharacterControl zcc = zomb.AddComponent<ZombieCharacterControl>();
        zcc.serverInfo = serverInfo;

        NetworkServer.Spawn(zomb);
    }
    
    
    /// <summary>
    /// Spawn multiple zombies
    /// </summary>
    void SpawnZombies()
    {
        spawnTimer += Time.deltaTime;
        if (compteur % 5 == 0)
        {
            if (spawnTimer >= 10f)
            {
                spawnTimer = 0f;

                for (int i = 0; i < 2; i++)
                {
                    SpawnZombie(Zombie_crawler);
                }
            }
            compteur++;
        }
        else
        {
            if (spawnTimer >= 10f)
            {
                spawnTimer = 0f;

                for (int i = 0; i < spawnCount; i++)
                {
                    SpawnZombie(Zombie_normal);
                }
            }

            compteur++;
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

