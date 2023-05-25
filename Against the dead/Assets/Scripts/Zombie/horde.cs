using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEditor;
using UnityEngine;

public class horde : MonoBehaviour
{
    public GameObject zombiePrefab;

    public GameObject crawlerPrefab;

    public TestHorde spawner;
    
    void Start()
    {
        int sizeHorde = Random.Range(1, 10);
        for (int i = 0; i < sizeHorde; i++)
        {
            var spawnpos = new Vector3(
                Random.Range(transform.position.x - 10, transform.position.x + 10),
                transform.position.y,
                Random.Range(transform.position.z - 10, transform.position.z + 10)
            );
            GameObject Zombie = Instantiate(zombiePrefab, spawnpos, Quaternion.identity);
            Zombie.transform.parent = gameObject.transform;
            ZombieCharacterControl zcc = Zombie.AddComponent<ZombieCharacterControl>();
            zcc.serverInfo = spawner.serverInfo;
            NetworkServer.Spawn(Zombie);
        }

        GameObject Boss = Instantiate(crawlerPrefab, transform.position, Quaternion.identity);
        Boss.transform.parent = gameObject.transform;
        ZombieCharacterControl zccc = Boss.AddComponent<ZombieCharacterControl>();
        zccc.serverInfo = spawner.serverInfo;
        NetworkServer.Spawn(Boss);
    }

    [Server] 
    
    void Update()
    {
        if (transform.childCount == 0)
        {
            spawner.SpawnHorde();
            NetworkServer.Destroy(this.gameObject);
        }
    }
}
