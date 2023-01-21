using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{

    private float timePassed = 0;
    private float clipLength;
    private bool attack = false;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>(); //cache animator
    }

    public void LateUpdate()
    {
         //counting time passed during animation
         if (_animator.GetBool("inCombat"))
        {
            clipLength = _animator.GetCurrentAnimatorClipInfo(1)[0].clip.length; //get length of clip
        }

        if (attack)
        {
            timePassed += Time.deltaTime; //keeps track of time passed
        }

        if (timePassed > clipLength)
        {
            EndAttack();
        }
      
    }
    
    public void StartAttack()
    {
        if (timePassed == 0)
        {
            _animator.applyRootMotion = true;
            _animator.SetTrigger("attackTrigger");
            attack = true;
        }
    }

    public void EndAttack()
    {
        attack = false;
        _animator.applyRootMotion = false;
        timePassed = 0;
    }




}
