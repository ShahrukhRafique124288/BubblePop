using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TapticPlugin;

public static class Vibration
{
#if UNITY_ANDROID && !UNITY_EDITOR
	private static AndroidJavaObject vibrationObj = Vibration.GetVibrator ();
#else
    private static AndroidJavaObject vibrationObj;
#endif

	private static AndroidJavaObject GetVibrator()
	{
		AndroidJavaClass unityplayerActivityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject unityplayerActivityObject = unityplayerActivityClass.GetStatic<AndroidJavaObject>("currentActivity");
		//return new AndroidJavaObject("com.izaron.androideffects.vibration.Vibration", unityplayerActivityClass.GetStatic<AndroidJavaObject>("currentActivity"));
		return unityplayerActivityObject.Call<AndroidJavaObject>("getSystemService", "vibrator");
	}

    public static void Vibrate()
    {
		if (Application.platform == RuntimePlatform.Android && GameData.IsVibrationOn)
            vibrationObj.Call("vibrate");
    }

    public static void Vibrate(long milliseconds)
    {
		if (Application.platform == RuntimePlatform.Android && GameData.IsVibrationOn)
            vibrationObj.Call("vibrate", milliseconds);
    }

	public static void Vibrate(ImpactFeedback feedback)
	{
		if (Utility.IsOldIphone || !GameData.IsVibrationOn)
			return;

		if (Application.platform == RuntimePlatform.IPhonePlayer)
			TapticManager.Impact (feedback);
	}

    public static void Vibrate(long[] pattern, int repeat)
    {
		if (Application.platform == RuntimePlatform.Android && GameData.IsVibrationOn)
            vibrationObj.Call("vibrate", pattern, repeat);
    }

    public static bool HasVibrator()
    {
        if (Application.platform == RuntimePlatform.Android)
            return vibrationObj.Call<bool>("hasVibrator");
        else
            return false;
    }

    public static void Cancel()
    {
        if (Application.platform == RuntimePlatform.Android)
            vibrationObj.Call("cancel");
    }
}