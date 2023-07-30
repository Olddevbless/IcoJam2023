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
                var partrigidBody = part.AddComponent<Rigidbody>();
                partrigidBody.AddRelativeForce(new Vector3(Random.Range(1,3),Random.Range(1,2),Random.Range(1,3)),ForceMode.Impulse);
                part.AddComponent<BoxCollider>();
                part.AddComponent<PickablePart>();
            }
        }
    }
}
