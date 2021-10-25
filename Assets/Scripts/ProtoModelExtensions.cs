using System;
using ProtoModels;

public static class ProtoModelExtensions
{
	public static CloudSyncData.CloudSyncFile GetFile(CloudSyncData request, string key)
	{
		if (request == null)
		{
			return null;
		}
		foreach (CloudSyncData.CloudSyncFile cloudSyncFile in request.files)
		{
			if (cloudSyncFile.key == key)
			{
				return cloudSyncFile;
			}
		}
		return null;
	}
}
