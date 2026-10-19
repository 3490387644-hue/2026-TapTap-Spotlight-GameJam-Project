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
    private float maxNum = 150; //角色最大耐力值
    private float startScaleY; //耐力条的初始缩放Y值
    private bool hasTriggered = false; //是否已经触发过耐力值为0的事件
    private bool isLow = false; //是否处于低耐力值状态
    private bool isLoading = false; //警告音效异步加载中标记，防止重复触发产生孤儿音源
    private AudioSource lowStaminaSound; //低耐力警告音效的音源

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

    void Update()
    {
        this.transform.position = target.transform.position + offset;
        if(isLow) return;
        if(currentNum < maxNum / 6)
        {
            isLow = true;
            if (isLoading) return; // 已在加载中，避免重复触发音源
            isLoading = true;
            GameSceneManager.Instance.PlaySoundByTriggerType(E_SoundTriggerType.Chapter3LowStaminaWarning, true, true, OnLowStaminaSoundLoaded);
        }
    }

    public void UpdateStaminaChange(float num)
    {
        currentNum += num;
        currentNum = Mathf.Clamp(currentNum, 0, maxNum); //当前耐力值限制在0~maxNum之间
        if (currentNum == 0)
        {
            player.GetComponent<PlayerMove3>().isFly = false; //耐力值为0时，退出飞行模式
            GameSceneManager.Instance.UpdateIsFlying(false); // 通知GameSceneManager玩家退出飞行状态
            if (!hasTriggered)
            {
                hasTriggered = true; //标记已经触发过耐力值为0的事件
                GameSceneManager.Instance.GetSceneObjectCollisionDialogData[10].HasTriggered = true; // 回写触发状态，用于存档
                DialogManager.Instance.ShowType2Dialog(E_DialogDataType.SceneObjectCollisionText, 10); //弹出提示对话框
            }
            player.GetComponent<Rigidbody2D>().gravityScale = 1; //同时恢复重力参数
        }
        if(currentNum > maxNum / 6)
        {
            isLow = false; //重置低耐力值状态
            StopLowStaminaSound(); //取消低耐力警告音效
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

    private void OnLowStaminaSoundLoaded(AudioSource source)
    {
        isLoading = false;
        lowStaminaSound = source;
        //音效加载完成前耐力值已恢复，则立即停止
        if (!isLow)
        {
            StopLowStaminaSound();
        }
    }

    private void StopLowStaminaSound()
    {
        if (lowStaminaSound == null)
            return;
        MusicMgr.Instance.StopSound(lowStaminaSound);
        lowStaminaSound = null;
    }

    private void OnDestroy()
    {
        StopLowStaminaSound();
    }
}
