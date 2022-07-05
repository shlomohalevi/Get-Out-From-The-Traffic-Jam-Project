using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
/// <summary>
/// responsible for game settings
/// </summary>

public class Setting : MonoBehaviour
{
    static readonly string lastUnlockedButtonIndexKey = "lastUnlockedButtonIndexKey";
    [SerializeField] Image settingWindow = default;
    [SerializeField] float tweenSettingDuration = default;
    [SerializeField] Vector3 CloseSettingWindowScaleValue = default, OpenSettingWindowScaleValue = default;
    [SerializeField] Ease  SettingWindowEaseType = default;
    [SerializeField] Image xMuteImage = default;
    [SerializeField] Text LevelsText = default;
    [SerializeField] Transform levelsButtonsParent = default;
    Button[] levelsButtons = new Button[40];
    bool isSettingWindowOpen = false;

  
    private void OnEnable()
    {
        GetAudioPrefs();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
   
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnApplicationQuit()
    {
        SetAudioPrefs();
    }
        
    private void Start()
    {
        LimitFramrate();
        InithilizeLevelsButtonsArray();
        UnlockButtonsLevelsWeAlreadyPassed();
    }
   

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
      
        if (scene.buildIndex != (int)SceneIndex.winSceneIndex) return;//dont keep process if we are not in win scene
        UnlockHigherButtonLevel();
        LoadNextLevel();
    }
    public void OpenSettingWindow()
    {
        if (isSettingWindowOpen) return;
        settingWindow.transform.gameObject.SetActive(true);
        settingWindow.transform.DOScale(OpenSettingWindowScaleValue, tweenSettingDuration).SetEase(SettingWindowEaseType);
    }
    public void CloseSettingWindow()
    {
        isSettingWindowOpen = false;
        settingWindow.transform.DOScale(CloseSettingWindowScaleValue, tweenSettingDuration).SetEase(SettingWindowEaseType)
        .OnComplete(()=> settingWindow.transform.gameObject.SetActive(false));
    }

    public void MuteAndUnmuteAudio()
    {
        AudioListener.pause = !AudioListener.pause;
        bool activateXmuteImage = AudioListener.pause == true;
        xMuteImage.gameObject.SetActive(activateXmuteImage);
        PlayerPrefs.SetInt("audioIsPaused", AudioListener.pause ? 0 : 1);
    }
    void GetAudioPrefs()
    {
        bool audioIsPaused = PlayerPrefs.GetInt("audioIsPaused",1) == 0;
        AudioListener.pause = audioIsPaused;
        xMuteImage.gameObject.SetActive(audioIsPaused);
    }
    void SetAudioPrefs()
    {
        PlayerPrefs.SetInt("audioIsPaused", AudioListener.pause ? 0 : 1);
    }
    public void QuitGame() => Application.Quit();
    
    void InithilizeLevelsButtonsArray()
    {
        if (levelsButtons[0] != null) return;
            for (int i = 0; i < levelsButtonsParent.childCount; i++)
                levelsButtons[i] = levelsButtonsParent.GetChild(i).GetComponent<Button>();
    }
   
   
    /// <summary>
    /// unlock saved levels button we alredy passed 
    /// </summary>
    void UnlockButtonsLevelsWeAlreadyPassed()
    {
        int numOfLevelsToUnlock = PlayerPrefs.GetInt(lastUnlockedButtonIndexKey, 0);
        for (int i = 1; i <= numOfLevelsToUnlock; i++)
        UnlockButtonLevel(i);
    }
    /// <summary>
    /// unlock higher button level when we passing level
    /// </summary>
    void UnlockHigherButtonLevel()
    {
        int lastUnlockedButtonIndex = PlayerPrefs.GetInt(lastUnlockedButtonIndexKey, 0);
        int lastPlayedLevelIndex = PlayerPrefs.GetInt(GenerateLevels.LastLevelTheUserPlayedKey);
        if (lastUnlockedButtonIndex == lastPlayedLevelIndex)//unlock higher button level only if we played higher unlocked possible level
        {
            int nextButtonToUnlock = lastUnlockedButtonIndex + 1;
            PlayerPrefs.SetInt(lastUnlockedButtonIndexKey, nextButtonToUnlock);
            //unlock higher button level
            UnlockButtonLevel(nextButtonToUnlock);
        }
    }
    /// <summary>
    /// next level to load after we successfully passed the last level
    /// </summary>
    public void LoadNextLevel()
    {
        int LastLevelTheUserPassed = PlayerPrefs.GetInt(GenerateLevels.LastLevelTheUserPlayedKey);
        int nextLevelToPlay = (int)Mathf.Repeat(LastLevelTheUserPassed + 1, 40);
        PlayerPrefs.SetInt(GenerateLevels.LastLevelTheUserPlayedKey, nextLevelToPlay);
    }
    /// <summary>
    /// unlock particular button 
    /// </summary>
    void UnlockButtonLevel(int levelButtonIndexToUnlock)
    {
        Button levelButtonToUnlock = levelsButtons[levelButtonIndexToUnlock];
        Text numOfLevelButtonText = levelButtonToUnlock.transform.GetChild(0).GetComponent<Text>();
        Image lockButtonImage = levelButtonToUnlock.transform.GetChild(1).GetComponent<Image>();
        levelButtonToUnlock.interactable = true;
        lockButtonImage.enabled = false;
        numOfLevelButtonText.text = $"{levelButtonIndexToUnlock + 1}";
    }
    /// <summary>
    /// load new level depends on the button level we prees
    /// </summary>
    public void LoadNewLevelButton()
    {
        int levelIndexToLoad = EventSystem.current.currentSelectedGameObject.transform.GetSiblingIndex();
        PlayerPrefs.SetInt(GenerateLevels.LastLevelTheUserPlayedKey, levelIndexToLoad);
        settingWindow.gameObject.SetActive(false);
        SceneManager.LoadScene(0);
    }
    void LimitFramrate()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }
}
    public enum SceneIndex
    {
        levelsSceneIndex,
        winSceneIndex,
        looseSceneIndex,
    }

   



















  





