using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirthPos : MonoBehaviour
{
    public E_SceneType sceneType; //出生点所在的场景类型
    void Awake()
    {
        GameSceneManager.Instance.GeneratePlayer(sceneType, transform.localPosition);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
