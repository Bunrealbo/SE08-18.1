using System;
using System.IO;

public class GGSnapshotFileIO : GGFileIO
{
	public GGSnapshotFileIO(GGSnapshotCloudSync snapshotSync)
	{
		this.snapshotSync = snapshotSync;
	}

	public override void Write(string path, string text)
	{
		GGFileIO.instance.Write(path, text);
		this.snapshotSync.UpdateSnapshot();
	}

	public override void Write(string path, byte[] bytes)
	{
		GGFileIO.instance.Write(path, bytes);
		this.snapshotSync.UpdateSnapshot();
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

	public override Stream FileReadStream(string path)
	{
		return GGFileIO.instance.FileReadStream(path);
	}

	private GGSnapshotCloudSync snapshotSync;
}
