using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	public class LightningBoltPathScript : LightningBoltPathScriptBase
	{
		public override void CreateLightningBolt(LightningBoltParameters parameters)
		{
			Vector3? vector = null;
			List<GameObject> currentPathObjects = base.GetCurrentPathObjects();
			if (currentPathObjects.Count < 2)
			{
				return;
			}
			if (this.nextIndex >= currentPathObjects.Count)
			{
				if (!this.Repeat)
				{
					return;
				}
				if (currentPathObjects[currentPathObjects.Count - 1] == currentPathObjects[0])
				{
					this.nextIndex = 1;
				}
				else
				{
					this.nextIndex = 0;
					this.lastPoint = null;
				}
			}
			try
			{
				if (this.lastPoint == null)
				{
					List<GameObject> list = currentPathObjects;
					int num = this.nextIndex;
					this.nextIndex = num + 1;
					this.lastPoint = new Vector3?(list[num].transform.position);
				}
				vector = new Vector3?(currentPathObjects[this.nextIndex].transform.position);
				if (this.lastPoint != null && vector != null)
				{
					parameters.Start = this.lastPoint.Value;
					parameters.End = vector.Value;
					base.CreateLightningBolt(parameters);
					if ((this.nextInterval -= this.Speed) <= 0f)
					{
						float num2 = UnityEngine.Random.Range(this.SpeedIntervalRange.Minimum, this.SpeedIntervalRange.Maximum);
						this.nextInterval = num2 + this.nextInterval;
						this.lastPoint = vector;
						this.nextIndex++;
					}
				}
			}
			catch (NullReferenceException)
			{
			}
		}

		public void Reset()
		{
			this.lastPoint = null;
			this.nextIndex = 0;
			this.nextInterval = 1f;
		}

		public float Speed = 1f;

		public RangeOfFloats SpeedIntervalRange = new RangeOfFloats
		{
			Minimum = 1f,
			Maximum = 1f
		};

		public bool Repeat = true;

		private float nextInterval = 1f;

		private int nextIndex;

		private Vector3? lastPoint;
	}
}
