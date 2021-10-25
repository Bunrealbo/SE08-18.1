using System;
using UnityEngine;

public class GGFixIslandEdges
{
	public static Material sharedMaterial
	{
		get
		{
			if (GGFixIslandEdges.material_ == null)
			{
				GGFixIslandEdges.material_ = GGPaintableShader.Build(GGPaintableShader.Load("GGPaintableTexture/FixIslandEdges"));
			}
			return GGFixIslandEdges.material_;
		}
	}

	private static Material material_;
}
