using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    public Text musicVolumeText;
    public Text soundVolumeText;
    public Button musicUpButton;
    public Button musicDownButton;
    public Button soundUpButton;
    public Button soundDownButton;
    public Button backButton;
    public Dropdown resolutionDropdown;
    private int musicVolume;
    private int soundVolume;
    private readonly Vector2Int[] resolutions =
    {
        new Vector2Int(2560, 1440),
        new Vector2Int(1920, 1080),
        new Vector2Int(1600, 900)
    };
    public override void Init()
    {
        // 初始化设置面板的内容
        Debug.Log("SettingPanel initialized.");

        UpdateInfo();
        InitResolutionDropdown();

        backButton.onClick.AddListener(()=>
        {
            UIManager.Instance.ShowPanelAsync<MainPanel>();
            UIManager.Instance.HidePanel<SettingPanel>();
        });

        // 设置按钮的点击事件
        musicUpButton.onClick.AddListener(()=>
        {
            Debug.Log("Music volume up button clicked.");
            // 告诉MusicMgr增加音量
            
            // 更新musicVolumeText的显示
            musicVolume++;
            if(musicVolume > 10)
                musicVolume = 10;
            musicVolumeText.text = "音量：" + musicVolume.ToString();
        });

        musicDownButton.onClick.AddListener(()=>
        {
            Debug.Log("Music volume down button clicked.");
            musicVolume--;
            if(musicVolume < 0)
                musicVolume = 0;
            musicVolumeText.text = "音量：" + musicVolume.ToString();
        });

        soundUpButton.onClick.AddListener(()=>
        {
            Debug.Log("Sound volume up button clicked.");
            soundVolume++;
            if(soundVolume > 10)
                soundVolume = 10;
            soundVolumeText.text = "音效：" + soundVolume.ToString();
        });

        soundDownButton.onClick.AddListener(()=>
        {
            Debug.Log("Sound volume down button clicked.");
            soundVolume--;
            if(soundVolume < 0)
                soundVolume = 0;
            soundVolumeText.text = "音效：" + soundVolume.ToString();
        });
    }

    /// <summary>
    /// 打开设置面板时需要更新面板信息
    /// </summary>
    private void UpdateInfo()
    {
        // 暂时先这么写
        musicVolume = 5;
        musicVolumeText.text = "音量：" + musicVolume.ToString();
        soundVolume = 5;
        soundVolumeText.text = "音效：" + soundVolume.ToString();
        // 后续从GameDataMgr.Instance.gameData中获取音量和音效的设置
        
    }

    private void InitResolutionDropdown()
    {
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>
        {
            "画面：2560*1440",
            "画面：1920*1080",
            "画面：1600*900"
        };

        resolutionDropdown.AddOptions(options);

        int currentIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (Screen.width == resolutions[i].x &&
                Screen.height == resolutions[i].y)
            {
                currentIndex = i;
                break;
            }
        }

        resolutionDropdown.SetValueWithoutNotify(currentIndex);
        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
    }

    private void OnResolutionChanged(int index)
    {
        if (index < 0 || index >= resolutions.Length)
        {
            return;
        }

        Debug.Log($"Resolution changed to: {resolutions[index].x}x{resolutions[index].y}");

        Vector2Int resolution = resolutions[index];

        Screen.SetResolution(
            resolution.x,
            resolution.y,
            Screen.fullScreenMode);
    }
}
