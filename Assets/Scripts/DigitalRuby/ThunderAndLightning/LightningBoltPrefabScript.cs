using System;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	public class LightningBoltPrefabScript : LightningBoltPrefabScriptBase
	{
		public override void CreateLightningBolt(LightningBoltParameters parameters)
		{
			parameters.Start = ((this.Source == null) ? parameters.Start : this.Source.transform.position);
			parameters.End = ((this.Destination == null) ? parameters.End : this.Destination.transform.position);
			parameters.StartVariance = this.StartVariance;
			parameters.EndVariance = this.EndVariance;
			base.CreateLightningBolt(parameters);
		}

		public GameObject Source;

		public GameObject Destination;

		public Vector3 StartVariance;

		public Vector3 EndVariance;
	}
}
