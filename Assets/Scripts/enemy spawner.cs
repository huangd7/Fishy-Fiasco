using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject anglerfishPrefab;
    public GameObject sharkPrefab;

    public GameObject fishType3Prefab;
    public GameObject fishType4Prefab;

    public int anglerfishCount = 2;
    public int sharkCount = 2;
    public int fishType3Count = 2;
    public int fishType4Count = 2;

    public Vector3 center = Vector3.zero;
    public float spawnRadius = 30f;

    public float spawnDelay = 1.5f;

    void Start()
    {
        StartCoroutine(SpawnAfterDelay());
    }

    IEnumerator SpawnAfterDelay()
    {
        yield return new WaitForSeconds(spawnDelay);

        SpawnMany(anglerfishPrefab, anglerfishCount);
        SpawnMany(sharkPrefab, sharkCount);
        SpawnMany(fishType3Prefab, fishType3Count);
        SpawnMany(fishType4Prefab, fishType4Count);
    }

    void SpawnMany(GameObject prefab, int count)
    {
        if (prefab == null) return;

        for (int i = 0; i < count; i++)
        {
            Vector3 pos = RandomPointOnNavMesh();
            Instantiate(prefab, pos + Vector3.up * 0.2f, Quaternion.identity);
        }
    }

    Vector3 RandomPointOnNavMesh()
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 random = center + Random.insideUnitSphere * spawnRadius;
            random.y = center.y;

            if (NavMesh.SamplePosition(random, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }
        return center;
    }
}