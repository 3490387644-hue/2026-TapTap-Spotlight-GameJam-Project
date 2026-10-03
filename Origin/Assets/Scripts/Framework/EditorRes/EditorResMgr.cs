using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 编辑器资源管理器
/// 注意：只有在开发时能使用该管理器加载资源 用于开发功能
/// 发布后 是无法使用该管理器的 因为它需要用到编辑器相关功能
/// </summary>
public class EditorResMgr :BaseManager<EditorResMgr>
{
    //用于放置需要打包进AB包中的资源路径
    private string rootPath = "Assets/Editor/ArtRes/";

    /// <summary>
    /// 加载单个资源的函数
    /// </summary>
    /// <typeparam name="T">加载资源的类型</typeparam>
    /// <param name="path">资源名</param>
    /// <returns></returns>
    public T LoadEditorRes<T>(string path) where T:Object
    {
        //资源后缀名
        string suffixName = "";
        //预设体，纹理（图片），材质球，音效等等
        if (typeof(T) == typeof(GameObject))
            suffixName = ".prefab";
        else if (typeof(T) == typeof(Texture2D))
            suffixName = ".png";
        else if (typeof(T) == typeof(Material))
            suffixName = ".mat";
        else if (typeof(T) == typeof(AudioClip))
            suffixName = ".MP3";

        T res=AssetDatabase.LoadAssetAtPath<T>(rootPath + path + suffixName);
        return res;
    }

    /// <summary>
    /// 加载图集相关资源的函数（方法一）
    /// </summary>
    /// <param name="path">加载的图集名字</param>
    /// <param name="spriteName">图集中子图的名字</param>
    /// <returns></returns>
    public Sprite LoadSprite(string path,string spriteName)
    {
        //加载图集中的所有子资源
        Object[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(rootPath + path);
        //遍历所有子资源 得到同名图片返回
        foreach (Object sprite in sprites)
        {
            if(spriteName==sprite.name)
                return sprite as Sprite;
        }
        return null;
    }

    /// <summary>
    /// 加载图集相关资源的函数（方法二）
    /// </summary>
    /// <param name="path">加载的图集名字</param>
    /// <returns></returns>
    public Dictionary<string,Sprite> LoadSprites(string path)
    {
        Dictionary<string,Sprite> spriteDic = new Dictionary<string,Sprite>();
        Object[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(rootPath + path);
        foreach(Object sprite in sprites)
        {
            spriteDic.Add(sprite.name, sprite as Sprite);
        }
        return spriteDic;
    }
}
