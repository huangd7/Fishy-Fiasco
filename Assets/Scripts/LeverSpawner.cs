using System.Collections.Generic;
using UnityEngine;

public class LeverSpawner : MonoBehaviour
{
    private static System.Random rand = new System.Random();

    public GameObject leverPrefab; // your "Goal" lever prefab
    // hatch is already in scene, so remove Hatch/HatchWin stuff

    void Start()
    {
        SpawnLevers();
    }

    void SpawnLevers()
    {
        int[] levers = new int[3];
        for (int x = 0; x < 3; x++) levers[x] = rand.Next(0, 8);

        for (int i = 0; i < 3; i++)
        {
            if (levers[i] == 0) Instantiate(leverPrefab, new Vector3(152, -7, 2), Quaternion.identity);
            if (levers[i] == 1) Instantiate(leverPrefab, new Vector3(90, -7, 0), Quaternion.identity);
            if (levers[i] == 2) Instantiate(leverPrefab, new Vector3(270, -7, 43), Quaternion.identity);
            if (levers[i] == 3) Instantiate(leverPrefab, new Vector3(285, -7, 120), Quaternion.identity);
            if (levers[i] == 4) Instantiate(leverPrefab, new Vector3(42, -7, 90), Quaternion.identity);
            if (levers[i] == 5) Instantiate(leverPrefab, new Vector3(0, -7, 224), Quaternion.identity);
            if (levers[i] == 6) Instantiate(leverPrefab, new Vector3(75, -7, 255), Quaternion.identity);
            if (levers[i] == 7) Instantiate(leverPrefab, new Vector3(195, -7, 271), Quaternion.identity);
        }
    }
}
