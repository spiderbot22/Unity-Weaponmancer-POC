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

    public void FixedUpdate()
    {
         //counting time passed during animation
        if (attack)
        {
            clipLength = _animator.GetCurrentAnimatorClipInfo(1)[0].clip.length; //get length of clip
            timePassed += Time.deltaTime; //keeps track of time passed
        }

        if (timePassed >= 0.2)
        {
            _animator.applyRootMotion = true;
        }

        if (timePassed >= clipLength - 0.2)
        {
            EndAttack();
        }
      
    }
    
    public void StartAttack()
    {
        
        if (timePassed == 0 && _animator.GetBool("inCombat"))
        {
            _animator.SetTrigger("attackTrigger");
            attack = true;
           // _animator.applyRootMotion = true;
        }

        if (timePassed >= clipLength*0.6) //start combo attack
        {
            timePassed = 0;
            _animator.SetTrigger("attackTrigger");
        }
    }

    public void EndAttack()
    {
        _animator.applyRootMotion = false;
        attack = false;
        timePassed = 0;
    }

    public void EndHeldAttack ()
    {
        _animator.SetBool("inputHeldDown", false);
        _animator.applyRootMotion = false;
    }

    public void RootMotionOn()
    {
        _animator.applyRootMotion = true;
    }

    public void RootMotionOff()
    {
        _animator.applyRootMotion = false;
    }

    public void ChangeAnimSpeed()
    {
        Debug.Log(_animator.speed);
        if (_animator.speed == 1)
        {
            _animator.speed = 1.5f;
        } 
        else
        {
            _animator.speed = 1;
        }
    }




}


