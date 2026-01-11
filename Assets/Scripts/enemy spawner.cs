using UnityEngine;
using UnityEngine.AI;
using System.Collections;
public class EnemySpawner : MonoBehaviour
{
    public GameObject anglerfishPrefab;
    public GameObject sharkPrefab;

    public int anglerfishCount = 2;
    public int sharkCount = 2;

    public Vector3 center = Vector3.zero;
    public float spawnRadius = 30f;

    void Start()
    {
        StartCoroutine(SpawnAfterDelay());
    }

    IEnumerator SpawnAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);
        SpawnMany(anglerfishPrefab, anglerfishCount);
        SpawnMany(sharkPrefab, sharkCount);
    }

    void SpawnMany(GameObject prefab, int count)
    {
        if (prefab == null) return;

        for (int i = 0; i < count; i++)
        {
            Vector3 pos = RandomPointOnNavMesh();
            GameObject e = Instantiate(prefab, pos + Vector3.up * 0.2f, Quaternion.identity);
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