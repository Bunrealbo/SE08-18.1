using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using ProtoModels;
using UnityEngine;

public class GGCloudSyncFileIOSync : GGFileIOCloudSync
{
	public event GGCloudSyncFileIOSync.OnNewCloudData onNewCloudData;

	public void SaveCloudSyncInfo()
	{
		ProtoIO.SaveToFile<CloudSyncInfo>(this.cloudSyncInfoFileName, this.cloudSyncInfo);
	}

	public void Init()
	{
		this.isInSync = false;
		this.syncFileIO = new GGCloudSyncFileIO(this);
		if (!ProtoIO.LoadFromFile<CloudSyncInfo>(this.cloudSyncInfoFileName, out this.cloudSyncInfo))
		{
			this.cloudSyncInfo = new CloudSyncInfo();
			this.cloudSyncInfo.files = new List<CloudSyncInfo.CloudSyncFileInfo>();
			this.SaveCloudSyncInfo();
		}
		foreach (string text in ConflictResolverBase.instance.FilesToSync())
		{
			GGDebug.DebugLog(text);
			if (this.GetCloudSyncFileInfo(text) == null)
			{
				GGDebug.DebugLog("info is null");
				string guid = Guid.NewGuid().ToString();
				this.SetGuid(text, guid);
			}
		}
		this.SyncWithDelay();
	}

	private CloudSyncInfo.CloudSyncFileInfo GetCloudSyncFileInfo(string filename)
	{
		if (this.cloudSyncInfo.files == null)
		{
			return null;
		}
		foreach (CloudSyncInfo.CloudSyncFileInfo cloudSyncFileInfo in this.cloudSyncInfo.files)
		{
			if (cloudSyncFileInfo.filename == filename)
			{
				return cloudSyncFileInfo;
			}
		}
		return null;
	}

	public override bool IsInConflict(string filename)
	{
		CloudSyncInfo.CloudSyncFileInfo cloudSyncFileInfo = this.GetCloudSyncFileInfo(filename);
		return cloudSyncFileInfo != null && cloudSyncFileInfo.isInConflict;
	}

	public override GGFileIO GetDefaultFileIO()
	{
		return this.syncFileIO;
	}

	public override GGFileIO GetCloudFileIO()
	{
		return null;
	}

	public override void synchronize()
	{
		GGDebug.DebugLog("Sending sync");
		if (ConflictResolverBase.instance != null)
		{
			ConflictResolverBase.instance.OnBeginSync();
		}
		if (this.HasStagedChanges())
		{
			CloudSyncData dataListForChangesSinceLastSync = this.GetDataListForChangesSinceLastSync();
			GGServerRequestsBackend.UpdateCloudSyncDataRequest updateCloudSyncDataRequest = new GGServerRequestsBackend.UpdateCloudSyncDataRequest();
			updateCloudSyncDataRequest.AddData(dataListForChangesSinceLastSync);
			BehaviourSingletonInit<GGServerRequestsBackend>.instance.UpdateCSData(updateCloudSyncDataRequest, new GGServerRequestsBackend.OnComplete(this.OnCloudSyncFinished));
			return;
		}
		GGServerRequestsBackend.GetCloudSyncDataRequest dataRequest = new GGServerRequestsBackend.GetCloudSyncDataRequest();
		BehaviourSingletonInit<GGServerRequestsBackend>.instance.GetCSData(dataRequest, new GGServerRequestsBackend.OnComplete(this.OnCloudSyncFinished));
	}

