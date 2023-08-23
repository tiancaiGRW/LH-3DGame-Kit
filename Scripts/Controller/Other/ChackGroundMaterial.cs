using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChackGroundMaterial : MonoBehaviour
{
    RaycastHit hit;
    public Material currentMaterial;
    private void Update()
    {
        Ray ray = new Ray(transform.position + Vector3.up, Vector3.down);
      if(  Physics.Raycast(ray,out hit, 2, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
           Renderer renderer = hit.collider.GetComponentInChildren<Renderer>();
            currentMaterial = renderer? renderer.sharedMaterial : null;
        }
        else
        {
            currentMaterial = null;
        }
    }
}
