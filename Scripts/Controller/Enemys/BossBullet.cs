using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : BulletBase
{
    public override void Shot(Vector3 target , Vector3 direction)//�����λ��(target)������ķ���(direction)
    {
        base.Shot(target , direction);
        transform.SetParent(null);
        mRigidbody.isKinematic = false;
        Vector3 ToTarget = target - transform.position;
        ToTarget.y = 0;
        float speed = ToTarget.magnitude / time * 1.4f;
        mRigidbody.velocity = direction.normalized * speed + Vector3.up * 3;
        Invoke("Attack", time);
    }
    public override void Attack()
    {
        base.Attack();
        //��ը
        Explosion();
        //��������й���
        transform.GetComponent<WeaponAttackControler>().BeginAttack();
    }

    public void Explosion()
    {
        if (ExplosionEffect != null)
        {
           GameObject explosion = GameObject.Instantiate(ExplosionEffect);
           explosion.transform.position = transform.position;  
            Destroy(explosion,2);
        }
        Destroy(gameObject,0.2f);
    }
}
