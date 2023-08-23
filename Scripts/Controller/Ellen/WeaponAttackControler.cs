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

    private List<GameObject> attackList =new List<GameObject>();//创建一个被攻击的物体的列表，防止对一个目标进行多次攻击

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
        attackList.Clear();//结束攻击，清除列表
    }
    //检测敌人
    public void ChackGameObject()
    {
        if (!Attack) { return; }
        for (int i = 0; i < chackPoints.Length; i++)
        {
            int count = Physics.SphereCastNonAlloc(chackPoints[i].point.position, chackPoints[i].radius, Vector3.forward, results, 0, layerMask.value);
            for (int j = 0; j < count; j++)
            {
                //Debug.Log("检测到敌人，进行攻击：" + results[j].transform.name);
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

   //对敌人造成伤害

    public bool ChackDamge(GameObject obj) 
    {
        //判断游戏物体是否有受伤的功能
        damagable damagable = obj.GetComponent<damagable>();
        if (damagable==false)
        {
            return false;
        }

        //检测到自己
        if (obj==myself)
        {
            return false;
        }

        if (attackList.Contains(obj))
        {
            return false;
        }

        //进行攻击
        DamageMessage data = new DamageMessage();
        data.damage = damage;
        data.DamagePostion = myself.transform.position;//伤害来源位置=进行攻击的物体本身的位置====>DamageMassage中对伤害来源的位置进行判断，从而朝向伤害来源处播放受伤动画
        damagable.OnDamage(data);

        OnAttack?.Invoke();
        attackList.Add(obj);//攻击后，将被攻击的物体加入到列表中

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
