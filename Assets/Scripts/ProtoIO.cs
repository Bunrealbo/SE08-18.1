using System;
using System.IO;
using ProtoBuf.Meta;

public class ProtoIO
{
	public static bool LoadFromFile<T>(string filename, out T model) where T : class
	{
		return ProtoIO.LoadFromFile<ProtoSerializer, T>(filename, GGFileIO.instance, out model);
	}

	public static bool LoadFromFileLocal<T>(string filename, out T model) where T : class
	{
		return ProtoIO.LoadFromFile<ProtoSerializer, T>(filename, GGFileIO.instance, out model);
	}

	public static bool LoadFromFileLocal<S, T>(string filename, out T model) where S : TypeModel, new() where T : class
	{
		return ProtoIO.LoadFromFile<S, T>(filename, GGFileIO.instance, out model);
	}

	public static bool LoadFromFile<S, T>(string filename, GGFileIO fileIO, out T model) where S : TypeModel, new() where T : class
	{
		model = default(T);
		if (!GGFileIO.instance.FileExists(filename))
		{
			return false;
		}
		try
		{
			S s = Activator.CreateInstance<S>();
			using (Stream stream = fileIO.FileReadStream(filename))
			{
				model = (s.Deserialize(stream, null, typeof(T)) as T);
			}
			return true;
		}
		catch
		{
		}
		return false;
	}

	public static bool SaveToFile<T>(string filename, T model) where T : new()
	{
		return ProtoIO.SaveToFile<ProtoSerializer, T>(filename, GGFileIO.instance, model);
	}

	public static bool SaveToFileCS<T>(string filename, T model) where T : new()
	{
		return ProtoIO.SaveToFile<ProtoSerializer, T>(filename, GGFileIOCloudSync.instance.GetDefaultFileIO(), model);
	}

	public static bool SaveToFile<S, T>(string filename, GGFileIO fileIO, T model) where S : TypeModel, new() where T : new()
	{
		S s = Activator.CreateInstance<S>();
		if (model == null)
		{
			model = Activator.CreateInstance<T>();
		}
		try
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				s.Serialize(memoryStream, model);
				memoryStream.Flush();
				fileIO.Write(filename, memoryStream.ToArray());
			}
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static byte[] SerializeToByteArray<S, T>(T model) where S : TypeModel, new() where T : new()
	{
		S s = Activator.CreateInstance<S>();
		if (model == null)
		{
			model = Activator.CreateInstance<T>();
		}
		byte[] result;
		try
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				s.Serialize(memoryStream, model);
				memoryStream.Flush();
				result = memoryStream.ToArray();
			}
		}
		catch
		{
			result = null;
		}
		return result;
	}

	public static bool LoadFromByteStream<T>(byte[] encoded, out T model) where T : class
	{
		return ProtoIO.LoadFromByteStream<ProtoSerializer, T>(encoded, out model);
	}

	private static bool LoadFromByteStream<S, T>(byte[] encoded, out T model) where S : TypeModel, new() where T : class
	{
		model = default(T);
		MemoryStream memoryStream = new MemoryStream(encoded);
		memoryStream.SetLength((long)encoded.Length);
		memoryStream.Capacity = encoded.Length;
		S s = Activator.CreateInstance<S>();
		try
		{
			model = (s.Deserialize(memoryStream, null, typeof(T)) as T);
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static string SerializeToByte64<T>(T Model) where T : class
	{
		MemoryStream memoryStream = new MemoryStream();
		new ProtoSerializer().Serialize(memoryStream, Model);
		return Convert.ToBase64String(memoryStream.ToArray(), 0, (int)memoryStream.Length);
	}

	public static bool LoadFromBase64String<T>(string base64String, out T model) where T : class
	{
		return ProtoIO.LoadFromBase64String<ProtoSerializer, T>(base64String, out model);
	}

	private static bool LoadFromBase64String<S, T>(string base64String, out T model) where S : TypeModel, new() where T : class
	{
		byte[] array = null;
		model = default(T);
		try
		{
			array = Convert.FromBase64String(base64String);
		}
		catch
		{
			return false;
		}
		MemoryStream memoryStream = new MemoryStream(array);
		memoryStream.SetLength((long)array.Length);
		memoryStream.Capacity = array.Length;
		S s = Activator.CreateInstance<S>();
		try
		{
			model = (s.Deserialize(memoryStream, null, typeof(T)) as T);
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static T Clone<T>(T model) where T : class, new()
	{
		if (model == null)
		{
			return default(T);
		}
		byte[] encoded = ProtoIO.SerializeToByteArray<ProtoSerializer, T>(model);
		T result = Activator.CreateInstance<T>();
		ProtoIO.LoadFromByteStream<T>(encoded, out result);
		return result;
	}
}
