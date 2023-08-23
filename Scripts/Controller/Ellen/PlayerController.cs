using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{

    #region 字段
   
    public float MaxMoveSpeed = 5;

    public  float MoveSpeed = 0;

    public float JumpSpeed = 10;
    public float Gravity = 20f;
    public bool isGrounded = true;//默认在地面上
    private float verticalSpeed = 0;

    private CharacterController characterController;

    private PlayerInput playerInput;

    private Vector3 move;

    public Transform randerCamera;

    //public float angleSpeed = 400;//旋转的角速度

    public float MaxAngleSpeed = 1200;//旋转的最大角速度
    public float MinAngleSpeed = 400;//旋转的最小角速度

    public float accelerateSpeed = 5;//人物的加速度

    private Animator animator;

    private AnimatorStateInfo currentStateInfo;//当前动画
    private AnimatorStateInfo nextStateInfo;//下一个动画

    public bool isCanAttack = false;

    public GameObject Weapon;

    public Vector3 respawnPosition;//重生位置

    public RandomAudioPlayer jumpPlayer;//跳跃声效
    public RandomAudioPlayer attackPLayer;//攻击声效

    #endregion
    #region 常量
    private int QuickTurnRightHash = Animator.StringToHash("EllenQuickTurnRight");
    private int QuickTurnLeftHash = Animator.StringToHash("EllenQuickTurnLeft");
    private int DeathHash = Animator.StringToHash("EllenDeath");
    #endregion
    #region Unity回调

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

        animator.ResetTrigger("Attack");//重置AttackTrigger;

        if (playerInput.Attack  && isCanAttack)
        {
            animator.SetTrigger("Attack");
        }
    }
    #endregion
    #region 函数方法

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
        move = animator.deltaPosition;//通过动画进行移动//根运动
          

        }
        else
        {
            move = MoveSpeed * transform.forward * Time.deltaTime;
        }


       // move.Set(playerInput.Move.x, 0, playerInput.Move.y);
        //move *= Time.deltaTime * MoveSpeed;

       // move = randerCamera.TransformDirection(move);//根据相机移动不需要加Y轴方向，所以在竖直移动方法前

        

        move += Vector3.up * verticalSpeed * Time.deltaTime;//竖直方向移动  

        transform.rotation *= animator.deltaRotation;//根运动旋转


        characterController.Move(move);
        isGrounded = characterController.isGrounded;

        animator.SetBool("isGrounded", isGrounded);//动画状态机设置

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
        animator.SetFloat("verticalSpeed", verticalSpeed);//动画状态机设置

        }

    }

    private void CaculateForwardSpeed()//人物速度渐变，向前加速到最大，停下减速到0
    {
        MoveSpeed = Mathf.MoveTowards(MoveSpeed, MaxMoveSpeed * playerInput.Move.normalized.magnitude, accelerateSpeed * Time.deltaTime);
        animator.SetFloat("ForwardSpeed", MoveSpeed);//动画状态机设置
    }

    private void CaculateRotaion()
    {
        if (playerInput.Move.x != 0 || playerInput.Move.y != 0)
        {
            Vector3 targetDirection = randerCamera.TransformDirection(new Vector3(playerInput.Move.x, 0, playerInput.Move.y));

            targetDirection.y = 0;

            float turnSpeed =Mathf.Lerp(MaxAngleSpeed,MinAngleSpeed,MoveSpeed/MaxAngleSpeed ) * Time.deltaTime;

            //transform.rotation = Quaternion.LookRotation(move);//转向跟随，忽略Y轴

            float turnAngle = Vector3.SignedAngle(transform.forward, targetDirection,Vector3.up);//（当前朝向，目标朝向，数值轴）、Angle返回值只是绝对值无法判断左右方向，signedAngle的返回值有正负

            if (IsUpdateDirection())
            {

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDirection), turnSpeed);//转向过渡跟随

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
        Vector3 direction = data.DamagePostion - transform.position;//朝向敌人的方向  = 敌人位置 - 自身位置
        direction.y = 0;

        Vector3 localDircetion = transform.InverseTransformDirection(direction);//把得到的向量转换为本体自身的方向(世界位置信息转换为自己的位置信息)

       
        //判断是否要重制位置
        if(data.isResetPosition) 
        {
            //丢失控制
            playerInput.LostConteol();
            //播放死亡动画
            animator.SetTrigger("death");
            //重置位置
            StartCoroutine(ResetPoistion());
        }
        else
        {
            //播放受伤动画

            animator.SetFloat("HurtX", localDircetion.x);//把自己的x轴信息传递给动画状态机的HurtX;
            animator.SetFloat("HurtY", localDircetion.z);//把自己的Y轴信息传递给动画状态机的HurtY;

            animator.SetTrigger("hurt");
        }
    }

    public void OnDeath(damagable damagable, DamageMessage data)
    {
        animator.SetTrigger("death");

        StartCoroutine(Respawn());//调用重生协程方法，协程的调用语句： StartCoroutine（函数方法（））；
    }
    //重置位置 其中涉及分步执行，使用协程函数
    public IEnumerator ResetPoistion() 
    {
        //判断是否播放死亡动画
        while (currentStateInfo.shortNameHash == DeathHash)
        {
            yield return null;//等一帧执行后续代码
        }
        yield return null;

        //屏幕变黑
        yield return StartCoroutine(BlackMaskView.Instance.FadeOut());//遮罩透明度为1；
        //重置玩家位置
        transform.position = respawnPosition;
        //播放重生动画
        animator.SetTrigger("respawn");

        yield return new WaitForSeconds(1f);


        //屏幕变亮
        yield return StartCoroutine(BlackMaskView.Instance.FadeIn());//遮罩透明度变为0；
      
        //重新获得控制权
        playerInput.GainControl();
    }


    //重生，
    public IEnumerator Respawn()
    {
       yield return StartCoroutine(ResetPoistion());
        //重置血量
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
