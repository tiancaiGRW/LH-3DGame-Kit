using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spitter : Chomper
{
    public float EscpeDistance;  //���ܾ���
    public GameObject bulletPerfab;

    public override void FollowTarget()
    {
        ListenerSpeed();
        // base.FollowTarget();
            
        if (Target != null)
        {
            try
            {
                //�ж��Ƿ�����
                if (Vector3.Distance(transform.position, Target.transform.position) <= EscpeDistance)
                {
                    //����
                    Escape();
                    return;
                }
                //��Ŀ���ƶ�
                MoveToTarget();
                //�ж�·���Ƿ���Ч �Ƿ��ܹ�����
                if (meshAgent.pathStatus == NavMeshPathStatus.PathPartial || meshAgent.pathStatus == NavMeshPathStatus.PathPartial)//pathStatus��·��״̬  PathPartial��·����Ч  PathPartial��·����ʧ
                {
                    if (Vector3.Distance(transform.position,Target.transform.position) > attackDistance)
                    {
                     //Ŀ�궪ʧ
                     LoseTarget();
                     return;
                   }
                }
                //�Ƿ�Ŀ���ڿ�׷�ٷ�Χ��
                if (Vector3.Distance(transform.position, Target.transform.position) > followDistance)
                {
                    //Ŀ�궪ʧ
                    LoseTarget();
                    return;
                }
                //׷��ʱͬ����⹥��Ŀ���Ƿ�����
                if (!Target.transform.GetComponent<damagable>().IsAlive)//�ܻ�Ŀ����ص�damagble���IsAlive����Ϊ��
                {
                    //Ŀ�궪ʧ
                    LoseTarget();
                    return;
                }
                //�Ƿ��ڹ�����Χ��
                if (Vector3.Distance(transform.position, Target.transform.position) <= attackDistance)
                {

                    if (isCanAttack)
                    {
                        Attack();
                        isCanAttack = false;
                    }

                }
            }
            catch (Exception e)
            {
                //׷�ٳ��� Ŀ�궪ʧ
                LoseTarget();
            }
            //try{}chach{}������������׷�ٺ͹��������д�����������߼��·���͹���Ŀ��ʱ����     ִ�к���������ʹ�õķ���

        }
    }

    public void Escape()
    {
        animator.ResetTrigger("attack");
        meshAgent.isStopped = false;
        meshAgent.speed = moveSpeed;
        Vector3 target = transform.position + (transform.position - Target.transform.position).normalized;
        meshAgent.SetDestination(target);
    }

    protected override void OnAnimatorMove()
    {
       
    }
    public override void AttackBegin()
    {
        if(Target != null)
        {
        //����һ���ӵ�
        GameObject bullet  = GameObject.Instantiate(bulletPerfab);
        bullet.transform.position = transform.Find("BullletPos").position;//�ӵ����ɵ�λ��
        bullet.GetComponent<SpitterBullet>().Shot(Target.transform.position, transform.forward);

        }
    }
    public override void AttackEnd()
    {
       
    }

    public override void MoveToTarget()
    {
        base.MoveToTarget();

        if (Vector3.Distance(transform.position,Target.transform.position) <= attackDistance)
        {
            meshAgent.isStopped = true;  
        }
        else
        {
            meshAgent.isStopped = false;
        }
    }
    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
       // UnityEditor.Handles.color = new Color(Color.gray.r, Color.gray.g, Color.gray.b, 0.2f);
        //UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, 360, EscpeDistance);
    }

}
