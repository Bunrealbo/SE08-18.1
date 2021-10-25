using System;
using UnityEngine;

public class GGClearCommand
{
	public static Material sharedMaterial
	{
		get
		{
			if (GGClearCommand.material_ == null)
			{
				GGClearCommand.material_ = GGPaintableShader.Build(GGPaintableShader.Load("GGPaintableTexture/Clear"));
			}
			return GGClearCommand.material_;
		}
	}

	private static Material material_;
}
