using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickGreenAlgae : MonoBehaviour
{
    //public LayerMask m_Mask; //范围检测层
    //public float radius = 0.7f; //范围检测半径
    public int algaeNum = 0; //吸收绿藻的数量
    private static PickGreenAlgae instance;
    public static PickGreenAlgae Instance=>instance;

    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        //当吸收绿藻数量达到五个时，玩家变大
        if(algaeNum==5)
        {
            if(CharacterController2D.Instance.m_FacingRight)
            {
                this.transform.localScale = new Vector3(0.35f, 0.35f, 1);
            }
            else
            {
                this.transform.localScale = new Vector3(-0.35f,0.35f,1);
            }
            CharacterController2D.Instance.jumpForce = 500;
        }
    }


    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    //按F键进行吸收并判断范围内有没有绿藻
    //    if(Input.GetKeyDown(KeyCode.F))
    //    {
    //        Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position,radius, m_Mask);
    //        for(int i=0;i<colliders.Length;i++)
    //        {
    //            print(colliders[i].gameObject.name);
    //            if (colliders[i].gameObject.tag == "Green Algae")
    //            {
    //                Destroy(colliders[i].gameObject);
    //                algaeNum++;
    //            }
    //        }
    //    }
    //}

    //// Gizmos绘制范围
    //void OnDrawGizmosSelected()
    //{
    //    //设置 gizmos颜色，这里绿色
    //    Gizmos.color = Color.green;
    //    // 线框球体：中心是玩家位置，半径 detectRadius
    //    Gizmos.DrawWireSphere(transform.position, 0.7f);
    //}
}
