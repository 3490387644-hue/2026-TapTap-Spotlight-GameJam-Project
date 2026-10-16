using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathUtil : MonoBehaviour
{
    public static string PlayerPath = "Prefabs/Player/";
    /// <summary>
    /// 面板路径
    /// </summary>
    public static string PanelPath = "UI/Panel/";
    /// <summary>
    /// 第一章音效路径
    /// </summary>
    public static string SoundPathOfChapter1 = "Music/Chapter1/Sound/";
    /// <summary>
    /// 第一章音乐路径
    /// </summary>
    public static string MusicPathOfChapter1 = "Music/Chapter1/Music/";
    /// <summary>
    /// 第二章音乐路径
    /// </summary>
    public static string MusicPathOfChapter2 = "Music/Chapter2/Music/";
    /// <summary>
    /// 第三章音乐路径
    /// </summary>
    public static string MusicPathOfChapter3 = "Music/Chapter3/Music/";
    /// <summary>
    /// 打印机文本路径
    /// </summary>
    public static string TypewritterTextPath = Application.persistentDataPath + "/TypewritterText.json";
    /// <summary>
    /// 绿藻啊岩浆啊等场景上的物体碰撞文本路径
    /// </summary>
    public static string SceneObjectCollisionTextPath = Application.persistentDataPath + "/SceneObjectCollisionText.json";
    /// <summary>
    /// 角色碰到空气墙时会触发的文本路径
    /// </summary>
    public static string AirwallTriggerTextPath = Application.persistentDataPath + "/AirwallTriggerText.json";
}
