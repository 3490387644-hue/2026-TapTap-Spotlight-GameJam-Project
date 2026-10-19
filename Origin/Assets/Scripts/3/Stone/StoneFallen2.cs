using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneFallen2 : MonoBehaviour
{
    public GameObject vine; //藤曼对象

    void Start()
    {
        if (GameSceneManager.Instance.GetStoneData.isStoneFallen)
        {
            Destroy(this.gameObject);
            Destroy(vine);
        }
    }

    public void isOut()
    {
        GameSceneManager.Instance.GetStoneData.isStoneFallen = true; // 存档石头已坠落
        vine.GetComponent<Animator>().SetTrigger("isOut");
        Destroy(vine, 2);
    }
}
