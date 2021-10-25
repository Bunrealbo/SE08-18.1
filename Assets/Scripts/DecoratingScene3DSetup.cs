using System;
using System.Collections.Generic;
using JSONData;
using UnityEngine;

public class DecoratingScene3DSetup : MonoBehaviour
{
	public DecoratingScene3DSetup.VisualObject GetForName(string name)
	{
		List<DecoratingScene3DSetup.VisualObject> list = this.visualObjectList;
		for (int i = 0; i < list.Count; i++)
		{
			DecoratingScene3DSetup.VisualObject visualObject = list[i];
			if (visualObject.name.ToLower() == name.ToLower())
			{
				return visualObject;
			}
		}
		return null;
	}

	public void Init()
	{
		Transform transform = base.transform;
		this.visualObjectList.Clear();
		foreach (object obj in transform)
		{
			Transform transform2 = (Transform)obj;
			if (!transform2.name.ToLower().Contains("camera"))
			{
				DecoratingScene3DSetup.VisualObject visualObject = new DecoratingScene3DSetup.VisualObject();
				visualObject.name = transform2.name;
				visualObject.rootTransform = transform2;
				this.FillVisualObject(visualObject, transform2);
				this.visualObjectList.Add(visualObject);
			}
		}
	}

	private void FillVisualObject(DecoratingScene3DSetup.VisualObject visualObject, Transform rootTransform)
	{
		foreach (object obj in rootTransform)
		{
			Transform transform = (Transform)obj;
			string text = transform.name.ToLower();
			if (text.StartsWith("data", StringComparison.Ordinal))
			{
				this.FillVisualObject(visualObject, transform);
			}
			if (text.EndsWith("_collision", StringComparison.Ordinal))
			{
				visualObject.collisionRoot = transform;
				this.FillCollisionConfig(visualObject, visualObject.collisionConfig, transform, null);
			}
			if (text.EndsWith("_dashbox", StringComparison.Ordinal))
			{
				visualObject.dashboxRoot = transform;
				this.FillDashboxConfig(visualObject.dashboxConfig, transform);
			}
			if (text.EndsWith("_pivot", StringComparison.Ordinal))
			{
				this.FillPivotConfig(visualObject.pivotConfig, transform);
			}
		}
	}

	private void FillPivotConfig(DecoratingScene3DSetup.PivotConfig pivotConfig, Transform root)
	{
		Camera camera = this.FindMainCamera();
		Vector3 forward = camera.transform.forward;
		Vector3 position = camera.transform.position;
		foreach (object obj in root)
		{
			Transform transform = (Transform)obj;
			DecoratingScene3DSetup.PivotConfig.Pivot pivot = new DecoratingScene3DSetup.PivotConfig.Pivot();
			pivot.name = transform.name;
			Vector3 position2 = transform.position;
			Vector3 vector = camera.WorldToScreenPoint(position2);
			if (vector.z < 0f)
			{
				vector.y *= -1f;
			}
			pivot.screenPosition = vector;
			pivotConfig.pivots.Add(pivot);
		}
	}

	private void FillDashboxConfig(DecoratingScene3DSetup.DashboxConfig dashboxConfig, Transform root)
	{
		MeshFilter component = root.GetComponent<MeshFilter>();
		Mesh mesh = null;
		if (component != null)
		{
			mesh = component.sharedMesh;
		}
		if (mesh != null)
		{
			this.FillMesh(dashboxConfig, root, mesh);
		}
		foreach (object obj in root)
		{
			Transform root2 = (Transform)obj;
			this.FillDashboxConfig(dashboxConfig, root2);
		}
	}

	private void FillMesh(DecoratingScene3DSetup.DashboxConfig dashboxConfig, Transform transform, Mesh mesh)
	{
		int[] triangles = mesh.triangles;
		Vector3[] vertices = mesh.vertices;
		Camera camera = this.FindMainCamera();
		ShapeGraphShape shapeGraphShape = new ShapeGraphShape();
		dashboxConfig.shapes.Add(shapeGraphShape);
		Vector3 forward = camera.transform.forward;
		Vector3 position = camera.transform.position;
		foreach (Vector3 position2 in vertices)
		{
			Vector3 position3 = transform.TransformPoint(position2);
			Vector3 vector = camera.WorldToScreenPoint(position3);
			if (vector.z < 0f)
			{
				vector.y *= -1f;
			}
			shapeGraphShape.points.Add(new Vector2(vector.x, vector.y));
		}
	}

	private void FillCollisionConfig(DecoratingScene3DSetup.VisualObject visualObject, DecoratingScene3DSetup.CollisionConfig collisionConfig, Transform root, DecoratingScene3DSetup.NamedCollisionConfig namedCollision)
	{
		MeshFilter component = root.GetComponent<MeshFilter>();
		Mesh mesh = null;
		if (component != null)
		{
			mesh = component.sharedMesh;
		}
		if (mesh != null)
		{
			this.FillMesh(visualObject, collisionConfig, root, mesh);
			if (namedCollision != null)
			{
				this.FillMesh(visualObject, namedCollision.collisionConfig, root, mesh);
			}
		}
		foreach (object obj in root)
		{
			Transform transform = (Transform)obj;
			string name = transform.name;
			DecoratingScene3DSetup.NamedCollisionConfig namedCollisionConfig = namedCollision;
			if (namedCollisionConfig == null)
			{
				namedCollisionConfig = new DecoratingScene3DSetup.NamedCollisionConfig();
				visualObject.namedCollisionConfigs.Add(namedCollisionConfig);
				namedCollisionConfig.name = name;
			}
			this.FillCollisionConfig(visualObject, collisionConfig, transform, namedCollisionConfig);
		}
	}

