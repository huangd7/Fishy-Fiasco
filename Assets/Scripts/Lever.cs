using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    private static System.Random rand = new System.Random();
    public GameObject HatchWin;
    public GameObject Goal;
    public GameObject Hatch;
    public int LeversFlicked;
    public bool FlickLever;
    // Start is called before the first frame update
    void Start()
    {
         LeversFlicked = 0;
        SpawnLevers();
        SpawnHatch();
    }

    // Update is called once per frame
    void Update()
    {
        CheckLeverFlick();
    }
    void SpawnLevers()
    {

        int[] Levers = new int [3];
        for (int x = 0; x < 3; x++)
        {
            Levers[x] = rand.Next(0, 8);
        }
        for (int i = 0; i < 3; i++)
        {
            if(Levers[i] == 0)
            {
                Vector3 position = new Vector3(152, -7, 2);
                Instantiate(Goal, position, Quaternion.identity);
            }
            if (Levers[i] == 1)
            {
                Instantiate(Goal, new Vector3(90, -7, 0), Quaternion.identity);
            }
            if (Levers[i] == 2)
            {
                Instantiate(Goal, new Vector3(270, -7, 43), Quaternion.identity);
            }
            if (Levers[i] == 3)
            {
                Instantiate(Goal, new Vector3(285, -7, 120), Quaternion.identity);
            }
            if (Levers[i] == 4)
            {
                Instantiate(Goal, new Vector3(42, -7, 90), Quaternion.identity);
            }
            if (Levers[i] == 5)
            {
                Instantiate(Goal, new Vector3(0, -7, 224), Quaternion.identity);
            }
            if (Levers[i] == 6)
            {
                Instantiate(Goal, new Vector3(75, -7, 255), Quaternion.identity);
            }
            if (Levers[i] == 7)
            {
                Instantiate(Goal, new Vector3(195, -7, 271), Quaternion.identity);
            }

        }

        
    }
    void CheckLeverFlick()
    {
        if (FlickLever == true)
        {
            LeversFlicked++;
        }
        if (LeversFlicked >= 3)
        {
            openHatch();
        }
        
    }
    void SpawnHatch()
    {
         HatchWin = Instantiate(Hatch, new Vector3(285, 6, 285), Quaternion.identity);
    }
    void openHatch()
    {
        Destroy(HatchWin);
    }

}
