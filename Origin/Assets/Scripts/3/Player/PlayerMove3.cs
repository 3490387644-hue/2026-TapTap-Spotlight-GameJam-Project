using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType
{
    IDLE,
    FLY,
}

public class PlayerMove3 : MonoBehaviour
{
    private static PlayerMove3 instance;
    public static PlayerMove3 Instance=>instance;

    public float hSpeed; //玩家对象水平方向的速度
    public float vSpeed; //玩家竖直方向的速度

    public bool isFacingRight=true; //玩家对象是否面向右边
    public float enterFlyTime; //进入飞行状态所需时间
    public bool isFly; //是否进入飞行状态
    public float reduceNum; //飞行时每帧消耗的体力
    public float addNum; //落地时每帧恢复的体力

    public StaminaChange stamina; //耐力条对象
    public bool isAdd; //是否进入耐力恢复状态
    public bool isStop=true; //是否处于停止状态

    private Rigidbody2D rb; //玩家对象上的刚体
    private float h; //记录水平方向的输入量
    private float v; //记录竖直方向的输入量
    private float lastTime; //按下W键时记录当前游戏时间
    private bool isEnterFly; //是否进入起飞状态

    private Animator animator; //玩家动画状态机组件
    private PlayerType playerType; //玩家动画状态枚举
    private bool isFirst=true; //游戏加载时第一次落地不算

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        UpdateAnimate(PlayerType.IDLE); //初始默认为待机状态
        
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        FlyCheck();
        if (isFly)
        {
            rb.gravityScale = 0; //进入飞行状态时，重力参数置为零
            UpdateAnimate(PlayerType.FLY);
        }
    }

    private void FixedUpdate()
    {
        Move(h * hSpeed,v * vSpeed);
        if (!isFly && isAdd)
        {
            stamina.UpdateStaminaChange(addNum);
        }
    }

    //玩家移动函数
    private void Move(float h,float v)
    {
        if(isFly)
        {
            rb.velocity = new Vector2(h,v);
            stamina.gameObject.SetActive(true);
            if (h!=0||v!=0)
            {
                stamina.UpdateStaminaChange(reduceNum);
                isStop=false;
            }
            if(h==0&&v==0)
            {
                isStop=true;
            }
        }
        else
        {
            rb.velocity = new Vector2(h,rb.velocity.y);
        }
        if ((h < 0 && isFacingRight) || (h > 0 && !isFacingRight))
        {
            Flip();
        }
    }

    //玩家对象左右翻转函数
    private void Flip()
    {
        isFacingRight=!isFacingRight;
        this.transform.localScale = new Vector3(-this.transform.localScale.x,this.transform.localScale.y,this.transform.localScale.z);
    }

    //飞行函数
    private void FlyCheck()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            lastTime = Time.time;
        }
        if(Input.GetKey(KeyCode.W))
        {
            if(Time.time-lastTime>=enterFlyTime&&!isEnterFly)
            {
                isFly=true;
                isEnterFly=true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //触地时恢复正常行走状态并进入耐力值恢复状态
        if(collision.gameObject.tag=="Ground")
        {
            rb.gravityScale = 1;
            isFly = false;
            isAdd = true;
            isEnterFly = false;
            UpdateAnimate(PlayerType.IDLE);
        }
    }

    //玩家动画状态更换函数
    private void UpdateAnimate(PlayerType type)
    {
        if(playerType==PlayerType.IDLE)
        {
            animator.SetBool("isIdle",false);
        }
        if (playerType == PlayerType.FLY)
        {
            animator.SetBool("isFly", false);
        }

        playerType = type;

        if (playerType == PlayerType.IDLE)
        {
            animator.SetBool("isIdle", true);
        }
        if (playerType == PlayerType.FLY)
        {
            animator.SetBool("isFly", true);
        }
    }
}
