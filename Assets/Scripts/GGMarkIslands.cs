using System;
using UnityEngine;

public class GGMarkIslands
{
	public static Material sharedMaterial
	{
		get
		{
			if (GGMarkIslands.material_ == null)
			{
				GGMarkIslands.material_ = GGPaintableShader.Build(GGPaintableShader.Load("GGPaintableTexture/MarkIslands"));
			}
			return GGMarkIslands.material_;
		}
	}

	private static Material material_;
}
