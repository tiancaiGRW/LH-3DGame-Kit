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
    #region �ֶ�
    public float chackDistance;//���ľ���
    public float maxHeightDiff;//���ĸ߶Ȳ�

    [Range(0, 180)]
    public float lookAngle;  //��Ұ��Χ
    RaycastHit[] results = new RaycastHit[10];//�ܻ�Ŀ���б�

    public float followDistance;//���پ���
    public float attackDistance;//��������

    public LayerMask layerMask;

    public GameObject Target;

    protected NavMeshAgent meshAgent;//����ϵͳ

    protected Vector3 startPosition;//��ʼλ��

    public float runSpeed = 4;
    public float walkSpeed = 2;
  protected float moveSpeed = 0;

  protected Animator animator;

    protected Rigidbody rigidbody;

    protected bool isCanAttack = true;

    public float attackTime;  //����ʱ����

   private float attackTimer;   //����ʱ���ʱ

    protected damagable damagable;
    #endregion


    #region ��������
    protected virtual  void Start()
    {
        damagable=transform.GetComponent<damagable>();
        meshAgent = transform.GetComponent<NavMeshAgent>();
        startPosition = transform.position;//��ʼλ��
        animator = transform.GetComponent<Animator>();
        rigidbody= transform.GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        //�ж��Լ��Ƿ��Ǵ��״̬
        if ( !damagable.IsAlive ) { return; }

        if (Target != null && Target.GetComponent<PlayerInput>() !=null)//��������¼����������ܱ��������ж�
        {
            if (Target.GetComponent<PlayerInput>().IsHaveControl() == false && Target.GetComponent<damagable>().IsAlive)
            {
                //Ŀ��ʧȥ����Ȩ�һ��ţ����ܹ���
                animator.speed = 0;//��ͣ��������ֹ���Ź������� ��������й���
                return;
            }
            else
            {
                animator.speed = 1;//���Խ��й������Ͳ��Ŷ���
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

    //��ʾ��ⷶΧ
    protected virtual void OnDrawGizmosSelected()
    {
        //������ⷶΧ
        Gizmos.color = new Color(Color.blue.r, Color.blue.g, Color.blue.b, 0.4f);
        Gizmos.DrawSphere(transform.position, chackDistance);

        //�������ٷ�Χ
        Gizmos.color = new Color(Color.gray.r, Color.gray.g, Color.gray.b, 0.4f);
        Gizmos.DrawSphere(transform.position, followDistance);

        //����������Χ
        Gizmos.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.4f);
        Gizmos.DrawSphere(transform.position, attackDistance);

        //�������߶Ȳ�
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * maxHeightDiff);
        Gizmos.DrawLine(transform.position, transform.position - Vector3.up * maxHeightDiff);
        //������Ұ��Χ
        //UnityEditor.Handles.color = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, 0.4f);
        //UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, lookAngle, chackDistance);
        //UnityEditor.Handles.DrawSolidArc(transform.position, -Vector3.up, transform.forward, lookAngle, chackDistance);
    }
    #endregion

    #region ��������

    //���Ŀ��
    public virtual void ChackTarget()
    {
        int cunt = Physics.SphereCastNonAlloc(transform.position, chackDistance, Vector3.forward, results, 0, layerMask.value);


        for (int i = 0; i < cunt; i++)
        {
            //�ж��Ƿ��ǿɹ�������Ϸ����
            if (results[i].transform.GetComponent<damagable>() == null)
            {
                continue;
            }

            //�жϸ߶Ȳ�
            if (Mathf.Abs(results[i].transform.position.y - transform.position.y) > maxHeightDiff)
            {
                continue;
            }


            //�Ƿ�����Ұ��Χ��
            if (Vector3.Angle(transform.forward, results[i].transform.position - transform.position) > lookAngle)
            {
                continue;
            }
            //�жϹ���Ŀ���Ƿ��ǻ��ŵ�״̬
            if (! results[i].transform.GetComponent<damagable>().IsAlive )//�ܻ�Ŀ����ص�damagble���IsAlive����Ϊ��
            {
                continue;
            }
            //�ҵ�Ŀ�꣨ѡ��һ�������Ŀ����й�����
            if (Target != null)
            {
                //�ж�һ�¾���
                float distance = Vector3.Distance(transform.position, Target.transform.position);//��ǰ�ѻ�ȡ��Ϸ�����target���Լ��ľ���
                float currentDistance = Vector3.Distance(transform.position, results[i].transform.position);//�Լ������¼�⵽����Ϸ����ľ���
                if (currentDistance < distance)//�¾���С��֮ǰ�Ѽ��ľ���
                {
                    Target = results[i].transform.gameObject;//target��ȡ�����¼�������
                }
            }
            else
            {
                Target = results[i].transform.gameObject;//targetΪ�գ���ȡ��һ��Ŀ����Ϸ����


            }

        }
    }
    //��Ŀ���ƶ�
    public virtual void MoveToTarget()
    {
        if (Target != null && transform.GetComponent<damagable>().IsAlive) 
        {
            meshAgent.SetDestination(Target.transform.position);
        }
    }
    //׷��Ŀ��
    public virtual void FollowTarget()
    {

        //�����ٶ�
        ListenerSpeed();

        if (Target != null)
        {
            try
            {
                //��Ŀ���ƶ�
                MoveToTarget();

                //�ж�·���Ƿ���Ч �Ƿ��ܹ�����
                if (meshAgent.pathStatus == NavMeshPathStatus.PathPartial || meshAgent.pathStatus == NavMeshPathStatus.PathPartial)//pathStatus��·��״̬  PathPartial��·����Ч  PathPartial��·����ʧ
                {
                    //Ŀ�궪ʧ
                    LoseTarget();
                    return;
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
    //Ŀ�궪ʧ����
    public virtual void LoseTarget()
    {
        Target = null;

        if (transform.GetComponent<damagable>().IsAlive)//������ﻹ���ţ�ִ�лص���ʼλ�ã��Ѿ�������ִ��
        {
        //�ص���ʼλ��
        meshAgent.SetDestination(startPosition);
        moveSpeed = walkSpeed;

        }
    }
    //�����ٶ�
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
