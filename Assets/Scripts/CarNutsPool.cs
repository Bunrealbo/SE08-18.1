using System;
using UnityEngine;

[Serializable]
public class CarNutsPool
{
	public void Clear()
	{
		this.nutComponents.Clear();
		this.nutComponents.HideNotUsed();
	}

	public CarNut NextNut()
	{
		return this.nutComponents.Next<CarNut>(false);
	}

	[SerializeField]
	private ComponentPool nutComponents = new ComponentPool();
}
