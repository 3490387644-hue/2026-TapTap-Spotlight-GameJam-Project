using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StaminaChange : MonoBehaviour
{
    private static StaminaChange instance;
    public static StaminaChange Instance => instance;

    public GameObject target; //耐力条跟随的对象
    public Vector3 offset; //耐力条偏移角色的位置
    public Transform spriteNum; //耐力值数值图片
    public GameObject player;  //玩家对象
    public float currentNum; //角色当前耐力值
    private float maxNum = 120; //角色最大耐力值
    private float startScaleY; //耐力条的初始缩放Y值
    private bool hasTriggered = false; //是否已经触发过耐力值为0的事件

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if(GameSceneManager.Instance.GetSceneObjectCollisionDialogData != null)
        {
            hasTriggered = GameSceneManager.Instance.GetSceneObjectCollisionDialogData[10].HasTriggered; //从游戏数据中获取是否已经触发过耐力值为0的事件
        }
        currentNum = maxNum;
        startScaleY = spriteNum.transform.localScale.y;
        this.gameObject.SetActive(false); //初始化之后 对象失活
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = target.transform.position + offset;
    }

    public void UpdateStaminaChange(float num)
    {
        currentNum += num;
        currentNum = Mathf.Clamp(currentNum, 0, maxNum); //当前耐力值限制在0~maxNum之间
        if (currentNum == 0)
        {
            player.GetComponent<PlayerMove3>().isFly = false; //耐力值为0时，退出飞行模式
            if (!hasTriggered)
            {
                hasTriggered = true; //标记已经触发过耐力值为0的事件
                DialogManager.Instance.ShowType2Dialog(E_DialogDataType.SceneObjectCollisionText, 10); //弹出提示对话框
            }
            player.GetComponent<Rigidbody2D>().gravityScale = 1; //同时恢复重力参数
        }
        if (currentNum == 100 && !player.GetComponent<PlayerMove3>().isFly)
        {
            //耐力值满时，退出耐力值恢复状态并隐藏耐力条
            player.GetComponent<PlayerMove3>().isAdd = false;
            this.gameObject.SetActive(false);
        }
        float newY = ((currentNum - num) / maxNum) * startScaleY;
        spriteNum.localScale = new Vector3(spriteNum.localScale.x, newY, spriteNum.localScale.z);
    }
}
