using System;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	public struct LightningBoltSegment
	{
		public override string ToString()
		{
			return this.Start.ToString() + ", " + this.End.ToString();
		}

		public Vector3 Start;

		public Vector3 End;
	}
}
