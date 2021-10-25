using System;
using UnityEngine;

namespace GGMatch3
{
	[Serializable]
	public class GeneralSettings
	{
		public GeneralSettings.BombRangeType bombRangeType
		{
			get
			{
				return GeneralSettings.BombRangeType.Diamond;
			}
		}

		public GeneralSettings.SeekingRangeType seekingRangeType
		{
			get
			{
				return GeneralSettings.SeekingRangeType.Normal;
			}
		}

		public float chipScaleMult = 1f;

		public float pickupScaleMult = 1f;

		public float bombScaleMult = 1f;

		public float trapScaleMult = 0.9f;

		public float shakeScale;

		public bool preventAutomatchesIfPossible;

		public bool strictAsPossibleToPrevent;

		public bool waitTillBoardStopsForMatches;

		public bool waitIfRocketHitsPowerup;

		public float scoreSpeed = 40f;

		public float lerpSpeed = 2f;

		public GeneralSettings.CameraShakeSettings shakeSettings;

		public enum BombRangeType
		{
			Full,
			Circle,
			Diamond,
			Candy
		}

		public enum SeekingRangeType
		{
			Normal,
			Candy
		}

		[Serializable]
		public class CameraShakeSettings
		{
			[SerializeField]
			public float magnitude = 1f;

			[SerializeField]
			public float roughness;

			[SerializeField]
			public float fadeInTime;

			[SerializeField]
			public float fadeOutTime;

			[SerializeField]
			public Vector3 posInfluence;

			[SerializeField]
			public Vector3 rotInfluence;
		}
	}
}
