using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickGreenAlgae : MonoBehaviour
{
    //public LayerMask m_Mask; //范围检测层
    //public float radius = 0.7f; //范围检测半径
    public int algaeNum = 0; //吸收绿藻的数量
    private static PickGreenAlgae instance;
    public static PickGreenAlgae Instance => instance;
    private bool isLarge = false; //玩家是否变大
    private int lastNum = 0;

    private void Start()
    {
        instance = this;
    }

    void OnEnable()
    {
        algaeNum = GameSceneManager.Instance.GetGreenAlgaeCount;
        lastNum = algaeNum;
        // 读档恢复时，也要立刻应用一次外观/弹跳状态
        // GameSceneManager.Instance.ChangePlayerScale(GameDataMgr.Instance.gameData.playerData.playerState);
    }

    private void Update()
    {
        if (algaeNum == lastNum) return;   // 数量没变，什么都不做
        lastNum = algaeNum;

        if (algaeNum >= 5)
        {
            if(CharacterController2D.Instance.m_FacingRight)
            {
                this.transform.localScale = new Vector3(0.28f, 0.28f, 1);
            }
            else
            {
                this.transform.localScale = new Vector3(-0.28f,0.28f,1);
            }
            CharacterController2D.Instance.jumpForce = 11;
        }
        if(algaeNum==10)
        {
            glow.Instance.isGlow = true;
            // 标记状态并应用（只触发这一次）
            GameSceneManager.Instance.UpdatePlayerState(E_PlayerState.Large);
            GameSceneManager.Instance.ChangePlayerScale(E_PlayerState.Large);
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
