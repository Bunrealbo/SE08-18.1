using System;
using UnityEngine;

public class IBLDescription : ScriptableObject
{
	[SerializeField]
	public float targetDiffuse = 2f;

	[SerializeField]
	public float targetSpecular = 2f;

	[SerializeField]
	public float diffuseLightAmmount = 1f;

	[SerializeField]
	public float specularLightAmmount = 1f;

	[SerializeField]
	public float minLuminosityInCubemap = 1f;

	[SerializeField]
	public float luminosityPower = 1f;

	[SerializeField]
	public float irradianceLuminosity = 1f;

	[SerializeField]
	public Cubemap cubemap;
}
