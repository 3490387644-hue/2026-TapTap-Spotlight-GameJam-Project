using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

public class ABMgr : SingletonAutoMono<ABMgr>
{
    //主包
    private AssetBundle mainAB = null;
    //依赖包获取用的配置文件
    private AssetBundleManifest manifest = null;

    //AB包不能够重复加载 重复加载会报错
    //字典 用字典来存储 加载过的AB包
    private Dictionary<string,AssetBundle> abDic = new Dictionary<string,AssetBundle>();

    /// <summary>
    /// 这个AB包存放路径 方便修改
    /// </summary>
    private string PathUrl
    {
        get
        {
            return Application.streamingAssetsPath + "/";
        }
    }

    /// <summary>
    /// 主包名 方便修改
    /// </summary>
    private string MainABName
    {
        get
        {
#if UNITY_IOS
            return "IOS";
#elif UNITY_ANDROID
            return "Android";
#else
            return "PC";
#endif
        }
    }

    //加载主包
    public void LoadMainAB()
    {
        //加载AB包
        if (mainAB == null)
        {
            mainAB = AssetBundle.LoadFromFile(PathUrl + MainABName);
            manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
    }

    //加载AB包
    public void LoadAB(string abName)
    {
        LoadMainAB();
        //我们获取依赖包相关信息
        string[] strs = manifest.GetAllDependencies(abName);
        AssetBundle ab = null;
        for (int i = 0; i < strs.Length; i++)
        {
            //判断包是否加载过
            if (!abDic.ContainsKey(strs[i]))
            {
                ab = AssetBundle.LoadFromFile(PathUrl + strs[i]);
                abDic.Add(strs[i], ab);
            }
        }

        //加载资源来源包
        //如果没有加载过 再加载
        if (!abDic.ContainsKey(abName))
        {
            ab = AssetBundle.LoadFromFile(PathUrl + abName);
            abDic.Add(abName, ab);
        }
    }

    #region 同步加载函数
    ////同步加载 不指定类型
    //public Object LoadRes(string abName,string resName)
    //{
    //    //加载AB包
    //    LoadAB(abName);
    //    //为了外面方便 在加载资源时 判断一下 资源是不是GameObject
    //    //如果是 直接实例化了 再返回给外部
    //    Object obj= abDic[abName].LoadAsset(resName);
    //    return obj;
    //}

    ////同步加载 根据type指定类型
    //public Object LoadRes(string abName, string resName,System.Type type)
    //{
    //    //加载AB包
    //    LoadAB(abName);
    //    //为了外面方便 在加载资源时 判断一下 资源是不是GameObject
    //    //如果是 直接实例化了 再返回给外部
    //    Object obj = abDic[abName].LoadAsset(resName,type);
    //    return obj;
    //}

    ////同步加载 根据泛型指定类型
    //public T LoadRes<T>(string abName, string resName) where T:Object
    //{
    //    //加载AB包
    //    LoadAB(abName);
    //    //为了外面方便 在加载资源时 判断一下 资源是不是GameObject
    //    //如果是 直接实例化了 再返回给外部
    //    T obj = abDic[abName].LoadAsset<T>(resName);
    //    return obj;
    //}
    #endregion

    /// <summary>
    /// 异步加载 不指定类型
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <param name="callBack"></param>
    /// <param name="isSync"></param>
    public void LoadResAsync(string abName, string resName, UnityAction<Object> callBack,bool isSync=false)
    {
        StartCoroutine(ReallyLoadResAsync(abName, resName, callBack,isSync));
    }

    private IEnumerator ReallyLoadResAsync(string abName, string resName, UnityAction<Object> callBack,bool isSync)
    {
        LoadMainAB();
        //我们获取依赖包相关信息
        string[] strs = manifest.GetAllDependencies(abName);
        AssetBundle ab = null;
        for (int i = 0; i < strs.Length; i++)
        {
            //判断包是否加载过
            if (!abDic.ContainsKey(strs[i]))
            {
                //同步加载
                if (isSync)
                {
                    ab = AssetBundle.LoadFromFile(PathUrl + strs[i]);
                    abDic.Add(strs[i], ab);
                }
                //异步加载
                else
                {
                    //一开始异步加载 就记录 如果此时的记录中的值 是null 就证明这个ab包正在被异步加载
                    abDic.Add(strs[i], null);
                    AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(PathUrl + strs[i]);
                    yield return req;
                    //异步加载结束后 再替换之前的null 这时 不为null 就证明加载结束了
                    abDic[strs[i]] = req.assetBundle;
                }
            }
            //就证明 字典中已经记录了一个AB包相关信息
            else
            {
                //如果字典中记录的信息是null 那就证明正在加载中
                //我们只需要等待它加载结束 就可以继续执行后面的代码了
                while (abDic[strs[i]] == null)
                {
                    //只要发现正在加载中 就不停的等待一帧 下一帧再进行判断
                    yield return 0;
                }
            }
        }

        //加载资源来源包
        //如果没有加载过 再加载
        if (!abDic.ContainsKey(abName))
        {
            if (isSync)
            {
                ab = AssetBundle.LoadFromFile(PathUrl + abName);
                abDic.Add(abName, ab);
            }
            else
            {
                //一开始异步加载 就记录 如果此时的记录中的值 是null 就证明这个ab包正在被异步加载
                abDic.Add(abName, null);
                AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(PathUrl + abName);
                yield return req;
                //异步加载结束后 再替换之前的null 这时 不为null 就证明加载结束了
                abDic[abName] = req.assetBundle;
            }
        }
        else
        {
            //如果字典中记录的信息是null 那就证明正在加载中
            //我们只需要等待它加载结束 就可以继续执行后面的代码了
            while (abDic[abName] == null)
            {
                //只要发现正在加载中 就不停的等待一帧 下一帧再进行判断
                yield return 0;
            }
        }
        
        //同步加载
        if(isSync)
        {
            Object res = abDic[abName].LoadAsset(resName);
            callBack(res);
        }
        //异步加载
        else
        {
            AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName);
            yield return abr;
            callBack?.Invoke(abr.asset);
        }
    }

