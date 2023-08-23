using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum DoorState//枚举类，定义状态，种类等
{
    OPen, 
    Close
}

public class Door : MonoBehaviour
{
    public DoorState doorState = DoorState.Close;

    public int KeyNum = 1;//打开门需要的钥匙数量

    private int currentKeyNum = 0;//当前钥匙数

    public UnityEvent onOpen;
    public UnityEvent onClose;
    public void Open()
    {
        currentKeyNum++;//每打开一次，当前数量+1


        if (doorState == DoorState.Close && currentKeyNum == KeyNum)//currentKeyNum == KeyNum，当前钥匙数等于开门需要数量
        {
            doorState = DoorState.OPen;
            onOpen?.Invoke();

        }
    }

    public void Close()
    {
        if (doorState == DoorState.OPen)
        {
            doorState = DoorState.Close;
            onClose?.Invoke();

        }


    }
}
