using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OxygenSpawner : MonoBehaviour
{
    private static System.Random rand = new System.Random();
    public GameObject BubblePrefab;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SpawnBubbles();
    }
    void SpawnBubbles()
    {
        int chance = rand.Next(0, 125);
        if (chance == 20)
        {
            float Xcoord = (float)(rand.Next(0, 20) * 14.25);
            float Zcoord = (float)(rand.Next(0, 20) * 14.25);

            Instantiate(BubblePrefab, new Vector3(Xcoord, -1, Zcoord), Quaternion.identity);
        }
    }
}