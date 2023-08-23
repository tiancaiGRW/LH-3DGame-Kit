using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EllenRespawnSMB : StateMachineBehaviour
{
    
   
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject obj_Pirate_Body  = animator.transform.GetComponentInChildren<Dissolve>().gameObject;
        obj_Pirate_Body.GetComponent<Dissolve>().enabled = true;
        for (int i = 0; i < Body_tab.instance.obj_A.Length; i++) {

            GameObject obj = Body_tab.instance.obj_A[i];
            obj.SetActive(false);

        }

        for (int i = 0; i < Body_tab.instance.obj_B.Length; i++)
        {

            GameObject Obj = Body_tab.instance.obj_B[i];
            Obj.SetActive(true);
           

        }

       
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject obj_Pirate_Body = animator.transform.GetComponentInChildren<Dissolve>().gameObject;
        obj_Pirate_Body.GetComponent<Dissolve>().enabled = false;
        for (int i = 0; i < Body_tab.instance.obj_A.Length; i++)
        {

            GameObject obj = Body_tab.instance.obj_A[i];
            obj.SetActive(true);

        }
        

        for (int i = 0; i < Body_tab.instance.obj_B.Length; i++)
        {

            GameObject Obj = Body_tab.instance.obj_B[i];
            Obj.SetActive(false);

        }


       
    }
}
