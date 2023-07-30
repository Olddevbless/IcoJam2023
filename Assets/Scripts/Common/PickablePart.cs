using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickablePart : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().GrowShield();
        }
    }
}
