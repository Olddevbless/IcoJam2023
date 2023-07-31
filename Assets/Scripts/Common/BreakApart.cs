using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakApart : MonoBehaviour
{
    [SerializeField] GameObject[] bodyParts;
    public bool isBreaking =false;
    private void Update()
    {
        if (isBreaking == true)
        {
            OnBreakApart(bodyParts);
        }
    }
    // Start is called before the first frame update
    public void OnBreakApart(GameObject[] bodyParts)
    {
        foreach (var part in bodyParts)
        {
            if (part != null)
            {
                part.transform.SetParent(null);
               
                if (part.GetComponent<PickablePart>() == null)
                {
                    part.AddComponent<PickablePart>();
                }
                if (part.GetComponent<BoxCollider>() == null)
                {
                    
                    part.AddComponent<BoxCollider>();
                    
                   
                }
                if (part.GetComponent<Rigidbody>() == null)
                {
                    part.AddComponent<Rigidbody>();
                }
                if (part.GetComponent<Rigidbody>() != null)
                {
                    var partrigidbody = part.GetComponent<Rigidbody>();
                    partrigidbody.AddRelativeForce(new Vector3(Random.Range(1, 3), Random.Range(1, 2), Random.Range(1, 3)), ForceMode.Impulse);
                }
                
            }
        }
    }
}
