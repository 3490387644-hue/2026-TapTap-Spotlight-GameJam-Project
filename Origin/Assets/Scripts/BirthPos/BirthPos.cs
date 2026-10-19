using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirthPos : MonoBehaviour
{
    public E_SceneType sceneType; //出生点所在的场景类型
    void Awake()
    {
        GameSceneManager.Instance.GeneratePlayer(sceneType, transform.position);
    }

    void Start()
    {
        PlayMusicBySceneType(sceneType);
    }

    private void PlayMusicBySceneType(E_SceneType sceneType)
    {
        if (sceneType == E_SceneType.GameScene01)
            GameSceneManager.Instance.PlayMusicByTriggerType(E_MusicTriggerType.Chapter1_Level1);
        else if (sceneType == E_SceneType.GameScene02)
            GameSceneManager.Instance.PlayMusicByTriggerType(E_MusicTriggerType.Chapter2_Level1);
        else if (sceneType == E_SceneType.GameScene03)
            GameSceneManager.Instance.PlayMusicByTriggerType(E_MusicTriggerType.Chapter3_Level1);
        else if(sceneType == E_SceneType.GameScene04)
            GameSceneManager.Instance.PlayMusicByTriggerType(E_MusicTriggerType.Chapter4_Level1);
        
        Debug.Log("BirthPos:根据出生点所在的场景类型播放音乐，当前场景类型为：" + sceneType);
    }
}
