using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DamageMessage
{
    public int damage;//�˺�

    public Vector3 DamagePostion;//�˺���Դ��λ��

    public bool isResetPosition;//�Ƿ�����λ��
}
[Serializable]
public class DamageEvent : UnityEvent<damagable,DamageMessage> { }
public class damagable : MonoBehaviour
{
    public int MaxHp;//���Ѫ��
    public int hp;//��ǰ��Ѫ��
    public float invincibleTime = 0;//�޵�ʱ��
    private bool isInvincible = false; //�ǲ��Ǵ����޵�ʱ��
    private float invincibleTimer=0;

    public DamageEvent OnHurt;
    public DamageEvent OnDeath;
    public DamageEvent onReste;
    public DamageEvent onInvincibleTimeOut;

    public int CurrentHp//��ȡ��ǰѪ�������ظ�hp
    {
        get { return hp; }
    }
//�ж��Ƿ񻹻���
    public bool IsAlive//
    {
        get
        {
            return CurrentHp > 0;//��ȡ��ǰѪ������0��
        }
    }

    private void Start()
    {
        hp = MaxHp; 
    }


    private void Update()
    {
        if (isInvincible)
        {
            invincibleTimer += Time.deltaTime;
            if (invincibleTimer >= invincibleTime)
            {
                isInvincible = false;
                invincibleTimer = 0;
                onInvincibleTimeOut?.Invoke(this, null);
            }
        }
    }

    //��������
    public void ResetDamage()
    {
       
        hp = MaxHp;
        isInvincible = false;
        invincibleTimer = 0;
        HPView.instance.UpdateHPView();

    }
    public void OnDamage(DamageMessage data)
    {
        if (hp<=0)
        {  
            return;
        }
        if (isInvincible)//������޵�״̬����������
        {
            return;
        }

        hp -= data.damage;

        isInvincible = true;
        if(hp<= 0)
        {
            //����
            OnDeath?.Invoke(this,data);
        }
        else 
        {
            //����
            OnHurt?.Invoke(this,data);
        }
        
    }

}
