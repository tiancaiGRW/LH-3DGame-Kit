using UnityEngine;
using UnityEngine.Events;


//ö�٣��ſ��ķ�ʽ������ 1.��ʼ������һ�� 2.��ʼ���������ٴ�ͷ��ʼ 3.��ʼ���������ӽ���λ���ٵ���ʼλ��

public enum MoveType
{
    once,
    loop,
    pingpang
}

public enum PositionType //λ����������
{
    world,
    Local
}



public class DoorMove : MonoBehaviour
{
    public Vector3 StartPosition;//��ʼ��λ��    
    public Vector3 EndPosition;//������λ��

    public float time = 1;//�ƶ���ʱ��

    private float timer;//�ƶ��ļ�ʱ
    protected float percent;//�ƶ����Ȱٷֱ�
    private bool isMoving = false;//�Ƿ������ƶ�

    public MoveType moveType = MoveType.once;
    public PositionType positionType = PositionType.Local;

    public UnityEvent OnMoveEnd;//�ƶ������󴥷����¼�


    public bool moveOnAwake = false;//�Ƿ��Զ�ִ���ƶ����ⲿbool���ؿ��ƿ���

    public float delayTime = 0;//�ӳ�ʱ��
    private float delayTimer;//�ӳټ�ʱ

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
        if (delayTimer < delayTime)//��ʱ�������ƶ���delayTimer < delayTime, return(�󷽴��벻ִ��)��-----��Э�̿��ܸ���
        {
            delayTimer += Time.deltaTime;
            return;
        }

        timer += Time.deltaTime / time;
       //ʱ����Ȱٷֱ�=>�ƶ����Ȱٷֱ�

        switch (moveType) //�ж��ſ�������
        {

            case MoveType.once:
                percent = Mathf.Clamp01(timer);//���ưٷֱȷ�Χ��0��1֮�䣬����timer����precent
                break;
            case MoveType.loop:
                percent = Mathf.Repeat(timer, 1);//ѭ���ٷֱȵ�ֵ��������0���1С����timer > 1ʱ����0��ʼ���Ӷ�ʵ��ѭ����
                break;
            case MoveType.pingpang:
                percent = Mathf.PingPong(timer, 1);//��0��1���ٴ�1��0������
                break;

        }

        MoveExcute();

        if (timer >= 1 && moveType == MoveType.once)//��һ�������ƶ��������ж�
        {
            isMoving = false;
            timer = 0;
            //�ƶ�����
            OnMoveEnd?.Invoke();
        }
    }

    protected virtual  void MoveExcute()//�ƶ��ľ��巽��
    {
        switch (positionType)
        {
            case PositionType.world:
                transform.position = Vector3.Lerp(StartPosition, EndPosition, percent);//����ֵ��StartPosition + ( EndPosition-StartPosition)*percent
                                                                                       //percent��һ����updateʵʱ���µ�ֵ������λ��Ҳ��ʵʱ���£�ʵ��һ���ƶ���Ч��
                break;
            case PositionType.Local:
                transform.localPosition = Vector3.Lerp(StartPosition, EndPosition, percent);//�������µ��������ƶ�
                break;

        }

    }

}
