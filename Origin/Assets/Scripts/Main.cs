using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 主场景入口
public class Main : SingletonAutoMono<Main>
{
    private Texture2D cursorTex; // 鼠标贴图
    private Vector2 hotSpot = new Vector2(15, 0); // 鼠标热点位置
    public CursorMode cursorMode = CursorMode.Auto; // 鼠标模式

    private static bool hasInstance = false; // 单例去重标记，防止场景切换产生多个 Main

    void Awake()
    {
        if (hasInstance)
        {
            Destroy(this.gameObject); // 已存在 Main 实例时销毁重复的
            return;
        }
        hasInstance = true;
        DontDestroyOnLoad(this.gameObject);
        cursorTex = ResMgr.Instance.Load<Texture2D>("ArtRes/UI/CursorIcon");
        // GameDataMgr.Instance.SaveGameData(new Vector3(0, 0, 0), 1f, 1f, E_SceneType.BeginScene); // 测试用
        // GameDataMgr.Instance.Init();
        // GameDataMgr.Instance.gameData.hasSaveData = false; // 测试用
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(cursorTex, hotSpot, cursorMode);
    }

    // 注释的地方是定时自动存档的功能，暂时不需要，先注释掉
    // private float autoSaveTimer = 0f;
    // private const float AutoSaveInterval = 60f;   // 每隔一段时间存一次，按需调

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameSceneManager.Instance.currentSceneType != E_SceneType.BeginScene && GameSceneManager.Instance.currentSceneType != E_SceneType.LoadingScene)
        {
            UIManager.Instance.ShowPanelAsync<GamePanel>();
        }

        // autoSaveTimer += Time.deltaTime;
        // if (autoSaveTimer >= AutoSaveInterval)
        // {
        //     autoSaveTimer = 0f;
        //     TrySave();
        // }
    }

    private void OnApplicationPause(bool paused)   // 移动端切后台
    {
        if (paused) TrySave();
    }

    private void OnApplicationFocus(bool focused)  // 桌面端失焦（alt-tab、点其它窗口）
    {
        if (!focused) TrySave();
    }

    private void OnApplicationQuit()               // 正常退出
    {
        TrySave();
    }

    private void TrySave()
    {
        // 只在游戏进行中（有玩家）才存，菜单里玩家为 null，避免空指针
        if (GameSceneManager.Instance == null || GameSceneManager.Instance.player == null)
            return;
        GameDataMgr.Instance.SaveGameData();
    }
}
