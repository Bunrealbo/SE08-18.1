using System;
using UnityEngine;

public class RealTime : MonoBehaviour
{
	public static float time
	{
		get
		{
			if (RealTime.mInst == null)
			{
				RealTime.Spawn();
			}
			return RealTime.mInst.mRealTime;
		}
	}

	public static DateTime frameDateTime
	{
		get
		{
			if (RealTime.mInst == null)
			{
				RealTime.Spawn();
			}
			if (!RealTime.mInst.isCurrentFrameTimeSet)
			{
				RealTime.mInst.currentFrameTime = DateTime.UtcNow;
				RealTime.mInst.isCurrentFrameTimeSet = true;
			}
			return RealTime.mInst.currentFrameTime;
		}
	}

	public static float deltaTimeIgnoreApplicationPause
	{
		get
		{
			if (RealTime.mInst == null)
			{
				RealTime.Spawn();
			}
			return RealTime.mInst.mRealDeltaIgnoreApplicationPause;
		}
	}

	public static float deltaTime
	{
		get
		{
			if (RealTime.mInst == null)
			{
				RealTime.Spawn();
			}
			return RealTime.mInst.mRealDelta;
		}
	}

	private static void Spawn()
	{
		if (RealTime.applicationIsQuitting)
		{
			return;
		}
		GameObject gameObject = new GameObject("_RealTime");
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		RealTime.mInst = gameObject.AddComponent<RealTime>();
		RealTime.mInst.mRealTime = Time.realtimeSinceStartup;
		RealTime.mInst.mRealTimeIgnoreApplicationPause = Time.realtimeSinceStartup;
	}

	private void OnDestroy()
	{
		RealTime.applicationIsQuitting = true;
	}

	private void Update()
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		this.mRealDelta = realtimeSinceStartup - this.mRealTime;
		this.mRealDeltaIgnoreApplicationPause = realtimeSinceStartup - this.mRealTimeIgnoreApplicationPause;
		this.mRealTime = realtimeSinceStartup;
		this.mRealTimeIgnoreApplicationPause = realtimeSinceStartup;
		RealTime.mInst.currentFrameTime = DateTime.UtcNow;
		RealTime.mInst.isCurrentFrameTimeSet = true;
	}

	private void OnApplicationPause(bool paused)
	{
		this.mRealTime = Time.realtimeSinceStartup;
		this.mRealDelta = 0f;
	}

	private static RealTime mInst;

	private float mRealTime;

	private float mRealDelta;

	private float mRealTimeIgnoreApplicationPause;

	private float mRealDeltaIgnoreApplicationPause;

	private bool isCurrentFrameTimeSet;

	private DateTime currentFrameTime;

	private static bool applicationIsQuitting;
}
