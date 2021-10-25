using System;
using UnityEngine;

namespace GGMatch3
{
	public class NetBehaviour : SlotComponentBehavoiour
	{
		public override void RemoveFromGame()
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
