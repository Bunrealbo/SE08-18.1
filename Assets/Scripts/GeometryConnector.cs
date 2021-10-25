using System;
using System.Collections.Generic;
using UnityEngine;

public class GeometryConnector : MonoBehaviour
{
	public GeometryConnectorSettings FindSettings()
	{
		return base.transform.GetComponentInParent<GeometryConnectorSettings>();
	}

	public void AddLoaded(string path, Transform loadedTransform)
	{
		GeometryConnector.LoadedTransform loadedTransform2 = new GeometryConnector.LoadedTransform();
		loadedTransform2.path = path;
		loadedTransform2.transform = loadedTransform;
		this.loaded.Add(loadedTransform2);
	}

	public void ClearLoaded()
	{
		for (int i = 0; i < this.loaded.Count; i++)
		{
			GeometryConnector.LoadedTransform loadedTransform = this.loaded[i];
			if (!(loadedTransform.transform == null))
			{
				UnityEngine.Object.DestroyImmediate(loadedTransform.transform.gameObject);
			}
		}
		this.loaded.Clear();
	}

	[SerializeField]
	public List<GeometryConnector.Connection> connections = new List<GeometryConnector.Connection>();

	[SerializeField]
	public bool replaceWithMeshColliders;

	[SerializeField]
	public List<GeometryConnector.LoadedTransform> loaded = new List<GeometryConnector.LoadedTransform>();

	[Serializable]
	public class Connection
	{
		public string path;
	}

	[Serializable]
	public class LoadedTransform
	{
		public string path;

		public Transform transform;
	}
}
