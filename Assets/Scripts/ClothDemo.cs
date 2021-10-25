using System;
using System.Collections.Generic;
using GGCloth;
using UnityEngine;

public class ClothDemo : MonoBehaviour
{
	private float maxDifferenceMagnitude
	{
		get
		{
			return this.maxDifferenceMagnitudeLocal * this.boardScale;
		}
	}

	private int GetPointIndex(int column, int row, int columnCount)
	{
		return column + row * (columnCount + 1);
	}

	private void Start()
	{
		this.Init();
	}

	private void LateUpdate()
	{
		Vector3 vector = this.centralTargetPosition - this.positionConstraint.centralPosition;
		vector.z = 0f;
		if (this.sleepAfterTime)
		{
			if (vector.x > this.minMoveDistance || vector.y > this.minMoveDistance)
			{
				this.lastMoveTime = Time.time;
			}
			float num = Time.time - this.lastMoveTime;
			if (this.sleepAfterTime && num > this.maxTimeBeforeSleep)
			{
				this.isSleeping = true;
				return;
			}
			this.isSleeping = false;
		}
		if (this.directlyFollow)
		{
			List<PointMass> points = this.cloth.pointWorld.Points;
			for (int i = 0; i < points.Count; i++)
			{
				PointMass pointMass = points[i];
				pointMass.currentPosition += vector;
				pointMass.SetRestingPostion(pointMass.currentPosition);
			}
		}
		else if (this.useMaxDifferenceMagnitude)
		{
			vector.z = 0f;
			float magnitude = vector.magnitude;
			if (magnitude > this.maxDifferenceMagnitude)
			{
				Vector3 b = vector.normalized * (magnitude - this.maxDifferenceMagnitude);
				List<PointMass> points2 = this.cloth.pointWorld.Points;
				for (int j = 0; j < points2.Count; j++)
				{
					PointMass pointMass2 = points2[j];
					pointMass2.currentPosition += b;
					pointMass2.previosPosition += b;
				}
			}
		}
		if (this.centralTarget != null)
		{
			this.positionConstraint.centralPosition = this.centralTargetPosition;
		}
		this.cloth.pointWorld.SetGravity(this.gravity);
		this.cloth.pointWorld.constraintRelaxationSteps = Mathf.Max(1, this.constraintRelaxationSteps);
		this.cloth.pointWorld.Step(Time.deltaTime);
		this.UpdateMaterialSettings();
		this.clothRenderer.DoUpdateMesh();
	}

	public void UpdateMaterialSettings()
	{
		if (!Application.isEditor)
		{
			return;
		}
		this.clothRenderer.UpdateMaterialSettings();
	}

	public Vector3 centralTargetPosition
	{
		get
		{
			if (this.useLocalPosition)
			{
				return this.localPositionTarget.localPosition;
			}
			return this.centralTarget.position;
		}
	}

	public void Init()
	{
		this.cloth.isWorldPosition = !this.useLocalPosition;
		if (this.useLocalPosition)
		{
			this.cloth.localPositionTransform = this.localPositionTarget;
		}
		this.cloth.stiffnessRandom = this.stiffnessRandom;
		this.cloth.Init(this.columnCount, this.rowCount, this.size, this.damping, this.stiffness, this.centralTargetPosition);
		this.cloth.pointWorld.SetGravity(this.gravity);
		int num = this.cloth.columnCount;
		if (this.centralTarget != null)
		{
			this.positionConstraint.Init(this.centralTargetPosition);
			int num2 = this.columnCount / 2;
			int num3 = this.columnCount % 2 + 1;
			int num4 = this.rowCount / 2;
			int num5 = this.rowCount % 2 + 1;
			if (this.attachPos == ClothDemo.AttachPos.Bottom)
			{
				for (int i = 0; i <= num; i++)
				{
					PointMass point = this.cloth.pointWorld.GetPoint(this.cloth.GetPointIndex(i, 0));
					this.positionConstraint.FixPoint(point);
				}
			}
			else if (this.attachPos == ClothDemo.AttachPos.Center)
			{
				for (int j = 0; j < num3; j++)
				{
					for (int k = 0; k < num5; k++)
					{
						PointMass point2 = this.cloth.pointWorld.GetPoint(this.cloth.GetPointIndex(num2 + j, num4 + k));
						this.positionConstraint.FixPoint(point2);
					}
				}
			}
			else if (this.attachPos == ClothDemo.AttachPos.BottomCenter)
			{
				for (int l = 0; l < num3; l++)
				{
					for (int m = 0; m < num5; m++)
					{
						PointMass point3 = this.cloth.pointWorld.GetPoint(this.cloth.GetPointIndex(num2 + l, 0));
						this.positionConstraint.FixPoint(point3);
					}
				}
			}
			else if (this.attachPos == ClothDemo.AttachPos.Top)
			{
				PointMass point4 = this.cloth.pointWorld.GetPoint(this.cloth.GetPointIndex(0, this.columnCount));
				this.positionConstraint.FixPoint(point4);
				point4 = this.cloth.pointWorld.GetPoint(this.cloth.GetPointIndex(this.rowCount, this.columnCount));
				this.positionConstraint.FixPoint(point4);
			}
			else if (this.attachPos == ClothDemo.AttachPos.WholeCenter)
			{
				for (int n = 0; n <= num; n++)
				{
					for (int num6 = 0; num6 < num5; num6++)
					{
						PointMass point5 = this.cloth.pointWorld.GetPoint(this.cloth.GetPointIndex(n, num4 + num6));
						this.positionConstraint.FixPoint(point5);
					}
				}
			}
			this.cloth.pointWorld.Prepend(this.positionConstraint);
		}
		this.clothRenderer.SetCloth(this.cloth);
		this.clothRenderer.DoUpdateMesh();
	}

