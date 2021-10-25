using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	public class LightningBoltSegmentGroup
	{
		public int SegmentCount
		{
			get
			{
				return this.Segments.Count - this.StartIndex;
			}
		}

		public void Reset()
		{
			this.LightParameters = null;
			this.Segments.Clear();
			this.Lights.Clear();
			this.StartIndex = 0;
		}

		public float LineWidth;

		public int StartIndex;

		public int Generation;

		public float Delay;

		public float PeakStart;

		public float PeakEnd;

		public float LifeTime;

		public float EndWidthMultiplier;

		public Color32 Color;

		public readonly List<LightningBoltSegment> Segments = new List<LightningBoltSegment>();

		public readonly List<Light> Lights = new List<Light>();

		public LightningLightParameters LightParameters;
	}
}
