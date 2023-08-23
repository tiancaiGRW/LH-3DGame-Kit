using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum SWitchStatus
{ 
    Open,
    Close
}

public class Switch : MonoBehaviour
{
    public SWitchStatus switchStatus = SWitchStatus.Close;

    public UnityEvent onOpen;
    public UnityEvent onClose;

    public void Open()
    {
       
        if (switchStatus == SWitchStatus.Close)
        {
            switchStatus = SWitchStatus.Open;
            onOpen?.Invoke();
        }
    }

    public void Colse()
    {
        if (switchStatus == SWitchStatus.Open)
        {
            switchStatus = SWitchStatus.Close;
            onClose?.Invoke();
        }
    }

}
