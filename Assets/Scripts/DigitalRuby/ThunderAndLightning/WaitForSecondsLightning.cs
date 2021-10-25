using System;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	public class WaitForSecondsLightning : CustomYieldInstruction
	{
		public WaitForSecondsLightning(float time)
		{
			this.remaining = time;
		}

		public override bool keepWaiting
		{
			get
			{
				if (this.remaining <= 0f)
				{
					return false;
				}
				this.remaining -= LightningBoltScript.DeltaTime;
				return true;
			}
		}

		private float remaining;
	}
}
