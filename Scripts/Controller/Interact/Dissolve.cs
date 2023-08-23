using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    Renderer[] renderers;

    public float DissolveTime = 3f;
    public float dissolveTimer = 0;
    MaterialPropertyBlock propertyBlock;

    private void Start()
    {
        renderers = transform.GetComponentsInChildren<Renderer>();
        propertyBlock = new MaterialPropertyBlock();
    }

    private void Update()
    {
        dissolveTimer += Time.deltaTime;

        if (dissolveTimer >= DissolveTime)
        {
            return;
        }

        for (int i=0; i<renderers.Length; i++)
        {
            //renderers[i].material.SetFloat("_Cutoff", dissolveTimer / DissolveTime);
            renderers[i].GetPropertyBlock(propertyBlock);
            propertyBlock.SetFloat("_Cutoff", dissolveTimer / DissolveTime);
            renderers[i].SetPropertyBlock(propertyBlock);
        }
    }
    private void OnEnable()
    {
        dissolveTimer = 0;
    }
}
