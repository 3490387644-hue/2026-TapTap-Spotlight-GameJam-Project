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
    private static JsonMgr instance = new JsonMgr();
    public static JsonMgr Instance => instance;
    private JsonMgr() { }
    public void SaveData(object data, string fileName, JsonType type = JsonType.LitJson)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".json";
        string jsonStr = (type == JsonType.JsonUtlity)
            ? JsonUtility.ToJson(data)
            : JsonMapper.ToJson(data);

        // 先写临时文件，再原子替换，避免写入中途被杀导致坏档
        string tmpPath = path + ".tmp";
        File.WriteAllText(tmpPath, jsonStr);
        if (File.Exists(path))
            File.Replace(tmpPath, path, path + ".bak");   // 替换，旧档留作 .bak 备份
        else
            File.Move(tmpPath, path);
    }

    public T LoadData<T>(string fileName, JsonType type = JsonType.LitJson) where T : new()
    {
        //先判断默认文件夹中有没有这个文件 如果有就从中取
        string path = Application.persistentDataPath + "/" + fileName + ".json";
        //如果默认文件夹没有这个文件 从读写文件夹寻找
        if (!File.Exists(path))
        {
            path = Application.streamingAssetsPath + "/" + fileName + ".json";
        }
        //如果读写文件夹没有这个文件 
        if (!File.Exists(path))
        {
            return new T();
        }
        string jsonStr = File.ReadAllText(path);
        T data = default(T);
        switch (type)
        {
            case JsonType.JsonUtlity:
                data = JsonUtility.FromJson<T>(jsonStr);
                break;
            case JsonType.LitJson:
                data = JsonMapper.ToObject<T>(jsonStr);
                break;
        }
        return data;
    }

    public void ClearAllSaveData()
    {
        string path = Application.persistentDataPath;
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
        Directory.CreateDirectory(path);   // 重建目录，避免后续 File.Copy 抛 DirectoryNotFoundException
        Debug.Log("清除所有存档文件");
    }
}
