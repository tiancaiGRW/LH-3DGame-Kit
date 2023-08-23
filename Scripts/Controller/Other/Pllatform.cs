using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pllatform : DoorMove
{
   public  CharacterController player;//玩家，多人则设为数组

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
            detalPosition =   Vector3.Lerp(StartPosition, EndPosition, percent) - transform.position;//返回值：StartPosition + ( EndPosition-StartPosition)*percent
                                                                                                     //percent是一个在update实时更新的值，所以位置也会实时更新，实现一个移动的效果
                                                                                                     //detalPosition是移动后位置与自身位置的差值
                break;
            case PositionType.Local:
                detalPosition = Vector3.Lerp(StartPosition, EndPosition, percent) - transform.localPosition;//处理父级下的子物体移动
                break;

        }

        base.MoveExcute();

        if (player != null)
        {
            player.Move(detalPosition);//人物跟随平台移动
        }
    }

}

 
