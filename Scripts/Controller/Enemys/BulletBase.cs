using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    #region 字段
  protected Rigidbody mRigidbody;
    public float time;
    public GameObject ExplosionEffect;
    #endregion

    #region 周期
    protected virtual void Awake()
    {
        mRigidbody = transform.GetComponent<Rigidbody>();
    }
    #endregion

    #region 方法

    public virtual void Shot(Vector3 target, Vector3 direction)//射击的位置(target)[攻击目标的位置]和射击的方向(direction)[一般是发射子弹物体的正前方] ,在调用射击方法时会传入参数，不用纠结参数的来源
    {
        
    }
    public virtual void Attack()
    {
       
    }

    public virtual void Explosion()
    {
        if (ExplosionEffect != null)
        {
            GameObject explosion = GameObject.Instantiate(ExplosionEffect);
            explosion.transform.position = transform.position;
            Destroy(explosion, 2);
        }
        Destroy(gameObject, 0.2f);
    }
    #endregion

}
