using System;

namespace DigitalRuby.ThunderAndLightning
{
	public class LightningQualityMaximum
	{
		public int MaximumGenerations
		{
			get
			{
				return this._003CMaximumGenerations_003Ek__BackingField;
			}
			set
			{
				this._003CMaximumGenerations_003Ek__BackingField = value;
			}
		}

		public float MaximumLightPercent
		{
			get
			{
				return this._003CMaximumLightPercent_003Ek__BackingField;
			}
			set
			{
				this._003CMaximumLightPercent_003Ek__BackingField = value;
			}
		}

		public float MaximumShadowPercent
		{
			get
			{
				return this._003CMaximumShadowPercent_003Ek__BackingField;
			}
			set
			{
				this._003CMaximumShadowPercent_003Ek__BackingField = value;
			}
		}

		private int _003CMaximumGenerations_003Ek__BackingField;

		private float _003CMaximumLightPercent_003Ek__BackingField;

		private float _003CMaximumShadowPercent_003Ek__BackingField;
	}
}