    /// <summary>
    /// 异步加载 指定类型
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <param name="type"></param>
    /// <param name="callBack"></param>
    /// <param name="isSync"></param>
    public void LoadResAsync(string abName, string resName, System.Type type, UnityAction<Object> callBack,bool isSync=false)
    {
        StartCoroutine(ReallyLoadResAsync(abName, resName, type, callBack,isSync));
    }

    private IEnumerator ReallyLoadResAsync(string abName, string resName, System.Type type, UnityAction<Object> callBack, bool isSync)
    {
        LoadMainAB();
        //我们获取依赖包相关信息
        string[] strs = manifest.GetAllDependencies(abName);
        AssetBundle ab = null;
        for (int i = 0; i < strs.Length; i++)
        {
            //判断包是否加载过
            if (!abDic.ContainsKey(strs[i]))
            {
                //同步加载
                if (isSync)
                {
                    ab = AssetBundle.LoadFromFile(PathUrl + strs[i]);
                    abDic.Add(strs[i], ab);
                }
                //异步加载
                else
                {
                    //一开始异步加载 就记录 如果此时的记录中的值 是null 就证明这个ab包正在被异步加载
                    abDic.Add(strs[i], null);
                    AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(PathUrl + strs[i]);
                    yield return req;
                    //异步加载结束后 再替换之前的null 这时 不为null 就证明加载结束了
                    abDic[strs[i]] = req.assetBundle;
                }
            }
            //就证明 字典中已经记录了一个AB包相关信息
            else
            {
                //如果字典中记录的信息是null 那就证明正在加载中
                //我们只需要等待它加载结束 就可以继续执行后面的代码了
                while (abDic[strs[i]] == null)
                {
                    //只要发现正在加载中 就不停的等待一帧 下一帧再进行判断
                    yield return 0;
                }
            }
        }

        //加载资源来源包
        //如果没有加载过 再加载
        if (!abDic.ContainsKey(abName))
        {
            if (isSync)
            {
                ab = AssetBundle.LoadFromFile(PathUrl + abName);
                abDic.Add(abName, ab);
            }
            else
            {
                //一开始异步加载 就记录 如果此时的记录中的值 是null 就证明这个ab包正在被异步加载
                abDic.Add(abName, null);
                AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(PathUrl + abName);
                yield return req;
                //异步加载结束后 再替换之前的null 这时 不为null 就证明加载结束了
                abDic[abName] = req.assetBundle;
            }
        }
        else
        {
            //如果字典中记录的信息是null 那就证明正在加载中
            //我们只需要等待它加载结束 就可以继续执行后面的代码了
            while (abDic[abName] == null)
            {
                //只要发现正在加载中 就不停的等待一帧 下一帧再进行判断
                yield return 0;
            }
        }

        //同步加载
        if (isSync)
        {
            Object res = abDic[abName].LoadAsset(resName,type);
            callBack(res);
        }
        //异步加载
        else
        {
            AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName, type);
            yield return abr;
            callBack?.Invoke(abr.asset);
        }
    }

    /// <summary>
    /// 异步加载 泛型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <param name="callBack"></param>
    /// <param name="isSync">是否使用同步加载，默认不使用</param>
    public void LoadResAsync<T>(string abName, string resName, UnityAction<T> callBack,bool isSync=false) where T : Object
    {
        StartCoroutine(ReallyLoadResAsync<T>(abName, resName, callBack,isSync));
    }

    private IEnumerator ReallyLoadResAsync<T>(string abName, string resName, UnityAction<T> callBack,bool isSync) where T:Object
    {
        LoadMainAB();
        //我们获取依赖包相关信息
        string[] strs = manifest.GetAllDependencies(abName);
        AssetBundle ab = null;
        for (int i = 0; i < strs.Length; i++)
        {
            //判断包是否加载过
            if (!abDic.ContainsKey(strs[i]))
            {
                //同步加载
                if(isSync)
                {
                    ab = AssetBundle.LoadFromFile(PathUrl + strs[i]);
                    abDic.Add(strs[i], ab);
                }
                //异步加载
                else
                {
                    //一开始异步加载 就记录 如果此时的记录中的值 是null 就证明这个ab包正在被异步加载
                    abDic.Add(strs[i], null);
                    AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(PathUrl + strs[i]);
                    yield return req;
                    //异步加载结束后 再替换之前的null 这时 不为null 就证明加载结束了
                    abDic[strs[i]] = req.assetBundle;
                }
            }
            //就证明 字典中已经记录了一个AB包相关信息
            else
            {
                //如果字典中记录的信息是null 那就证明正在加载中
                //我们只需要等待它加载结束 就可以继续执行后面的代码了
                while (abDic[strs[i]]==null)
                {
                    //只要发现正在加载中 就不停的等待一帧 下一帧再进行判断
                    yield return 0;
                }
            }
        }

        //加载资源来源包
        //如果没有加载过 再加载
        if (!abDic.ContainsKey(abName))
        {
            if(isSync)
            {
                ab = AssetBundle.LoadFromFile(PathUrl + abName);
                abDic.Add(abName, ab);
            }
            else
            {
                //一开始异步加载 就记录 如果此时的记录中的值 是null 就证明这个ab包正在被异步加载
                abDic.Add(abName, null);
                AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(PathUrl + abName);
                yield return req;
                //异步加载结束后 再替换之前的null 这时 不为null 就证明加载结束了
                abDic[abName] = req.assetBundle;
            }
        }
        else
        {
            //如果字典中记录的信息是null 那就证明正在加载中
            //我们只需要等待它加载结束 就可以继续执行后面的代码了
            while (abDic[abName] == null)
            {
                //只要发现正在加载中 就不停的等待一帧 下一帧再进行判断
                yield return 0;
            }
        }

        //同步加载
        if(isSync)
        {
            T res = abDic[abName].LoadAsset<T>(resName);
            callBack(res);
        }
        //异步加载
        else
        {
            AssetBundleRequest abr = abDic[abName].LoadAssetAsync<T>(resName);
            yield return abr;
            callBack?.Invoke(abr.asset as T);
        }
    }

    //单个包卸载
    public void UnLoad(string abName,UnityAction<bool> callBackResult)
    {
        if(abDic.ContainsKey(abName))
        {
            if (abDic[name]==null)
            {
                //代表正在异步加载 没有卸载成功
                callBackResult(false);
                return;
            }
            abDic[abName].Unload(false);
            abDic.Remove(abName);
            //卸载成功
            callBackResult(true);
        }
    }

    //所有包的卸载
    public void ClearAB()
    {
        //由于AB包都是异步加载了 因此在清理之前 停止协同程序
        StopAllCoroutines();
        AssetBundle.UnloadAllAssetBundles(false);
        abDic.Clear();
        mainAB = null;
        manifest = null;
    }
}
