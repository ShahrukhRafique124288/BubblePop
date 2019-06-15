using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsController : BaseViewController {

	#region Variables

	private SettingsViewController m_settingsViewController;

	private bool m_isSoundOn = true;
	private bool m_isVibrationOn = true;

	#endregion Variables

	#region Life Cycle Methods

	public SettingsController()
	{
		LoadState ();
        m_settingsViewController = new SettingsViewController ();
	}

	public void Open(GameObject obj, object viewModel = null)
	{
		InitializeEvents ();
        m_settingsViewController.Open (obj);
		UpdateSoundView ();
		UpdateVibrationView ();
	}

	public void Close()
	{
		UnInitializeEvents ();
        m_settingsViewController.Close ();
	}

	private void InitializeEvents()
	{
		EventManager.OnChangeSound += OnChangeSound;
		EventManager.OnChangeVibration += OnChangeVibration;
	}

	private void UnInitializeEvents()
	{
		EventManager.OnChangeSound -= OnChangeSound;
		EventManager.OnChangeVibration -= OnChangeVibration;
	}

	#endregion Life Cycle Methods

	#region Save/Load State

	public void LoadState()
	{
		m_isSoundOn = SoundController.Instance.SoundState;
		m_isVibrationOn = GameData.IsVibrationOn;
	}

	public void SaveState()
	{
		SoundController.Instance.SoundState = m_isSoundOn;
		GameData.IsVibrationOn = m_isVibrationOn;
	}

	#endregion Save/Load State

	#region Event Call Back

	private void OnChangeSound()
	{
        m_isSoundOn = !m_isSoundOn;
		UpdateSoundView ();
		SaveState ();
	}

	private void OnChangeVibration()
	{
        m_isVibrationOn = !m_isVibrationOn;
		UpdateVibrationView ();
		SaveState ();
	}

	#endregion Event Call Back

	#region View Handling

	private void UpdateSoundView()
	{
        m_settingsViewController.UpdateSoundView (m_isSoundOn);
	}

	private void UpdateVibrationView()
	{
        m_settingsViewController.UpdateVibrationView (m_isVibrationOn);
	}

	#endregion View Handling
}
