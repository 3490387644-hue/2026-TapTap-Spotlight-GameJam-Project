using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class glow : SingletonMono<glow>
{
    public Light2D light; //玩家子物体上的光源组件

    public bool isGlow=false; //是否允许发光

    public Transform checkPoint; //范围检测点

    public LayerMask mask;//检测层

    private float radius = 9; //检测半径

    private GameObject obj; //检测到的黑雾对象

    private float lastTime; //发光持续时间
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isGlow)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                light.gameObject.SetActive(true);
                obj = Physics2D.OverlapCircle(checkPoint.position, radius, mask).gameObject;

            }
            if(Input.GetKey(KeyCode.E))
            {
                lastTime += Time.deltaTime;
                if (light.intensity >=0.5)
                    light.intensity -= 0.001f;
                if (light.falloffIntensity >= 0.21)
                    light.falloffIntensity -= 0.001f;
                if(light.pointLightOuterRadius<=2)
                    light.pointLightOuterRadius += 0.001f;
                if (light.pointLightInnerRadius <= 0.5)
                    light.pointLightInnerRadius += 0.001f;
                if (lastTime >= 3)
                {
                    obj.GetComponent<blackfog>().FadeOut();
                    Destroy(obj,2.2f);
                }
            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                light.intensity = 1;
                light.falloffIntensity = 1;
                light.pointLightOuterRadius = 1;
                light.pointLightInnerRadius = 0;
                obj = null;
                lastTime = 0; //刷新持续时间
                light.gameObject.SetActive(false);
            }
        }
    }
}
