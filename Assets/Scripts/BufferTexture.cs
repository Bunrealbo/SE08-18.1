using System;
using UnityEngine;

public class BufferTexture
{
	public RenderTexture CopyToBuffer(RenderTexture source)
	{
		RenderTexture temporary = RenderTexture.GetTemporary(source.descriptor);
		Material sharedMaterial = GGClearCommand.sharedMaterial;
		sharedMaterial.SetColor(GGPaintableShader._Color, Color.white);
		sharedMaterial.SetTexture(GGPaintableShader._Texture, source);
		Graphics.Blit(null, temporary, sharedMaterial);
		return temporary;
	}

	public void ReleaseBuffer(RenderTexture texture)
	{
		RenderTexture.ReleaseTemporary(texture);
	}
}
