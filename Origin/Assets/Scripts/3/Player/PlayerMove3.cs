using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove3 : MonoBehaviour
{
    public float hSpeed; //玩家对象水平方向的速度
    public float vSpeed; //玩家竖直方向的速度

    public bool isFacingRight=true; //玩家对象是否面向右边
    private Rigidbody2D rb; //玩家对象上的刚体
    private float h; //记录水平方向的输入量
    private float v; //记录竖直方向的输入量

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
         h = Input.GetAxis("Horizontal");
        
    }

    private void FixedUpdate()
    {
        Move(h * hSpeed);
    }

    //玩家移动函数
    private void Move(float h)
    {
        rb.velocity=new Vector2 (h,rb.velocity.y);
        if((h<0&&isFacingRight)||(h>0&&!isFacingRight))
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
}
