using System;
using System.IO;
using UnityEngine;

public class GGFileIOUnity : GGFileIO
{
	public GGFileIOUnity()
	{
		RuntimePlatform platform = Application.platform;
	}

	private string GetInternalPath()
	{
		return Application.persistentDataPath;
	}

	protected string FullPath(string filename)
	{
		return this.GetInternalPath() + "/" + filename;
	}

	private void WritePlayerPrefs(string path, string text)
	{
		PlayerPrefs.SetString(path, text);
		PlayerPrefs.Save();
	}

	private void WritePlayerPrefs(string path, byte[] bytes)
	{
		string value = Convert.ToBase64String(bytes);
		PlayerPrefs.SetString(path, value);
		PlayerPrefs.Save();
	}

	public override void Write(string path, string text)
	{
		this.WritePlayerPrefs(path, text);
		File.WriteAllText(this.FullPath(path), text);
	}

	public override void Write(string path, byte[] bytes)
	{
		this.WritePlayerPrefs(path, bytes);
		File.WriteAllBytes(this.FullPath(path), bytes);
	}

	public override string ReadText(string path)
	{
		if (PlayerPrefs.HasKey(path))
		{
			return PlayerPrefs.GetString(path);
		}
		return File.ReadAllText(this.FullPath(path));
	}

	public override byte[] Read(string path)
	{
		if (PlayerPrefs.HasKey(path))
		{
			string @string = PlayerPrefs.GetString(path);
			try
			{
				return Convert.FromBase64String(@string);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log("PROBLEM WITH LOADING " + path + " " + ex.ToString());
				return null;
			}
		}
		return File.ReadAllBytes(this.FullPath(path));
	}

	public override bool FileExists(string path)
	{
		if (PlayerPrefs.HasKey(path))
		{
			return true;
		}
		FileInfo fileInfo = new FileInfo(this.FullPath(path));
		return fileInfo != null && fileInfo.Exists;
	}

	public override Stream FileReadStream(string path)
	{
		if (PlayerPrefs.HasKey(path))
		{
			string @string = PlayerPrefs.GetString(path);
			try
			{
				byte[] array = Convert.FromBase64String(@string);
				MemoryStream memoryStream = new MemoryStream(array);
				memoryStream.SetLength((long)array.Length);
				memoryStream.Capacity = array.Length;
				return memoryStream;
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log("PROBLEM WITH LOADING " + path + " " + ex.ToString());
				return null;
			}
		}
		return new FileStream(this.FullPath(path), FileMode.Open, FileAccess.Read);
	}
}
