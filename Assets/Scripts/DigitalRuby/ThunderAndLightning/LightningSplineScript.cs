using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	public class LightningSplineScript : LightningBoltPathScriptBase
	{
		private bool SourceChanged()
		{
			if (this.sourcePoints.Count != this.prevSourcePoints.Count)
			{
				return true;
			}
			for (int i = 0; i < this.sourcePoints.Count; i++)
			{
				if (this.sourcePoints[i] != this.prevSourcePoints[i])
				{
					return true;
				}
			}
			return false;
		}

		protected override void Start()
		{
			base.Start();
		}

		protected override void Update()
		{
			base.Update();
		}

		public override void CreateLightningBolt(LightningBoltParameters parameters)
		{
			if (this.LightningPath == null)
			{
				return;
			}
			this.sourcePoints.Clear();
			try
			{
				foreach (GameObject gameObject in this.LightningPath)
				{
					if (gameObject != null)
					{
						this.sourcePoints.Add(gameObject.transform.position);
					}
				}
			}
			catch (NullReferenceException)
			{
				return;
			}
			if (this.sourcePoints.Count < 4)
			{
				UnityEngine.Debug.LogError("To create spline lightning, you need a lightning path with at least " + 4 + " points.");
				return;
			}
			this.Generations = (parameters.Generations = Mathf.Clamp(this.Generations, 1, 5));
			parameters.Points.Clear();
			if (this.previousGenerations != this.Generations || this.previousDistancePerSegment != this.DistancePerSegmentHint || this.SourceChanged())
			{
				this.previousGenerations = this.Generations;
				this.previousDistancePerSegment = this.DistancePerSegmentHint;
				LightningSplineScript.PopulateSpline(parameters.Points, this.sourcePoints, this.Generations, this.DistancePerSegmentHint, this.Camera);
				this.prevSourcePoints.Clear();
				this.prevSourcePoints.AddRange(this.sourcePoints);
				this.savedSplinePoints.Clear();
				this.savedSplinePoints.AddRange(parameters.Points);
			}
			else
			{
				parameters.Points.AddRange(this.savedSplinePoints);
			}
			parameters.SmoothingFactor = (parameters.Points.Count - 1) / this.sourcePoints.Count;
			base.CreateLightningBolt(parameters);
		}

		protected override LightningBoltParameters OnCreateParameters()
		{
			LightningBoltParameters orCreateParameters = LightningBoltParameters.GetOrCreateParameters();
			orCreateParameters.Generator = LightningGeneratorPath.PathGeneratorInstance;
			return orCreateParameters;
		}

		public void Trigger(List<Vector3> points, bool spline)
		{
			if (points.Count < 2)
			{
				return;
			}
			this.Generations = Mathf.Clamp(this.Generations, 1, 5);
			LightningBoltParameters lightningBoltParameters = base.CreateParameters();
			lightningBoltParameters.Points.Clear();
			if (spline && points.Count > 3)
			{
				LightningSplineScript.PopulateSpline(lightningBoltParameters.Points, points, this.Generations, this.DistancePerSegmentHint, this.Camera);
				lightningBoltParameters.SmoothingFactor = (lightningBoltParameters.Points.Count - 1) / points.Count;
			}
			else
			{
				lightningBoltParameters.Points.AddRange(points);
				lightningBoltParameters.SmoothingFactor = 1;
			}
			base.CreateLightningBolt(lightningBoltParameters);
			base.CreateLightningBoltsNow();
		}

		public static void PopulateSpline(List<Vector3> splinePoints, List<Vector3> sourcePoints, int generations, float distancePerSegmentHit, Camera camera)
		{
			splinePoints.Clear();
			PathGenerator.Is2D = (camera != null && camera.orthographic);
			if (distancePerSegmentHit > 0f)
			{
				PathGenerator.CreateSplineWithSegmentDistance(splinePoints, sourcePoints, distancePerSegmentHit / (float)generations, false);
				return;
			}
			PathGenerator.CreateSpline(splinePoints, sourcePoints, sourcePoints.Count * generations * generations, false);
		}

		public const int MaxSplineGenerations = 5;

		public float DistancePerSegmentHint;

		private readonly List<Vector3> prevSourcePoints = new List<Vector3>(new Vector3[]
		{
			Vector3.zero
		});

		private readonly List<Vector3> sourcePoints = new List<Vector3>();

		private List<Vector3> savedSplinePoints = new List<Vector3>();

		private int previousGenerations = -1;

		private float previousDistancePerSegment = -1f;
	}
}
