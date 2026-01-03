using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public int width, height;
    public float wallSize = 1f;
    public bool[,] visited;
    public Material wallMaterial;
    public List<GameObject> walls = new List<GameObject>();

    void Start()
    {
        GenerateMaze();
    }

    void Update()
    {
        // Space no longer regenerates the maze
        // (We just leave Update empty for now.)
    }

    void GenerateMaze()
    {
        if (width <= 0 || height <= 0) return;

        visited = new bool[width, height];
        CreateGrid();

        StartCoroutine(Generate(0, 0));
    }

    void CreateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 center = new Vector3(x * wallSize, 0f, y * wallSize);
                float halfWS = 0.5f * wallSize;

                Vector3 horizontal = new Vector3(wallSize, wallSize, 0.1f * wallSize);
                Vector3 vertical = new Vector3(0.1f * wallSize, wallSize, wallSize);

                Vector3 north = center + new Vector3(0f, 0f, halfWS);
                Vector3 east = center + new Vector3(halfWS, 0f, 0f);
                Vector3 south = center + new Vector3(0f, 0f, -halfWS);
                Vector3 west = center + new Vector3(-halfWS, 0f, 0f);

                CreateWall(north, horizontal);
                CreateWall(east, vertical);
                CreateWall(south, horizontal);
                CreateWall(west, vertical);
            }
        }
    }

    void CreateWall(Vector3 position, Vector3 scale)
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.transform.position = position;
        wall.transform.localScale = scale;

        // Apply material if assigned
        if (wallMaterial != null)
            wall.GetComponent<Renderer>().material = wallMaterial;

        walls.Add(wall);
    }

    void ClearMaze()
    {
        foreach (GameObject wall in walls)
        {
            if (wall != null)
                Destroy(wall);
        }
        walls.Clear();
    }

    IEnumerator Generate(int x, int y)
    {
        visited[x, y] = true;

        while (true)
        {
            List<Vector2Int> neighbors = GetUnvisitedNeighbors(x, y);

            if (neighbors.Count == 0)
                yield break;

            int randIndex = Random.Range(0, neighbors.Count);
            Vector2Int next = neighbors[randIndex];

            RemoveWallBetween(x, y, next.x, next.y);

            yield return new WaitForSeconds(0.01f);
            yield return StartCoroutine(Generate(next.x, next.y));
        }
    }

    List<Vector2Int> GetUnvisitedNeighbors(int x, int y)
    {
        List<Vector2Int> result = new List<Vector2Int>();

        if (x - 1 >= 0 && !visited[x - 1, y])
            result.Add(new Vector2Int(x - 1, y));

        if (x + 1 < width && !visited[x + 1, y])
            result.Add(new Vector2Int(x + 1, y));

        if (y - 1 >= 0 && !visited[x, y - 1])
            result.Add(new Vector2Int(x, y - 1));

        if (y + 1 < height && !visited[x, y + 1])
            result.Add(new Vector2Int(x, y + 1));

        return result;
    }

    void RemoveWallBetween(int x1, int y1, int x2, int y2)
    {
        Vector3 pos1 = new Vector3(x1 * wallSize, 0f, y1 * wallSize);
        Vector3 pos2 = new Vector3(x2 * wallSize, 0f, y2 * wallSize);
        Vector3 mid = (pos1 + pos2) / 2f;

        float threshold = 0.01f;

        for (int i = walls.Count - 1; i >= 0; i--)
        {
            GameObject wall = walls[i];
            if (wall == null) continue;

            if (Vector3.Distance(wall.transform.position, mid) < threshold)
            {
                Destroy(wall);
                walls.RemoveAt(i);
            }
        }
    }
}
