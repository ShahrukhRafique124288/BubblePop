using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : BaseViewController
{
    #region Variables

    private MainMenuViewController m_mainMenuViewController;

    #endregion

    #region Life Cycle Methods

    public MainMenuController()
    {
        m_mainMenuViewController = new MainMenuViewController();
    }

    public void Open(GameObject obj, object viewModel = null)
    {
        m_mainMenuViewController.Open(obj);
    }

    public void Close()
    {
        m_mainMenuViewController.Close();
    }

    #endregion

}
