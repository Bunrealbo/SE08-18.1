using System;

public class GGUID
{
	protected static string filePath
	{
		get
		{
			return "gguid.txt";
		}
	}

	protected static void Save()
	{
		try
		{
			GGFileIO.instance.Write(GGUID.filePath, GGUID.uid);
		}
		catch
		{
		}
	}

	public static string NewGuid()
	{
		return Guid.NewGuid().ToString();
	}

	public static string InstallId()
	{
		if (!string.IsNullOrEmpty(GGUID.uid))
		{
			return GGUID.uid;
		}
		if (!GGFileIO.instance.FileExists(GGUID.filePath))
		{
			GGUID.uid = Guid.NewGuid().ToString();
			GGUID.Save();
		}
		else
		{
			try
			{
				GGUID.uid = GGFileIO.instance.ReadText(GGUID.filePath);
			}
			catch
			{
			}
		}
		return GGUID.uid;
	}

	protected static string uid;
}
