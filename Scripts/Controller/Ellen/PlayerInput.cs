using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    public static PlayerInput instance; 
    // C#属性 ：它允许我们通过访问器（Accessor）来访问一个类的私有字段（Private Field）使用属性，我们可以将一个字段的值读取（get）或者设置(set)，而不用暴露字段本身。
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

    private Vector2 _move;//水平方向数值
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
    //获得控制
    public void GainControl()
    {
        _isCanControl = true;
    }
    //失去控制
    public void LostConteol()
    {
        _isCanControl = false;
    }
    //是否拥有控制权
    public bool IsHaveControl()
    {
        return _isCanControl;
    }
}
