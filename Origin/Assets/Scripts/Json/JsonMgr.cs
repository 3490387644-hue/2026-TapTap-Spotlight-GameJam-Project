using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public enum JsonType
{
    JsonUtlity,
    LitJson,
}

public class JsonMgr
{
    private static JsonMgr instance=new JsonMgr();
    public static JsonMgr Instance=>instance;
    private JsonMgr() { }
    public void SaveData(object data,string fileName,JsonType type=JsonType.LitJson)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".json";
        string jsonStr = "";
        switch(type)
        {
            case JsonType.JsonUtlity:
                jsonStr=JsonUtility.ToJson(data);
                break;
            case JsonType.LitJson:
                jsonStr=JsonMapper.ToJson(data);
                break;
        } 
        File.WriteAllText(path, jsonStr);
    }

    public T LoadData<T>(string fileName, JsonType type = JsonType.LitJson)where T : new()
    {
        //先判断默认文件夹中有没有这个文件 如果有就从中取
        string path = Application.streamingAssetsPath + "/" + fileName + ".json";
        //如果默认文件夹没有这个文件 从读写文件夹寻找
        if(!File.Exists(path))
        {
            path = Application.persistentDataPath + "/" + fileName + ".json";
        }
        //如果读写文件夹没有这个文件 
        if (!File.Exists(path))
        {
            return new T();
        }
        string jsonStr = File.ReadAllText(path);
        T data = default(T);
        switch(type)
        {
            case JsonType.JsonUtlity:
                data=JsonUtility.FromJson<T>(jsonStr);
                break;
            case JsonType.LitJson:
                data=JsonMapper.ToObject<T>(jsonStr);
                break;
        }
        return data;
    }
}
