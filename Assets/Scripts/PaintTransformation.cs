using System;
using System.Collections.Generic;
using UnityEngine;

public class PaintTransformation : MonoBehaviour
{
	public void Init()
	{
	}

	public void ClearTexturesToColor(Color color)
	{
		for (int i = 0; i < this.paintableTextures.Count; i++)
		{
			GGPaintableTexture ggpaintableTexture = this.paintableTextures[i];
			ggpaintableTexture.ClearToColor(color);
			ggpaintableTexture.ApplyRenderTextureToMaterials();
		}
	}

	public void RenderSphere(GGPSphereCommand.Params sphereParams)
	{
		for (int i = 0; i < this.paintableTextures.Count; i++)
		{
			this.paintableTextures[i].RenderSphere(sphereParams);
		}
	}

	public float FillPercent()
	{
		float num = 0f;
		if (this.paintableTextures.Count == 0)
		{
			return 1f;
		}
		for (int i = 0; i < this.paintableTextures.Count; i++)
		{
			GGPaintableTexture ggpaintableTexture = this.paintableTextures[i];
			num += ggpaintableTexture.PaintInPercentage();
		}
		return num / (float)this.paintableTextures.Count;
	}

	public void ReleaseAll()
	{
		for (int i = 0; i < this.paintableTextures.Count; i++)
		{
			GGPaintableTexture ggpaintableTexture = this.paintableTextures[i];
			ggpaintableTexture.RemoveRenderTextureFromMaterials();
			ggpaintableTexture.ReleaseRenderTexture();
		}
	}

	[SerializeField]
	public List<GGPaintableTexture> paintableTextures = new List<GGPaintableTexture>();
}
