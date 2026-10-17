using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneFallen : MonoBehaviour
{
    public float hSpeed; //水平方向上的速度
    public float vSpeed; //竖直方向上的速度
    private Rigidbody2D rb; //落石身上的刚体脚本
    private bool hasTriggered = false; //是否已经触发过碰撞事件
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(hSpeed,vSpeed);
        EventCenter.Instance.AddEventListener<bool>(E_EventType.StoneTrigger, OnTriggeded);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if(collision.gameObject.tag=="Player")
    //    {
    //        collision.gameObject.GetComponent<PlayerMove3>().isFly = false;
    //        collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMove3>().isFly = false;
            collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
            if(!hasTriggered)
            {
                EventCenter.Instance.EventTrigger<bool>(E_EventType.StoneTrigger, true); // 通知所有监听者落石触发事件
                // 显示对话面板
                DialogManager.Instance.ShowType2Dialog(E_DialogDataType.SceneObjectCollisionText, 11);
            }
        }
    }

    void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener<bool>(E_EventType.StoneTrigger, OnTriggeded);
    }

    private void OnTriggeded(bool isTriggered)
    {
        hasTriggered = isTriggered;
        GameSceneManager.Instance.GetSceneObjectCollisionDialogData[11].HasTriggered = true; // 写回触发状态，防止重复触发
    }
}
