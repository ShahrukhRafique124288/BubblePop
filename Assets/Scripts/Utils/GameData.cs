using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData {

	#region Properties And Variables

	private static bool isVibrationOn;

    private static double scoreToLevelUp;

    public static bool IsVibrationOn
    {
        get
        { 
            return isVibrationOn;
        }

        set
        {
            isVibrationOn = value;
            SaveState ();
        }
    }

    public static double ScoreToLevelUp
    {
        get
        {
            return scoreToLevelUp;
        }

        set
        {
            scoreToLevelUp = value;
            SaveState();
        }
    }

    #endregion Properties And Variables

    #region Load/Save State

    public static void LoadState()
	{
		isVibrationOn = DatabaseManager.GetBool (GameConstants.g_isVibrationOn, true);
        scoreToLevelUp = DatabaseManager.GetDouble(GameConstants.g_scoreToLevelUp, GameConstants.g_levelUpScore);
    }

    public static void SaveState()
    {
        DatabaseManager.SetBool(GameConstants.g_isVibrationOn, isVibrationOn);
        DatabaseManager.SetDouble(GameConstants.g_scoreToLevelUp, scoreToLevelUp);
    }

	#endregion Load/Save State
}
