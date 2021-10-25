using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	public class LightningMeshSurfaceScript : LightningBoltPrefabScriptBase
	{
		private void CheckMesh()
		{
			if (this.MeshFilter == null || this.MeshFilter.sharedMesh == null)
			{
				this.meshHelper = null;
				return;
			}
			if (this.MeshFilter.sharedMesh != this.previousMesh)
			{
				this.previousMesh = this.MeshFilter.sharedMesh;
				this.meshHelper = new MeshHelper(this.previousMesh);
			}
		}

		protected override LightningBoltParameters OnCreateParameters()
		{
			LightningBoltParameters lightningBoltParameters = base.OnCreateParameters();
			lightningBoltParameters.Generator = LightningGeneratorPath.PathGeneratorInstance;
			return lightningBoltParameters;
		}

		protected virtual void PopulateSourcePoints(List<Vector3> points)
		{
			if (this.meshHelper != null)
			{
				this.CreateRandomLightningPath(this.sourcePoints);
			}
		}

		public void CreateRandomLightningPath(List<Vector3> points)
		{
			if (this.meshHelper == null)
			{
				return;
			}
			RaycastHit raycastHit = default(RaycastHit);
			this.maximumPathDistanceSquared = this.MaximumPathDistance * this.MaximumPathDistance;
			int num;
			this.meshHelper.GenerateRandomPoint(ref raycastHit, out num);
			raycastHit.distance = UnityEngine.Random.Range(this.MeshOffsetRange.Minimum, this.MeshOffsetRange.Maximum);
			Vector3 vector = raycastHit.point + raycastHit.normal * raycastHit.distance;
			float num2 = UnityEngine.Random.Range(this.MinimumPathDistanceRange.Minimum, this.MinimumPathDistanceRange.Maximum);
			num2 *= num2;
			this.sourcePoints.Add(this.MeshFilter.transform.TransformPoint(vector));
			int num3 = (UnityEngine.Random.Range(0, 1) == 1) ? 3 : -3;
			int num4 = UnityEngine.Random.Range(this.PathLengthCount.Minimum, this.PathLengthCount.Maximum);
			while (num4 != 0)
			{
				num += num3;
				if (num >= 0 && num < this.meshHelper.Triangles.Length)
				{
					this.meshHelper.GetRaycastFromTriangleIndex(num, ref raycastHit);
					raycastHit.distance = UnityEngine.Random.Range(this.MeshOffsetRange.Minimum, this.MeshOffsetRange.Maximum);
					Vector3 vector2 = raycastHit.point + raycastHit.normal * raycastHit.distance;
					float sqrMagnitude = (vector2 - vector).sqrMagnitude;
					if (sqrMagnitude > this.maximumPathDistanceSquared)
					{
						break;
					}
					if (sqrMagnitude >= num2)
					{
						vector = vector2;
						this.sourcePoints.Add(this.MeshFilter.transform.TransformPoint(vector2));
						num4--;
						num2 = UnityEngine.Random.Range(this.MinimumPathDistanceRange.Minimum, this.MinimumPathDistanceRange.Maximum);
						num2 *= num2;
					}
				}
				else
				{
					num3 = -num3;
					num += num3;
					num4--;
				}
			}
		}

		protected override void Start()
		{
			base.Start();
		}

		protected override void Update()
		{
			this.CheckMesh();
			base.Update();
		}

		public override void CreateLightningBolt(LightningBoltParameters parameters)
		{
			if (this.meshHelper == null)
			{
				return;
			}
			this.Generations = (parameters.Generations = Mathf.Clamp(this.Generations, 1, 5));
			this.sourcePoints.Clear();
			this.PopulateSourcePoints(this.sourcePoints);
			if (this.sourcePoints.Count > 1)
			{
				parameters.Points.Clear();
				if (this.Spline && this.sourcePoints.Count > 3)
				{
					LightningSplineScript.PopulateSpline(parameters.Points, this.sourcePoints, this.Generations, this.DistancePerSegmentHint, this.Camera);
					parameters.SmoothingFactor = (parameters.Points.Count - 1) / this.sourcePoints.Count;
				}
				else
				{
					parameters.Points.AddRange(this.sourcePoints);
					parameters.SmoothingFactor = 1;
				}
				base.CreateLightningBolt(parameters);
			}
		}

		public MeshFilter MeshFilter;

		public Collider MeshCollider;

		public RangeOfFloats MeshOffsetRange = new RangeOfFloats
		{
			Minimum = 0.5f,
			Maximum = 1f
		};

		public RangeOfIntegers PathLengthCount = new RangeOfIntegers
		{
			Minimum = 3,
			Maximum = 6
		};

		public RangeOfFloats MinimumPathDistanceRange = new RangeOfFloats
		{
			Minimum = 0.5f,
			Maximum = 1f
		};

		public float MaximumPathDistance = 2f;

		private float maximumPathDistanceSquared;

		public bool Spline;

		public float DistancePerSegmentHint;

		private readonly List<Vector3> sourcePoints = new List<Vector3>();

		private Mesh previousMesh;

		private MeshHelper meshHelper;
	}
}
