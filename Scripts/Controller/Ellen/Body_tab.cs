using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body_tab : MonoBehaviour
{
    public static Body_tab instance;

    public GameObject[] obj_A;
    public GameObject[] obj_B;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        GameObject obj_Pirate_Body = transform.GetComponentInChildren<Dissolve>().gameObject;
        obj_Pirate_Body.GetComponent<Dissolve>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
