using System;
using ProtoModels;
using UnityEngine;

public class PlayerPositionRequestTest : MonoBehaviour
{
	public void GetPlayerPositionList(PlayerPositionRequestTest.OnComplete onComplete)
	{
		this.onPopulationComplete = onComplete;
		GGServerRequestsBackend.GetPlayerPositionsRequest getPlayerPositionsRequest = new GGServerRequestsBackend.GetPlayerPositionsRequest();
		PlayerPositions playerPositions = new PlayerPositions();
		for (int i = this.startPid; i <= this.endPid; i++)
		{
			PlayerPositions.PlayerPosition playerPosition = new PlayerPositions.PlayerPosition();
			playerPosition.playerId = i.ToString();
			playerPositions.players.Add(playerPosition);
		}
		getPlayerPositionsRequest.AddData(playerPositions);
		BehaviourSingletonInit<GGServerRequestsBackend>.instance.GetPlayerPositionList(getPlayerPositionsRequest, new GGServerRequestsBackend.OnComplete(this.OnRequestComplete));
	}

	public void OnRequestComplete(GGServerRequestsBackend.ServerRequest request)
	{
		if (this.onPopulationComplete != null)
		{
			this.onPopulationComplete();
			this.onPopulationComplete = null;
		}
	}

	public int startPid;

	public int endPid;

	private PlayerPositionRequestTest.OnComplete onPopulationComplete;

	public delegate void OnComplete();
}
