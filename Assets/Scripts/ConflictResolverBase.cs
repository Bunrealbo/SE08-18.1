using System;
using System.Collections.Generic;
using UnityEngine;

public class ConflictResolverBase : ScriptableObject
{
	public static ConflictResolverBase instance
	{
		get
		{
			if (ConflictResolverBase.instance_ == null)
			{
				ConflictResolverBase.instance_ = (Resources.Load("GGServerAssets/ConflictResolver", typeof(ConflictResolverBase)) as ConflictResolverBase);
			}
			if (ConflictResolverBase.instance_ == null)
			{
				UnityEngine.Debug.LogError("No CloudSync resolver defined");
			}
			return ConflictResolverBase.instance_;
		}
	}

	public virtual bool ResolveConflict(GGSnapshotCloudSync.CloudSyncConflict conflict)
	{
		return false;
	}

	public virtual void OnConflict(GGCloudSyncFileIOSync sync)
	{
	}

	public virtual void OnConflict()
	{
	}

	public virtual void OnBeginSync()
	{
	}

	public virtual List<string> FilesToSync()
	{
		return null;
	}

	private static ConflictResolverBase instance_;
}
