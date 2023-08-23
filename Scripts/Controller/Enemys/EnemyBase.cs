using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(damagable))]

public class EnemyBase : MonoBehaviour
{
    #region 字段
    public float chackDistance;//检测的距离
    public float maxHeightDiff;//最大的高度差

    [Range(0, 180)]
    public float lookAngle;  //视野范围
    RaycastHit[] results = new RaycastHit[10];//受击目标列表

    public float followDistance;//跟踪距离
    public float attackDistance;//攻击距离

    public LayerMask layerMask;

    public GameObject Target;

    protected NavMeshAgent meshAgent;//导航系统

    protected Vector3 startPosition;//初始位置

    public float runSpeed = 4;
    public float walkSpeed = 2;
  protected float moveSpeed = 0;

  protected Animator animator;

    protected Rigidbody rigidbody;

    protected bool isCanAttack = true;

    public float attackTime;  //攻击时间间隔

   private float attackTimer;   //攻击时间计时

    protected damagable damagable;
    #endregion


    #region 生命周期
    protected virtual  void Start()
    {
        damagable=transform.GetComponent<damagable>();
        meshAgent = transform.GetComponent<NavMeshAgent>();
        startPosition = transform.position;//初始位置
        animator = transform.GetComponent<Animator>();
        rigidbody= transform.GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        //判断自己是否是存活状态
        if ( !damagable.IsAlive ) { return; }

        if (Target != null && Target.GetComponent<PlayerInput>() !=null)//人物进行事件交互，不能被攻击的判断
        {
            if (Target.GetComponent<PlayerInput>().IsHaveControl() == false && Target.GetComponent<damagable>().IsAlive)
            {
                //目标失去控制权且活着，不能攻击
                animator.speed = 0;//暂停动画，防止播放攻击动画 对人物进行攻击
                return;
            }
            else
            {
                animator.speed = 1;//可以进行攻击，就播放动画
            }
        }
        ChackTarget();
        FollowTarget();

        if (!isCanAttack)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer>= attackTime)
            {
                isCanAttack = true;
                attackTimer = 0;
            }
        }
    }

    protected virtual void OnAnimatorMove()
    {
        rigidbody.MovePosition(transform.position + animator.deltaPosition);
    }

    //显示检测范围
    protected virtual void OnDrawGizmosSelected()
    {
        //画出检测范围
        Gizmos.color = new Color(Color.blue.r, Color.blue.g, Color.blue.b, 0.4f);
        Gizmos.DrawSphere(transform.position, chackDistance);

        //画出跟踪范围
        Gizmos.color = new Color(Color.gray.r, Color.gray.g, Color.gray.b, 0.4f);
        Gizmos.DrawSphere(transform.position, followDistance);

        //画出攻击范围
        Gizmos.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.4f);
        Gizmos.DrawSphere(transform.position, attackDistance);

        //画出最大高度差
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * maxHeightDiff);
        Gizmos.DrawLine(transform.position, transform.position - Vector3.up * maxHeightDiff);
        //画出视野范围
        //UnityEditor.Handles.color = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, 0.4f);
        //UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, lookAngle, chackDistance);
        //UnityEditor.Handles.DrawSolidArc(transform.position, -Vector3.up, transform.forward, lookAngle, chackDistance);
    }
    #endregion

    #region 函数方法

    //检测目标
    public virtual void ChackTarget()
    {
        int cunt = Physics.SphereCastNonAlloc(transform.position, chackDistance, Vector3.forward, results, 0, layerMask.value);


        for (int i = 0; i < cunt; i++)
        {
            //判断是否是可攻击的游戏物体
            if (results[i].transform.GetComponent<damagable>() == null)
            {
                continue;
            }

            //判断高度差
            if (Mathf.Abs(results[i].transform.position.y - transform.position.y) > maxHeightDiff)
            {
                continue;
            }


            //是否在视野范围内
            if (Vector3.Angle(transform.forward, results[i].transform.position - transform.position) > lookAngle)
            {
                continue;
            }
            //判断攻击目标是否是活着的状态
            if (! results[i].transform.GetComponent<damagable>().IsAlive )//受击目标挂载的damagble里的IsAlive属性为否
            {
                continue;
            }
            //找到目标（选择一个最近的目标进行攻击）
            if (Target != null)
            {
                //判断一下距离
                float distance = Vector3.Distance(transform.position, Target.transform.position);//当前已获取游戏物体的target与自己的距离
                float currentDistance = Vector3.Distance(transform.position, results[i].transform.position);//自己与最新检测到的游戏物体的距离
                if (currentDistance < distance)//新距离小于之前已检测的距离
                {
                    Target = results[i].transform.gameObject;//target获取到最新检测的物体
                }
            }
            else
            {
                Target = results[i].transform.gameObject;//target为空，获取到一个目标游戏物体


            }

        }
    }
    //向目标移动
    public virtual void MoveToTarget()
    {
        if (Target != null && transform.GetComponent<damagable>().IsAlive) 
        {
            meshAgent.SetDestination(Target.transform.position);
        }
    }
    //追踪目标
    public virtual void FollowTarget()
    {

        //监听速度
        ListenerSpeed();

        if (Target != null)
        {
            try
            {
                //向目标移动
                MoveToTarget();

                //判断路径是否有效 是否能够到达
                if (meshAgent.pathStatus == NavMeshPathStatus.PathPartial || meshAgent.pathStatus == NavMeshPathStatus.PathPartial)//pathStatus：路径状态  PathPartial：路径无效  PathPartial：路径丢失
                {
                    //目标丢失
                    LoseTarget();
                    return;
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
    //目标丢失方法
    public virtual void LoseTarget()
    {
        Target = null;

        if (transform.GetComponent<damagable>().IsAlive)//如果怪物还活着，执行回到初始位置，已经死亡不执行
        {
        //回到初始位置
        meshAgent.SetDestination(startPosition);
        moveSpeed = walkSpeed;

        }
    }
    //监听速度
    public virtual void ListenerSpeed()
    {
        if (Target != null)
        {
            moveSpeed = runSpeed;
        }

        meshAgent.speed = moveSpeed;

        animator.SetFloat("Speed", meshAgent.velocity.magnitude);
    }

    public virtual void Attack()
    {
       
    }

    public virtual void OnDath(damagable damagable,DamageMessage data )
    {

    }

  
    #endregion


}
