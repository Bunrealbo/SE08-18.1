using System;

public class GGUIDPrivate
{
	public static void Reset()
	{
		GGUIDPrivate.uid = Guid.NewGuid().ToString();
		GGUIDPrivate.Save();
	}

	protected static string filePath
	{
		get
		{
			return "gguid_private.txt";
		}
	}

	protected static void Save()
	{
		try
		{
			GGFileIO.instance.Write(GGUIDPrivate.filePath, GGUIDPrivate.uid);
		}
		catch
		{
		}
	}

	public static string InstallId()
	{
		if (!string.IsNullOrEmpty(GGUIDPrivate.uid))
		{
			return GGUIDPrivate.uid;
		}
		if (!GGFileIO.instance.FileExists(GGUIDPrivate.filePath))
		{
			GGUIDPrivate.uid = Guid.NewGuid().ToString();
			GGUIDPrivate.Save();
		}
		else
		{
			try
			{
				GGUIDPrivate.uid = GGFileIO.instance.ReadText(GGUIDPrivate.filePath);
			}
			catch
			{
			}
		}
		return GGUIDPrivate.uid;
	}

	protected static string uid;
}
