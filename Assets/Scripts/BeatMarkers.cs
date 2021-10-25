using System;
using System.Collections.Generic;
using UnityEngine;

public class BeatMarkers
{
	private List<Transform> markers = new List<Transform>();

	private AnimationBeats.Sequence sequence;

	private Transform root;

	private bool initialized;
}
