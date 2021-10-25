using System;
using System.Collections.Generic;
using ProtoModels;

public class GGRequestFileCache : SingletonInit<GGRequestFileCache>
{
	public override void Init()
	{
		if (!ProtoIO.LoadFromFile<RequestCache>(this.filename, out this.cache))
		{
			this.cache = new RequestCache();
			this.cache.requests = new List<RequestCache.Request>();
			this.Save();
		}
	}

	public void Clear()
	{
		this.cache = new RequestCache();
		this.cache.requests = new List<RequestCache.Request>();
		this.Save();
	}

	private void Save()
	{
		ProtoIO.SaveToFile<RequestCache>(this.filename, this.cache);
	}

	public void Put(string key, byte[] cachedObject, TimeSpan timeToLive)
	{
		GGDebug.DebugLog("CACHE FILE-PUT: " + key);
		RequestCache.Request request = this.GetRequest(key);
		if (request == null)
		{
			request = new RequestCache.Request();
			if (this.cache.requests == null)
			{
				this.cache.requests = new List<RequestCache.Request>();
			}
			this.cache.requests.Add(request);
		}
		request.key = key;
		request.bytesString = Convert.ToBase64String(cachedObject, 0, cachedObject.Length);
		request.ticksToExpire = (DateTime.Now + timeToLive).Ticks;
		this.Save();
	}

	private RequestCache.Request GetRequest(string key)
	{
		if (this.cache == null || this.cache.requests == null)
		{
			return null;
		}
		foreach (RequestCache.Request request in this.cache.requests)
		{
			if (request.key == key)
			{
				return request;
			}
		}
		return null;
	}

	public int Count()
	{
		return this.cache.requests.Count;
	}

	private bool IsExpired(RequestCache.Request request)
	{
		return DateTime.Now >= new DateTime(request.ticksToExpire);
	}

	public T Get<T>(string key) where T : class
	{
		GGDebug.DebugLog("CACHE FILE-GET: " + key);
		RequestCache.Request request = this.GetRequest(key);
		if (request == null)
		{
			return default(T);
		}
		GGDebug.DebugLog("Cache Hit: " + key + ", expired " + this.IsExpired(request).ToString());
		if (this.IsExpired(request))
		{
			this.cache.requests.Remove(request);
			this.Save();
			return default(T);
		}
		return Convert.FromBase64String(request.bytesString) as T;
	}

	public string filename = "requestFileCache.bytes";

	private RequestCache cache;
}
