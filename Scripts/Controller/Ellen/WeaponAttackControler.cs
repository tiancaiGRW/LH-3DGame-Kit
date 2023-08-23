using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class ChackPoint
{
    public Transform point;
    public float radius;
}
   
public class WeaponAttackControler : MonoBehaviour
{
    public ChackPoint[] chackPoints;
    public Color color;
    private RaycastHit[] results = new RaycastHit[10];
    public LayerMask layerMask;

    public int damage;
    public GameObject myself;

    private List<GameObject> attackList =new List<GameObject>();//����һ����������������б���ֹ��һ��Ŀ����ж�ι���

    private bool Attack = false;

    public GameObject HitPrefab;

    public UnityEvent OnAttack;
 
    private void Update()
    {

        ChackGameObject();


    }


    public void BeginAttack()
    {
        Attack = true;
    }
    public void EndAttack()
    {
        Attack = false;
        attackList.Clear();//��������������б�
    }
    //������
    public void ChackGameObject()
    {
        if (!Attack) { return; }
        for (int i = 0; i < chackPoints.Length; i++)
        {
            int count = Physics.SphereCastNonAlloc(chackPoints[i].point.position, chackPoints[i].radius, Vector3.forward, results, 0, layerMask.value);
            for (int j = 0; j < count; j++)
            {
                //Debug.Log("��⵽���ˣ����й�����" + results[j].transform.name);
               if(ChackDamge(results[j].transform.gameObject))
                {
                    if(HitPrefab != null)
                    {
                        GameObject hit = GameObject.Instantiate(HitPrefab);  
                        hit.transform.position = chackPoints[i].point.position;
                        Destroy(hit,2);
                    }
                }
            }
        }
    }

   //�Ե�������˺�

    public bool ChackDamge(GameObject obj) 
    {
        //�ж���Ϸ�����Ƿ������˵Ĺ���
        damagable damagable = obj.GetComponent<damagable>();
        if (damagable==false)
        {
            return false;
        }

        //��⵽�Լ�
        if (obj==myself)
        {
            return false;
        }

        if (attackList.Contains(obj))
        {
            return false;
        }

        //���й���
        DamageMessage data = new DamageMessage();
        data.damage = damage;
        data.DamagePostion = myself.transform.position;//�˺���Դλ��=���й��������屾���λ��====>DamageMassage�ж��˺���Դ��λ�ý����жϣ��Ӷ������˺���Դ���������˶���
        damagable.OnDamage(data);

        OnAttack?.Invoke();
        attackList.Add(obj);//�����󣬽���������������뵽�б���

        return true;
    }

    private void OnDrawGizmosSelected()
    {
        if (chackPoints == null) { return; }
        for(int i = 0; i < chackPoints.Length; i++)
        {
            if(chackPoints[i] == null) { return; }
            Gizmos.color = color;
            Gizmos.DrawSphere(chackPoints[i].point.position, chackPoints[i].radius);
        }
    }



}
