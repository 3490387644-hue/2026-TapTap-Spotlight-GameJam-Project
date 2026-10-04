using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagmaTrigger : MonoBehaviour
{
    //玩家碰到岩浆会回到最新的复活点
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            collision.transform.position=CheckpointMgr.Instance.GetCheckPoint().transform.position;
        }
    }
}