	public void OnCloudSyncFinished(GGServerRequestsBackend.ServerRequest request)
	{
		GGDebug.DebugLog("on cloud sync finished");
		GGServerRequestsBackend.CloudSyncRequest cloudSyncRequest = request as GGServerRequestsBackend.CloudSyncRequest;
		if (request.status != GGServerRequestsBackend.ServerRequest.RequestStatus.Success)
		{
			if (this.onNewCloudData != null)
			{
				this.onNewCloudData(this);
			}
			return;
		}
		CloudSyncData requestData = cloudSyncRequest.GetRequestData();
		foreach (CloudSyncData.CloudSyncFile cloudSyncFile in request.GetResponse<CloudSyncData>().files)
		{
			CloudSyncInfo.CloudSyncFileInfo cloudSyncFileInfo = this.GetCloudSyncFileInfo(cloudSyncFile.key);
			if (cloudSyncFileInfo == null)
			{
				cloudSyncFileInfo = new CloudSyncInfo.CloudSyncFileInfo();
				if (this.cloudSyncInfo.files == null)
				{
					this.cloudSyncInfo.files = new List<CloudSyncInfo.CloudSyncFileInfo>();
				}
				cloudSyncFileInfo.filename = cloudSyncFile.key;
				cloudSyncFileInfo.localGuid = cloudSyncFile.guid;
				this.cloudSyncInfo.files.Add(cloudSyncFileInfo);
			}
			cloudSyncFileInfo.previousServerState = cloudSyncFileInfo.serverState;
			cloudSyncFileInfo.serverState = cloudSyncFile;
			cloudSyncFileInfo.isInConflict = (cloudSyncFileInfo.previousServerState == null || cloudSyncFileInfo.previousServerState.guid != cloudSyncFileInfo.serverState.guid);
			CloudSyncData.CloudSyncFile file = ProtoModelExtensions.GetFile(requestData, cloudSyncFileInfo.filename);
			if (file != null && file.guid == cloudSyncFile.guid)
			{
				cloudSyncFileInfo.isInConflict = false;
			}
		}
		this.SaveCloudSyncInfo();
		if (ConflictResolverBase.instance != null)
		{
			ConflictResolverBase.instance.OnConflict(this);
		}
		if (this.onNewCloudData != null)
		{
			this.onNewCloudData(this);
		}
	}

	public byte[] GetServerBytes(string filename)
	{
		CloudSyncInfo.CloudSyncFileInfo cloudSyncFileInfo = this.GetCloudSyncFileInfo(filename);
		if (cloudSyncFileInfo != null && cloudSyncFileInfo.serverState != null)
		{
			return Convert.FromBase64String(cloudSyncFileInfo.serverState.data);
		}
		return null;
	}

	public byte[] GetPreviousServerBytes(string filename)
	{
		CloudSyncInfo.CloudSyncFileInfo cloudSyncFileInfo = this.GetCloudSyncFileInfo(filename);
		if (cloudSyncFileInfo != null && cloudSyncFileInfo.serverState != null)
		{
			return Convert.FromBase64String(cloudSyncFileInfo.previousServerState.data);
		}
		return null;
	}

	public byte[] GetLocalBytes(string filename)
	{
		return this.syncFileIO.Read(filename);
	}

	public string GetLocalFileGuid(string filename)
	{
		CloudSyncInfo.CloudSyncFileInfo cloudSyncFileInfo = this.GetCloudSyncFileInfo(filename);
		if (cloudSyncFileInfo != null)
		{
			return cloudSyncFileInfo.localGuid;
		}
		return "";
	}

	public string GetServerFileGuid(string filename)
	{
		CloudSyncInfo.CloudSyncFileInfo cloudSyncFileInfo = this.GetCloudSyncFileInfo(filename);
		if (cloudSyncFileInfo != null && cloudSyncFileInfo.serverState != null)
		{
			return cloudSyncFileInfo.serverState.guid;
		}
		return "";
	}

	public void MarkResolved(string filename)
	{
		this.GetCloudSyncFileInfo(filename).isInConflict = false;
	}

	public string GetPreviousServerFileGuid(string filename)
	{
		CloudSyncInfo.CloudSyncFileInfo cloudSyncFileInfo = this.GetCloudSyncFileInfo(filename);
		if (cloudSyncFileInfo != null && cloudSyncFileInfo.previousServerState != null)
		{
			return cloudSyncFileInfo.previousServerState.guid;
		}
		return "";
	}

	public void SetGuid(string filename, string guid)
	{
		CloudSyncInfo.CloudSyncFileInfo cloudSyncFileInfo = this.GetCloudSyncFileInfo(filename);
		if (cloudSyncFileInfo == null)
		{
			cloudSyncFileInfo = new CloudSyncInfo.CloudSyncFileInfo();
			cloudSyncFileInfo.isInConflict = false;
			if (this.cloudSyncInfo.files == null)
			{
				this.cloudSyncInfo.files = new List<CloudSyncInfo.CloudSyncFileInfo>();
			}
			cloudSyncFileInfo.filename = filename;
			this.cloudSyncInfo.files.Add(cloudSyncFileInfo);
		}
		cloudSyncFileInfo.localGuid = guid;
		this.SaveCloudSyncInfo();
		this.SyncWithDelay();
	}

