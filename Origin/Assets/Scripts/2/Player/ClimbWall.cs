using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WallType
{
    WallUp,
    WallDown,
    WallWait,
    None
}

public class ClimbWall : MonoBehaviour
{
    public Vector3 wallOffset; //以对象位置为原点偏移
    public LayerMask mask; //范围检测层
    public float climbSpeed=3; //攀爬速度
    public float radius = 0.1f; //检测范围半径
    private bool isLeftWall; //是否检测到左墙体
    private bool isRightWall; //是否检测到右墙体
    private bool onWall; //是否在墙体上
    private bool beforeClimb=false; //进入攀爬状态动画是否播放
    private Rigidbody2D rb;
    private Animator animator;
    private WallType type;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        type=WallType.None; //初始默认为None状态
    }

    // Update is called once per frame
    void Update()
    {
        if (GameSceneManager.Instance.IsInputLocked)
            return;
        float v = Input.GetAxis("Vertical");
        CheckWall();
        if(Input.GetKey(KeyCode.LeftShift)&&(isRightWall||isLeftWall))
        {
            onWall = true;
            WallEnter();
        }
        if(!isRightWall&&!isLeftWall)
        {
            onWall = false;
        }
        if(onWall)
        {
            rb.gravityScale = 0;
            rb.constraints = RigidbodyConstraints2D.None;
            if (v>0)
            {
                WallW();
            }
            else if (v < 0)
            {
                WallS();
            }
            else
            {
                WallStop();
            }
        }
        else
        {
            type = WallType.None;
            //rb.gravityScale = 1;
            PlayerMove2.Instance.isMove = true;
            beforeClimb = false;
        }
    }

    void CheckWall()
    {
        isRightWall=Physics2D.OverlapCircle(this.transform.position + wallOffset, radius, mask); //检测到右墙体 返回true
        isLeftWall = Physics2D.OverlapCircle(this.transform.position - wallOffset, radius, mask); //检测到左墙体 返回true
    }

    //上爬函数
    void WallW()
    {
        rb.velocity = Vector3.zero;
        type = WallType.WallUp;
        animator.SetBool("isClimb",true);
        rb.velocity = new Vector2(rb.velocity.x,climbSpeed);
    }

    //下降函数
    void WallS()
    {
        rb.velocity=Vector3.zero;
        type = WallType.WallDown;
        animator.SetBool("isClimb", true);
        rb.velocity = new Vector2(rb.velocity.x,-climbSpeed);
    }

    //攀爬停止函数
    void WallStop()
    {
        rb.velocity= Vector3.zero;
        animator.SetBool("isClimb", false);
        type =WallType.WallWait;
    }

    //进入攀爬状态函数
    void WallEnter()
    {
        PlayerMove2.Instance.isMove = false;
        animator.SetTrigger("beforeClimb");
        beforeClimb = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + wallOffset, radius);
        Gizmos.DrawWireSphere(transform.position - wallOffset, radius);
    }
}
