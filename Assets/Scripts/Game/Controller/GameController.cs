

public class GameController : Singleton<GameController> {

	#region Variables

	private ViewController m_viewController;
    
	#endregion Variables

	#region Initialization

	public void Initialize()
	{
		AnimationHandler.Initialize ();

        Utility.Initialize();

		GamePlayController.Instance.Initialize ();
		m_viewController = new ViewController ();

		SoundController.Instance.Initialize ();

        GameLoadingComplete();

    }

	private void GameLoadingComplete()
	{
		LoadGameData ();
        ShowMainMenu();
    }
        
    private void LoadGameData()
    {
        GameData.LoadState ();
        PlayerData.LoadState ();
        GamePlayController.Instance.LoadData();
        SoundController.Instance.LoadState ();
    }

	#endregion Initialization

	#region Game Flow Handling

	public void StartGame()
	{
		m_viewController.CloseAllViews ();
        GamePlayController.Instance.GameStart ();
	}

    public void ShowMainMenu()
    {
        EventManager.DoFireOpenViewEvent(Views.MainMenu);
    }

	#endregion Game Flow Handling
}
