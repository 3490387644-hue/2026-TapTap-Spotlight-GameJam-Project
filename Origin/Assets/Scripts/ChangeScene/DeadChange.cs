using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static Unity.VisualScripting.Member;

public class DeadChange : MonoBehaviour
{
    //提供的外部接口
    private static DeadChange instance;
    public static DeadChange Instance => instance;

    private Light2D light2D;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        light2D = GetComponent<Light2D>();
    }

    /// <summary>
    /// 玩家死亡时渐变黑然后恢复
    /// </summary>
    public void DarkChange()
    {
        Debug.Log("DarkChange:玩家死亡，开始渐变黑");
        StartCoroutine(DarkChangeCoroutine());
    }

    IEnumerator DarkChangeCoroutine()
    {
        //等待Mycoroutine全部执行完毕，才往下走
        yield return StartCoroutine(Mycoroutine());
        //上面协程结束后才执行这行
        yield return StartCoroutine(Mycoroutine1());
    }

    IEnumerator Mycoroutine()
    {
        float t = 0;
        float time = 2f;
        while (t < time)
        {
            t += Time.deltaTime;
            float progress = Mathf.Clamp01(t / time);
            light2D.intensity = Mathf.Lerp(light2D.intensity, 0, progress);
            yield return null;
        }
        light2D.intensity = 0;
    }

    IEnumerator Mycoroutine1()
    {
        float t = 0;
        float time = 2f;
        while (t < time)
        {
            t += Time.deltaTime;
            float progress = Mathf.Clamp01(t / time);
            light2D.intensity = Mathf.Lerp(light2D.intensity, 1, progress);
            yield return null;
        }
        light2D.intensity = 1;
    }
}
