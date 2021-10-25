using System;
using UnityEngine;

public class GGFileIOCloudSync : MonoBehaviour
{
	public static bool isCloudSyncNotification(string message)
	{
		return "CloudSync.NewData" == message || "CloudSync.DataUploaded" == message;
	}

	public static GGFileIOCloudSync instance
	{
		get
		{
			if (GGFileIOCloudSync.instance_ == null)
			{
				GameObject gameObject = new GameObject();
				gameObject.name = "GGFileIOCloudSync";
				switch (ConfigBase.instance.cloudSyncType)
				{
				case ConfigBase.GGFileIOCloudSyncTypes.WhisperSync:
					GGFileIOCloudSync.instance_ = gameObject.AddComponent<GGFileIOCloudSync>();
					break;
				case ConfigBase.GGFileIOCloudSyncTypes.GGCloudSync:
				{
					GGCloudSyncFileIOSync ggcloudSyncFileIOSync = gameObject.AddComponent<GGCloudSyncFileIOSync>();
					ggcloudSyncFileIOSync.Init();
					GGFileIOCloudSync.instance_ = ggcloudSyncFileIOSync;
					DestroyUtil.DontDestroyOnLoad(GGFileIOCloudSync.instance_.gameObject);
					break;
				}
				case ConfigBase.GGFileIOCloudSyncTypes.GGSaveLocalOnly:
					GGFileIOCloudSync.instance_ = gameObject.AddComponent<GGFileIOCloudSync>();
					break;
				case ConfigBase.GGFileIOCloudSyncTypes.GGSnapshotCloudSync:
				{
					GGSnapshotCloudSync ggsnapshotCloudSync = gameObject.AddComponent<GGSnapshotCloudSync>();
					ggsnapshotCloudSync.Init();
					GGFileIOCloudSync.instance_ = ggsnapshotCloudSync;
					DestroyUtil.DontDestroyOnLoad(GGFileIOCloudSync.instance_.gameObject);
					break;
				}
				default:
					GGFileIOCloudSync.instance_ = gameObject.AddComponent<GGFileIOCloudSync>();
					break;
				}
			}
			return GGFileIOCloudSync.instance_;
		}
	}

	public virtual GGFileIO GetDefaultFileIO()
	{
		return GGFileIO.instance;
	}

	public virtual GGFileIO GetCloudFileIO()
	{
		return GGFileIO.instance;
	}

	public virtual bool IsInConflict(string name)
	{
		return false;
	}

	public virtual void synchronize()
	{
	}

	public const string NOTIFICATION_NEW_DATA = "CloudSync.NewData";

	public const string NOTIFICATION_DATA_UPLOADED = "CloudSync.DataUploaded";

	public static GGFileIOCloudSync instance_;
}
