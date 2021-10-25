using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using ProtoModels;
using UnityEngine;

public class GGSnapshotCloudSync : GGFileIOCloudSync
{
	public event GGSnapshotCloudSync.OnConflict onConflict;

	public static bool syncNeeded
	{
		get
		{
			GGSnapshotCloudSync ggsnapshotCloudSync = GGFileIOCloudSync.instance as GGSnapshotCloudSync;
			return !(ggsnapshotCloudSync == null) && ggsnapshotCloudSync.shouldTryToSyncFromMain;
		}
	}

	public static void StopSyncNeeded()
	{
		GGSnapshotCloudSync ggsnapshotCloudSync = GGFileIOCloudSync.instance as GGSnapshotCloudSync;
		if (ggsnapshotCloudSync == null)
		{
			return;
		}
		ggsnapshotCloudSync.SyncReceieved();
	}

	public bool shouldTryToSyncFromMain
	{
		get
		{
			return GGPlayerSettings.instance.canCloudSync && (this.applicationPausedTimes != this.applicationPausedTimesWhenLastSync || DateTime.Now - this.lastSyncTime > TimeSpan.FromMinutes(30.0));
		}
	}

	public static void CallOnFocus(bool pause)
	{
		GGSnapshotCloudSync ggsnapshotCloudSync = GGFileIOCloudSync.instance as GGSnapshotCloudSync;
		if (ggsnapshotCloudSync == null)
		{
			return;
		}
		ggsnapshotCloudSync.OnApplicationFocus(pause);
	}

	private void OnApplicationFocus(bool pause)
	{
		if (!pause)
		{
			this.applicationPausedTimes++;
		}
	}

	public void SaveActiveSnapshot()
	{
		ProtoIO.SaveToFile<SnapshotSyncInfo>(this.snapshotSyncFileName, this.activeSnapshot);
	}

	public void Init()
	{
		this.syncFileIO = new GGSnapshotFileIO(this);
		if (!ProtoIO.LoadFromFile<SnapshotSyncInfo>(this.snapshotSyncFileName, out this.activeSnapshot))
		{
			this.activeSnapshot = new SnapshotSyncInfo();
			this.activeSnapshot.localSnapVersion = -1;
			this.activeSnapshot.serverSnapSentVersion = -1;
			this.activeSnapshot.serverSnapAcceptedVersion = -1;
			this.SaveActiveSnapshot();
		}
		base.StartCoroutine(this.DoPeriodicallyTryToSync());
	}

	public override GGFileIO GetDefaultFileIO()
	{
		if (GGPlayerSettings.instance.canCloudSync)
		{
			return this.syncFileIO;
		}
		return GGFileIO.instance;
	}

	public override GGFileIO GetCloudFileIO()
	{
		return this.syncFileIO;
	}

	public void UpdateSnapshot()
	{
		this.activeSnapshot.localGuid = Guid.NewGuid().ToString();
		this.SaveActiveSnapshot();
	}

	private IEnumerator DoPeriodicallyTryToSync()
	{
		return new GGSnapshotCloudSync._003CDoPeriodicallyTryToSync_003Ed__32(0)
		{
			_003C_003E4__this = this
		};
	}

	private void SynchronizeIfPossible()
	{
		if (this.isSynchronizationInProgress || this.isInConflict)
		{
			return;
		}
		this.SynchronizeNow(new GGServerRequestsBackend.OnComplete(this.HandleSyncRequestResult));
	}

	private bool isSynchronizationInProgress
	{
		get
		{
			return this.inProgressRequest != null;
		}
	}

	private bool haveLocalChanges
	{
		get
		{
			return this.activeSnapshot.serverAcceptedGuid != this.activeSnapshot.localGuid;
		}
	}

	public void SynchronizeNow(GGServerRequestsBackend.OnComplete onComplete)
	{
		UnityEngine.Debug.Log("Sync now");
		if (this.isSynchronizationInProgress)
		{
			GGSnapshotCloudSync.PendingSynchronizeNowRequests item = new GGSnapshotCloudSync.PendingSynchronizeNowRequests
			{
				onComplete = onComplete
			};
			this.pendingSyncRequests.Add(item);
			return;
		}
		this.DoSynchronize(onComplete);
	}

