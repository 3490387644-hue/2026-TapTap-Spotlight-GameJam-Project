using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneFallen : MonoBehaviour
{
    public float hSpeed; //水平方向上的速度
    public float vSpeed; //竖直方向上的速度

    private Rigidbody2D rb; //落石身上的刚体脚本
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(hSpeed,vSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            collision.gameObject.GetComponent<PlayerMove3>().isFly = false;
            collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
        }
    }
}
