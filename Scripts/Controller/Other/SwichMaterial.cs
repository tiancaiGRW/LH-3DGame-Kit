using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwichMaterial : MonoBehaviour
{
    public Material[] target;

    private Renderer renderer;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();

        if (renderer == null)
        {
            throw new System.Exception("δ��⵽render���");
        }
    }

    public void Swichmaterial()
    {
        renderer.materials = target;
    }

}