	private bool HasStagedChanges()
	{
		if (this.cloudSyncInfo.files == null)
		{
			return false;
		}
		foreach (CloudSyncInfo.CloudSyncFileInfo cloudSyncFileInfo in this.cloudSyncInfo.files)
		{
			if (cloudSyncFileInfo.serverState == null || cloudSyncFileInfo.localGuid != cloudSyncFileInfo.serverState.guid)
			{
				return true;
			}
		}
		return false;
	}

	private CloudSyncData GetDataListForChangesSinceLastSync()
	{
		GGDebug.DebugLog("changes list");
		CloudSyncData cloudSyncData = new CloudSyncData();
		if (this.cloudSyncInfo.files == null)
		{
			return cloudSyncData;
		}
		foreach (CloudSyncInfo.CloudSyncFileInfo cloudSyncFileInfo in this.cloudSyncInfo.files)
		{
			if (cloudSyncFileInfo.serverState == null || cloudSyncFileInfo.localGuid != cloudSyncFileInfo.serverState.guid)
			{
				GGDebug.DebugLog(cloudSyncFileInfo.filename);
				CloudSyncData.CloudSyncFile cloudSyncFile = new CloudSyncData.CloudSyncFile();
				cloudSyncFile.key = cloudSyncFileInfo.filename;
				cloudSyncFile.guid = cloudSyncFileInfo.localGuid;
				cloudSyncFile.revision = ((cloudSyncFileInfo.serverState != null) ? (cloudSyncFileInfo.serverState.revision + 1) : 0);
				byte[] array = this.syncFileIO.Read(cloudSyncFileInfo.filename);
				cloudSyncFile.data = ((array != null) ? Convert.ToBase64String(array, 0, array.Length) : "");
				GGDebug.DebugLog(cloudSyncFile.data);
				cloudSyncData.files.Add(cloudSyncFile);
			}
		}
		return cloudSyncData;
	}

	public void SyncWithDelay()
	{
		if (!this.isInSync)
		{
			this.isInSync = true;
			this.syncCountForMinute++;
			if (this.cloudSyncStartTime.AddMinutes(1.0) < DateTime.Now)
			{
				this.syncCountForMinute = 0;
				this.cloudSyncStartTime = DateTime.Now;
			}
			base.StartCoroutine(this.DoSyncWithDelay());
		}
	}

	private IEnumerator DoSyncWithDelay()
	{
		return new GGCloudSyncFileIOSync._003CDoSyncWithDelay_003Ed__29(0)
		{
			_003C_003E4__this = this
		};
	}

	private void OnApplicationPause(bool paused)
	{
		if (paused)
		{
			return;
		}
		this.SyncWithDelay();
	}

	private string cloudSyncInfoFileName = "cloudSyncInfo.bytes";

	private CloudSyncInfo cloudSyncInfo;

	private GGCloudSyncFileIO syncFileIO;

	private bool isInSync;

	private DateTime cloudSyncStartTime = DateTime.Now;

	private int syncCountForMinute;

	public delegate void OnNewCloudData(GGCloudSyncFileIOSync sync);

	private sealed class _003CDoSyncWithDelay_003Ed__29 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoSyncWithDelay_003Ed__29(int _003C_003E1__state)
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
			GGCloudSyncFileIOSync ggcloudSyncFileIOSync = this._003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				this._003C_003E1__state = -1;
			}
			else
			{
				this._003C_003E1__state = -1;
				GGDebug.DebugLog("Starting sync");
				this._003CcurrentDelay_003E5__2 = 0f;
				this._003CsyncDelay_003E5__3 = ((ggcloudSyncFileIOSync.syncCountForMinute < ConfigBase.instance.maxSyncFrequency) ? ConfigBase.instance.cloudSyncTimeDelay : Mathf.Max(0f, Convert.ToSingle(ggcloudSyncFileIOSync.cloudSyncStartTime.AddMinutes(1.0).Subtract(DateTime.Now).TotalSeconds)));
			}
			if (this._003CcurrentDelay_003E5__2 >= this._003CsyncDelay_003E5__3)
			{
				ggcloudSyncFileIOSync.synchronize();
				ggcloudSyncFileIOSync.isInSync = false;
				return false;
			}
			this._003CcurrentDelay_003E5__2 += Time.deltaTime;
			this._003C_003E2__current = null;
			this._003C_003E1__state = 1;
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

		public GGCloudSyncFileIOSync _003C_003E4__this;

		private float _003CcurrentDelay_003E5__2;

		private float _003CsyncDelay_003E5__3;
	}
}
