using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : BulletBase
{
    public override void Shot(Vector3 target , Vector3 direction)//射击的位置(target)和射击的方向(direction)
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
        //爆炸
        Explosion();
        //对人物进行攻击
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
