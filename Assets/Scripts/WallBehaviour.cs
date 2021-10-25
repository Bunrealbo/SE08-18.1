using System;
using GGMatch3;
using UnityEngine;

public class WallBehaviour : MonoBehaviour
{
	public void Init(IntVector2 direction)
	{
		GGUtil.SetActive(this, true);
		bool flag = Mathf.Abs(direction.x) > 0;
		bool active = !flag;
		GGUtil.SetActive(this.wallVertical, flag);
		GGUtil.SetActive(this.wallHorizontal, active);
		Vector3 localPosition = new Vector3((float)direction.x, (float)direction.y, 0f) * 0.5f;
		this.wallVertical.localPosition = localPosition;
		this.wallHorizontal.localPosition = localPosition;
	}

	[SerializeField]
	private Transform wallVertical;

	[SerializeField]
	private Transform wallHorizontal;
}
