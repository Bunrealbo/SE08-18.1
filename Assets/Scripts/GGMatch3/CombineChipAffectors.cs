using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGMatch3
{
	public class CombineChipAffectors : ChipAffectorBase
	{
		private CombineChipAffectors.Settings settings
		{
			get
			{
				return Match3Settings.instance.combineChipAffectorSettings;
			}
		}

		public override void Clear()
		{
			base.Clear();
			DiscoBallAffector.RemoveFromGame(this.bolts);
			this.bolts.Clear();
			this.Reset(this.swipedSlot.slot);
			this.Reset(this.swipedSlot.mixSlot);
		}

		private void Reset(Slot slot)
		{
			slot.offsetScale = Vector3.one;
			slot.offsetPosition = Vector3.zero;
		}

		public void Init(CompositeAffector.SwipedSlot swipedSlot, Match3Game game)
		{
			this.swipedSlot = swipedSlot;
			this.game = game;
			LightingBolt lightingBolt = game.CreateLightingBoltPowerup();
			lightingBolt.Init(swipedSlot.slot, swipedSlot.mixSlot);
			this.bolts.Add(lightingBolt);
			Slot slot = swipedSlot.slot;
			Slot mixSlot = swipedSlot.mixSlot;
		}

		public override void Update()
		{
			base.Update();
			this.time += Time.deltaTime;
			LightingBolt lightingBolt = this.bolts[0];
			Slot slot = this.swipedSlot.slot;
			Slot mixSlot = this.swipedSlot.mixSlot;
			CombineChipAffectors.Settings settings = this.settings;
			float t = Mathf.InverseLerp(0f, settings.durationToGetClose, this.time);
			float d = Mathf.Lerp(1f, settings.maxScale, t);
			float t2 = Mathf.Lerp(0f, settings.normalizedClosePosition, t);
			slot.offsetPosition = Vector3.Lerp(slot.localPositionOfCenter, mixSlot.localPositionOfCenter, t2) - slot.localPositionOfCenter;
			mixSlot.offsetScale = Vector3.one * d;
			lightingBolt.SetStartPosition(slot.localPositionOfCenter + slot.offsetPosition);
			lightingBolt.SetEndPosition(mixSlot.localPositionOfCenter + mixSlot.offsetPosition);
		}

		private CompositeAffector.SwipedSlot swipedSlot;

		private List<LightingBolt> bolts = new List<LightingBolt>();

		private Match3Game game;

		private float time;

		public class Settings
		{
			public float durationToGetClose = 0.2f;

			public float maxScale = 1.2f;

			public float normalizedClosePosition = 0.5f;
		}
	}
}
