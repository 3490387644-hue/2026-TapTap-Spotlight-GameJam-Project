using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ChangePoint : MonoBehaviour
{
    [SerializeField] private int targetIndex;
    private bool hasTriggered;
    private Light2D light;
    // Start is called before the first frame update
    void Start()
    {
        light = GameObject.Find("Light 2D").GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!hasTriggered && collision.CompareTag("Player"))
        {
            hasTriggered = true;
            StartCoroutine(ClosedLight(collision.gameObject));
        }
    }

    IEnumerator ClosedLight(GameObject obj)
    {
        light.intensity = 0;
        // 更新角色位置
        BornPointMgr.Instance.UpdatePoint(obj, targetIndex);
        //更新摄像机边界范围
        CameraShapeMgr.Instance.UpdateCameraShape();
        yield return new WaitForSeconds(2);
        light.intensity = 1;
    }
}
