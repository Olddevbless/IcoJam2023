using CommonComponents.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldThrow : MonoBehaviour, IDamageDealer
{
    public int Damage { get => 1; }

    // Start is called before the first frame update
    [SerializeField] float collisionsCounter;
    [SerializeField] float timeToDestruction;
    public GameObject[] shieldParts;
    PlayerController player;
    private void Start()
    {
        timeToDestruction = 0f;
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 10, 0);
        
        timeToDestruction += Time.deltaTime;
        if (collisionsCounter == 5||timeToDestruction>5f)
        {
            BreakApart breakApart = GetComponent<BreakApart>();
            breakApart.OnBreakApart(shieldParts);
            Destroy(this.gameObject);
        }
        
        
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        collisionsCounter++;
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyFSM>().TakeDamage(Damage*shieldParts.Length);
        }
    }
}
