using System;
using System.Collections;
using System.Collections.Generic;

public class GGRequestLimitter : BehaviourSingleton<GGRequestLimitter>
{
	public int GetGroupId()
	{
		int num = this.nextUnusedId;
		this.nextUnusedId = num + 1;
		return num;
	}

	public void Add(GGServerRequestsBackend.ServerRequest request)
	{
		this.pendingRequests.Add(request);
	}

	private void Update()
	{
		this.CheckRunningRequests();
		this.StartNewRequests();
	}

	private void CheckRunningRequests()
	{
		for (int i = this.runningRequests.Count - 1; i >= 0; i--)
		{
			GGRequestLimitter.RunningRequest runningRequest = this.runningRequests[i];
			if (!runningRequest.query.MoveNext())
			{
				if (runningRequest.request.onComplete != null)
				{
					runningRequest.request.onComplete(runningRequest.request);
				}
				this.runningRequests.RemoveAt(i);
			}
		}
	}

	private void StartNewRequests()
	{
		while (this.runningRequests.Count < this.requestLimit && this.pendingRequests.Count > 0)
		{
			GGRequestLimitter.RunningRequest runningRequest = new GGRequestLimitter.RunningRequest();
			runningRequest.request = this.pendingRequests[0];
			this.pendingRequests.RemoveAt(0);
			if (runningRequest.request != null)
			{
				runningRequest.query = runningRequest.request.RequestCoroutine();
				this.runningRequests.Add(runningRequest);
			}
		}
	}

	public void StopRequest(GGServerRequestsBackend.ServerRequest request)
	{
		if (request == null)
		{
			return;
		}
		for (int i = this.pendingRequests.Count - 1; i >= 0; i--)
		{
			if (this.pendingRequests[i] == request)
			{
				this.pendingRequests.RemoveAt(i);
				request.onComplete = null;
			}
		}
		for (int j = this.runningRequests.Count - 1; j >= 0; j--)
		{
			if (this.runningRequests[j].request == request)
			{
				this.runningRequests.RemoveAt(j);
				request.onComplete = null;
			}
		}
	}

	public void StopRequestsWithGroup(int groupId)
	{
		for (int i = this.pendingRequests.Count - 1; i >= 0; i--)
		{
			if (this.pendingRequests[i].groupId == groupId)
			{
				this.pendingRequests.RemoveAt(i);
			}
		}
		for (int j = this.runningRequests.Count - 1; j >= 0; j--)
		{
			if (this.runningRequests[j].request.groupId == groupId)
			{
				this.runningRequests.RemoveAt(j);
			}
		}
	}

	public int requestLimit = 4;

	private int nextUnusedId;

	private List<GGServerRequestsBackend.ServerRequest> pendingRequests = new List<GGServerRequestsBackend.ServerRequest>();

	private List<GGRequestLimitter.RunningRequest> runningRequests = new List<GGRequestLimitter.RunningRequest>();

	public class RunningRequest
	{
		public IEnumerator query;

		public GGServerRequestsBackend.ServerRequest request;
	}
}
