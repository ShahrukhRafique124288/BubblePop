using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayController : Singleton<GamePlayController>
{

    #region Variable And Properties
    
    private GamePlayViewController m_gamePlayViewController;
    private GameState m_gameState = GameState.None;

    private AimController m_aimController;
    private BubbleController m_bubbleController;

    private bool m_isLevelUp;

    private WaitForSeconds m_allowShootCRWait;
    private readonly float m_allowShootCRDelay = 1f;

    #endregion Variable And Properties

    #region Initialization

    public void Initialize ()
	{
		InitializeEvents ();

        m_gamePlayViewController = new GamePlayViewController();
        m_aimController = new AimController();
        m_bubbleController = new BubbleController();

        m_allowShootCRWait = new WaitForSeconds(m_allowShootCRDelay);
  	}

	private void InitializeEvents()
	{
        EventManager.OnPauseGame += PauseGame;
        EventManager.OnShootBubble += BubbleShot;
        EventManager.OnAllowShoot += AllowShoot;
        EventManager.OnShowStreakValue += ShowStreakValue;
        EventManager.OnUpdateMergeValue += UpdateMergeValue;
        EventManager.OnMergeComplete += MergeComplete;
        EventManager.OnHideLevelUpBadge += HideLevelUpBadge;
        EventManager.OnShakeBoard += ShakeBoard;
        EventManager.OnPerfectShoot += PerfectShotHit;
    }

	#endregion

	#region Game Event Handling

    public void LoadData()
    {

    }

	public void GameStart()
	{
        if (m_gameState == GameState.Pause)
        {
            ResumeGame();
        }
        else
        {
            AllowShoot();
            m_gamePlayViewController.Open();
            m_bubbleController.StartGame();
        }

        Vibration.Vibrate(TapticPlugin.ImpactFeedback.Medium);
    }

	private void PauseGame()
	{
        m_gameState = GameState.Pause;
        EventManager.DoFireOpenViewEvent (Views.MainMenu);
        m_aimController.PauseGame();
    }

	private void ResumeGame()
	{
        AllowShoot();
    }

    #endregion Game Event Handling

    #region Gameplay Flow

    private void Update()
	{
		if(m_gameState == GameState.Start)
		{
            m_aimController.AimToShoot();
		}
	}

    #endregion Gameplay Flow

    #region Bubble Shoot

    private void BubbleShot(ShootModel shootModel)
    {
        m_gameState = GameState.Shoot;
        m_bubbleController.BubbleShot(shootModel);
    }

    private void AllowShoot()
    {
        CoRoutineRunner.Instance.StartCoroutine(AllowShootCR());
    }

    private IEnumerator AllowShootCR()
    {
        yield return m_allowShootCRWait;

        m_gameState = GameState.Start;
    }

    #endregion Bubble Shoot

    #region Score & Leveling

    private void ShowStreakValue(int value)
    {
        m_gamePlayViewController.ShowStreakValue(value);
    }

    private void UpdateMergeValue(int value)
    {
        PlayerData.Score += value;
        GameplayData.g_Score += value;

        float fillAmount = (float)(GameplayData.g_Score / GameData.ScoreToLevelUp);

        m_isLevelUp = fillAmount >= 0.99f;

        m_gamePlayViewController.UpdateMergeValue(fillAmount);
    }

    private void MergeComplete()
    {
        if (m_isLevelUp)
        {
            m_isLevelUp = false;
            m_gameState = GameState.Pause;
            GameplayData.g_Score = 0;
            PlayerData.CurrentLevel++;
            GameData.ScoreToLevelUp += GameConstants.g_levelUpScore;
            GameRefs.Instance.m_starConfetti.Play();
            m_gamePlayViewController.LevelUp();
        }
        else
            AllowShoot();        
    }

    private void HideLevelUpBadge()
    {
        m_gamePlayViewController.HideLevelUpBadge();
        AllowShoot();
    }

    private void PerfectShotHit()
    {
        m_gamePlayViewController.ShowPerfectNotification();
    }

    #endregion Score & Leveling

    #region Shake

    private void ShakeBoard(bool isExplosionShake)
    {
        if (isExplosionShake)
            m_gamePlayViewController.ShakeBoard(0.5f, 8);
        else
            m_gamePlayViewController.ShakeBoard();        
    }

    #endregion Shake

    #region Slot Check

    public bool IsGridSlotAvailable(int index)
    {
        return m_bubbleController.IsGridSlotAvailable(index);
    }
    
    public bool HasRoof(int index)
    {
        return m_bubbleController.HasRoof(index);
    }

    #endregion Slot Check
}
