using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//boss������ʽΪ���� Զ�� �����뷶Χ�� ��������ͨ��
public class Grenadier : EnemyBase
{
    #region �ֶ�
    public float shortAttackDistance;//�̾��빥��
    //����ת�򶯻���name
    private int rangeAttackHash = Animator.StringToHash("GrenadierRangeAttack");
    private int meleeAttackHash = Animator.StringToHash("GrenadierMeleeAttack");
    private int rangeAttack2Hash = Animator.StringToHash("GrenadierRangeAttack2");

    private AnimatorStateInfo currentAnimatorInfo;//��ǰ��������Ϣ

    public GameObject bossBulletPrefab;         //boss���ӵ�Ԥ����

    public Transform ShootPosition;

    private BossBullet bossbullet;      //boss��ǰ���ӵ�
    public WeaponAttackControler meleeAttackController;//��ͨ����
    public WeaponAttackControler ranageAttackController;//��Χ����

    #endregion

    #region ����
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

    #region ����
    public override void Attack()
    {
        base.Attack();

        if (currentAnimatorInfo.shortNameHash == rangeAttack2Hash || currentAnimatorInfo.shortNameHash == rangeAttackHash || currentAnimatorInfo.shortNameHash == meleeAttackHash) 
        {
            //ˢ�¹���trigger
            animator.ResetTrigger("attack");
            animator.ResetTrigger("attack_range");
            animator.ResetTrigger("attack_shot");
            return; 
        }//�ڲ���ת���ʱ�򲻽��й�������(���ö�������ȡ������Ϣ ��⵱ǰ����״̬ʵ�ֶԶ����Ŀ���)

        

        if (Vector3.Distance(transform.position, Target.transform.position) > shortAttackDistance) //boss����Ҽ������빥���̾���
        {
            Turn();
            //Զ���빥��
            // Shot();
            animator.ResetTrigger("attack");
            animator.SetTrigger("attack");
        }
        else
        {
            //�����빥��

            if (Vector3.Angle(transform.forward, Target.transform.position - transform.position) > 20)//��������ͨ������ҰΪboss��ǰ��20�����(boss����ҵķ���������boss��ǰ���н�)
            {
                //��Χ����
                animator.ResetTrigger("attack_range");
                animator.SetTrigger("attack_range");
            }
            else
            {
                //��ͨ����
                animator.ResetTrigger("attack_shot");
                animator.SetTrigger("attack_shot");
            }
        }
    }


    //ת����׼
    public void Turn()
    {
        //����ת�Ƕ���
        //Vector3.Angle(transform.forward,Target.transform.position - transform.position);//������Ǿ���ֵ����������
        float angle = Vector3.SignedAngle(transform.forward, Target.transform.position - transform.position, Vector3.up);

        if (Mathf.Abs(angle) > 10)//�Ƕȵľ���ֵ����ʮ�Ƚ���ת�䡣�Ƕȹ�Сû��Ҫת��
        {
            animator.SetFloat("TurnAngle", angle);
            animator.SetTrigger("turn");
        }

    }
    //�����ӵ�
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
           
            bossbullet.Shot(Target.transform.position, transform.forward);//���빥��Ŀ���λ�� �� boss�Լ���ǰ������
          
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
        //������������
        animator.SetTrigger("death");
        //�����Լ�
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
