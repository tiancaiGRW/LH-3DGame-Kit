using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pllatform : DoorMove
{
   public  CharacterController player;//��ң���������Ϊ����

    Vector3 detalPosition;
    private void OnTriggerEnter(Collider other)
    {
        player = other.transform.GetComponent<CharacterController>();

    }

    private void OnTriggerExit(Collider other)
    {
        player = null;
    }
  

    protected override void MoveExcute()
    {
        switch (positionType)
        {
            case PositionType.world:
            detalPosition =   Vector3.Lerp(StartPosition, EndPosition, percent) - transform.position;//����ֵ��StartPosition + ( EndPosition-StartPosition)*percent
                                                                                                     //percent��һ����updateʵʱ���µ�ֵ������λ��Ҳ��ʵʱ���£�ʵ��һ���ƶ���Ч��
                                                                                                     //detalPosition���ƶ���λ��������λ�õĲ�ֵ
                break;
            case PositionType.Local:
                detalPosition = Vector3.Lerp(StartPosition, EndPosition, percent) - transform.localPosition;//�������µ��������ƶ�
                break;

        }

        base.MoveExcute();

        if (player != null)
        {
            player.Move(detalPosition);//�������ƽ̨�ƶ�
        }
    }

}

 