	private void DoSynchronize(GGServerRequestsBackend.OnComplete onComplete)
	{
		GGSnapshotCloudSync._003C_003Ec__DisplayClass39_0 _003C_003Ec__DisplayClass39_ = new GGSnapshotCloudSync._003C_003Ec__DisplayClass39_0();
		_003C_003Ec__DisplayClass39_._003C_003E4__this = this;
		_003C_003Ec__DisplayClass39_.onComplete = onComplete;
		this.inProgressRequest = this.CreateRequest();
		BehaviourSingletonInit<GGServerRequestsBackend>.instance.GetCSData(this.inProgressRequest, new GGServerRequestsBackend.OnComplete(_003C_003Ec__DisplayClass39_._003CDoSynchronize_003Eb__0));
	}

	private GGServerRequestsBackend.CloudSyncRequest CreateRequest()
	{
		if (this.isInConflict)
		{
			this.conflict.CancelConflict();
		}
		if (this.haveLocalChanges)
		{
			UnityEngine.Debug.Log("cloud sync update");
			List<string> files = ConflictResolverBase.instance.FilesToSync();
			CloudSyncData snapshotForSync = this.GetSnapshotForSync(files);
			GGServerRequestsBackend.UpdateCloudSyncDataRequest updateCloudSyncDataRequest = new GGServerRequestsBackend.UpdateCloudSyncDataRequest();
			updateCloudSyncDataRequest.AddData(snapshotForSync);
			UnityEngine.Debug.Log("Sending local changes " + this.activeSnapshot.serverSnapAcceptedVersion + 1);
			updateCloudSyncDataRequest.SetVersionInfo(this.activeSnapshot.serverSnapAcceptedVersion + 1, this.activeSnapshot.localGuid);
			return updateCloudSyncDataRequest;
		}
		UnityEngine.Debug.Log("cloud sync get");
		GGServerRequestsBackend.GetCloudSyncDataRequest getCloudSyncDataRequest = new GGServerRequestsBackend.GetCloudSyncDataRequest();
		getCloudSyncDataRequest.SetVersionInfo(this.activeSnapshot.serverSnapAcceptedVersion, this.activeSnapshot.localGuid);
		return getCloudSyncDataRequest;
	}

	private void SyncReceieved()
	{
		this.lastSyncTime = DateTime.Now;
		this.applicationPausedTimesWhenLastSync = this.applicationPausedTimes;
	}

