using System;
using System.Collections.Generic;
using UnityEngine;

public class CarModelColliders : MonoBehaviour
{
	public void Init(CarModel model)
	{
		Transform transform = base.transform;
		List<Transform> list = new List<Transform>();
		foreach (object obj in transform)
		{
			Transform item = (Transform)obj;
			list.Add(item);
		}
		for (int i = 0; i < list.Count; i++)
		{
			UnityEngine.Object.DestroyImmediate(list[i].gameObject);
		}
		List<CarModelPart> parts = model.parts;
		for (int j = 0; j < parts.Count; j++)
		{
			CarModelPart carModelPart = parts[j];
			GameObject gameObject = new GameObject(carModelPart.gameObject.name);
			gameObject.transform.parent = base.transform;
			CarModelColliders.State state = new CarModelColliders.State();
			state.part = carModelPart;
			carModelPart.colliderRoot = gameObject.transform;
			this.CopyRecursively(gameObject.transform, carModelPart.transform, state, new CarModelColliders.OutState());
		}
	}

	private void CopyRecursively(Transform localParent, Transform searchParent, CarModelColliders.State state, CarModelColliders.OutState outState)
	{
		GGUtil.CopyWorldTransform(searchParent, localParent);
		foreach (object obj in searchParent)
		{
			Transform transform = (Transform)obj;
			CarModelColliders.OutState outState2 = new CarModelColliders.OutState();
			outState2.isInColliderSubtree = outState.isInColliderSubtree;
			outState2.subpart = outState.subpart;
			CarModelSubpart component = transform.GetComponent<CarModelSubpart>();
			if (component != null)
			{
				outState2.subpart = component;
			}
			MeshFilter component2 = transform.GetComponent<MeshFilter>();
			if (component2 != null)
			{
				Mesh sharedMesh = component2.sharedMesh;
			}
			Collider component3 = transform.GetComponent<Collider>();
			bool flag = component3 != null;
			if (transform.name.ToLower().Contains("_collider"))
			{
				outState2.isInColliderSubtree = true;
			}
			GameObject gameObject;
			if (flag)
			{
				outState.hasCollider = true;
				gameObject = UnityEngine.Object.Instantiate<GameObject>(transform.gameObject, localParent);
				this.StripObject(gameObject);
				if (!outState2.isInColliderSubtree)
				{
					component3.enabled = false;
				}
				gameObject.GetComponent<Collider>().enabled = true;
				PartCollider partCollider = gameObject.AddComponent<PartCollider>();
				partCollider.part = state.part;
				partCollider.subpart = outState2.subpart;
			}
			else
			{
				gameObject = new GameObject(transform.name);
			}
			bool flag2 = transform.GetComponent<PaintTransformation>() != null;
			gameObject.name = transform.name;
			gameObject.transform.parent = localParent;
			GGUtil.SetActive(gameObject, true);
			if (!flag2)
			{
				this.CopyRecursively(gameObject.transform, transform, state, outState2);
				if (outState2.hasCollider)
				{
					outState.hasCollider = true;
				}
			}
			if (gameObject.GetComponent<Collider>() == null && !outState2.hasCollider)
			{
				UnityEngine.Object.DestroyImmediate(gameObject);
			}
		}
	}

	private void StripObject(GameObject objectToStrip)
	{
		List<Transform> list = new List<Transform>();
		foreach (object obj in objectToStrip.transform)
		{
			Transform item = (Transform)obj;
			list.Add(item);
		}
		foreach (Component component in objectToStrip.GetComponents(typeof(Component)))
		{
			if (!(component == null) && !(component is Transform) && !(component is Collider))
			{
				UnityEngine.Object.DestroyImmediate(component);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			UnityEngine.Object.DestroyImmediate(list[j].gameObject);
		}
	}

	private class State
	{
		public CarModelPart part;
	}

	private class OutState
	{
		public bool hasCollider;

		public bool isInColliderSubtree;

		public CarModelSubpart subpart;
	}
}
