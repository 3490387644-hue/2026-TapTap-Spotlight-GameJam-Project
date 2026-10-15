using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StonePut : MonoBehaviour
{
    public bool isCanPutStone=false;
    private GameObject stoneObj; //石头对象
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G)&&isCanPutStone)
        {
            stoneObj.GetComponent<Animator>().SetTrigger("isFall");
            Destroy(stoneObj,3);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Stone")
        {
            isCanPutStone = true;
            stoneObj = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Stone")
        {
            isCanPutStone = false;
            stoneObj = null;
        }
    }
}