	public void HandleSyncRequestResult(GGServerRequestsBackend.ServerRequest request)
	{
		UnityEngine.Debug.Log("on cloud sync finished");
		GGServerRequestsBackend.CloudSyncRequest cloudSyncRequest = request as GGServerRequestsBackend.CloudSyncRequest;
		this.SyncReceieved();
		if (request.status != GGServerRequestsBackend.ServerRequest.RequestStatus.Success)
		{
			this.HandlePendingRequests();
			return;
		}
		CloudSyncData response = request.GetResponse<CloudSyncData>();
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			this.activeSnapshot.serverSnapAcceptedVersion,
			" ",
			cloudSyncRequest.snapshotId,
			" ",
			response.snapshotId,
			" server GUID ",
			response.snapshotGUID
		}));
		bool flag = response.snapshotGUID == cloudSyncRequest.snapshotGUID;
		bool flag2 = cloudSyncRequest is GGServerRequestsBackend.UpdateCloudSyncDataRequest;
		bool flag3 = cloudSyncRequest.snapshotGUID != this.activeSnapshot.localGuid;
		bool flag4 = response.snapshotId > cloudSyncRequest.snapshotId;
		bool flag5 = !string.IsNullOrEmpty(response.snapshotGUID);
		if (!flag2 && !flag3 && flag4)
		{
			UnityEngine.Debug.Log("shouldTakeNewServerVesion");
			this.ResolveConflictTakeTheirs(response);
		}
		else if (!flag && flag5)
		{
			UnityEngine.Debug.Log("is in conflict");
			this.SetConflictedState(cloudSyncRequest);
		}
		else
		{
			UnityEngine.Debug.Log("not in conflict");
			this.UpdateLastKnownServerValues(response.snapshotId, response.snapshotGUID, this.activeSnapshot.localGuid);
		}
		this.HandlePendingRequests();
		if (this.isInConflict && !this.isSynchronizationInProgress && !ConflictResolverBase.instance.ResolveConflict(this.conflict))
		{
			GGSnapshotCloudSync.OnConflict onConflict = this.onConflict;
			if (onConflict == null)
			{
				return;
			}
			onConflict(this.conflict);
		}
	}

	public void HandlePendingRequests()
	{
		if (this.pendingSyncRequests.Count <= 0)
		{
			return;
		}
		GGSnapshotCloudSync.PendingSynchronizeNowRequests pendingSynchronizeNowRequests = this.pendingSyncRequests[0];
		this.pendingSyncRequests.RemoveAt(0);
		this.DoSynchronize(pendingSynchronizeNowRequests.onComplete);
	}

	private void UpdateLastKnownServerValues(int serverSnapshotId, string serverSnapshotGUID, string localGUID)
	{
		this.activeSnapshot.serverSnapAcceptedVersion = serverSnapshotId;
		this.activeSnapshot.serverAcceptedGuid = serverSnapshotGUID;
		this.activeSnapshot.localGuid = localGUID;
		this.SaveActiveSnapshot();
	}

	private void SetConflictedState(GGServerRequestsBackend.CloudSyncRequest conflictedRequest)
	{
		if (this.conflict != null)
		{
			this.conflict.CancelConflict();
		}
		this.conflict = new GGSnapshotCloudSync.CloudSyncConflict(conflictedRequest);
	}

	private bool isInConflict
	{
		get
		{
			return this.conflict != null && !this.conflict.isResolved;
		}
	}

	public void ResolveConflictTakeTheirs(CloudSyncData serverResponseData)
	{
		foreach (CloudSyncData.CloudSyncFile cloudSyncFile in serverResponseData.files)
		{
			byte[] bytes = Convert.FromBase64String(cloudSyncFile.data);
			GGFileIO.instance.Write(cloudSyncFile.key, bytes);
		}
		this.UpdateLastKnownServerValues(serverResponseData.snapshotId, serverResponseData.snapshotGUID, serverResponseData.snapshotGUID);
		ConflictResolverBase.instance.OnConflict();
		BehaviourSingletonInit<GGNotificationCenter>.instance.Broadcast("MessageConflictResolved");
	}

	public void ResolveConflictTakeMine(CloudSyncData serverResponseData)
	{
		this.UpdateLastKnownServerValues(serverResponseData.snapshotId, serverResponseData.snapshotGUID, serverResponseData.snapshotGUID);
		this.SynchronizeNow(new GGServerRequestsBackend.OnComplete(this.HandleSyncRequestResult));
		this.synchronize();
		BehaviourSingletonInit<GGNotificationCenter>.instance.Broadcast("MessageConflictResolved");
	}

	private CloudSyncData GetSnapshotForSync(List<string> files)
	{
		CloudSyncData cloudSyncData = new CloudSyncData();
		cloudSyncData.snapshotId = this.activeSnapshot.localSnapVersion;
		cloudSyncData.snapshotGUID = this.activeSnapshot.localGuid;
		foreach (string text in files)
		{
			CloudSyncData.CloudSyncFile cloudSyncFile = new CloudSyncData.CloudSyncFile();
			cloudSyncFile.key = text;
			cloudSyncFile.guid = "";
			cloudSyncFile.revision = 0;
			byte[] array = this.syncFileIO.Read(text);
			cloudSyncFile.data = ((array != null) ? Convert.ToBase64String(array, 0, array.Length) : "");
			cloudSyncData.files.Add(cloudSyncFile);
		}
		return cloudSyncData;
	}

	public const string MESSAGE_CONFLICT_RESOLVED = "MessageConflictResolved";

	private DateTime lastSyncTime;

	private int applicationPausedTimes;

	private int applicationPausedTimesWhenLastSync;

	public GGSnapshotCloudSync.CanUseCloudSyncDelegate canUseCloudSyncDelegate;

	private List<GGSnapshotCloudSync.PendingSynchronizeNowRequests> pendingSyncRequests = new List<GGSnapshotCloudSync.PendingSynchronizeNowRequests>();

	private GGServerRequestsBackend.ServerRequest lastFinishedRequest;

	private GGServerRequestsBackend.CloudSyncRequest inProgressRequest;

	private GGSnapshotCloudSync.CloudSyncConflict conflict;

	private string snapshotSyncFileName = "snapshotSync.bytes";

	private SnapshotSyncInfo activeSnapshot;

	private GGSnapshotFileIO syncFileIO;

	public delegate void OnConflict(GGSnapshotCloudSync.CloudSyncConflict conflict);

	public delegate bool CanUseCloudSyncDelegate();

	private class PendingSynchronizeNowRequests
	{
		public GGServerRequestsBackend.OnComplete onComplete;
	}

	public class CloudSyncConflict
	{
		public CloudSyncConflict(GGServerRequestsBackend.CloudSyncRequest conflictRequest)
		{
			this.conflictRequest = conflictRequest;
			this.isResolved = false;
		}

		public CloudSyncData serverData
		{
			get
			{
				return this.conflictRequest.GetResponse<CloudSyncData>();
			}
		}

		public bool isResolved
		{
			get
			{
				return this._003CisResolved_003Ek__BackingField;
			}
			protected set
			{
				this._003CisResolved_003Ek__BackingField = value;
			}
		}

		public void CancelConflict()
		{
			this.isResolved = true;
		}

		public void ResolveConflictUsingServerVersion()
		{
			if (this.isResolved)
			{
				return;
			}
			CloudSyncData response = this.conflictRequest.GetResponse<CloudSyncData>();
			(GGFileIOCloudSync.instance as GGSnapshotCloudSync).ResolveConflictTakeTheirs(response);
			this.isResolved = true;
		}

		public void ResolveConflictUsingLocalVersion()
		{
			if (this.isResolved)
			{
				return;
			}
			CloudSyncData response = this.conflictRequest.GetResponse<CloudSyncData>();
			(GGFileIOCloudSync.instance as GGSnapshotCloudSync).ResolveConflictTakeMine(response);
			this.isResolved = true;
		}

		private GGServerRequestsBackend.CloudSyncRequest conflictRequest;

		private bool _003CisResolved_003Ek__BackingField;
	}

	private sealed class _003CDoPeriodicallyTryToSync_003Ed__32 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoPeriodicallyTryToSync_003Ed__32(int _003C_003E1__state)
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
			GGSnapshotCloudSync ggsnapshotCloudSync = this._003C_003E4__this;
			switch (num)
			{
			case 0:
				this._003C_003E1__state = -1;
				break;
			case 1:
				this._003C_003E1__state = -1;
				goto IL_AD;
			case 2:
				this._003C_003E1__state = -1;
				goto IL_ED;
			default:
				return false;
			}
			IL_29:
			this._003CellapsedSecondsSinceHaveLocalChanges_003E5__2 = 0f;
			this._003CcloudSyncTimeDelaySeconds_003E5__3 = ((ggsnapshotCloudSync.lastFinishedRequest != null && ggsnapshotCloudSync.lastFinishedRequest.status > GGServerRequestsBackend.ServerRequest.RequestStatus.Success) ? ConfigBase.instance.cloudSyncTimeDelayWhenRequestFails : ConfigBase.instance.cloudSyncTimeDelay);
			IL_AD:
			if (!ggsnapshotCloudSync.haveLocalChanges || this._003CellapsedSecondsSinceHaveLocalChanges_003E5__2 < this._003CcloudSyncTimeDelaySeconds_003E5__3)
			{
				if (ggsnapshotCloudSync.haveLocalChanges)
				{
					this._003CellapsedSecondsSinceHaveLocalChanges_003E5__2 += RealTime.deltaTime;
				}
				else
				{
					this._003CellapsedSecondsSinceHaveLocalChanges_003E5__2 = 0f;
				}
				this._003C_003E2__current = null;
				this._003C_003E1__state = 1;
				return true;
			}
			this._003CellapsedSecondsSinceHaveLocalChanges_003E5__2 = 0f;
			ggsnapshotCloudSync.SynchronizeIfPossible();
			IL_ED:
			if (!ggsnapshotCloudSync.isSynchronizationInProgress && !ggsnapshotCloudSync.isInConflict)
			{
				goto IL_29;
			}
			this._003C_003E2__current = null;
			this._003C_003E1__state = 2;
			return true;
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

		public GGSnapshotCloudSync _003C_003E4__this;

		private float _003CellapsedSecondsSinceHaveLocalChanges_003E5__2;

		private float _003CcloudSyncTimeDelaySeconds_003E5__3;
	}

	private sealed class _003C_003Ec__DisplayClass39_0
	{
		internal void _003CDoSynchronize_003Eb__0(GGServerRequestsBackend.ServerRequest request)
		{
			this._003C_003E4__this.lastFinishedRequest = request;
			this._003C_003E4__this.inProgressRequest = null;
			this.onComplete(request);
		}

		public GGSnapshotCloudSync _003C_003E4__this;

		public GGServerRequestsBackend.OnComplete onComplete;
	}
}
