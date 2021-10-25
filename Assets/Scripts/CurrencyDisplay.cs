using System;
using TMPro;
using UnityEngine;

public class CurrencyDisplay : MonoBehaviour
{
	public void Init(long count)
	{
		this.particleCreator.DestroyCreatedObjects();
		GGUtil.ChangeText(this.countLabel, count);
	}

	public void DisplayCount(long count)
	{
		GGUtil.ChangeText(this.countLabel, count);
	}

	public void ShowShineParticle()
	{
		this.particleCreator.CreateAndRunParticles("Shine", this.currencySymbol);
	}

	[SerializeField]
	private TextMeshProUGUI countLabel;

	[SerializeField]
	private Transform currencySymbol;

	[SerializeField]
	public CurrencyType currencyType;

	[SerializeField]
	private UIGGParticleCreator particleCreator = new UIGGParticleCreator();
}
