using System;
using UnityEngine;

public class GameTapToContinueContainer : MonoBehaviour
{
	public bool isTapped
	{
		get
		{
			return this._003CisTapped_003Ek__BackingField;
		}
		protected set
		{
			this._003CisTapped_003Ek__BackingField = value;
		}
	}

	public void Show(Action onTapped)
	{
		GGUtil.Show(this);
		this.isTapped = false;
		this.onTapped = onTapped;
	}

	public void Hide()
	{
		GGUtil.Hide(this);
	}

	public void ButtonCallback_OnTap()
	{
		this.isTapped = true;
		if (this.onTapped != null)
		{
			this.onTapped();
		}
	}

	private Action onTapped;

	private bool _003CisTapped_003Ek__BackingField;
}