	public Camera FindMainCamera()
	{
		Camera[] componentsInChildren = base.transform.GetComponentsInChildren<Camera>(true);
		if (componentsInChildren == null || componentsInChildren.Length == 0)
		{
			return null;
		}
		return componentsInChildren[0];
	}

	private void FillMesh(DecoratingScene3DSetup.VisualObject visualObject, DecoratingScene3DSetup.CollisionConfig collisionConfig, Transform transform, Mesh mesh)
	{
		int[] triangles = mesh.triangles;
		Vector3[] normals = mesh.normals;
		Vector3[] vertices = mesh.vertices;
		Camera camera = this.FindMainCamera();
		Vector3 forward = camera.transform.forward;
		Vector3 position = camera.transform.position;
		for (int i = 0; i < triangles.Length / 3; i++)
		{
			int num = i * 3;
			int num2 = triangles[num];
			int num3 = triangles[num + 1];
			int num4 = triangles[num + 2];
			DecoratingScene3DSetup.CollisionConfig.Triangle triangle = new DecoratingScene3DSetup.CollisionConfig.Triangle();
			Vector3 position2 = vertices[num2];
			Vector3 position3 = vertices[num3];
			Vector3 position4 = vertices[num4];
			Vector3 vector = transform.TransformPoint(position2);
			Vector3 vector2 = transform.TransformPoint(position3);
			Vector3 vector3 = transform.TransformPoint(position4);
			Vector3 vector4 = camera.WorldToScreenPoint(vector);
			Vector3 vector5 = camera.WorldToScreenPoint(vector2);
			Vector3 vector6 = camera.WorldToScreenPoint(vector3);
			if (vector4.z < 0f)
			{
				vector4.y *= -1f;
			}
			if (vector5.z < 0f)
			{
				vector5.y *= -1f;
			}
			if (vector6.z < 0f)
			{
				vector6.y *= -1f;
			}
			vector4.z = Vector3.Dot(vector - position, forward);
			vector5.z = Vector3.Dot(vector2 - position, forward);
			vector6.z = Vector3.Dot(vector3 - position, forward);
			triangle.p1 = vector4;
			triangle.p2 = vector5;
			triangle.p3 = vector6;
			Vector3 lhs = vector2 - vector;
			Vector3 rhs = vector3 - vector;
			Vector3 vector7 = Vector3.Cross(lhs, rhs);
			if (!(vector7 == Vector3.zero))
			{
				Vector3 rhs2 = vector7;
				if (Vector3.Dot(vector - position, rhs2) <= 0f)
				{
					float distanceFromCamera = Vector3.Dot((vector + vector2 + vector3) / 3f - position, forward);
					triangle.distanceFromCamera = distanceFromCamera;
					collisionConfig.triangles.Add(triangle);
				}
			}
		}
	}

	public int selectedObject;

	[SerializeField]
	public List<DecoratingScene3DSetup.VisualObject> visualObjectList = new List<DecoratingScene3DSetup.VisualObject>();

	[Serializable]
	public class PivotConfig
	{
		public List<DecoratingScene3DSetup.PivotConfig.Pivot> pivots = new List<DecoratingScene3DSetup.PivotConfig.Pivot>();

		[Serializable]
		public class Pivot
		{
			public string name;

			public Vector3 screenPosition;
		}
	}

	[Serializable]
	public class DashboxConfig
	{
		public List<ShapeGraphShape> shapes = new List<ShapeGraphShape>();
	}

	[Serializable]
	public class NamedCollisionConfig
	{
		public string name;

		public DecoratingScene3DSetup.CollisionConfig collisionConfig = new DecoratingScene3DSetup.CollisionConfig();
	}

	[Serializable]
	public class CollisionConfig
	{
		public List<DecoratingScene3DSetup.CollisionConfig.Triangle> triangles = new List<DecoratingScene3DSetup.CollisionConfig.Triangle>();

		[Serializable]
		public class Triangle
		{
			public float distanceFromCamera;

			public Vector3 p1;

			public Vector3 p2;

			public Vector3 p3;
		}
	}

	[Serializable]
	public class VisualObject
	{
		public string name;

		public Transform rootTransform;

		public Transform collisionRoot;

		public Transform dashboxRoot;

		public DecoratingScene3DSetup.CollisionConfig collisionConfig = new DecoratingScene3DSetup.CollisionConfig();

		public DecoratingScene3DSetup.DashboxConfig dashboxConfig = new DecoratingScene3DSetup.DashboxConfig();

		public DecoratingScene3DSetup.PivotConfig pivotConfig = new DecoratingScene3DSetup.PivotConfig();

		public List<DecoratingScene3DSetup.NamedCollisionConfig> namedCollisionConfigs = new List<DecoratingScene3DSetup.NamedCollisionConfig>();
	}
}
