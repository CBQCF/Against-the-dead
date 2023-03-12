using UnityEngine;
using UnityEngine.Serialization;

public class SpawnZone : MonoBehaviour
{
    [SerializeField] private GameObject petitZombie;
    
    [SerializeField] private GameObject BossZombie;

    [SerializeField] private Vector3 zoneSize;

    private float spawnTimer = 0f;
    private int spawnCount = 10;
    private int compteur = 0;

    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (compteur % 5 == 0)
        {
            if (spawnTimer >= 10f)
            {
                spawnTimer = 0f;

                for (int i = 0; i < 7; i++)
                {
                    GameObject instantiated = Instantiate(BossZombie);

                    instantiated.transform.position = new Vector3(
                        Random.Range(transform.position.x - zoneSize.x / 2, transform.position.x + zoneSize.x / 2),
                        Random.Range(transform.position.y - zoneSize.y / 2, transform.position.y + zoneSize.y / 2),
                        Random.Range(transform.position.z - zoneSize.z / 2, transform.position.z + zoneSize.z / 2)
                    );
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
                    GameObject instantiated = Instantiate(petitZombie);

                    instantiated.transform.position = new Vector3(
                        Random.Range(transform.position.x - zoneSize.x / 2, transform.position.x + zoneSize.x / 2),
                        Random.Range(transform.position.y - zoneSize.y / 2, transform.position.y + zoneSize.y / 2),
                        Random.Range(transform.position.z - zoneSize.z / 2, transform.position.z + zoneSize.z / 2)
                    );
                }
            }

            compteur++;
        }
        
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, zoneSize);
    }
}

