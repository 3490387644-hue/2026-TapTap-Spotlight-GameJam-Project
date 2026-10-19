using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirthPos : MonoBehaviour
{
    void Awake()
    {
        GameSceneManager.Instance.GeneratePlayer(E_SceneType.GameScene01, transform.localPosition);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
