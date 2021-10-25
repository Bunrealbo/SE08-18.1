using System;
using System.Collections.Generic;
using UnityEngine;

public class GeometryConnectorSettings : MonoBehaviour
{
	private GeometryConnectorSettings.Link GetLink(string name)
	{
		for (int i = 0; i < this.links.Count; i++)
		{
			GeometryConnectorSettings.Link link = this.links[i];
			if (link.name == name)
			{
				return link;
			}
		}
		return null;
	}

	public void HideAllLinks()
	{
		this.SetAllLinksActive(false);
	}

	public void SetAllLinksActive(bool active)
	{
		for (int i = 0; i < this.links.Count; i++)
		{
			GGUtil.SetActive(this.links[i].gameObject, active);
		}
	}

	public GameObject GetGameObject(string path)
	{
		string[] array = path.Split(new char[]
		{
			']'
		});
		if (array.Length != 2)
		{
			return null;
		}
		string text = array[0].Substring(1);
		GeometryConnectorSettings.Link link = this.GetLink(text);
		if (link == null)
		{
			UnityEngine.Debug.Log("Missing Link " + text);
			return null;
		}
		string searchPath = array[1];
		GeometryConnectorSettings.Output output = new GeometryConnectorSettings.Output();
		this.FindPath(new GeometryConnectorSettings.InParams
		{
			root = link.gameObject.transform,
			searchPath = searchPath,
			stopSearchOnFirst = true
		}, output);
		if (!output.HasFound)
		{
			UnityEngine.Debug.Log("Missing Path " + path);
			return null;
		}
		return output.found[0].transform.gameObject;
	}

	public Vector3 GetWorldPositionUnderLink(GeometryConnectorSettings.Link link, Transform transform)
	{
		if (link.positionRelativeToRoot)
		{
			return link.gameObject.transform.InverseTransformPoint(transform.position) + link.offset;
		}
		return transform.position;
	}

	public GameObject InstantiateGameObject(string path, Transform parent)
	{
		string[] array = path.Split(new char[]
		{
			']'
		});
		if (array.Length != 2)
		{
			return null;
		}
		string text = array[0].Substring(1);
		GeometryConnectorSettings.Link link = this.GetLink(text);
		if (link == null)
		{
			UnityEngine.Debug.Log("Missing Link " + text);
			return null;
		}
		string searchPath = array[1];
		GeometryConnectorSettings.Output output = new GeometryConnectorSettings.Output();
		this.FindPath(new GeometryConnectorSettings.InParams
		{
			root = link.gameObject.transform,
			searchPath = searchPath,
			stopSearchOnFirst = true
		}, output);
		if (!output.HasFound)
		{
			UnityEngine.Debug.Log("Missing Path " + path);
			return null;
		}
		GameObject gameObject = output.found[0].transform.gameObject;
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject, parent);
		gameObject2.name = gameObject.name;
		GGUtil.SetActive(gameObject2, true);
		if (link.positionRelativeToRoot)
		{
			GGUtil.CopyWorldTransform(gameObject.transform, gameObject2.transform);
			gameObject2.transform.position = link.gameObject.transform.InverseTransformPoint(gameObject2.transform.position) + link.offset;
		}
		else
		{
			GGUtil.CopyWorldTransform(gameObject.transform, gameObject2.transform);
		}
		return gameObject2;
	}

	private void FindPath(GeometryConnectorSettings.InParams inParams, GeometryConnectorSettings.Output output)
	{
		if (inParams.searchPath == "" || inParams.searchPath == "/")
		{
			GeometryConnectorSettings.Output.FoundItem foundItem = new GeometryConnectorSettings.Output.FoundItem();
			foundItem.transform = inParams.root;
			foundItem.name = inParams.searchPath;
			output.found.Add(foundItem);
			return;
		}
		if (output.HasFound && inParams.stopSearchOnFirst)
		{
			return;
		}
		foreach (object obj in inParams.root)
		{
			Transform transform = (Transform)obj;
			string text = inParams.namePrefix + "/" + transform.name;
			if (inParams.searchPath == text || inParams.searchPath == transform.name)
			{
				GeometryConnectorSettings.Output.FoundItem foundItem2 = new GeometryConnectorSettings.Output.FoundItem();
				foundItem2.transform = transform;
				foundItem2.name = text;
				output.found.Add(foundItem2);
			}
			else
			{
				GeometryConnectorSettings.InParams inParams2 = inParams;
				inParams2.namePrefix = text;
				inParams2.root = transform;
				this.FindPath(inParams2, output);
			}
			if (output.HasFound && inParams.stopSearchOnFirst)
			{
				break;
			}
		}
	}

	[SerializeField]
	public List<GeometryConnectorSettings.Link> links = new List<GeometryConnectorSettings.Link>();

	[Serializable]
	public class Link
	{
		public string name;

		public GameObject gameObject;

		public bool positionRelativeToRoot;

		public Vector3 offset;
	}

	private struct InParams
	{
		public Transform root;

		public string namePrefix;

		public string searchPath;

		public bool stopSearchOnFirst;
	}

	private class Output
	{
		public bool HasFound
		{
			get
			{
				return this.found.Count > 0;
			}
		}

		public List<GeometryConnectorSettings.Output.FoundItem> found = new List<GeometryConnectorSettings.Output.FoundItem>();

		public class FoundItem
		{
			public Transform transform;

			public string name;
		}
	}
}
