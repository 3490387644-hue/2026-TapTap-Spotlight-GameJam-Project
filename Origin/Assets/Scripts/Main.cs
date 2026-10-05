using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 主场景入口
public class Main : SingletonAutoMono<Main>
{
    private Texture2D cursorTex; // 鼠标贴图
    private Vector2 hotSpot = new Vector2(15, 0); // 鼠标热点位置
    public CursorMode cursorMode = CursorMode.Auto; // 鼠标模式

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        cursorTex = ResMgr.Instance.Load<Texture2D>("ArtRes/CursorIcon");
        GameDataMgr.Instance.Init();
        GameDataMgr.Instance.gameData.hasSaveData = true; // 测试用，默认有存档数据
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(cursorTex, hotSpot, cursorMode);
        UIManager.Instance.ShowPanelAsync<MainPanel>((panel) =>
        {
            Debug.Log("MainPanel loaded and shown");
        });
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.ShowPanelAsync<GamePanel>();
        }
    }
}
