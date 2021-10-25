using System;
using UnityEngine;

public class GGDebug
{
	public static void Log(string logMsg)
	{
		if (ConfigBase.instance.debug)
		{
			UnityEngine.Debug.Log(logMsg);
		}
	}

	public static void DebugLog(string logMsg)
	{
		if (ConfigBase.instance.debug)
		{
			UnityEngine.Debug.Log(logMsg);
		}
	}

	public static void DebugLog(int logMsg)
	{
		if (ConfigBase.instance.debug)
		{
			UnityEngine.Debug.Log(logMsg);
		}
	}
}
