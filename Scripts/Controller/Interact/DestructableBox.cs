using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableBox : MonoBehaviour
{
  
    public void OnDeath()
    {
        Destroy(transform.GetComponent<MeshRenderer>());
        Destroy(transform.GetComponent<BoxCollider>());
        Destroy(gameObject,5);


        transform.Find("DestructableBoxBreak").gameObject.SetActive(true);
    }


}
