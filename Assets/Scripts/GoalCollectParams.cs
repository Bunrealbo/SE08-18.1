using System;
using GGMatch3;

public struct GoalCollectParams
{
	public GoalCollectParams(Match3Goals.GoalBase goal, SlotDestroyParams destroyParams)
	{
		this.goal = goal;
		this.destroyParams = destroyParams;
	}

	public Match3Goals.GoalBase goal;

	public SlotDestroyParams destroyParams;
}
