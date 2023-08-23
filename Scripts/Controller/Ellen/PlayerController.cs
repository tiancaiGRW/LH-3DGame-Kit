using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{

    #region �ֶ�
   
    public float MaxMoveSpeed = 5;

    public  float MoveSpeed = 0;

    public float JumpSpeed = 10;
    public float Gravity = 20f;
    public bool isGrounded = true;//Ĭ���ڵ�����
    private float verticalSpeed = 0;

    private CharacterController characterController;

    private PlayerInput playerInput;

    private Vector3 move;

    public Transform randerCamera;

    //public float angleSpeed = 400;//��ת�Ľ��ٶ�

    public float MaxAngleSpeed = 1200;//��ת�������ٶ�
    public float MinAngleSpeed = 400;//��ת����С���ٶ�

    public float accelerateSpeed = 5;//����ļ��ٶ�

    private Animator animator;

    private AnimatorStateInfo currentStateInfo;//��ǰ����
    private AnimatorStateInfo nextStateInfo;//��һ������

    public bool isCanAttack = false;

    public GameObject Weapon;

    public Vector3 respawnPosition;//����λ��

    public RandomAudioPlayer jumpPlayer;//��Ծ��Ч
    public RandomAudioPlayer attackPLayer;//������Ч

    #endregion
    #region ����
    private int QuickTurnRightHash = Animator.StringToHash("EllenQuickTurnRight");
    private int QuickTurnLeftHash = Animator.StringToHash("EllenQuickTurnLeft");
    private int DeathHash = Animator.StringToHash("EllenDeath");
    #endregion
    #region Unity�ص�

    private void Awake()
    {
        characterController = transform.GetComponent<CharacterController>();
        playerInput=transform.GetComponent<PlayerInput>();
        animator = transform.GetComponent<Animator>();

    }

    private void Update()
    {
        currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        nextStateInfo = animator.GetNextAnimatorStateInfo(0);

        // CaculateMove();
        CaculateVerticalSpeed();
        CaculateForwardSpeed();
        CaculateRotaion();
       animator.SetFloat("normalizedTime",Mathf.Repeat(currentStateInfo.normalizedTime,1));

        animator.ResetTrigger("Attack");//����AttackTrigger;

        if (playerInput.Attack  && isCanAttack)
        {
            animator.SetTrigger("Attack");
        }
    }
    #endregion
    #region ��������

    private void OnAnimatorMove()
    {
        CaculateMove();
    }


    public void CaculateMove()
    {
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");

        if (isGrounded)
        {
        move = animator.deltaPosition;//ͨ�����������ƶ�//���˶�
          

        }
        else
        {
            move = MoveSpeed * transform.forward * Time.deltaTime;
        }


       // move.Set(playerInput.Move.x, 0, playerInput.Move.y);
        //move *= Time.deltaTime * MoveSpeed;

       // move = randerCamera.TransformDirection(move);//��������ƶ�����Ҫ��Y�᷽����������ֱ�ƶ�����ǰ

        

        move += Vector3.up * verticalSpeed * Time.deltaTime;//��ֱ�����ƶ�  

        transform.rotation *= animator.deltaRotation;//���˶���ת


        characterController.Move(move);
        isGrounded = characterController.isGrounded;

        animator.SetBool("isGrounded", isGrounded);//����״̬������

    }

    private void CaculateVerticalSpeed()
    {
        if (isGrounded)
        {
            verticalSpeed = -Gravity * 0.3f;
            if (playerInput.Jump && jumpPlayer != null)
            {
                verticalSpeed = JumpSpeed;
                isGrounded = false;

                jumpPlayer.PlayerRandomAudio();
            }
           
        }
        else
        {

            if ( !Input.GetKey(KeyCode.Space) && verticalSpeed > 0 )
            {
                verticalSpeed -= Gravity * Time.deltaTime;
            }

            verticalSpeed -= Gravity * Time.deltaTime;
        }

        if ( !isGrounded )
        {
        animator.SetFloat("verticalSpeed", verticalSpeed);//����״̬������

        }

    }

    private void CaculateForwardSpeed()//�����ٶȽ��䣬��ǰ���ٵ����ͣ�¼��ٵ�0
    {
        MoveSpeed = Mathf.MoveTowards(MoveSpeed, MaxMoveSpeed * playerInput.Move.normalized.magnitude, accelerateSpeed * Time.deltaTime);
        animator.SetFloat("ForwardSpeed", MoveSpeed);//����״̬������
    }

    private void CaculateRotaion()
    {
        if (playerInput.Move.x != 0 || playerInput.Move.y != 0)
        {
            Vector3 targetDirection = randerCamera.TransformDirection(new Vector3(playerInput.Move.x, 0, playerInput.Move.y));

            targetDirection.y = 0;

            float turnSpeed =Mathf.Lerp(MaxAngleSpeed,MinAngleSpeed,MoveSpeed/MaxAngleSpeed ) * Time.deltaTime;

            //transform.rotation = Quaternion.LookRotation(move);//ת����棬����Y��

            float turnAngle = Vector3.SignedAngle(transform.forward, targetDirection,Vector3.up);//����ǰ����Ŀ�곯����ֵ�ᣩ��Angle����ֵֻ�Ǿ���ֵ�޷��ж����ҷ���signedAngle�ķ���ֵ������

            if (IsUpdateDirection())
            {

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDirection), turnSpeed);//ת����ɸ���

            }
            animator.SetFloat("TurnAngleRad", turnAngle * Mathf.Deg2Rad);

        }
    }

    public bool IsUpdateDirection()
    {
        bool isUpdate = currentStateInfo.shortNameHash != QuickTurnLeftHash && currentStateInfo.shortNameHash != QuickTurnRightHash;
        isUpdate = nextStateInfo.shortNameHash != QuickTurnLeftHash && nextStateInfo.shortNameHash != QuickTurnRightHash;                                                                           


        return isUpdate;
    }


    public void SetCanAttack(bool isAttack)
    {
        isCanAttack = isAttack;
    }


    public void ShowWeapon()
    {
        CancelInvoke("HideWeaponExcute");
        Weapon.SetActive(true);
    }

    public void HideWeapon()
    {
        Invoke("HideWeaponExcute", 1);
    }

    private void HideWeaponExcute()
    {
        Weapon.SetActive(false);
    }

    public void OnHurt(damagable damagable,DamageMessage data)
    {
        Vector3 direction = data.DamagePostion - transform.position;//������˵ķ���  = ����λ�� - ����λ��
        direction.y = 0;

        Vector3 localDircetion = transform.InverseTransformDirection(direction);//�ѵõ�������ת��Ϊ��������ķ���(����λ����Ϣת��Ϊ�Լ���λ����Ϣ)

       
        //�ж��Ƿ�Ҫ����λ��
        if(data.isResetPosition) 
        {
            //��ʧ����
            playerInput.LostConteol();
            //������������
            animator.SetTrigger("death");
            //����λ��
            StartCoroutine(ResetPoistion());
        }
        else
        {
            //�������˶���

            animator.SetFloat("HurtX", localDircetion.x);//���Լ���x����Ϣ���ݸ�����״̬����HurtX;
            animator.SetFloat("HurtY", localDircetion.z);//���Լ���Y����Ϣ���ݸ�����״̬����HurtY;

            animator.SetTrigger("hurt");
        }
    }

    public void OnDeath(damagable damagable, DamageMessage data)
    {
        animator.SetTrigger("death");

        StartCoroutine(Respawn());//��������Э�̷�����Э�̵ĵ�����䣺 StartCoroutine������������������
    }
    //����λ�� �����漰�ֲ�ִ�У�ʹ��Э�̺���
    public IEnumerator ResetPoistion() 
    {
        //�ж��Ƿ񲥷���������
        while (currentStateInfo.shortNameHash == DeathHash)
        {
            yield return null;//��һִ֡�к�������
        }
        yield return null;

        //��Ļ���
        yield return StartCoroutine(BlackMaskView.Instance.FadeOut());//����͸����Ϊ1��
        //�������λ��
        transform.position = respawnPosition;
        //������������
        animator.SetTrigger("respawn");

        yield return new WaitForSeconds(1f);


        //��Ļ����
        yield return StartCoroutine(BlackMaskView.Instance.FadeIn());//����͸���ȱ�Ϊ0��
      
        //���»�ÿ���Ȩ
        playerInput.GainControl();
    }


    //������
    public IEnumerator Respawn()
    {
       yield return StartCoroutine(ResetPoistion());
        //����Ѫ��
        transform.GetComponent<damagable>().ResetDamage();
      
    }

    public void SetRespawnPoint( Vector3 position)
    {
        respawnPosition = position;
    }

    #endregion
    #region Animation Events    
    public void OnIdleStart()
    {
        animator.SetInteger("RanDomIdle", -1);
    }

    public void OnIdleEnd()
    {
        animator.SetInteger("RanDomIdle", Random.Range(0,3));
    }

    public void MeleeAttackStart()
    {
        Weapon.GetComponent<WeaponAttackControler>().BeginAttack();

        attackPLayer.PlayerRandomAudio();
    }

  public void MeleeAttackEnd() 
   {

        Weapon.GetComponent<WeaponAttackControler>().EndAttack();
    }

    #endregion

  

}
