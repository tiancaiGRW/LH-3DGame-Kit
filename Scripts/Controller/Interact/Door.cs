using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum DoorState//ö���࣬����״̬�������
{
    OPen, 
    Close
}

public class Door : MonoBehaviour
{
    public DoorState doorState = DoorState.Close;

    public int KeyNum = 1;//������Ҫ��Կ������

    private int currentKeyNum = 0;//��ǰԿ����

    public UnityEvent onOpen;
    public UnityEvent onClose;
    public void Open()
    {
        currentKeyNum++;//ÿ��һ�Σ���ǰ����+1


        if (doorState == DoorState.Close && currentKeyNum == KeyNum)//currentKeyNum == KeyNum����ǰԿ�������ڿ�����Ҫ����
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
