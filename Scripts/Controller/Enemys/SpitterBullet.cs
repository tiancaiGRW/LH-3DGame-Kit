using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitterBullet : BulletBase
{
    protected override void Awake()
    {
        base.Awake();
      
    }
    public override void Shot(Vector3 target, Vector3 direction)
    {
        base.Shot(target, direction);
        target += Vector3.up * 0.5f;//增加子弹在目标上落点的y值0.5米,子弹不再从脚下经过
        float g = Mathf.Abs(Physics.gravity.y);

        float V0 = 8;  //竖直向上的初速度
        float t0 = V0 / g;
        float y0= 0.5f * g * t0 * t0;

        float t = 0;

        if(transform.position.y + y0 > target.y)//能够到达目标y值
        {

         float y = transform.position.y - target.y + y0;
         t = Mathf.Sqrt(y * 2 / g) + t0; 
        }
        else
        {
            t = t0;  //设置子弹总运动时间为上升时间t0,让子弹直接下落，无法到达目标位置；
           
        }

        Vector3  transPos= transform.position;
        transPos.y = 0;
        target.y = 0;
        float speed=Vector3.Distance(transPos, target) / t;
        Vector3 veclocity = direction.normalized * speed + Vector3.up * V0;

        mRigidbody.isKinematic = false;
        mRigidbody.velocity = veclocity;

        transform.GetComponent<WeaponAttackControler>().BeginAttack();
        Destroy(gameObject, 8);
    }

    public override void Attack()
    {
        base.Attack();
        Destroy(gameObject);
    }
}
