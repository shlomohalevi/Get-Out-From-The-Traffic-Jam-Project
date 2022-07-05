using UnityEngine.SceneManagement;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class WinAndLooseUi : MonoBehaviour
{
    [SerializeField] Vector3 closeWinAndLooseButtonsScaleWindow = default;
    [SerializeField] Ease LooseAndWinWindowEaseType = default;
    [SerializeField] float tweenLooseAndWinDuration = default;
    [SerializeField] Ease moveEase = default, ScaleEase = default;
    [Tooltip("next or continue button to tween depending on the scene")]
    [SerializeField] Button NextLevelbuttonToTween = default;
    [SerializeField] Image[] emojiImages = new Image[2];
    [SerializeField] Text[] Compliments = new Text[3];
    [SerializeField] AudioClip conffetySound = default;
    [Range(0,1)]
    [SerializeField] float audioVolume = 0.5f;
    void Start()
    {
        WinAndLooseButtonsTwining();
        if (SceneManager.GetActiveScene().buildIndex != (int)SceneIndex.winSceneIndex) return;
        PlayConffetSound();
        EmojiImageTwining();
        ShowCompliment();
    }
    void WinAndLooseButtonsTwining()
    {
     NextLevelbuttonToTween.transform.DOScale(closeWinAndLooseButtonsScaleWindow, tweenLooseAndWinDuration).SetEase(LooseAndWinWindowEaseType)
    .SetLoops(-1, LoopType.Restart).SetAutoKill(false);
    }
    void EmojiImageTwining()
    {
        int rnd = Random.Range(0, emojiImages.Length);
        Image emojiToTween = emojiImages[rnd];
        emojiToTween.enabled = true;
        var sequence = DOTween.Sequence();
        sequence.Append( emojiToTween.rectTransform.DOLocalRotate(new Vector3(0, 0, 20), 0.1f).SetEase(moveEase).SetLoops(2, LoopType.Yoyo)).
        Append(emojiToTween.rectTransform.DOLocalMoveY(330, 0.3f).SetEase(moveEase)).
        Append(emojiToTween.rectTransform.DOScale(new Vector3(1.1f, 0.9f, 1), 0.1f).SetEase(ScaleEase)).
        SetLoops(-1, LoopType.Yoyo);
    }
    void ShowCompliment()
    {
        int rnd = Random.Range(0, Compliments.Length);
        Compliments[rnd].enabled = true;
    }
    void PlayConffetSound() => AudioSource.PlayClipAtPoint(conffetySound, Camera.main.transform.position, audioVolume);

    public void LoadLevelsScene() => SceneManager.LoadScene((int)SceneIndex.levelsSceneIndex);
    private void OnDisable()
    {
        DOTween.KillAll();
    }
  
  
        




}
