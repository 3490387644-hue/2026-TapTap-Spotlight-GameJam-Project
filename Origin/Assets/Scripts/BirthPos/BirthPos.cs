using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirthPos : MonoBehaviour
{
    void Awake()
    {
        GameSceneManager.Instance.GeneratePlayer(E_SceneType.GameScene1, transform.localPosition);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
