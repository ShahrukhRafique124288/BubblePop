using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsEventHandler : MonoBehaviour {

	public void ChangeSoundState()
	{
		EventManager.DoFireChangeSoundEvent ();
		SoundController.Instance.PlaySfx (Sfx.ButtonClick);
	}

	public void ChangeVibrationState()
	{
		SoundController.Instance.PlaySfx (Sfx.ButtonClick);
		EventManager.DoFireChangeVibrationEvent ();
	}

	public void CloseSettings()
	{
		EventManager.DoFireCloseViewEvent ();
	}
}
