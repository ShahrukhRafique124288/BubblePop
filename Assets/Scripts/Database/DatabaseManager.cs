using UnityEngine;
using System;

public class DatabaseManager  
{

    #region private methods()
    private static string Encrypt(string text)
    {        
		return text.Encrypt ();
    }
        
    #endregion

    #region public methods

    public static void Save()
    {
		PlayerPrefs.Save ();
    }
        
    public static void RemoveKey(string key)
    {
		string encryptedKey = Encrypt (key);
		PlayerPrefs.DeleteKey (encryptedKey);
    }

    public static void SetInt(string key, int value)
    {
		string encryptedKey = Encrypt (key);
		string encryptedValue = Encrypt (value.ToString ());
		PlayerPrefs.SetString (encryptedKey, encryptedValue);
    }

    public static int GetInt(string key, int defaultValue = 0)
    {
		string encryptedKey = Encrypt (key);
		string encryptedValue = PlayerPrefs.GetString (encryptedKey, string.Empty);
		return string.IsNullOrEmpty (encryptedValue) ? defaultValue : int.Parse (Encrypt (encryptedValue));
    }

    public static void SetFloat(string key, float value)
    {
		string encryptedKey = Encrypt (key);
		string encryptedValue = Encrypt (value.ToString ());
		PlayerPrefs.SetString (encryptedKey, encryptedValue);
    }

    public static float GetFloat(string key, float defaultValue = 0.0f)
    {
		string encryptedKey = Encrypt (key);
		string encryptedValue = PlayerPrefs.GetString (encryptedKey, string.Empty);
		return string.IsNullOrEmpty (encryptedValue) ? defaultValue : float.Parse (Encrypt (encryptedValue));
    }

    public static void SetDouble(string key, double value)
    {
		string encryptedKey = Encrypt (key);
		string encryptedValue = Encrypt (value.ToString ());
		PlayerPrefs.SetString (encryptedKey, encryptedValue);
    }

    public static double GetDouble(string key, double defaultValue = 0.0d)
    {
		string encryptedKey = Encrypt (key);
		string encryptedValue = PlayerPrefs.GetString (encryptedKey, string.Empty);
		return string.IsNullOrEmpty (encryptedValue) ? defaultValue : double.Parse (Encrypt (encryptedValue));
    }

	public static void SetString(string key, string value)
	{
		string encryptedKey = Encrypt (key);
		string encryptedValue = Encrypt (value);
		PlayerPrefs.SetString (encryptedKey, encryptedValue);
	}

	public static void SetTime(string key, DateTime value)
	{
		string encryptedKey = Encrypt (key);
		PlayerPrefs.SetString (encryptedKey, value.ToBinary ().ToString ());
	}

	public static DateTime GetTime(string key, DateTime defaultValue)
	{
		string encryptedKey = Encrypt (key);
		string value = PlayerPrefs.GetString (encryptedKey, string.Empty);
		try{
			return DateTime.FromBinary (Convert.ToInt64 (value));
		}catch
		{
			return defaultValue;
		}
	}

	public static string GetString(string key, string defaultValue = "")
	{
		string encryptedKey = Encrypt (key);
		if (PlayerPrefs.HasKey (encryptedKey))
		{
			string encryptedValue = PlayerPrefs.GetString (encryptedKey, defaultValue);
			return Encrypt (encryptedValue);
		} 
		else
		{
			return defaultValue;
		}
	}

	public static void SetBool(string key, bool value)
	{
		SetInt (key, value ? 1 : 0);
	}

	public static bool GetBool(string key, bool defaultValue = false)
	{
		return GetInt (key, defaultValue ? 1 : 0) == 1;
	}

	public static bool HasKey (string key)
	{
		string encryptedKey = Encrypt (key);
		return PlayerPrefs.HasKey (encryptedKey);
	}

    #endregion
}