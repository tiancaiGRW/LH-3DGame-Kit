using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrendierDeathSMB : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       animator.transform.GetComponent<BoxCollider>().enabled = false;

        animator.transform.Find("DeathEffect").gameObject.SetActive(false);
        animator.transform.Find("DeathEffect").gameObject.SetActive(true);
    }


}
