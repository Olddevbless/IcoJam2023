using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickablePart : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        Destroy(this.gameObject, 3f);
    }
    private void OnCollisionEnter(Collision collision)
    
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().GrowShield();
        }
        this.transform.position = Vector3.MoveTowards(this.transform.position, collision.transform.position, Time.deltaTime);
        Destroy(this.gameObject,2f);
    }
}
