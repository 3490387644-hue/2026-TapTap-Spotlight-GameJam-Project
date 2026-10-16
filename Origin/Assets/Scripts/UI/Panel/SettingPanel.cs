using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum E_SettingSource
{
    MainPanel,
    GamePanel
}

public class SettingPanel : BasePanel
{
    public Button backButton;
    public Dropdown resolutionDropdown;
    public Slider musicVolumeSlider;
    public Slider soundVolumeSlider;
    private int musicVolume;
    private int soundVolume;
    private E_SettingSource source;
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

        UpdateVolumeInfo();

        InitResolutionDropdown();

        backButton.onClick.AddListener(()=>
        {
            UIManager.Instance.HidePanel<SettingPanel>();
            switch(source)
            {
                case E_SettingSource.MainPanel:
                    UIManager.Instance.ShowPanelAsync<MainPanel>();
                    break;
                case E_SettingSource.GamePanel:
                    UIManager.Instance.ShowPanelAsync<GamePanel>();
                    break;
                default:
                    Debug.LogWarning("找不到设置面板的来源");
                    break;
            }
        });

        musicVolumeSlider.onValueChanged.AddListener((value) =>
        {
            MusicMgr.Instance.ChangeBKMusicValue(value);
        });

        soundVolumeSlider.onValueChanged.AddListener((value) =>
        {
            MusicMgr.Instance.ChangeSoundValue(value);
        });
    }

    private void UpdateVolumeInfo()
    {
        musicVolumeSlider.value = MusicMgr.Instance.GetBkMusicValue;
        soundVolumeSlider.value = MusicMgr.Instance.GetSoundValue;
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
            FullScreenMode.Windowed);
    }

    public void SetSource(E_SettingSource source)
    {
        this.source = source;
    }
}
