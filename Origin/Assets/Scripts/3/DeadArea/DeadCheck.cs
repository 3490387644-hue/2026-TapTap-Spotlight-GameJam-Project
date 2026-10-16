using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadCheck : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            StartCoroutine(myCroutine(collision.gameObject));
        }
    }

    IEnumerator myCroutine(GameObject player)
    {
        Camera.main.DOShakePosition(0.7f, 2);
        yield return new WaitForSeconds(1);
        //死亡一秒后清理残影对象
        Destroy(ShadowSkill.Instance.Shadow1);
        Destroy(ShadowSkill.Instance.Shadow2);
        ShadowSkill.Instance.ClearShadow();

        //死亡时恢复角色的耐力值
        StaminaChange.Instance.currentNum = 100;

        yield return new WaitForSeconds(2);
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        player.transform.position = CheckpointMgr.Instance.GetCheckPoint().transform.position;
        if (player.transform.localScale != CheckpointMgr.Instance.GetCheckPoint().transform.localScale)
        {
            player.gameObject.GetComponent<PlayerMove3>().isFacingRight = !player.gameObject.GetComponent<PlayerMove3>().isFacingRight;
        }
        player.transform.localScale=CheckpointMgr.Instance.GetCheckPoint().transform.localScale;
    }
}
