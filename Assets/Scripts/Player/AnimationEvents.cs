using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    // Start is called before the first frame update
    public void InitiatePlayerAttackFromAnimation()
    {
        var player = FindAnyObjectByType<PlayerController>();
        //player.Attack();
    }
}
