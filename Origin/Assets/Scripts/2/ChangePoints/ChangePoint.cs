using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ChangePoint : MonoBehaviour
{
    private Light2D light1;
    // Start is called before the first frame update
    void Start()
    {
        light1 = GameObject.Find("Light 2D").GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Player")
        {
            StartCoroutine(ClosedLight(collision.gameObject));
        }
    }

    IEnumerator ClosedLight(GameObject obj)
    {
        light1.intensity = 0;
        //更新角色位置
        BornPointMgr.Instance.UpdatePoint(obj);
        //更新相机边界范围
        CameraShapeMgr.Instance.UpdateCameraShape();
        yield return new WaitForSeconds(2);
        light1.intensity = 1;
    }
}
