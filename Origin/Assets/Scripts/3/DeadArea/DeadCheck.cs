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
        //Camera.main.DOShakePosition(0.3f, 2);
        yield return new WaitForSeconds(3);
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        player.transform.position = CheckpointMgr.Instance.GetCheckPoint().transform.position;
        player.transform.localScale=CheckpointMgr.Instance.GetCheckPoint().transform.localScale;
    }
}
