using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)//other是触发 Trigger触发器的物体
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.SetRespawnPoint(transform.position);//把当前复活点位置赋值给SetRespawnPoint方法传进去的位置参数Vector3 position;
           
        }
    }
}
