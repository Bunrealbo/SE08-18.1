using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GGFetchTextureIncremental : BehaviourSingletonInit<GGFetchTextureIncremental>
{
	public override void Init()
	{
		GGDebug.DebugLog("clear cache");
		Caching.ClearCache();
	}

	public int GetTicketForFetchTexturesQueue(List<GGFetchTextureIncremental.TextureRequest> request, GGFetchTextureIncremental.OnIncrementComplete incrementCallback)
	{
		GGDebug.DebugLog(request.Count);
		int num = this.nextUnusedTicket;
		this.nextUnusedTicket = num + 1;
		int num2 = num;
		this.QueueNewRequestRecord(request, incrementCallback, num2);
		if (this.currentTicket < 0)
		{
			this.BeginProcessingNewRequest();
		}
		return num2;
	}

	private void QueueNewRequestRecord(List<GGFetchTextureIncremental.TextureRequest> request, GGFetchTextureIncremental.OnIncrementComplete incrementCallback, int requestId)
	{
		GGFetchTextureIncremental.Request value = new GGFetchTextureIncremental.Request
		{
			request = request,
			incrementCallback = incrementCallback,
			currentIncrement = 0
		};
		this.requests.Add(requestId, value);
		this.idTickets.Enqueue(requestId);
	}

	private void BeginProcessingNewRequest()
	{
		this.currentTicket = this.GetNextTicket();
		if (this.currentTicket >= 0)
		{
			this.TryFetchIncrement();
		}
	}

	private int GetNextTicket()
	{
		while (this.idTickets.Count > 0)
		{
			int num = (int)this.idTickets.Dequeue();
			if (this.requests.ContainsKey(num))
			{
				return num;
			}
		}
		return -1;
	}

	private void TryFetchIncrement()
	{
		GGFetchTextureIncremental.Request request;
		this.requests.TryGetValue(this.currentTicket, out request);
		if (request.request.Count > 0)
		{
			this.FetchIncrement();
			return;
		}
		this.OnIncrementGetFinished();
	}

	private void FetchIncrement()
	{
		GGFetchTextureIncremental.Request request;
		this.requests.TryGetValue(this.currentTicket, out request);
		request.result = new List<GGFetchTextureIncremental.TextureResult>();
		int num = request.currentIncrement * this.texturesInOneIncrement;
		for (int i = num; i < num + this.texturesInOneIncrement; i++)
		{
			if (i < request.request.Count)
			{
				base.StartCoroutine(this.QueryTexture(request.request[i]));
			}
		}
	}

	private IEnumerator QueryTexture(GGFetchTextureIncremental.TextureRequest request)
	{
		return new GGFetchTextureIncremental._003CQueryTexture_003Ed__17(0)
		{
			_003C_003E4__this = this,
			request = request
		};
	}

	private Texture2D TryGetFromCache(string key)
	{
		if (string.IsNullOrEmpty(key))
		{
			return null;
		}
		return Singleton<GGRequestCache>.Instance.Get<Texture2D>(key);
	}

	private void TryCacheTexture(WWW query)
	{
		if (string.IsNullOrEmpty(query.error) && !string.IsNullOrEmpty(query.url) && query.texture != null)
		{
			Singleton<GGRequestCache>.Instance.Put(query.url, query.texture, this.cacheTimeToLive);
		}
	}

	private void ReceiveTexture(GGFetchTextureIncremental.TextureResult result)
	{
		GGFetchTextureIncremental.Request request;
		this.requests.TryGetValue(this.currentTicket, out request);
		request.result.Add(result);
		if (this.IsIncrementProcessingFinished())
		{
			this.OnIncrementGetFinished();
		}
	}

	private bool IsIncrementProcessingFinished()
	{
		GGFetchTextureIncremental.Request request;
		this.requests.TryGetValue(this.currentTicket, out request);
		int b = request.request.Count - request.currentIncrement * this.texturesInOneIncrement;
		return request.result.Count >= Mathf.Min(this.texturesInOneIncrement, b);
	}

	private void OnIncrementGetFinished()
	{
		GGFetchTextureIncremental.Request request;
		this.requests.TryGetValue(this.currentTicket, out request);
		request.currentIncrement++;
		if (request.incrementCallback != null)
		{
			request.incrementCallback(request.result, this.IsCurrentRequestProcessed());
		}
		this.AdvanceRequestsToNextIncrement();
	}

	private void AdvanceRequestsToNextIncrement()
	{
		if (this.IsCurrentRequestProcessed())
		{
			this.RemoveRequest(this.currentTicket);
			this.BeginProcessingNewRequest();
			return;
		}
		this.FetchIncrement();
	}

	private bool IsCurrentRequestProcessed()
	{
		GGFetchTextureIncremental.Request request;
		this.requests.TryGetValue(this.currentTicket, out request);
		return request.currentIncrement * this.texturesInOneIncrement >= request.request.Count;
	}

	public void StopRequest(int requestTicket)
	{
		this.StopCoroutinesForTicket(requestTicket);
		this.RemoveRequest(requestTicket);
		this.BeginProcessingNewRequest();
	}

	private void StopCoroutinesForTicket(int requestTicket)
	{
		if (requestTicket == this.currentTicket)
		{
			base.StopAllCoroutines();
		}
	}

	private void RemoveRequest(int requestTicket)
	{
		if (this.requests.ContainsKey(requestTicket))
		{
			this.requests.Remove(requestTicket);
		}
	}

	public int texturesInOneIncrement = 4;

	public TimeSpan cacheTimeToLive = TimeSpan.FromHours(6.0);

	private int nextUnusedTicket;

	private Dictionary<int, GGFetchTextureIncremental.Request> requests = new Dictionary<int, GGFetchTextureIncremental.Request>();

	private Queue idTickets = new Queue();

	private int currentTicket = -1;

	public class TextureResult
	{
		public int textureSetId;

		public Texture2D texture;
	}

	public class TextureRequest
	{
		public int textureSetId;

		public string url;
	}

	public delegate void OnIncrementComplete(List<GGFetchTextureIncremental.TextureResult> textureIncrement, bool isFinished);

	private class Request
	{
		public List<GGFetchTextureIncremental.TextureRequest> request;

		public GGFetchTextureIncremental.OnIncrementComplete incrementCallback;

		public List<GGFetchTextureIncremental.TextureResult> result;

		public int currentIncrement;
	}

	private sealed class _003CQueryTexture_003Ed__17 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CQueryTexture_003Ed__17(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		bool IEnumerator.MoveNext()
		{
			int num = this._003C_003E1__state;
			GGFetchTextureIncremental ggfetchTextureIncremental = this._003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				this._003C_003E1__state = -1;
				ggfetchTextureIncremental.TryCacheTexture(this._003Cwww_003E5__3);
				this._003CtextureResult_003E5__2.texture = (string.IsNullOrEmpty(this._003Cwww_003E5__3.error) ? this._003Cwww_003E5__3.texture : null);
				ggfetchTextureIncremental.ReceiveTexture(this._003CtextureResult_003E5__2);
				return false;
			}
			else
			{
				this._003C_003E1__state = -1;
				this._003CtextureResult_003E5__2 = new GGFetchTextureIncremental.TextureResult
				{
					textureSetId = this.request.textureSetId
				};
				this._003CtextureResult_003E5__2.texture = ggfetchTextureIncremental.TryGetFromCache(this.request.url);
				if (this._003CtextureResult_003E5__2.texture != null)
				{
					ggfetchTextureIncremental.ReceiveTexture(this._003CtextureResult_003E5__2);
					return false;
				}
				this._003Cwww_003E5__3 = new WWW(this.request.url);
				this._003C_003E2__current = this._003Cwww_003E5__3;
				this._003C_003E1__state = 1;
				return true;
			}
		}

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this._003C_003E2__current;
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this._003C_003E2__current;
			}
		}

		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public GGFetchTextureIncremental.TextureRequest request;

		public GGFetchTextureIncremental _003C_003E4__this;

		private GGFetchTextureIncremental.TextureResult _003CtextureResult_003E5__2;

		private WWW _003Cwww_003E5__3;
	}
}
