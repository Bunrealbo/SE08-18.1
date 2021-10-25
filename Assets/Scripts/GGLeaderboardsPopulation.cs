using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GGLeaderboardsPopulation : MonoBehaviour
{
	public void UpdateLeaderboards(GGLeaderboardsPopulation.OnComplete onComplete)
	{
		this.onPopulationComplete = onComplete;
		this.currentPid = ((this.currentPid >= 0) ? (this.currentPid + 1) : this.startPid);
		GGServerRequestsBackend.UpdateRequest updateRequest = new GGServerRequestsBackend.UpdateRequest(this.level, this.nameBase + this.currentPid, this.updateCountry.ToLower());
		int num = this.startScore;
		updateRequest.SetScore(num);
		updateRequest.SetPid(this.currentPid.ToString());
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"pid: ",
			this.currentPid,
			", name: ",
			this.nameBase,
			", level: ",
			this.level,
			", score: ",
			num
		}));
		base.StartCoroutine(this.DoUpdateUser(updateRequest, new GGServerRequestsBackend.OnComplete(this.onRequestComplete)));
	}

	private IEnumerator DoUpdateUser(GGServerRequestsBackend.UpdateRequest update, GGServerRequestsBackend.OnComplete onComplete)
	{
		return new GGLeaderboardsPopulation._003CDoUpdateUser_003Ed__10(0)
		{
			_003C_003E4__this = this,
			update = update,
			onComplete = onComplete
		};
	}

	public void onRequestComplete(GGServerRequestsBackend.ServerRequest request)
	{
		if (this.currentPid < this.endPid)
		{
			this.UpdateLeaderboards(this.onPopulationComplete);
			return;
		}
		this.currentPid = -1;
		if (this.onPopulationComplete != null)
		{
			this.onPopulationComplete();
			this.onPopulationComplete = null;
		}
	}

	public int level;

	public int startPid;

	public int endPid;

	public string nameBase;

	public int startScore;

	public string updateCountry = "czech";

	private int currentPid = -1;

	private GGLeaderboardsPopulation.OnComplete onPopulationComplete;

	public delegate void OnComplete();

	private sealed class _003CDoUpdateUser_003Ed__10 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoUpdateUser_003Ed__10(int _003C_003E1__state)
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
			GGLeaderboardsPopulation ggleaderboardsPopulation = this._003C_003E4__this;
			switch (num)
			{
			case 0:
				this._003C_003E1__state = -1;
				this._003CnonceReq_003E5__2 = new GGServerRequestsBackend.NonceRequest();
				this._003C_003E2__current = ggleaderboardsPopulation.StartCoroutine(this._003CnonceReq_003E5__2.RequestCoroutine());
				this._003C_003E1__state = 1;
				return true;
			case 1:
				this._003C_003E1__state = -1;
				if (this._003CnonceReq_003E5__2.status == GGServerRequestsBackend.ServerRequest.RequestStatus.Success)
				{
					this.update.nonce = this._003CnonceReq_003E5__2.GetResponse<string>();
					UnityEngine.Debug.Log("Nonce: " + this.update.nonce);
					this._003C_003E2__current = ggleaderboardsPopulation.StartCoroutine(this.update.RequestCoroutine());
					this._003C_003E1__state = 2;
					return true;
				}
				break;
			case 2:
				this._003C_003E1__state = -1;
				break;
			default:
				return false;
			}
			UnityEngine.Debug.Log("Update success " + this.update.status);
			if (this.onComplete != null)
			{
				this.onComplete(this.update);
			}
			return false;
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

		public GGLeaderboardsPopulation _003C_003E4__this;

		public GGServerRequestsBackend.UpdateRequest update;

		public GGServerRequestsBackend.OnComplete onComplete;

		private GGServerRequestsBackend.NonceRequest _003CnonceReq_003E5__2;
	}
}
