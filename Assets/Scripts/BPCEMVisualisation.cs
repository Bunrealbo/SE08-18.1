using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BPCEMVisualisation : MonoBehaviour
{
	private void OnDrawGizmos()
	{
		if (!base.enabled)
		{
			return;
		}
		if (!this.enableGizmos)
		{
			return;
		}
		Renderer component = base.GetComponent<Renderer>();
		if (component == null)
		{
			return;
		}
		if (component.sharedMaterial == null)
		{
			return;
		}
		Color color = Gizmos.color;
		Gizmos.color = Color.green;
		Vector3 b = this.GetPosition(this.center);
		Vector3 vector = this.GetPosition(this.center + this.size) - b;
		Gizmos.DrawWireCube(b, vector);
		Gizmos.color = color;
	}

	private Vector3 WorldToLocal(Vector3 pos)
	{
		return base.transform.InverseTransformPoint(pos);
	}

	private Vector3 GetPositionToSet(Vector3 pos)
	{
		if (this.useLocalValues)
		{
			return this.WorldToLocal(pos);
		}
		return pos;
	}

	private Vector3 GetPosition(Vector3 pos)
	{
		if (this.useLocalValues)
		{
			return base.transform.TransformPoint(pos);
		}
		return pos;
	}

	private void SetParamsFromReflectionProbe()
	{
		if (this.reflectionProbe == null)
		{
			return;
		}
		this.center = this.GetPositionToSet(this.reflectionProbe.bounds.center);
		this.size = 2f * (this.GetPositionToSet(this.reflectionProbe.bounds.max) - this.center);
		this.position = this.GetPositionToSet(this.reflectionProbe.transform.position);
		if (this.reflectionProbe1 == null)
		{
			return;
		}
		this.params1.center = this.GetPositionToSet(this.reflectionProbe1.bounds.center);
		this.params1.size = 2f * (this.GetPositionToSet(this.reflectionProbe1.bounds.max) - this.params1.center);
		this.params1.position = this.GetPositionToSet(this.reflectionProbe1.transform.position);
	}

	private void CreateGameObjectsListFromChildren()
	{
		this.gameObjects.Clear();
		this.materialsList.Clear();
		this.CreateGameObjectsListFromChildren(base.transform, this.materialsList);
		for (int i = 0; i < this.includeOtherTransforms.Count; i++)
		{
			Transform transform = this.includeOtherTransforms[i];
			this.CreateGameObjectsListFromChildren(transform, this.materialsList);
		}
	}

	public void CreateGameObjectsListFromChildren(Transform transform, List<Material> materialsList)
	{
		foreach (object obj in transform)
		{
			Transform transform2 = (Transform)obj;
			if (transform2.GetComponent<BPCEMVisualisation>() == null)
			{
				this.CreateGameObjectsListFromChildren(transform2, materialsList);
			}
			if ((1 << transform2.gameObject.layer & this.includeLayers.value) != 0 || !this.useLayerMask)
			{
				Renderer component = transform2.gameObject.GetComponent<Renderer>();
				if (!(component == null) && !(component.sharedMaterial == null))
				{
					foreach (Material material in component.sharedMaterials)
					{
						if (!(material == null) && material.HasProperty("_BBoxMin") && material.HasProperty("_BBoxMax") && material.HasProperty("_EnviCubeMapPos"))
						{
							materialsList.Add(material);
						}
					}
				}
			}
		}
	}

	public void UpdateMaterials(List<Material> materialsList)
	{
		Vector3 vector = this.GetPosition(this.center);
		Vector3 a = this.GetPosition(this.center + this.size) - vector;
		Vector3 v = this.GetPosition(this.position);
		Vector3 vector2 = this.GetPosition(this.params1.center);
		Vector3 a2 = this.GetPosition(this.params1.center + this.params1.size) - vector2;
		Vector3 v2 = this.GetPosition(this.params1.position);
		if (!this.usePosition)
		{
			v = vector;
			v2 = vector2;
		}
		Vector3 v3 = vector - a / 2f;
		Vector3 v4 = vector + a / 2f;
		Vector3 v5 = vector2 - a2 / 2f;
		Vector3 v6 = vector2 + a2 / 2f;
		for (int i = 0; i < materialsList.Count; i++)
		{
			Material material = materialsList[i];
			if (!(material == null))
			{
				material.SetVector("_BBoxMin", v3);
				material.SetVector("_BBoxMax", v4);
				material.SetVector("_EnviCubeMapPos", v);
				if (material.HasProperty("_BBoxMax1"))
				{
					material.SetVector("_BBoxMin1", v5);
					material.SetVector("_BBoxMax1", v6);
					material.SetVector("_EnviCubeMapPos1", v2);
				}
			}
		}
	}

	public void UpdateMaterials()
	{
		if (this.imageBasedLighting != null)
		{
			Shader.SetGlobalFloat("_GG_DiffuseAmount_", this.imageBasedLighting.diffuseLightAmmount * this.mainLightFactor);
			Shader.SetGlobalFloat("_GG_SpecularAmount_", this.imageBasedLighting.specularLightAmmount * this.mainLightFactor);
			Shader.SetGlobalFloat("_GG_Min_Luminosity_", this.imageBasedLighting.minLuminosityInCubemap);
			Shader.SetGlobalFloat("_GG_Luminosity_Pow_", this.imageBasedLighting.luminosityPower);
			Shader.SetGlobalFloat("_GG_Irradiance_Luminosity_", this.imageBasedLighting.irradianceLuminosity);
			if (Application.isEditor && this.reflectionProbe != null)
			{
				this.reflectionProbe.customBakedTexture = this.imageBasedLighting.cubemap;
			}
		}
		else
		{
			Shader.SetGlobalFloat("_GG_DiffuseAmount_", this.diffuseLightAmmount * this.mainLightFactor);
			Shader.SetGlobalFloat("_GG_SpecularAmount_", this.specularLightAmmount * this.mainLightFactor);
			Shader.SetGlobalFloat("_GG_Min_Luminosity_", this.minLuminosityInCubemap);
			Shader.SetGlobalFloat("_GG_Luminosity_Pow_", this.luminosityPower);
			Shader.SetGlobalFloat("_GG_Irradiance_Luminosity_", this.irradianceLuminosity);
		}
		if (this.setParamsFromReflectionProbe)
		{
			this.SetParamsFromReflectionProbe();
		}
		Vector3 vector = this.GetPosition(this.center);
		Vector3 a = this.GetPosition(this.center + this.size) - vector;
		Vector3 v = this.GetPosition(this.position);
		Vector3 vector2 = this.GetPosition(this.params1.center);
		Vector3 a2 = this.GetPosition(this.params1.center + this.params1.size) - vector2;
		Vector3 v2 = this.GetPosition(this.params1.position);
		if (!this.usePosition)
		{
			v = vector;
			v2 = vector2;
		}
		Vector3 v3 = vector - a / 2f;
		Vector3 v4 = vector + a / 2f;
		Vector3 v5 = vector2 - a2 / 2f;
		Vector3 v6 = vector2 + a2 / 2f;
		if (Application.isEditor && !Application.isPlaying && this.createGameObjectsListFromChildren)
		{
			this.CreateGameObjectsListFromChildren();
		}
		List<Material> list = this.materialsList;
		for (int i = 0; i < list.Count; i++)
		{
			Material material = list[i];
			if (!(material == null))
			{
				material.SetVector("_BBoxMin", v3);
				material.SetVector("_BBoxMax", v4);
				material.SetVector("_EnviCubeMapPos", v);
				if (material.HasProperty("_BBoxMax1"))
				{
					material.SetVector("_BBoxMin1", v5);
					material.SetVector("_BBoxMax1", v6);
					material.SetVector("_EnviCubeMapPos1", v2);
				}
			}
		}
	}

	private void OnEnable()
	{
		this.UpdateMaterials();
	}

	private void Update()
	{
		if (!Application.isEditor)
		{
			return;
		}
		this.UpdateMaterials();
	}

	public Vector3 center;

	public Vector3 size;

	public Vector3 position;

	[SerializeField]
	private bool enableGizmos;

	public List<Transform> includeOtherTransforms = new List<Transform>();

	public bool useLayerMask;

	public LayerMask includeLayers;

	public BPCEMVisualisation.Params params1 = new BPCEMVisualisation.Params();

	public bool useLocalValues;

	public ReflectionProbe reflectionProbe;

	public ReflectionProbe reflectionProbe1;

	public bool setParamsFromReflectionProbe;

	public bool createGameObjectsListFromChildren;

	public bool usePosition;

	public List<GameObject> gameObjects = new List<GameObject>();

	public List<Material> materialsList = new List<Material>();

	[SerializeField]
	private float mainLightFactor = 1f;

	[SerializeField]
	private IBLDescription imageBasedLighting;

	[SerializeField]
	private float diffuseLightAmmount = 1f;

	[SerializeField]
	private float specularLightAmmount = 1f;

	[SerializeField]
	private float minLuminosityInCubemap = 1f;

	[SerializeField]
	private float luminosityPower = 1f;

	[SerializeField]
	private float irradianceLuminosity = 1f;

	private const string _BBoxMin = "_BBoxMin";

	private const string _BBoxMax = "_BBoxMax";

	private const string _EnviCubeMapPos = "_EnviCubeMapPos";

	private const string _BBoxMin1 = "_BBoxMin1";

	private const string _BBoxMax1 = "_BBoxMax1";

	private const string _EnviCubeMapPos1 = "_EnviCubeMapPos1";

	[Serializable]
	public class Params
	{
		public Vector3 center;

		public Vector3 size;

		public Vector3 position;
	}
}
