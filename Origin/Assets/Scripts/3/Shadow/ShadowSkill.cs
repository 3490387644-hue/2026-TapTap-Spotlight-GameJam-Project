using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowSkill : MonoBehaviour
{
    private static ShadowSkill instance;
    public static ShadowSkill Instance=>instance;

    public PlayerMove3 player; //玩家对象上的PlayerMove3脚本
    public Vector3 offset; //范围检测点偏离原点的位置
    public float radius; //检测范围半径大小
    public LayerMask mask; //范围检测层
    public GameObject Shadow1; //影子对象1（记录点）
    public GameObject Shadow2; //影子对象2（残影点）
    private int fClick=2; //F键可点击的次数

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerShadowSkill();
    }

    //移形换影能力函数
    private void PlayerShadowSkill()
    {
        //按'F'键设置残影点
        if (Input.GetKeyDown(KeyCode.F))
        {
            //在地面时且是第一次按下F键时可以设置记录点
            if (fClick == 2 && Physics2D.OverlapCircle(this.transform.position + offset, radius, mask))
            {
                Shadow1 = Instantiate(ResMgr.Instance.Load<GameObject>("Prefabs/Shadow/Shadow1"), this.transform.position, this.transform.rotation);
                Shadow1.transform.localScale = this.transform.localScale;
                fClick--;
            }
            //在飞行状态且是第二次按F键时可以在原地生成残影并回到记录点的位置
            if (fClick == 1 && player.isFly)
            {
                Shadow2 = Instantiate(ResMgr.Instance.Load<GameObject>("Prefabs/Shadow/Shadow2"), this.transform.position, this.transform.rotation);
                Shadow2.transform.localScale = this.transform.localScale;
                this.transform.position = Shadow1.transform.position;
                if (this.transform.localScale != Shadow1.transform.localScale)
                {
                    player.isFacingRight = !player.isFacingRight;
                }
                this.transform.localScale = Shadow1.transform.localScale;
                Destroy(Shadow1); //回到记录点位置时把记录点销毁
                fClick--;
            }
        }

        //按'E'键回到残影的位置（需要有残影记录）
        if (Input.GetKeyDown(KeyCode.E) && Shadow2 != null)
        {
            player.isFly = true;
            this.transform.position = Shadow2.transform.position;
            if (this.transform.localScale != Shadow2.transform.localScale)
            {
                player.isFacingRight = !player.isFacingRight;
            }
            this.transform.localScale = Shadow2.transform.localScale;
            Destroy(Shadow2); //回到残影位置时把记录点销毁
            print(1);
            ClearShadow();
        }
    }

    //清空影子函数
    public void ClearShadow()
    {
        Shadow1 = null;
        Shadow2 = null;
        fClick = 2; //F键可点击次数重新设置为2
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position+offset, radius);
    }
}
