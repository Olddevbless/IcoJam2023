using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    [SerializeField] int maxHP = 3;
    public int currentHP;
    public int TakeDamage(int damage)
    {
        currentHP -= damage;
        return currentHP;
    }
}
