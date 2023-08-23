using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spitter : Chomper
{
    public float EscpeDistance;  //逃跑距离
    public GameObject bulletPerfab;

    public override void FollowTarget()
    {
        ListenerSpeed();
        // base.FollowTarget();
            
        if (Target != null)
        {
            try
            {
                //判断是否逃跑
                if (Vector3.Distance(transform.position, Target.transform.position) <= EscpeDistance)
                {
                    //逃跑
                    Escape();
                    return;
                }
                //向目标移动
                MoveToTarget();
                //判断路径是否有效 是否能够到达
                if (meshAgent.pathStatus == NavMeshPathStatus.PathPartial || meshAgent.pathStatus == NavMeshPathStatus.PathPartial)//pathStatus：路径状态  PathPartial：路径无效  PathPartial：路径丢失
                {
                    if (Vector3.Distance(transform.position,Target.transform.position) > attackDistance)
                    {
                     //目标丢失
                     LoseTarget();
                     return;
                   }
                }
                //是否目标在可追踪范围内
                if (Vector3.Distance(transform.position, Target.transform.position) > followDistance)
                {
                    //目标丢失
                    LoseTarget();
                    return;
                }
                //追踪时同样检测攻击目标是否死亡
                if (!Target.transform.GetComponent<damagable>().IsAlive)//受击目标挂载的damagble里的IsAlive属性为否
                {
                    //目标丢失
                    LoseTarget();
                    return;
                }
                //是否在攻击范围内
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
                //追踪出错 目标丢失
                LoseTarget();
            }
            //try{}chach{}方法：怪物在追踪和攻击过程中打到其他怪物，或者检测路径和攻击目标时出错     执行后续动作所使用的方法

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
        //创建一个子弹
        GameObject bullet  = GameObject.Instantiate(bulletPerfab);
        bullet.transform.position = transform.Find("BullletPos").position;//子弹生成的位置
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
