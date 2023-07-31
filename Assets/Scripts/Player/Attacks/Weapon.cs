using CommonComponents.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName =("Attacks/Weapon"))]
public class Weapon : ScriptableObject , IDamageDealer
{
    

    public int Damage { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
}
