using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSce : MonoBehaviour
{
    public E_SceneType sceneType; //要切换的场景类型
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ChangeScene();
            // StartCoroutine(myCoroutine());
        }
    }

    private void ChangeScene()
    {
        if (GameSceneManager.Instance.player != null)
            GameDataMgr.Instance.SaveGameData();
        if (sceneType == E_SceneType.GameScene02)
        {
            GameSceneManager.Instance.LoadScene(sceneType, () =>
            {
                
            });
        }
        else if(sceneType == E_SceneType.GameScene03)
        {
            GameSceneManager.Instance.LoadScene(sceneType, ()=>
            {
                
            });
        }
    }

    IEnumerator myCoroutine()
    {
        // DeadChange.Instance.DarkChange();
        // yield return new WaitForSeconds(1);
        // AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        // op.allowSceneActivation = false; // 可选：加载完不立刻切换
        // while (op.progress < 0.9f)
        // {
        //     yield return null;
        // }
        // op.allowSceneActivation = true;
        // yield return op; // 等待场景切换完成

        yield return null;
    }
}
