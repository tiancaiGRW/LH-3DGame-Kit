using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    #region �ֶ�
  protected Rigidbody mRigidbody;
    public float time;
    public GameObject ExplosionEffect;
    #endregion

    #region ����
    protected virtual void Awake()
    {
        mRigidbody = transform.GetComponent<Rigidbody>();
    }
    #endregion

    #region ����

    public virtual void Shot(Vector3 target, Vector3 direction)//�����λ��(target)[����Ŀ���λ��]������ķ���(direction)[һ���Ƿ����ӵ��������ǰ��] ,�ڵ����������ʱ�ᴫ����������þ����������Դ
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
