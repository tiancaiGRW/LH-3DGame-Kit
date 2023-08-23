using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    public static PlayerInput instance; 
    // C#���� ������������ͨ����������Accessor��������һ�����˽���ֶΣ�Private Field��ʹ�����ԣ����ǿ��Խ�һ���ֶε�ֵ��ȡ��get����������(set)�������ñ�¶�ֶα���
    public Vector2 Move
    {
        get
        {
            if (!_isCanControl)
            {
                return Vector2.zero;
            }

            return _move;

        }
    }

    public bool Jump
    {
        get
        {
            return _jump && _isCanControl;
        }
    }

    public bool Attack
    {
        get
        {
            return _Attack && _isCanControl;
        }
    }

    public bool Pause
    {
        get { return _pause && _isCanControl;}
    }

    private Vector2 _move;//ˮƽ������ֵ
    private bool _jump;
    private bool _Attack;
    public bool _isCanControl = true;

    private bool _pause;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        _move.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _jump = Input.GetButtonDown("Jump");
        _Attack = Input.GetButtonDown("Fire1");
        _pause = Input.GetButtonDown("Pause");
    }
    //��ÿ���
    public void GainControl()
    {
        _isCanControl = true;
    }
    //ʧȥ����
    public void LostConteol()
    {
        _isCanControl = false;
    }
    //�Ƿ�ӵ�п���Ȩ
    public bool IsHaveControl()
    {
        return _isCanControl;
    }
}
