using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    
    public DamageMessage damageMessage;

    public LayerMask layerMask;
    private void OnTriggerEnter(Collider other)
    {
        //�ж��ǲ��Ƕ�Ӧ�Ĳ㼶
        if((layerMask.value & (1 << other.gameObject.layer)) == 0)
        {
            return;
        }

        //�ж�ˮ�����Ϸ�����Ƿ���Թ���
        damagable damagable = other.gameObject.GetComponent<damagable>();
        if (damagable != null)
        {
            
            damagable.OnDamage(damageMessage);
        }
    }



}
