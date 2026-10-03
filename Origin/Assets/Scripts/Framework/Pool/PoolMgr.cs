using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 抽屉（池子中的数据）对象
/// </summary>
public class PoolData
{
    //用来存储抽屉中的对象
    private Stack<GameObject> dataStack=new Stack<GameObject>();

    //用来记录使用中的对象
    private List<GameObject> usedList=new List<GameObject>();

    //抽屉根对象 用来进行布局管理的对象
    private GameObject rootObj;

    //获取容器中是否有对象
    public int Count=>dataStack.Count;
    //得到使用中的对象的数量
    public int UsedCount=>usedList.Count;

    /// <summary>
    /// 初始化构造函数
    /// </summary>
    /// <param name="root">柜子（缓存池）父对象</param>
    /// <param name="name">抽屉父对象的名字</param>
    public PoolData(GameObject root,string name,GameObject usedObj)
    {
        if(PoolMgr.isOpenLayout)
        {
            //创建抽屉父对象
            rootObj = new GameObject(name);
            //和柜子父对象建立父子关系
            rootObj.transform.SetParent(root.transform);
        }

        //创建抽屉时 外部肯定是会动态创建一个对象的
        //我们应该将其记录到 使用中的对象容器中
        PushUsedList(usedObj);
    }

    /// <summary>
    /// 从抽屉中弹出数据对象
    /// </summary>
    /// <returns>想要的对象数据</returns>
    public GameObject Pop()
    {
        //取出对象
        GameObject obj;

        if(Count>0)
        {
            //从没用的容器当中取出使用
            obj = dataStack.Pop();
            //现在要使用了 应该要用使用中的容器记录它
            usedList.Add(obj);
        }
        else
        {
            //取0索引的对象 代表的就是使用时间最长的对象
            obj = usedList[0];
            //并且把它从使用着的对象中移除
            usedList.RemoveAt(0);
            //由于它还要拿出去用 所以我们应该把它又记录到 使用中的容器中去
            //并且添加到尾部 表示 比较新的对象
            usedList.Add(obj);
        }

        //激活对象
        obj.SetActive(true);
        if(PoolMgr.isOpenLayout)
        //断开父子关系
        obj.transform.SetParent(null);

        return obj;
    }

    /// <summary>
    /// 将物体放入到抽屉对象中
    /// </summary>
    /// <param name="obj"></param>
    public void Push(GameObject obj)
    {
        //失活放入抽屉的对象
        obj.SetActive(false);
        //放入对应抽屉的根物体中 建立父子关系
        if(PoolMgr.isOpenLayout)
            obj.transform.SetParent(rootObj.transform);
        //通过栈记录对应的对象数据
        dataStack.Push(obj);
        //这个对象已经不再使用了 应该把它从记录容器中移除
        usedList.Remove(obj);
    }

    /// <summary>
    /// 将对象压入到使用中的容器中记录
    /// </summary>
    /// <param name="obj"></param>
    public void PushUsedList(GameObject obj)
    {
        usedList.Add(obj);
    }
}

/// <summary>
/// 缓存池(对象池)模块 管理器
/// </summary>
public class PoolMgr : BaseManager<PoolMgr>
{
    //柜子容器当中有抽屉的体现
    //值 其实代表的就是一个 抽屉对象
    private Dictionary<string,PoolData> poolDic= new Dictionary<string,PoolData>();

    //池子根对象
    private GameObject poolObj;

    //是否开启布局功能
    public static bool isOpenLayout=true;

    /// <summary>
    /// 拿东西的方法
    /// </summary>
    /// <param name="name">抽屉容器的名字</param>
    /// <returns>从缓存池中取出的对象</returns>
    public GameObject GetObj(string name,int maxNum=20)
    {
        GameObject obj;
        //如果根物体为空 就创建
        if (poolObj == null && isOpenLayout)
            poolObj = new GameObject("Pool");

        if (!poolDic.ContainsKey(name)||
            (poolDic[name].Count == 0 && poolDic[name].UsedCount<maxNum))
        {
            //动态创建对象
            //没有的时候 通过资源加载 去实例化出一个GameObject
            obj = GameObject.Instantiate(Resources.Load<GameObject>(name));
            //避免实例化出来的对象 默认会在名字后面加一个（Clone）
            //我们重命名过后 方便往里面放
            obj.name = name;

            //创建抽屉
            if (!poolDic.ContainsKey(name))
                poolDic.Add(name, new PoolData(poolObj, name, obj));
            //实例化出来的对象 需要记录到使用中的对象容器中
            else
                poolDic[name].PushUsedList(obj);
        }
        //当抽屉中有对象 或者 使用中的对象超上限了 直接去取出来使用
        else
        {
            obj = poolDic[name].Pop();
        }
        return obj;
    }

    /// <summary>
    /// 往缓存池中放入对象
    /// </summary>
    /// <param name="name">抽屉（对象）的名字</param>
    /// <param name="obj">希望放入的对象</param>
    public void PushObj(GameObject obj)
    {
        //往栈（抽屉）中放入对象
        poolDic[obj.name].Push(obj);
    }

    /// <summary>
    /// 用于清理整个柜子当中的数据
    /// 使用场景 主要是 切场景时
    /// </summary>
    public void ClearPool()
    {
        poolDic.Clear();
        poolObj = null;
    }
}
