using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    //第一次经过的时候才会设置为新的重生点
    int count = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player"&&count>0)
        {
            CheckpointMgr.Instance.PushCheckPoint(this.gameObject);
            count--;
        }
    }
}
