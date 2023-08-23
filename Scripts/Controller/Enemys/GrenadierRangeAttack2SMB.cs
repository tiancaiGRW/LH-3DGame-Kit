using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadierRangeAttack2SMB : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //������Ӧ����Ч
       GameObject obj = animator.transform.GetComponent<Grenadier>().ShootPosition.Find("GrenadeForm").gameObject;
        obj.SetActive(false);
        obj.SetActive(true);
        //�����ӵ�  ��������
        animator.transform.GetComponent<Grenadier>().CreateBullet();
    }

}
