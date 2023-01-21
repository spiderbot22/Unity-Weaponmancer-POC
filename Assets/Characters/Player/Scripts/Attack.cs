using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{

    private float timePassed;
    private float clipLength;
    private float clipSpeed;
    bool attack = false;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void LateUpdate()
    {
        timePassed += Time.deltaTime; //counting time passed during animation
        clipLength = _animator.GetCurrentAnimatorClipInfo(1)[0].clip.length; //get length of clip

        if (timePassed >= clipLength && attack && _animator.GetBool("inCombat"))
        {
            StartAttack();
            Debug.Log("start");
        }

        if (timePassed >= clipLength)
        {
            EndAttack();
            Debug.Log("end");
        }
      
    }

    public void Attacking()
    {
        attack = true;
    }

    public void StartAttack()
    {
        _animator.applyRootMotion = true;
        _animator.SetTrigger("attackTrigger");
        timePassed = 0;
    }

    public void EndAttack()
    {
        attack = false;
        _animator.applyRootMotion = false;
    }




}
