using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EllenWeaponSMB :StateMachineBehaviour
{

    public int index;

     Transform AttackEffect;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.GetComponent<PlayerController>().ShowWeapon();

        AttackEffect = animator.transform.Find("TrailEffect /Ellen_Staff_Swish0" + index);
        AttackEffect.gameObject.SetActive(false);
        AttackEffect.gameObject.SetActive(true);

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.GetComponent<PlayerController>().HideWeapon();
        AttackEffect.gameObject.SetActive(false);
    }
}
