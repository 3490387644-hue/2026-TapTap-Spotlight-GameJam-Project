using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneFallen2 : MonoBehaviour
{
    public GameObject vine; //ÌÙÂü¶ÔÏó

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
        vine.GetComponent<Animator>().SetTrigger("isOut");
        Destroy(vine, 2);
    }
}
