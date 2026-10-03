using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //玩家移动速度
    public float moveSpeed = 1;
    //玩家跳跃速度
    public float jumpSpeed = 10;
    //范围检测判断点
    public Transform checkPoint;

    //玩家对象上的刚体
    private Rigidbody2D rb;
    //判断是否起跳
    private bool isJump=false;

    // Start is called before the first frame update
    void Start()
    {
        rb= GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, rb.velocity.y);
        //是否落地
        bool isOnGround = Physics2D.OverlapCircle(checkPoint.position, 0.01f, 1 << LayerMask.NameToLayer("Ground"));

        if (Input.GetKeyDown(KeyCode.W) && isOnGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
    }

    private void FixedUpdate()
    {
        
    }

    
}
