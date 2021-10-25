using System;
using System.Collections;
using UnityEngine;

public class CurrencyPurchaseDialogNextButton : MonoBehaviour
{
	public void Init()
	{
		base.transform.localScale = Vector3.one;
	}

	public void AnimateIn(float delay = 0f)
	{
		this.animationIn.Init();
		this.animationIn.Play(delay, null);
	}

	public void AnimateOut(float delay = 0f)
	{
		this.animationIn.Init();
		this.animationOut.Play(delay, null);
	}

	public IEnumerator DoAnimateOut(float delay)
	{
		return this.animationOut.DoPlay(delay, null);
	}

	public IEnumerator DoAnimateIn(float delay)
	{
		return this.animationIn.DoPlay(delay, null);
	}

	[SerializeField]
	private CurrencyPrefabAnimation animationIn;

	[SerializeField]
	private CurrencyPrefabAnimation animationOut;
}
