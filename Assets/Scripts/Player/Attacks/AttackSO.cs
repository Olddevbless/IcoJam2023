using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Attacks/NormalAttack")]
public class AttackSO : ScriptableObject
{
    public Weapon weapon;
    public int damageMultiplier;
    public AnimatorOverrideController animatorOV;
}
