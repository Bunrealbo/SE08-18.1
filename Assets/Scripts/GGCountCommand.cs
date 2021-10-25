using System;
using UnityEngine;

public class GGCountCommand
{
	public static Texture2D GetReadableCopy(RenderTexture renderTexture, TextureFormat format = TextureFormat.ARGB32, bool mipMaps = false)
	{
		if (renderTexture != null)
		{
			Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height, format, mipMaps, QualitySettings.activeColorSpace == ColorSpace.Linear);
			RenderTexture active = RenderTexture.active;
			RenderTexture.active = renderTexture;
			texture2D.ReadPixels(new Rect(0f, 0f, (float)renderTexture.width, (float)renderTexture.height), 0, 0);
			RenderTexture.active = active;
			texture2D.Apply();
			return texture2D;
		}
		return null;
	}

	public static Material sharedMaterial
	{
		get
		{
			if (GGCountCommand.material_ == null)
			{
				GGCountCommand.material_ = GGPaintableShader.Build(GGPaintableShader.Load("GGPaintableTexture/Count"));
			}
			return GGCountCommand.material_;
		}
	}

	private static Material material_;
}
