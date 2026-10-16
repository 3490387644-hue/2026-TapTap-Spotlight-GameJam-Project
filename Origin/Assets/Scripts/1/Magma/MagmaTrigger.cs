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
            StartCoroutine(myCoroutine(collision.gameObject));
        }
    }

    IEnumerator myCoroutine(GameObject obj)
    {
        DeadChange.Instance.DarkChange();
        yield return new WaitForSeconds(1.2f);
        obj.GetComponent<Rigidbody2D>().velocity=Vector2.zero;
        obj.transform.position = CheckpointMgr.Instance.GetCheckPoint().transform.position;
    }
}
