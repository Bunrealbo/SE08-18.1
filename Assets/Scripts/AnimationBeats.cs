using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnimationBeats
{
	[Serializable]
	public class Animation
	{
		public AnimationBeats.Animation.Character character;

		public AnimationBeats.Animation.AnimationType type;

		public string stringValue;

		public Vector3 position;

		public Vector3 rotation;

		public float duration;

		public float transitionDuration;

		public float startOffset;

		public enum AnimationType
		{
			PlayAnimation,
			Say,
			LookAtRoom,
			LookAtCharacter,
			Animate,
			StopLooking,
			SetTransform,
			LookAtMarker,
			SetTransformToMarker,
			LookAtPosition
		}

		public enum Character
		{
			C0,
			C1
		}
	}

	[Serializable]
	public class Beat
	{
		[SerializeField]
		private string name;

		public List<AnimationBeats.Animation> animations = new List<AnimationBeats.Animation>();
	}

	[Serializable]
	public class TestSetup
	{
		public bool ShouldBeOwned(string name)
		{
			return this.objectsToOwn.Contains(name);
		}

		public List<string> objectsToOwn = new List<string>();
	}

	[Serializable]
	public class Sequence
	{
		public string groupName;

		public string animationName;

		public List<AnimationBeats.Beat> beats = new List<AnimationBeats.Beat>();

		public bool isExpressionSet;

		public string expressionString;

		public string openAnimationFor;

		public AnimationBeats.TestSetup testSetup = new AnimationBeats.TestSetup();

		[SerializeField]
		public List<AnimationBeats.Sequence.MarkerTransform> markers = new List<AnimationBeats.Sequence.MarkerTransform>();

		[Serializable]
		public class MarkerTransform
		{
			public string name;

			public Vector3 position;

			public Vector3 rotationEuler;
		}
	}
}
