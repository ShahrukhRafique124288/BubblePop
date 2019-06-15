using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationController : Singleton<ApplicationController> {

	#region Variables And Properties

	private bool m_isGameLoaded = false;

	#endregion Variables And Properties

	#region Life Cyle Methods

	private void Awake()
	{
        Application.targetFrameRate = 60;
        LoadGame();
	}

	private void OnApplicationQuit()
	{
		AppInBackGroundState ();
	}

	private void OnApplicationPause(bool isPaused)
	{
		if (isPaused)
			AppInBackGroundState ();
		else
			ResumeApplication ();
	}

	private void AppInBackGroundState()
	{
        if (m_isGameLoaded)
        {
         
        }
	}

	private void ResumeApplication()
	{
        if (m_isGameLoaded)
        {
        
        }
	}

	#endregion Life Cyle Methods

	#region Game Load

	private void LoadGame()
	{
		GameController.Instance.Initialize ();
        m_isGameLoaded = true;
	}

	#endregion Game Load

	//#if UNITY_ANDROID

	//private void Update()
	//{
	//	if (Input.GetKeyDown (KeyCode.Escape) && isGameLoaded) {
	//		EventManager.DoFireBackButtonEvent ();
	//	}
	//}

	//#endif
}
