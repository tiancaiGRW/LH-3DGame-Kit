using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    
    public DamageMessage damageMessage;

    public LayerMask layerMask;
    private void OnTriggerEnter(Collider other)
    {
        //判断是不是对应的层级
        if((layerMask.value & (1 << other.gameObject.layer)) == 0)
        {
            return;
        }

        //判断水面的游戏物体是否可以攻击
        damagable damagable = other.gameObject.GetComponent<damagable>();
        if (damagable != null)
        {
            
            damagable.OnDamage(damageMessage);
        }
    }



}
