using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : Singleton<SoundController> {

	#region Variables And Properties

	private SoundRefs m_refs;

	private bool soundState = true;

	public bool SoundState
	{
		get
		{
			return soundState;
		}
		set
		{
			soundState = value;
			SaveState ();
		}
	}

	#endregion Variables And Properties

	#region Initialization

	public void Initialize()
	{
        m_refs = GameRefs.Instance.m_soundRefs;
	}

	#endregion Initialization

	#region Save/Load State

	public void SaveState()
	{
		DatabaseManager.SetBool (GameConstants.g_soundState, soundState);
	}

	public void LoadState()
	{
		soundState = DatabaseManager.GetBool (GameConstants.g_soundState, true);
	}

	#endregion Save/Load State

	#region Sfx

	public void PlaySfx(Sfx type, float volume = 1)
	{
        //if (soundState) {
        //	m_refs.sfxAudioSource.volume = volume;
        //	m_refs.sfxAudioSource.PlayOneShot (GetSfxAudioClip (type));
        //}
    }

    private AudioClip GetSfxAudioClip(Sfx type)
	{
		switch (type) {
		case Sfx.ButtonClick:
			return m_refs.m_buttonClick;
		case Sfx.MenuOpen:
			return m_refs.m_menuOpen;
		case Sfx.MenuClose:
			return m_refs.m_menuClose;
		default:
			return m_refs.m_buttonClick;
		}
	}

	public void StopSfx()
	{
        m_refs.m_sfxAudioSource.Stop ();
	}
	#endregion Sfx

}
