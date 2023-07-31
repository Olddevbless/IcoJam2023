using CommonComponents.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public List<AttackSO> combo;
    float lastClickedTime;
    float lastComboEnd;
    [SerializeField]int comboCounter;
    Animator animator;
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
                        hit.collider.GetComponent<EnemyFSM>().TakeDamage(combo[comboCounter].weapon.Damage);

                        hit.collider.attachedRigidbody.AddForce(this.transform.forward * 5, ForceMode.Impulse);


                    }
                }

            }
        }
    }
    void ExitAttack()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime>0.9f && animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            Invoke("EndCombo",1);
        }
    }
    void EndCombo()
    {
        comboCounter = 0;
        lastComboEnd = Time.time;
    }
}
