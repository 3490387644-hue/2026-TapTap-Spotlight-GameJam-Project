using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blackfog : MonoBehaviour
{
    public SpriteRenderer spr;
    [Header("淡入淡出一共花多少秒")]
    public float fadeTime = 2.5f;
    public int fogID;

    private float targetAlpha; //目标透明度 0完全透明 1不透明
    private float currentAlpha;
    private bool isDestroyed = false; //是否已经被销毁

    void Start()
    {
        var fogData = GameSceneManager.Instance.GetBlackFogDataList.Find(f => f.blackFogID == fogID);
        if(fogData != null && fogData.isDestroyed)
        {
            Destroy(this.gameObject);   // 已销毁，直接不生成
            return;
        }
        if(fogData == null)               // 第一次进入，注册一条默认数据
        {
            GameSceneManager.Instance.GetBlackFogDataList.Add(new BlackFogData { blackFogID = fogID, isDestroyed = false });
        }
        //初始化读取当前的透明度
        currentAlpha = spr.color.a;
        targetAlpha = currentAlpha;
    }

    void OnDestroy()
    {
        if(!isDestroyed)
        {
            isDestroyed = true;
            GameSceneManager.Instance.UpdateBlackFogDataList(fogID);
        }
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
