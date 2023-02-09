using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    [SerializeField]
    private GameObject cubePrefab;

    [SerializeField]
    private Vector3 zoneSize;

    private float spawnTimer = 0f;
    private int spawnCount = 10;

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= 10f)
        {
            spawnTimer = 0f;

            for (int i = 0; i < spawnCount; i++)
            {
                GameObject instantiated = Instantiate(cubePrefab);
                instantiated.AddComponent<BoxCollider>();

                instantiated.transform.position = new Vector3(
                    Random.Range(transform.position.x - zoneSize.x / 2, transform.position.x + zoneSize.x / 2),
                    Random.Range(transform.position.y - zoneSize.y / 2, transform.position.y + zoneSize.y / 2),
                    Random.Range(transform.position.z - zoneSize.z / 2, transform.position.z + zoneSize.z / 2)
                    );
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, zoneSize);
    }
}

