 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;

public class Chomper : EnemyBase
{


    public WeaponAttackControler weapon;//�ֶ�weapon,�൱���ֶδ���ű����������еĹ���

    protected override void Start()
    {
        base.Start();
        animator.Play("ChomperIdle",0,UnityEngine.Random.Range(0f,1f));//nomalizedTime��ʾ��������0��1������һ��0��1֮������������ÿ��Chomper��idle������ͬ��,�����Ҫ�����㣨f����׺,���������ֻ��0��1��������
    }
    public override void Attack()
    {
        ChangreDistance();
        base.Attack();
        animator.SetTrigger("attack");
    }

    //�޸Ĺ�����׼����  
    public void ChangreDistance()
    {
        if(Target != null)
        {
            
            Vector3 direction = Target.transform.position - transform.position;//����һ���Լ�������Ŀ�������
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    public virtual void AttackBegin()
    {
       

        weapon.BeginAttack();
    }
    public virtual void AttackEnd()
    {
        weapon.EndAttack();
    }
    public override void OnDath(damagable damagable, DamageMessage data)//override��д
    {
        base.OnDath(damagable, data);

        //Destroy(gameObject);

        //��ʧĿ��
        LoseTarget();
        //ֹͣ׷��
        meshAgent.isStopped= true;
        meshAgent.enabled= false;
        //������������
        

        animator.SetTrigger("death");
       //���һ������chomper�����
        Vector3 force = transform.position - data.DamagePostion;//������ָ������λ�õ�����
        force.y = 0;
        rigidbody.isKinematic = false;
        rigidbody.AddForce(force.normalized * 8 + Vector3.up * 4 , ForceMode.Impulse);

        //3s������
        Invoke("Dissolve", 3);
    }

   
    public void Dissolve()
    {
        transform.Find("Body_Dissolve").gameObject.SetActive(true);
        transform.Find("Body").gameObject.SetActive(false);
        Destroy(gameObject,2);
    }
}
