using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    private static UIManager instance=new UIManager();
    public static UIManager Instance => instance;

    //存储面板的容器（采用里氏替换原则表示不同面板类型）
    private Dictionary<string,BasePanel> panelDic = new Dictionary<string,BasePanel>();
    //应该一开始  就得到我们的Canvas对象（作为不同面板的父对象）
    private Transform canvasTrans;

    private UIManager()
    {
        //得到场景中的Canvas对象
        //canvasTrans=GameObject.Find("Canvas").transform;
        GameObject canvas = GameObject.Instantiate(Resources.Load<GameObject>("UI/Canvas"));
        canvasTrans = canvas.transform;
        //过场景不删除Canvas对象
        GameObject.DontDestroyOnLoad(canvasTrans.gameObject);
    }
   
    //显示面板
    public T ShowPanel<T>() where T : BasePanel
    {
        //我们只需要保证 泛型T的类型和面板名一致
        string panelName=typeof(T).Name;

        //是否已经有显示着的该面板了 如果有 不用创建 直接返回给外部使用
        if(panelDic.ContainsKey(panelName))
        {
            return panelDic[panelName] as T;
        }

        //显示面板 就是 动态的创建面板预设体 设置父对象
        GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("UI/" + panelName));
        panelObj.transform.SetParent(canvasTrans, false);
        //接着 就是得到对应的脚本存储起来
        T panel = panelObj.GetComponent<T>();
        //把面板脚本存储到对应的容器中  方便我们获取它
        panelDic.Add(panelName, panel);
        //调用显示自己的逻辑
        panel.ShowMe();
        return panel;
    }

    //隐藏面板
    //参数一：希望淡出默认为true,希望删掉则传false
    public void HidePanel<T>(bool isFade=true)where T : BasePanel
    {
        //根据 泛型类型 得到面板名字
        string panelName= typeof(T).Name;
        //判断当前显示的面板 有没有该名字的面板
        if(panelDic.ContainsKey(panelName))
        {
            if(isFade)
            {
                panelDic[panelName].HideMe(()=>
                {
                    //面板淡出成功后 删除面板
                    GameObject.Destroy(panelDic[panelName].gameObject);
                    //删除面板后 从字典中移除
                    panelDic.Remove(panelName);
                });
            }
            else
            {
                //直接删除面板
                GameObject.Destroy(panelDic[panelName].gameObject);
                //删除面板后 从字典中移除
                panelDic.Remove(panelName);
            }
        }
    }

    //获得面板
    public T GetPanel<T>()where T : BasePanel
    {
        string panelName=typeof (T).Name;
        if(panelDic.ContainsKey(panelName))
        {
            return panelDic[panelName] as T;
        }
        //如果没有 直接返回空
        return null;
    }
}
