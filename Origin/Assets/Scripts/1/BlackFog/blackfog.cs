using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blackfog : MonoBehaviour
{
    public SpriteRenderer spr;
    [Header("淡入淡出一共花多少秒")]
    public float fadeTime = 2.5f;

    private float targetAlpha; //目标透明度 0完全透明 1不透明
    private float currentAlpha;

    void Start()
    {
        //初始化读取当前的透明度
        currentAlpha = spr.color.a;
        targetAlpha = currentAlpha;
    }

    void Update()
    {
        //平滑移动alpha值
        currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, (1f / fadeTime) * Time.deltaTime);
        Color c = spr.color;
        c.a = currentAlpha;
        spr.color = c;
    }

    /// <summary>淡出(慢慢消失到透明)</summary>
    public void FadeOut()
    {
        targetAlpha = 0f;
    }
    /// <summary>淡入(慢慢显示出来)</summary>
    public void FadeIn()
    {
        targetAlpha = 1f;
    }
}
