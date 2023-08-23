using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DamageMessage
{
    public int damage;//伤害

    public Vector3 DamagePostion;//伤害来源的位置

    public bool isResetPosition;//是否重置位置
}
[Serializable]
public class DamageEvent : UnityEvent<damagable,DamageMessage> { }
public class damagable : MonoBehaviour
{
    public int MaxHp;//最大血量
    public int hp;//当前的血量
    public float invincibleTime = 0;//无敌时间
    private bool isInvincible = false; //是不是处于无敌时间
    private float invincibleTimer=0;

    public DamageEvent OnHurt;
    public DamageEvent OnDeath;
    public DamageEvent onReste;
    public DamageEvent onInvincibleTimeOut;

    public int CurrentHp//获取当前血量，返回给hp
    {
        get { return hp; }
    }
//判断是否还活着
    public bool IsAlive//
    {
        get
        {
            return CurrentHp > 0;//读取当前血量大于0；
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

    //重置数据
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
        if (isInvincible)//如果是无敌状态，不能受伤
        {
            return;
        }

        hp -= data.damage;

        isInvincible = true;
        if(hp<= 0)
        {
            //死亡
            OnDeath?.Invoke(this,data);
        }
        else 
        {
            //受伤
            OnHurt?.Invoke(this,data);
        }
        
    }

}
