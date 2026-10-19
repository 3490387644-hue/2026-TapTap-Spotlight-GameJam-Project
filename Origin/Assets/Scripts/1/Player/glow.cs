using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class glow : SingletonMono<glow>
{
    public Light2D light1; // ???Light2D??

    public bool isGlow = false; // ??????/??

    public Transform checkPoint; // ????????

    public LayerMask mask; // ???????

    private float radius = 9; // ????

    private GameObject obj; // ????????

    private float lastTime; // ?????????

    protected override void Awake()
    {
        base.Awake();
        if (Instance != this)
            return;

        if (light1 == null)
            light1 = GetComponentInChildren<Light2D>(true);

        if (light1 == null)
            Debug.LogError("glow???Light2D??", this);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isGlow)
        {
            if (light1 == null)
                return;

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (checkPoint == null)
                    return;

                light1.gameObject.SetActive(true);
                Collider2D target = Physics2D.OverlapCircle(checkPoint.position, radius, mask);
                obj = target != null ? target.gameObject : null;

            }
            if (Input.GetKey(KeyCode.E))
            {
                lastTime += Time.deltaTime;
                if (light1.intensity >= 0.5)
                    light1.intensity -= 0.001f;
                if (light1.falloffIntensity >= 0.21)
                    light1.falloffIntensity -= 0.001f;
                if (light1.pointLightOuterRadius <= 2)
                    light1.pointLightOuterRadius += 0.001f;
                if (light1.pointLightInnerRadius <= 0.5)
                    light1.pointLightInnerRadius += 0.001f;
                if (lastTime >= 3 && obj != null)
                {
                    blackfog fog = obj.GetComponent<blackfog>();
                    if (fog != null)
                    {
                        fog.FadeOut();
                        Destroy(obj, 2.2f);
                    }
                }
            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                if (light1 == null)
                    return;

                light1.intensity = 1;
                light1.falloffIntensity = 1;
                light1.pointLightOuterRadius = 1;
                light1.pointLightInnerRadius = 0;
                obj = null;
                lastTime = 0; // ?????
                light1.gameObject.SetActive(false);
            }
        }
    }
}