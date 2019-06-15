using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SettingsViewController : BaseViewController {

    #region Variables

    private SettingsRefs m_refs;

	#endregion Variables

	#region Life Cycle Methods

	public void Open(GameObject obj, object viewModel = null)
	{
		if (m_refs == null) {
            m_refs = obj.GetComponent<SettingsRefs> ();
			SetTexts ();
		}
        m_refs.m_closeButton.interactable = true;
		SoundController.Instance.PlaySfx (Sfx.MenuOpen);
		AnimationHandler.SlideDown (m_refs.m_dialogContainer, 0.15f);
		SetState (true);
	}

	public void Close()
	{
        m_refs.m_closeButton.interactable = false;
		SoundController.Instance.PlaySfx (Sfx.MenuClose);
        AnimationHandler.SlideBackUp(m_refs.m_dialogContainer, SlideComplete, 0.15f);
	}

	#endregion #region Life Cycle Methods

	#region Texts Initialization

	private void SetTexts()
	{
        m_refs.m_headerText.text = TextConstants.SettingsHeader.ToUpper ();
        m_refs.m_soundText.text = TextConstants.Sound.ToUpper ();
        m_refs.m_vibrationText.text = TextConstants.Vibration.ToUpper ();
	}

	#endregion Texts Initialization

	#region State Handling

	private void SetState(bool state)
	{
        m_refs.gameObject.SetActive (state);
	}

	#endregion State Handling

	#region View Handling

	public void UpdateSoundView(bool soundState)
	{
        m_refs.m_soundImage.sprite = soundState ? m_refs.m_soundOnSprite : m_refs.m_soundOffSprite;
        m_refs.m_soundText.color = soundState ? Color.white : Color.gray;
    }

	public void UpdateVibrationView(bool vibration)
	{
        m_refs.m_vibrationImage.sprite = vibration ? m_refs.m_vibrationOnSprite : m_refs.m_vibrationOffSprite;
        m_refs.m_vibrationText.color = vibration ? Color.white : Color.gray;
    }

	#endregion View Handling

	#region Tween Call Back

	private void SlideComplete()
	{
        SetState(false);
    }

	#endregion Tween Call Back
}
