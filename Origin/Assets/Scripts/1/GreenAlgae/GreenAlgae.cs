using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenAlgae : MonoBehaviour
{
    private GameObject player; //玩家位置

    private Vector3 lastPlayerPosition; //上一帧的玩家位置

    private Tweener tween;

    private bool isForward; //是否开启自动跟随

    private void Start()
    {
        
    }

    private void Update()
    {
        if(isForward)
        {
            //如果玩家位置改变则更改目标值，否则不用更改
            if(lastPlayerPosition==player.transform.position)
             return;
            tween.ChangeEndValue(player.transform.position, true).Restart();
            lastPlayerPosition = player.transform.position;
        }
        //当绿藻与玩家距离小于0.01时，绿藻自动销毁
        if(isForward && Vector3.Distance(this.transform.position,player.transform.position)<=1)
        {
            PickGreenAlgae.Instance.algaeNum++;
            Destroy(this.gameObject);
            tween.Kill(); //销毁实例 避免报错
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            tween = this.transform.DOMove(collision.transform.position, 0.5f).SetAutoKill(false); //关闭实例的自动销毁
            player=collision.gameObject;
            lastPlayerPosition = collision.transform.position;
            isForward = true;
        }
    }
}
