using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager {

	#region Views

	public delegate void OpenView(Views id,object viewModel = null);
	public static OpenView OnOpenView;

	public delegate void CloseView();
	public static CloseView OnCloseView;

	public delegate void BackButtonPressed();
	public static BackButtonPressed OnBackButtonPressed;

	public static void DoFireOpenViewEvent(Views id,object viewModel = null)
	{
		if (OnOpenView != null)
			OnOpenView (id, viewModel);
	}

	public static void DoFireCloseViewEvent()
	{
		if (OnCloseView != null)
			OnCloseView ();
	}

	public static void DoFireBackButtonEvent()
	{
		if (OnBackButtonPressed != null)
			OnBackButtonPressed ();
	}

	#endregion Views

	#region Settings

	public delegate void ChangeSound();
	public static ChangeSound OnChangeSound;

	public delegate void ChangeVibration();
	public static ChangeVibration OnChangeVibration;

	public static void DoFireChangeSoundEvent()
	{
		if (OnChangeSound != null)
			OnChangeSound ();
	}

	public static void DoFireChangeVibrationEvent()
	{
		if (OnChangeVibration != null)
			OnChangeVibration ();
	}

	#endregion Settings

	#region Game Play Events

	public delegate void StartGame();
	public static StartGame OnStartGame;

	public delegate void PauseGame();
	public static PauseGame OnPauseGame;
    
    public delegate void ShootBubble(ShootModel shootModel);
    public static ShootBubble OnShootBubble;

    public delegate void AllowShoot();
    public static AllowShoot OnAllowShoot;

    public delegate void ShowStreakValue(int value);
    public static ShowStreakValue OnShowStreakValue;

    public delegate void UpdateMergeValue(int value);
    public static UpdateMergeValue OnUpdateMergeValue;

    public delegate void MergeComplete();
    public static MergeComplete OnMergeComplete;

    public delegate void HideLevelUpBadge();
    public static HideLevelUpBadge OnHideLevelUpBadge;

    public delegate void ShakeBoard(bool isExplosionShake);
    public static ShakeBoard OnShakeBoard;

    public delegate void PerfectShoot();
    public static PerfectShoot OnPerfectShoot;

    public static void DoFireStartGameEvent()
	{
		if (OnStartGame != null)
			OnStartGame ();
	}

	public static void DoFirePauseGameEvent()
	{
		if (OnPauseGame != null)
			OnPauseGame ();
	}

    public static void DoFireShootBubbleEvent(ShootModel shootModel)
    {
        if (OnShootBubble != null)
            OnShootBubble(shootModel);
    }

    public static void DoFireAllowShootEvent()
    {
        if (OnAllowShoot != null)
            OnAllowShoot();
    }

    public static void DoFireShowStreakValueEvent(int value)
    {
        if (OnShowStreakValue != null)
            OnShowStreakValue(value);
    }

    public static void DoFireUpdateMergeValueEvent(int value)
    {
        if (OnUpdateMergeValue != null)
            OnUpdateMergeValue(value);
    }

    public static void DoFireMergeCompleteEvent()
    {
        if (OnMergeComplete != null)
            OnMergeComplete();
    }

    public static void DoFireHideLevelUpBadgeEvent()
    {
        if (OnHideLevelUpBadge != null)
            OnHideLevelUpBadge();
    }

    public static void DoFireShakeBoardEvent(bool isExplosionShake)
    {
        if (OnShakeBoard != null)
            OnShakeBoard(isExplosionShake);
    }

    public static void DoFirePerfectShootEvent()
    {
        if (OnPerfectShoot != null)
            OnPerfectShoot();
    }

    #endregion Game Play Events

    #region Confirmation Dialog

    public delegate void NoPressed();
	public static NoPressed OnNoPressed;

	public delegate void YesPressed();
	public static YesPressed OnYesPressed;

	public static void DoFireNoPressedEvent()
	{
		if (OnNoPressed != null)
			OnNoPressed ();	
	}

	public static void DoFireYesPressedEvent()
	{
		if (OnYesPressed != null)
			OnYesPressed ();	
	}

	#endregion Confirmation Dialog

	#region UnAssignEvents

	public static void UnAssignAllEvents()
	{
		OnOpenView = null;
		OnCloseView = null;
		OnBackButtonPressed = null;
		OnChangeSound = null;
		OnStartGame = null;
		OnPauseGame = null;
        OnShootBubble = null;
        OnAllowShoot = null;
    }

    #endregion UnAssignEvents
}
