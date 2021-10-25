using System;
using UnityEngine;

namespace GGMatch3
{
	[Serializable]
	public class WinScreenBoardOutAnimation
	{
		public float duration;

		public Vector3 offset;

		public Vector3 scale = Vector3.one;

		public AnimationCurve outCurve;
	}
}
