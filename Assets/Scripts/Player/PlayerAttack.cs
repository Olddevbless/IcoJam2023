using CommonComponents.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public List<AttackSO> combo;
    public AttackSO special;
    [SerializeField] GameObject specialPrefab;
    [SerializeField] GameObject shieldThrowPosition;
    float lastClickedTime;
    float lastComboEnd;
    [SerializeField]int comboCounter;
    Animator animator;
    [SerializeField] float shieldThrowPower;
    [SerializeField] float playerAttackRange;
    [SerializeField] float playerAttackRadius;
    [SerializeField] float timeBetweenAttacks;
    [SerializeField] float timeBetweenCombos;

    




    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ExitAttack();
    }

    public void Attack()
    {
        
        if (Time.time - lastComboEnd > timeBetweenCombos && comboCounter <= combo.Count)
        {
            CancelInvoke("EndCombo");

            if (Time.time-lastClickedTime >= timeBetweenAttacks)
            {
                animator.runtimeAnimatorController = combo[comboCounter].animatorOV;
                
                animator.Play("Attack", 0, 0);
                this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                comboCounter++;
                if (comboCounter+1 > combo.Count)
                {
                    comboCounter = 0;
                }
                lastClickedTime = Time.time;
                RaycastHit[] hits = Physics.SphereCastAll(this.transform.position, playerAttackRadius, Vector3.forward, playerAttackRange);
                foreach (RaycastHit hit in hits)
                {
                    if (hit.collider.CompareTag("Enemy"))
                    {
                        Debug.Log("AttackSuccessful");
                        if (comboCounter == 3)
                        {
                            hit.collider.GetComponent<EnemyFSM>().TakeDamage(3);//combo[comboCounter].weapon.Damage);
                        }
                        else
                        {
                            hit.collider.GetComponent<EnemyFSM>().TakeDamage(1);
                        }
                        

                        hit.collider.attachedRigidbody.AddForce(this.transform.forward * 10, ForceMode.Impulse);


                    }
                }

            }
        }
    }
    public void ThrowShield()
    {
        animator.runtimeAnimatorController = special.animatorOV;
        animator.Play("Attack", 0, 0);
        var shield = Instantiate(specialPrefab,shieldThrowPosition.transform.position,Quaternion.Euler(0,0,90));
        if (GetComponent<PlayerController>().shieldSize == 1)
        {
            shield.GetComponent<ShieldThrow>().shieldParts[0].SetActive(true);
        }
        if (GetComponent<PlayerController>().shieldSize == 2)
        {
            shield.GetComponent<ShieldThrow>().shieldParts[0].SetActive(true);
            shield.GetComponent<ShieldThrow>().shieldParts[1].SetActive(true);
        }
        if (GetComponent<PlayerController>().shieldSize == 3)
        {
            shield.GetComponent<ShieldThrow>().shieldParts[0].SetActive(true);
            shield.GetComponent<ShieldThrow>().shieldParts[1].SetActive(true);
            shield.GetComponent<ShieldThrow>().shieldParts[2].SetActive(true);
        }
        shield.SetActive(true);
        shield.GetComponent<Rigidbody>().AddForce(this.transform.forward * shieldThrowPower, ForceMode.Impulse);
        GetComponent<PlayerController>().shieldSize = 0;
        
        
    }
    void ExitAttack()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime>0.7f && animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
            Invoke("EndCombo",1);
        }
    }
    void EndCombo()
    {
        comboCounter = 0;
        lastComboEnd = Time.time;
    }
}
