using System;
using UnityEngine;

public class LoadingPanel : MonoBehaviour
{
	public void Show(bool flag)
	{
		base.gameObject.SetActive(flag);
	}
}
