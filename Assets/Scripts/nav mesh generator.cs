using UnityEngine;
using Unity.AI.Navigation;
using System.Collections;

public class RuntimeNavMeshBuild : MonoBehaviour
{
    public NavMeshSurface surface;
    public float buildDelay = 0.2f;

    void Start()
    {
        if (surface == null) surface = GetComponent<NavMeshSurface>();
        StartCoroutine(BuildSoon());
    }

    IEnumerator BuildSoon()
    {
        yield return new WaitForSeconds(buildDelay);
        surface.BuildNavMesh();
    }
}