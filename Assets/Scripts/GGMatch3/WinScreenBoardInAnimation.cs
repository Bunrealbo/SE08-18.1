using System;
using UnityEngine;

namespace GGMatch3
{
	[Serializable]
	public class WinScreenBoardInAnimation
	{
		public float duration;

		public Vector3 offset;

		public Vector3 scale = Vector3.one;

		public AnimationCurve curve;
	}
}
