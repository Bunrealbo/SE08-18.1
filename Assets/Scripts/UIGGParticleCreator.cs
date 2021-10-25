using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UIGGParticleCreator
{
	public UIGGParticleCreator.ParticleSettings GetSettings(string name)
	{
		for (int i = 0; i < this.settings.Count; i++)
		{
			UIGGParticleCreator.ParticleSettings particleSettings = this.settings[i];
			if (particleSettings.name == name)
			{
				return particleSettings;
			}
		}
		return null;
	}

	public void DestroyCreatedObjects()
	{
		for (int i = 0; i < this.createdGameObjects.Count; i++)
		{
			GameObject gameObject = this.createdGameObjects[i];
			if (!(gameObject == null))
			{
				UnityEngine.Object.Destroy(gameObject);
			}
		}
		this.createdGameObjects.Clear();
	}

	private GameObject Create(GameObject prefab)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
		this.createdGameObjects.Add(gameObject);
		return gameObject;
	}

	public void CreateAndRunParticles(string name, Transform origin)
	{
		UIGGParticleCreator.ParticleSettings particleSettings = this.GetSettings(name);
		if (particleSettings == null)
		{
			return;
		}
		this.CreateAndRunParticles(particleSettings.particlePrefab, particleSettings.parent, origin, particleSettings.keepTransform);
	}

	public void CreateAndRunParticles(GameObject particlePrefab, Transform parent, Transform origin, bool keepTransform)
	{
		if (particlePrefab == null)
		{
			return;
		}
		GameObject gameObject = this.Create(particlePrefab);
		Transform transform = gameObject.transform;
		transform.parent = parent;
		transform.localPosition = Vector3.zero;
		transform.localScale = Vector3.one;
		transform.localRotation = Quaternion.identity;
		if (keepTransform)
		{
			transform.localPosition = parent.InverseTransformPoint(particlePrefab.transform.position);
			transform.rotation = particlePrefab.transform.rotation;
		}
		if (origin != null)
		{
			transform.localPosition = parent.InverseTransformPoint(origin.position);
		}
		ParticleSystem component = transform.GetComponent<ParticleSystem>();
		GGUtil.SetActive(gameObject, true);
		component.Play();
	}

	private List<GameObject> createdGameObjects = new List<GameObject>();

	public List<UIGGParticleCreator.ParticleSettings> settings = new List<UIGGParticleCreator.ParticleSettings>();

	[Serializable]
	public class ParticleSettings
	{
		public string name;

		public GameObject particlePrefab;

		public Transform parent;

		public bool keepTransform;
	}
}