	public void ScaleOutBy(float scaleOut)
	{
		List<PointMass> points = this.cloth.pointWorld.Points;
		for (int i = 0; i < points.Count; i++)
		{
			points[i].currentPosition *= scaleOut;
		}
	}

	public void MoveBy(Vector3 offset)
	{
		List<PointMass> points = this.cloth.pointWorld.Points;
		int pointIndex = this.cloth.GetPointIndex((int)Mathf.Lerp(0f, (float)this.cloth.columnCount, this.column), (int)Mathf.Lerp(0f, (float)this.cloth.rowCount, this.row));
		points[pointIndex].currentPosition += offset;
	}

	public void MoveAllBy(Vector3 offset)
	{
		List<PointMass> points = this.cloth.pointWorld.Points;
		for (int i = 0; i < points.Count; i++)
		{
			points[i].currentPosition += offset;
		}
	}

	[SerializeField]
	private bool useLocalPosition;

	[SerializeField]
	private bool isSleeping;

	[SerializeField]
	public bool useMaxDifferenceMagnitude;

	[SerializeField]
	private float maxDifferenceMagnitudeLocal = 0.25f;

	[SerializeField]
	private float maxTimeBeforeSleep = 4f;

	[SerializeField]
	private bool sleepAfterTime;

	[SerializeField]
	private ClothDemo.AttachPos attachPos;

	[SerializeField]
	private Transform centralTarget;

	[SerializeField]
	private Transform localPositionTarget;

	[SerializeField]
	public bool drawGrid;

	[SerializeField]
	public float scaleOutBy = 1f;

	[SerializeField]
	public float stiffnessRandom = 0.05f;

	[SerializeField]
	public Vector3 moveBy;

	[SerializeField]
	private int constraintRelaxationSteps = 1;

	[NonSerialized]
	public SquareCloth cloth = new SquareCloth();

	[SerializeField]
	private SquareClothRenderer clothRenderer;

	[SerializeField]
	private Vector3 size = new Vector3(1f, 1f, 0f);

	[SerializeField]
	public Vector3 gravity = Vector3.down * 9.81f;

	[SerializeField]
	private float damping = 0.5f;

	[SerializeField]
	private float stiffness = 0.5f;

	[SerializeField]
	private int rowCount = 4;

	[SerializeField]
	private int columnCount = 4;

	[SerializeField]
	private float minMoveDistance = 0.001f;

	[SerializeField]
	private float row;

	[SerializeField]
	private float column;

	[SerializeField]
	public float impulseMult = 0.2f;

	public bool directlyFollow;

	public bool debugOut;

	public float boardScale = 1f;

	private float lastMoveTime = -100f;

	private MultiPointAttachConstraint positionConstraint = new MultiPointAttachConstraint();

	public enum AttachPos
	{
		Bottom,
		Center,
		BottomCenter,
		Top,
		WholeCenter
	}
}
