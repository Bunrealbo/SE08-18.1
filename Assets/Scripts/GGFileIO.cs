using System;
using System.IO;

public class GGFileIO
{
	public static GGFileIO instance
	{
		get
		{
			if (GGFileIO.instance_ == null)
			{
				GGFileIO.instance_ = new GGFileIOUnity();
			}
			return GGFileIO.instance_;
		}
	}

	public virtual void Write(string path, string text)
	{
	}

	public virtual void Write(string path, byte[] bytes)
	{
	}

	public virtual string ReadText(string path)
	{
		return null;
	}

	public virtual byte[] Read(string path)
	{
		return null;
	}

	public virtual bool FileExists(string path)
	{
		return false;
	}

	public virtual Stream FileReadStream(string path)
	{
		byte[] array = this.Read(path);
		return new MemoryStream(array, 0, array.Length);
	}

	public static GGFileIO instance_;
}
