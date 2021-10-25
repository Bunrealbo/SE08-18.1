using System;
using UnityEngine;

namespace EZCameraShake
{
	public class CameraUtilities
	{
		public static Vector3 MultiplyVectors(Vector3 v, Vector3 w)
		{
			v.x *= w.x;
			v.y *= w.y;
			v.z *= w.z;
			return v;
		}
	}
}
