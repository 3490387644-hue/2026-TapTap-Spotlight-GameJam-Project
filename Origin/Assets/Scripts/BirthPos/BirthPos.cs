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
        PlayMusicBySceneType();
    }

    private void PlayMusicBySceneType()
    {
        GameSceneManager.Instance.PlayMusicByTriggerType(GameSceneManager.Instance.currentMusicTriggerType);
    }

}
