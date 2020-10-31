using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableAnimatorTime : MonoBehaviour
{
    [SerializeField] Animator anim;

    void Start()
    {
        Invoke("EnableAnimator", 1);
    }

    void EnableAnimator()
    {
        anim.enabled = true;
    }
    
}
