using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System;
using System.IO;

public static class Utility {

    private static bool isIphoneX;
    private static bool isOldIphone;

    public static bool IsIphoneX
    {
        get
        { 
            return isIphoneX;
        }
    }

    public static bool IsOldIphone
    {
        get
        { 
            return isOldIphone;
        }
    }

    public static void Initialize()
    {
        isOldIphone = IsOlderIphone ();
        isIphoneX = IsiPhoneX ();
    }

	private static List<string> currencyStringsShortNotations;

	public static void AddDictionaryElementInDictionary<T, S>(this Dictionary<T, S> source, Dictionary<T, S> collection)
	{
		if (collection == null)
		{
			return;  //throw new ArgumentNullException("Collection is null");
		}

		foreach (var item in collection)
		{
			if(!source.ContainsKey(item.Key)){ 
				source.Add(item.Key, item.Value);
			}
			else
			{
				// handle duplicate key issue here
			}  
		} 
	}

	public static string DictionaryToString(Dictionary<string, string> data)
	{
		string str = string.Join(";", data.Select(x => x.Key + "=" + x.Value).ToArray());
		return str;
	}

	public static Dictionary<string,string> StringToDictionary(string data)
	{
		Dictionary<string,string> dictionary = data
			.Split(';')
			.Select(part => part.Split('='))
			.Where(part => part.Length == 2)
			.ToDictionary(sp => sp[0], sp => sp[1]);
		return dictionary;
	}

	public static float DeviceAspectRatio()
	{
		return (float)Screen.height / Screen.width;
	}

	public static bool IsInternetAvailable()
	{
		return Application.internetReachability != NetworkReachability.NotReachable;
	}

	public static string Encrypt(this string text, int key = GameConstants.g_kEncyptionStringKey)
	{        
		StringBuilder inSb = new StringBuilder(text);
		StringBuilder outSb = new StringBuilder(text.Length);
		char c;
		for (int i = 0; i < text.Length; i++)
		{
			c = inSb[i];
			c = (char)(c ^ key);
			outSb.Append(c);
		}
		return outSb.ToString();
	}

	public static double ToDouble(string val)
	{
		if (val.Length == 0 || val == "-")
		{
			return 0.0f;
		}
		else
		{
			return double.Parse(val);
		}
	}

	public static float ToFloat(string val)
	{
		if (val.Length == 0 || val == "-")
		{
			return 0f;
		}
		else
		{
			return float.Parse(val);
		}
	}

	public static int ToInt(string val)
	{
		if (val.Length == 0 || val == "-" || string.IsNullOrEmpty(val))
		{
			return 0;
		}
		else
		{
			return int.Parse(val.Split('.')[0]);
		}
	}
		

	public static Vector3 ToVector3(string sVector)
	{
		if (sVector.Length == 0 || sVector == "-")
		{
			return Vector3.zero;
		}

		// Remove the parentheses
		if (sVector.StartsWith ("(") && sVector.EndsWith (")")) {
			sVector = sVector.Substring(1, sVector.Length-2);
		}

		// split the items
		string[] sArray = sVector.Split(':');

		// store as a Vector3
		Vector3 result = new Vector3(
			float.Parse(sArray[0]),
			float.Parse(sArray[1]),
			float.Parse(sArray[2]));

		return result;
	}

	public static string TrimBundleVersionString(this string str)
	{
		return str.Trim().Replace (".", "");
	}

	public static TValue GetValue<TKey, TValue>(
		this Dictionary<TKey, TValue> data, TKey key)
	{
		TValue result;
		if (!data.TryGetValue (key, out result))
		{
			return default(TValue);
		}

		return result;
	}

	public static void RecreateDirectory(string path)
	{
		#if UNITY_EDITOR
		if (Directory.Exists(path))
		{    
			UnityEditor.FileUtil.DeleteFileOrDirectory(path);
		}          
		Directory.CreateDirectory(path);
		#endif
	}
    
    private static bool IsiPhoneX ()
    {
        #if UNITY_EDITOR
        #if UNITY_IOS
        return false;
        #else
        return false;
        #endif
        #else
        if (Screen.width == 1125 && Screen.height == 2436)
        {
        return true;
        }
        return false;
        #endif
    }

    private static bool IsOlderIphone()
    {
        #if UNITY_IOS
        return UnityEngine.iOS.Device.generation.ToString ().Contains ("4")
            || UnityEngine.iOS.Device.generation.ToString ().Contains ("5")
            || UnityEngine.iOS.Device.generation.ToString ().Contains ("6")
            || UnityEngine.iOS.Device.generation.ToString ().Contains ("SE");
        #else
        return true;
        #endif
    }
}
