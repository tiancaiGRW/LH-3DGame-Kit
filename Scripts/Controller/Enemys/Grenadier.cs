using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//boss攻击方式为三种 远攻 近距离范围攻 近距离普通攻
public class Grenadier : EnemyBase
{
    #region 字段
    public float shortAttackDistance;//短距离攻击
    //定义转向动画的name
    private int rangeAttackHash = Animator.StringToHash("GrenadierRangeAttack");
    private int meleeAttackHash = Animator.StringToHash("GrenadierMeleeAttack");
    private int rangeAttack2Hash = Animator.StringToHash("GrenadierRangeAttack2");

    private AnimatorStateInfo currentAnimatorInfo;//当前动画的信息

    public GameObject bossBulletPrefab;         //boss的子弹预制体

    public Transform ShootPosition;

    private BossBullet bossbullet;      //boss当前的子弹
    public WeaponAttackControler meleeAttackController;//普通攻击
    public WeaponAttackControler ranageAttackController;//范围攻击

    #endregion

    #region 周期
    protected override void Update()
    {
        base.Update();
        currentAnimatorInfo = animator.GetCurrentAnimatorStateInfo(0);
    }
    protected override void OnAnimatorMove()
    {
        //as base.OnAnimatorMove();
       transform.rotation *= animator.deltaRotation;

    }
    #endregion

    #region 方法
    public override void Attack()
    {
        base.Attack();

        if (currentAnimatorInfo.shortNameHash == rangeAttack2Hash || currentAnimatorInfo.shortNameHash == rangeAttackHash || currentAnimatorInfo.shortNameHash == meleeAttackHash) 
        {
            //刷新攻击trigger
            animator.ResetTrigger("attack");
            animator.ResetTrigger("attack_range");
            animator.ResetTrigger("attack_shot");
            return; 
        }//在播放转向的时候不进行攻击方法(利用动画名获取动画信息 检测当前动画状态实现对动画的控制)

        

        if (Vector3.Distance(transform.position, Target.transform.position) > shortAttackDistance) //boss和玩家间距离大与攻击短距离
        {
            Turn();
            //远距离攻击
            // Shot();
            animator.ResetTrigger("attack");
            animator.SetTrigger("attack");
        }
        else
        {
            //近距离攻击

            if (Vector3.Angle(transform.forward, Target.transform.position - transform.position) > 20)//近距离普通攻击视野为boss正前方20°角内(boss到玩家的方向向量与boss正前方夹角)
            {
                //范围攻击
                animator.ResetTrigger("attack_range");
                animator.SetTrigger("attack_range");
            }
            else
            {
                //普通攻击
                animator.ResetTrigger("attack_shot");
                animator.SetTrigger("attack_shot");
            }
        }
    }


    //转弯瞄准
    public void Turn()
    {
        //计算转角度数
        //Vector3.Angle(transform.forward,Target.transform.position - transform.position);//算出的是绝对值，不分左右
        float angle = Vector3.SignedAngle(transform.forward, Target.transform.position - transform.position, Vector3.up);

        if (Mathf.Abs(angle) > 10)//角度的绝对值大于十度进行转弯。角度过小没必要转向
        {
            animator.SetFloat("TurnAngle", angle);
            animator.SetTrigger("turn");
        }

    }
    //创建子弹
    public void CreateBullet()
    {
        GameObject bullet = GameObject.Instantiate(bossBulletPrefab,ShootPosition);
        bullet.transform.localPosition= Vector3.zero;
        bossbullet = bullet.GetComponent<BossBullet>();
    }
    public void Shot()
    {
      
        if (Target != null)
        {
           
            bossbullet.Shot(Target.transform.position, transform.forward);//传入攻击目标的位置 和 boss自己的前方方向
          
        }
        else 
        {
            Destroy(bossbullet.gameObject);
          
        }
        bossbullet = null;
    }

    public override void OnDath(damagable damagable, DamageMessage data)
    {
        base.OnDath(damagable, data);
        //播放死亡动画
        animator.SetTrigger("death");
        //销毁自己
        Destroy(gameObject, 6);
    }

    #endregion

    #region AnimationEvents
    public void MeleeAttackStart()
    {
        meleeAttackController.BeginAttack();
    }

    public void MeleeAttackEnd() 
    {
        meleeAttackController.EndAttack();
    }

    public void RanageAttackStart()
    {
        ranageAttackController.BeginAttack();
       GameObject shild = transform.Find("Shield").gameObject;
        shild.SetActive(false);
        shild.SetActive(true);

    }
    public void RanageAttackEnd()
    {
        ranageAttackController.EndAttack();
        transform.Find("Shield").gameObject.SetActive(false);
    }
    public void RanageAttackBegin()
    {
      GameObject charge = transform.Find("GrenadierSkeleton/Grenadier_Root/Grenadier_Hips/Grenadier_Sphere/Charge").gameObject;
        charge.SetActive(false);
        charge.SetActive(true);
    }
    #endregion
}
