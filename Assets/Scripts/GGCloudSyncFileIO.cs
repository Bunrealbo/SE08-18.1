using System;

public class GGCloudSyncFileIO : GGFileIO
{
	public GGCloudSyncFileIO(GGCloudSyncFileIOSync ggSync)
	{
		this.sync = ggSync;
	}

	public override void Write(string path, string text)
	{
		GGDebug.DebugLog("CloudSyncWrite");
		GGFileIO.instance.Write(path, text);
		string guid = Guid.NewGuid().ToString();
		this.sync.SetGuid(path, guid);
	}

	public override void Write(string path, byte[] bytes)
	{
		GGDebug.DebugLog("CloudSyncWrite");
		GGFileIO.instance.Write(path, bytes);
		string guid = Guid.NewGuid().ToString();
		this.sync.SetGuid(path, guid);
	}

	public override string ReadText(string path)
	{
		return GGFileIO.instance.ReadText(path);
	}

	public override byte[] Read(string path)
	{
		if (this.FileExists(path))
		{
			return GGFileIO.instance.Read(path);
		}
		return null;
	}

	public override bool FileExists(string path)
	{
		return GGFileIO.instance.FileExists(path);
	}

	private GGCloudSyncFileIOSync sync;
}
