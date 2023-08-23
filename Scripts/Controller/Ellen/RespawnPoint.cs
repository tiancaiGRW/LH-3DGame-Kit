using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)//other�Ǵ��� Trigger������������
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.SetRespawnPoint(transform.position);//�ѵ�ǰ�����λ�ø�ֵ��SetRespawnPoint��������ȥ��λ�ò���Vector3 position;
           
        }
    }
}
