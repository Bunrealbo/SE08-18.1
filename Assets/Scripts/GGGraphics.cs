using System;
using UnityEngine;

public class GGGraphics
{
	public static void CopyTexture(RenderTexture from, RenderTexture to)
	{
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = to;
		Material sharedMaterial = GGClearCommand.sharedMaterial;
		sharedMaterial.SetColor(GGPaintableShader._Color, Color.white);
		sharedMaterial.SetTexture(GGPaintableShader._Texture, from);
		sharedMaterial.SetPass(0);
		Graphics.Blit(null, to, sharedMaterial);
		RenderTexture.active = active;
	}
}
