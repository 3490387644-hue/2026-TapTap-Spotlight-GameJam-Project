using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DeadCheck2 : MonoBehaviour
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
        if(collision.tag=="Player")
        {
            StartCoroutine(myCoroutine(collision.gameObject));
        }
    }

    IEnumerator myCoroutine(GameObject obj)
    {
        DeadChange.Instance.DarkChange();
        yield return new WaitForSeconds(2);
        obj.transform.position = BornPointMgr.Instance.bornPoints[BornPointMgr.Instance.index].position;
        obj.transform.localScale = BornPointMgr.Instance.bornPoints[BornPointMgr.Instance.index].localScale;
    }
}
