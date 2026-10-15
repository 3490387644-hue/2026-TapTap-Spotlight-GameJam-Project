using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneFallen2 : MonoBehaviour
{
    public GameObject vine; //лыбЭ╤тоС

    public void isOut()
    {
        vine.GetComponent<Animator>().SetTrigger("isOut");
        Destroy(vine, 2);
    }
}
