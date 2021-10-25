using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class FXAA : MonoBehaviour
{
	public void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		this.material.SetFloat(FXAA.sharpnessString, this.Sharpness);
		this.material.SetFloat(FXAA.thresholdString, this.Threshold);
		Graphics.Blit(source, destination, this.material);
	}

	public Material material;

	public float Sharpness = 4f;

	public float Threshold = 0.2f;

	private static readonly int sharpnessString = Shader.PropertyToID("_Sharpness");

	private static readonly int thresholdString = Shader.PropertyToID("_Threshold");
}
