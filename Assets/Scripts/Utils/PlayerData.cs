using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData {

	#region Properties And Variables

    private static int currentLevel;

    private static double score;

    public static int CurrentLevel
    {
        get
        {
            return currentLevel;
        }
        set
        {
            currentLevel = value;
            SaveState();
        }
    }

    public static double Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            SaveState();
        }
    }


    #endregion Properties And Variables

    #region Load/Save State

    public static void LoadState()
	{
        currentLevel = DatabaseManager.GetInt(GameConstants.g_currentLevel, 1);
        score = DatabaseManager.GetDouble(GameConstants.g_score, 0);
    }

	public static void SaveState()
	{
        DatabaseManager.SetInt(GameConstants.g_currentLevel, currentLevel);
        DatabaseManager.SetDouble(GameConstants.g_score, score);
    }

	#endregion Load/Save State
}
