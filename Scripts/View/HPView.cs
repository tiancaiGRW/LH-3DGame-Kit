using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPView : ViewBase
{
    public static HPView instance;

    public GameObject HpItemParfab;
    public damagable damagable;

    private Toggle[] hps;
    private void Awake()
    {
        instance = this;
    }

    private IEnumerator Start()
    {
        hps = new Toggle[damagable.MaxHp];
        yield return null;
        for(int i =0; i<damagable.MaxHp; i++)
        {
            yield return new WaitForSeconds(0.1f);
            GameObject HpItem =  GameObject.Instantiate(HpItemParfab, transform.Find("Hps"));
            hps[i] = HpItem.GetComponent<Toggle>();
        }
    }

    public void UpdateHPView()
    { 
       
        for (int i=0; i<hps.Length; i++)
        {
            if(hps[i].isOn && i >= damagable.CurrentHp)
            {
            hps[i].transform.Find("Background/Dissolve").gameObject.SetActive(false);
            hps[i].transform.Find("Background/Dissolve").gameObject.SetActive(true);
            }

            hps[i].isOn = i < damagable.CurrentHp;

        }

    }



}
