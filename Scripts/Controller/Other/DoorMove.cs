using UnityEngine;
using UnityEngine.Events;


//枚举，门开的方式有三种 1.开始到结束一次 2.开始到结束后再从头开始 3.开始到结束，从结束位置再到开始位置

public enum MoveType
{
    once,
    loop,
    pingpang
}

public enum PositionType //位置坐标类型
{
    world,
    Local
}



public class DoorMove : MonoBehaviour
{
    public Vector3 StartPosition;//开始的位置    
    public Vector3 EndPosition;//结束的位置

    public float time = 1;//移动的时间

    private float timer;//移动的计时
    protected float percent;//移动进度百分比
    private bool isMoving = false;//是否正在移动

    public MoveType moveType = MoveType.once;
    public PositionType positionType = PositionType.Local;

    public UnityEvent OnMoveEnd;//移动结束后触发的事件


    public bool moveOnAwake = false;//是否自动执行移动，外部bool开关控制开门

    public float delayTime = 0;//延迟时间
    private float delayTimer;//延迟计时

    private void Awake()
    {
        if (moveOnAwake)
        {
            StartMove();
        }
    }

    private void Update()
    {
        if (isMoving)
        {
            CaculateMove();
        }

    }


    public void StartMove()
    {
        isMoving = true;
        timer = 0;
    }
    public void CaculateMove()
    {
        if (delayTimer < delayTime)//延时进行门移动，delayTimer < delayTime, return(后方代码不执行)，-----用协程可能更好
        {
            delayTimer += Time.deltaTime;
            return;
        }

        timer += Time.deltaTime / time;
       //时间进度百分比=>移动进度百分比

        switch (moveType) //判断门开的类型
        {

            case MoveType.once:
                percent = Mathf.Clamp01(timer);//限制百分比范围在0到1之间，传出timer赋给precent
                break;
            case MoveType.loop:
                percent = Mathf.Repeat(timer, 1);//循环百分比的值，让它比0大比1小（当timer > 1时，从0开始，从而实现循环）
                break;
            case MoveType.pingpang:
                percent = Mathf.PingPong(timer, 1);//从0到1，再从1到0；往返
                break;

        }

        MoveExcute();

        if (timer >= 1 && moveType == MoveType.once)//第一种类型移动结束的判断
        {
            isMoving = false;
            timer = 0;
            //移动结束
            OnMoveEnd?.Invoke();
        }
    }

    protected virtual  void MoveExcute()//移动的具体方法
    {
        switch (positionType)
        {
            case PositionType.world:
                transform.position = Vector3.Lerp(StartPosition, EndPosition, percent);//返回值：StartPosition + ( EndPosition-StartPosition)*percent
                                                                                       //percent是一个在update实时更新的值，所以位置也会实时更新，实现一个移动的效果
                break;
            case PositionType.Local:
                transform.localPosition = Vector3.Lerp(StartPosition, EndPosition, percent);//处理父级下的子物体移动
                break;

        }

    }

}
