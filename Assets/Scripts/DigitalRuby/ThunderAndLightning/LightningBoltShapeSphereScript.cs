using System;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	public class LightningBoltShapeSphereScript : LightningBoltPrefabScriptBase
	{
		public override void CreateLightningBolt(LightningBoltParameters parameters)
		{
			Vector3 start = UnityEngine.Random.insideUnitSphere * this.InnerRadius;
			Vector3 end = UnityEngine.Random.onUnitSphere * this.Radius;
			parameters.Start = start;
			parameters.End = end;
			base.CreateLightningBolt(parameters);
		}

		public float InnerRadius = 0.1f;

		public float Radius = 4f;
	}
}
