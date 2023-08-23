 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;

public class Chomper : EnemyBase
{


    public WeaponAttackControler weapon;//字段weapon,相当于字段代替脚本，调用其中的功能

    protected override void Start()
    {
        base.Start();
        animator.Play("ChomperIdle",0,UnityEngine.Random.Range(0f,1f));//nomalizedTime表示动画进度0到1，给其一个0到1之间的随机数，让每个Chomper的idle动画不同步,随机数要带浮点（f）后缀,否则随机数只有0和1两个整形
    }
    public override void Attack()
    {
        ChangreDistance();
        base.Attack();
        animator.SetTrigger("attack");
    }

    //修改攻击瞄准方向  
    public void ChangreDistance()
    {
        if(Target != null)
        {
            
            Vector3 direction = Target.transform.position - transform.position;//建立一个自己到攻击目标的向量
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
    public override void OnDath(damagable damagable, DamageMessage data)//override重写
    {
        base.OnDath(damagable, data);

        //Destroy(gameObject);

        //丢失目标
        LoseTarget();
        //停止追踪
        meshAgent.isStopped= true;
        meshAgent.enabled= false;
        //播放死亡动画
        

        animator.SetTrigger("death");
       //添加一个力让chomper被打飞
        Vector3 force = transform.position - data.DamagePostion;//攻击者指向自身位置的向量
        force.y = 0;
        rigidbody.isKinematic = false;
        rigidbody.AddForce(force.normalized * 8 + Vector3.up * 4 , ForceMode.Impulse);

        //3s后销毁
        Invoke("Dissolve", 3);
    }

   
    public void Dissolve()
    {
        transform.Find("Body_Dissolve").gameObject.SetActive(true);
        transform.Find("Body").gameObject.SetActive(false);
        Destroy(gameObject,2);
    }
}
