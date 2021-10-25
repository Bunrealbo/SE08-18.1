using System;
using UnityEngine;

public class GGPSphereCommand
{
	public static Material sharedMaterial
	{
		get
		{
			if (GGPSphereCommand.material_ == null)
			{
				GGPSphereCommand.material_ = GGPaintableShader.Build(GGPaintableShader.Load("GGPaintableTexture/Sphere"));
			}
			return GGPSphereCommand.material_;
		}
	}

	private static Material material_;

	public struct Params
	{
		public void SetToMaterial(Material material)
		{
			material.SetVector(GGPaintableShader._Position, this.worldPosition);
			material.SetFloat(GGPaintableShader._BrushSize, this.brushSize);
			material.SetFloat(GGPaintableShader._BrushHardness, this.brushHardness);
			material.SetColor(GGPaintableShader._Color, this.brushColor);
		}

		public Vector3 worldPosition;

		public Color brushColor;

		public float brushSize;

		public float brushHardness;
	}
}
