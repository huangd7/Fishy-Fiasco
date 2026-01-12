using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubbles : MonoBehaviour
{
    public float oxygenAmount = 25f;

    private void OnTriggerEnter(Collider other)
    {
        OxygenSystem oxy = other.GetComponent<OxygenSystem>();
        if (oxy != null)
        {
            oxy.AddOxygen(oxygenAmount);
            Destroy(gameObject);
        }
    }
}