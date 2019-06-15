using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewController {

	#region Variables

	private readonly ViewRefs m_refs;

    private Dictionary<Views, RectTransform> m_viewDict;
	private Stack<BaseViewController> m_viewStack;

    private MainMenuController m_mainMenuController;
    private SettingsController m_settingsController;

	#endregion Variables

	#region Initialization

	public ViewController()
	{
		m_refs = GameRefs.Instance.m_viewRefs;
		Initialize ();
	}

	private void Initialize()
	{
		m_viewDict = new Dictionary<Views,RectTransform> ();
		m_viewStack = new Stack<BaseViewController> ();

		InitializeEvents ();
	}

	private void InitializeEvents()
	{
		EventManager.OnOpenView += OnOpenView;
		EventManager.OnCloseView += OnCloseView;
		EventManager.OnBackButtonPressed += BackButtonPressed;
	}

	#endregion Initialization

	#region Create/Open/Close

	private void OnOpenView(Views id, object viewModel = null)
	{
		RectTransform view;
        
        if (m_viewDict.ContainsKey(id))
            view = m_viewDict[id];
        else
        {
            view = (Object.Instantiate<RectTransform>(Resources.Load<RectTransform>("Views/" + id.ToString())));
            view.gameObject.SetActive(false);
            view.transform.SetParent(m_refs.m_viewContainer);
            view.localPosition = Vector3.zero;
            view.transform.localScale = Vector3.one;
            view.offsetMin = view.offsetMax = Vector2.zero;
            m_viewDict.Add(id, view);
        }
        
		view.SetAsLastSibling ();
		BaseViewController baseController = GetController (id);
		baseController.Open (view.gameObject, viewModel);
		m_viewStack.Push (baseController);
	}

	private BaseViewController GetController(Views id)
	{
        switch (id)
        {
            case Views.Settings:
                if (m_settingsController == null)
                    m_settingsController = new SettingsController();
                return m_settingsController;
            case Views.MainMenu:
                if (m_mainMenuController == null)
                    m_mainMenuController = new MainMenuController();
                return m_mainMenuController;
            default:
                if (m_settingsController == null)
                    m_settingsController = new SettingsController();
                return m_settingsController;
        }
	}

	private void OnCloseView()
	{
		if (m_viewStack.Count != 0) {
			BaseViewController baseController = m_viewStack.Pop ();
			baseController.Close ();
		}
	}

	public void CloseAllViews()
	{
        while (m_viewStack.Count != 0)
        {
            OnCloseView();
        }
	}

	public void OpenView(Views id)
	{
		OnOpenView (id);
	}

	private void BackButtonPressed()
	{
        if (m_viewStack.Count == 0)
        {
            EventManager.DoFirePauseGameEvent();
        }
        else
        {
            OnCloseView();
        }
	}

	#endregion Create/Open/Close

}