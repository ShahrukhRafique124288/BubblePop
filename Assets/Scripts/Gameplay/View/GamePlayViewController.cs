using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GamePlayViewController : BaseViewController
{
    #region Variables

    private GameplayViewRefs m_gameplayViewRefs;
    private Sequence m_multiplierTextSeq;

    #endregion Variables

    #region Life Cycle Methods

    public GamePlayViewController()
    {
        m_gameplayViewRefs = GameRefs.Instance.m_gameplayViewRefs;

        m_multiplierTextSeq = DOTween.Sequence();
    }

    public void Open(GameObject obj = null, object viewModel = null)
    {
        SetLevelData();
        SetState(true);
    }

    public void Close()
    {
        SetState(false);
    }

    #endregion Life Cycle Methods

    #region State Handling

    private void SetState(bool state)
    {
        m_gameplayViewRefs.gameObject.SetActive(state);
    }

    #endregion State Handling

    #region Level & Score Handling

    private void SetLevelData()
    {
        int currentLevelColor = PlayerData.CurrentLevel;
        int nextLevel = currentLevelColor + 1;

        m_gameplayViewRefs.m_currentLevelBG.color = ColorsData.Instance.m_uiColors[(currentLevelColor - 1) % ColorsData.Instance.m_uiColors.Length];
        m_gameplayViewRefs.m_nextLevelBG.color = ColorsData.Instance.m_uiColors[(nextLevel - 1) % ColorsData.Instance.m_uiColors.Length];
        m_gameplayViewRefs.m_levelFillBar.color = ColorsData.Instance.m_uiColors[(currentLevelColor - 1) % ColorsData.Instance.m_uiColors.Length];


        m_gameplayViewRefs.m_currentLevelText.text = currentLevelColor.ToString();
        m_gameplayViewRefs.m_nextLevelText.text = nextLevel.ToString();

        UpdateMergeValue(0);
    }

    private void UpdateFillBar(float fillAmount)
    {
        m_gameplayViewRefs.m_levelFillBar.DOFillAmount(fillAmount, 0.3f).SetEase(Ease.Linear);
    }

    private void UpdateScore()
    {
        string score = PlayerData.Score.ToString();

        if (PlayerData.Score > 10000)
            score = PlayerData.Score / 1000 + "k";

        m_gameplayViewRefs.m_ScoreText.text = score;
    }

    private void ShowLevelUpBadge()
    {
        m_gameplayViewRefs.m_levelBadgeText.text = TextConstants.LevelCompleteBadge[Random.Range(0, TextConstants.LevelCompleteBadge.Length)].ToUpper();
        m_gameplayViewRefs.m_levelUpBadge.SetActive(true);
        AnimationHandler.PlayPunchAnimation(m_gameplayViewRefs.m_levelUpBadge.transform, 0.15f, 0.075f, PopInComplete);
    }

    public void HideLevelUpBadge()
    {
        m_gameplayViewRefs.m_levelBadgeCross.interactable = false;
        AnimationHandler.PlayPunchAnimation(m_gameplayViewRefs.m_levelUpBadge.transform, 0.15f, 0.075f, PopOutComplete);
    }

    public void ShowStreakValue(int multiplierValue)
    {
        m_multiplierTextSeq.Kill();
        m_multiplierTextSeq = DOTween.Sequence();

        Color temp = m_gameplayViewRefs.m_streakText.color;
        temp.a = 0;
        m_gameplayViewRefs.m_streakText.color = temp;

        m_gameplayViewRefs.m_streakText.transform.localScale = Vector3.zero;

        m_gameplayViewRefs.m_streakText.text = multiplierValue + "X";

        m_multiplierTextSeq.Append(m_gameplayViewRefs.m_streakText.transform.DOScale(0.5f, 0.8f).SetEase(Ease.OutBack))
            .Join(m_gameplayViewRefs.m_streakText.DOFade(1, 0.6f).SetEase(Ease.Linear))
            .Insert(0.85f, m_gameplayViewRefs.m_streakText.DOFade(0, 0.2f).SetEase(Ease.Linear))
            .Play();
    }

    public void UpdateMergeValue(float fillAmount)
    {
        UpdateFillBar(fillAmount);
        UpdateScore();
    }

    public void LevelUp()
    {
        SetLevelData();
        ShowLevelUpBadge();
    }

    #endregion Level & Score Handling

    #region Perfect

    public void ShowPerfectNotification()
    {
        AnimationHandler.PlayShakeAnim(m_gameplayViewRefs.m_perfectImage.transform);

        DOTween.Sequence().Append(m_gameplayViewRefs.m_perfectImage.DOFade(1, 0.2f).SetEase(Ease.Linear))
           .Insert(1.3f, m_gameplayViewRefs.m_perfectImage.DOFade(0, 0.2f).SetEase(Ease.Linear));
    }

    #endregion Perfect

    #region Tween Callback

    private void PopInComplete()
    {
        m_gameplayViewRefs.m_levelBadgeCross.interactable = true;
    }

    private void PopOutComplete()
    {
        m_gameplayViewRefs.m_levelUpBadge.SetActive(false);
    }

    #endregion Tween Callback

    #region Shake

    public void ShakeBoard(float duration = 0.15f, float strength = 3)
    {
        m_gameplayViewRefs.m_bubbleContainer.DOShakePosition(duration, strength, 10, 90);
    }

    #endregion Shake
}
