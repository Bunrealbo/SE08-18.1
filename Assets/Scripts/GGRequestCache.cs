using System;
using System.Collections.Generic;

public class GGRequestCache : Singleton<GGRequestCache>
{
	public void Clear()
	{
		this.cache.Clear();
	}

	public void Put(string key, object cachedObject, TimeSpan timeToLive)
	{
		if (cachedObject != null)
		{
			GGDebug.DebugLog(string.Concat(new object[]
			{
				"CACHE MEM-PUT: ",
				key,
				" time to live ",
				timeToLive
			}));
			this.cache[key] = new GGRequestCache.CachedRequest(cachedObject, timeToLive);
		}
	}

	public T Get<T>(string key) where T : class
	{
		GGDebug.DebugLog("CACHE MEM-GET: " + key);
		if (!this.cache.ContainsKey(key))
		{
			return default(T);
		}
		GGRequestCache.CachedRequest cachedRequest = this.cache[key];
		GGDebug.DebugLog("Cache Hit: " + key + ", expired " + cachedRequest.isExpired.ToString());
		if (cachedRequest.isExpired)
		{
			this.cache.Remove(key);
			return default(T);
		}
		return cachedRequest.cachedObject as T;
	}

	private Dictionary<string, GGRequestCache.CachedRequest> cache = new Dictionary<string, GGRequestCache.CachedRequest>();

	public class CachedRequest
	{
		public CachedRequest(object obj, TimeSpan timeToLive)
		{
			this.cachedObject = obj;
			this.expireTime = DateTime.Now + timeToLive;
		}

		public bool isExpired
		{
			get
			{
				return DateTime.Now > this.expireTime;
			}
		}

		protected DateTime expireTime;

		public object cachedObject;
	}
}
