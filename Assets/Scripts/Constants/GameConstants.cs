
public static class GameConstants
{
    #region PlayerPrefs Ids

    public const int g_kEncyptionStringKey = 910867680;
    
    public const string g_isVibrationOn = "isVibrationOn";
    public const string g_scoreToLevelUp = "scoreToLevelUp";

    public const string g_currentLevel = "currentLevel";
    public const string g_score = "score";

    public const string g_soundState = "SoundState";

    #endregion PlayerPrefs Ids

    #region Gameplay

    public const string g_leftWallTag = "LeftWall";
    public const string g_rightWallTag = "RightWall";
    
    public const float g_xSpawnDelta = 65;
    public const float g_ySpawnDelta = 55;
    public const float g_spawnDeltaOffset = 32.5f;
    
    public const int g_maxRows = 8;
    public const int g_maxColumns = 6;
    
    public const int g_rowsToStart = 4;
    
    public const int g_minRowsOnScreen = 6;

    public const int g_bubbleBlastValue = 2048;

    public const int g_levelUpScore = 5000;

    public readonly static int[] g_bubbleValues = { 4, 8, 16, 32, 64, 128 };

    #endregion Gameplay
}
