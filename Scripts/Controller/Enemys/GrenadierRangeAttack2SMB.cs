using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadierRangeAttack2SMB : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //播放相应的特效
       GameObject obj = animator.transform.GetComponent<Grenadier>().ShootPosition.Find("GrenadeForm").gameObject;
        obj.SetActive(false);
        obj.SetActive(true);
        //创建子弹  并不发射
        animator.transform.GetComponent<Grenadier>().CreateBullet();
    }

}
