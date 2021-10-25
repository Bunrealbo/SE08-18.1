using System;
using System.Collections.Generic;
using GGMatch3;
using UnityEngine;

public class GGPaintableTexture : MonoBehaviour
{
	private RenderTexture renderTexture
	{
		get
		{
			if (this.renderTexture_ == null)
			{
				RenderTextureDescriptor desc = new RenderTextureDescriptor(this.imageSize.x, this.imageSize.y, this.format, 0);
				this.renderTexture_ = new RenderTexture(desc);
			}
			return this.renderTexture_;
		}
	}

	private RenderTexture islandMapTexture
	{
		get
		{
			if (this.islandMapTexture_ == null)
			{
				RenderTextureDescriptor desc = new RenderTextureDescriptor(this.imageSize.x, this.imageSize.y, this.format, 0);
				this.islandMapTexture_ = new RenderTexture(desc);
			}
			return this.islandMapTexture_;
		}
	}

	public void ReleaseRenderTexture()
	{
		if (this.islandMapTexture_ != null)
		{
			this.islandMapTexture_.Release();
			this.islandMapTexture_ = null;
		}
		if (this.renderTexture_ != null)
		{
			this.renderTexture_.Release();
			this.renderTexture_ = null;
		}
	}

	public void RemoveRenderTextureFromMaterials()
	{
		for (int i = 0; i < this.meshes.Count; i++)
		{
			Material[] materials = this.meshes[i].meshRenderer.materials;
			for (int j = 0; j < materials.Length; j++)
			{
				materials[j].SetTexture("_LayerLerp", null);
			}
		}
	}

	public void ApplyRenderTextureToMaterials()
	{
		RenderTexture renderTexture = this.renderTexture;
		for (int i = 0; i < this.meshes.Count; i++)
		{
			foreach (Material material in this.meshes[i].meshRenderer.materials)
			{
				material.SetTexture("_LayerLerp", renderTexture);
				material.SetFloat("_LayerLerpSlider", 1f);
			}
		}
	}

	private void OnDestroy()
	{
		this.ReleaseRenderTexture();
	}

	public void ClearToColor(Color color)
	{
		RenderTexture renderTexture = this.renderTexture;
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = renderTexture;
		GL.Clear(true, true, color);
		RenderTexture.active = active;
		this.CreateIslandMap();
	}

	public void CreateIslandMap()
	{
		RenderTexture islandMapTexture = this.islandMapTexture;
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = islandMapTexture;
		GL.Clear(true, true, new Color(0f, 0f, 0f, 0f));
		Material sharedMaterial = GGMarkIslands.sharedMaterial;
		sharedMaterial.SetColor(GGPaintableShader._Color, Color.red);
		for (int i = 0; i < this.meshes.Count; i++)
		{
			GGPaintableMesh ggpaintableMesh = this.meshes[i];
			if (!ggpaintableMesh.ignoreForRendering)
			{
				Mesh cachedMesh = ggpaintableMesh.cachedMesh;
				Matrix4x4 localToWorldMatrix = ggpaintableMesh.transform.localToWorldMatrix;
				sharedMaterial.SetVector(GGPaintableShader._Channel, GGPaintableShader.IndexToVector(ggpaintableMesh.uvIndex));
				for (int j = 0; j < cachedMesh.subMeshCount; j++)
				{
					int materialIndex = j;
					sharedMaterial.SetPass(0);
					Graphics.DrawMeshNow(cachedMesh, localToWorldMatrix, materialIndex);
				}
			}
		}
		RenderTexture.active = active;
	}

	public float PaintInPercentage()
	{
		RenderTexture renderTexture = this.renderTexture;
		RenderTextureDescriptor descriptor = renderTexture.descriptor;
		descriptor.width = this.countImageSize.x;
		descriptor.height = this.countImageSize.y;
		RenderTexture temporary = RenderTexture.GetTemporary(descriptor);
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = temporary;
		GL.Clear(true, true, Color.black);
		Material sharedMaterial = GGCountCommand.sharedMaterial;
		sharedMaterial.SetTexture(GGPaintableShader._Texture, renderTexture);
		for (int i = 0; i < this.meshes.Count; i++)
		{
			GGPaintableMesh ggpaintableMesh = this.meshes[i];
			if (!ggpaintableMesh.ignoreForRendering)
			{
				Mesh cachedMesh = ggpaintableMesh.cachedMesh;
				Matrix4x4 localToWorldMatrix = ggpaintableMesh.transform.localToWorldMatrix;
				sharedMaterial.SetVector(GGPaintableShader._Channel, GGPaintableShader.IndexToVector(ggpaintableMesh.uvIndex));
				for (int j = 0; j < cachedMesh.subMeshCount; j++)
				{
					int materialIndex = j;
					sharedMaterial.SetPass(0);
					Graphics.DrawMeshNow(cachedMesh, localToWorldMatrix, materialIndex);
				}
			}
		}
		RenderTexture.active = active;
		Texture2D readableCopy = GGCountCommand.GetReadableCopy(temporary, TextureFormat.ARGB32, false);
		Color32[] pixels = readableCopy.GetPixels32();
		UnityEngine.Object.Destroy(readableCopy);
		RenderTexture.ReleaseTemporary(temporary);
		int num = 0;
		int num2 = 0;
		foreach (Color32 color in pixels)
		{
			if (color.g >= 128)
			{
				num++;
				if (color.r > 8)
				{
					num2++;
				}
			}
		}
		return Mathf.InverseLerp(0f, (float)num, (float)num2);
	}

	public void RenderSphere(GGPSphereCommand.Params sphereParams)
	{
		RenderTexture renderTexture = this.renderTexture;
		RenderTexture active = RenderTexture.active;
		RenderTexture renderTexture2 = this.bufferTexture.CopyToBuffer(renderTexture);
		RenderTexture.active = renderTexture;
		Material sharedMaterial = GGPSphereCommand.sharedMaterial;
		sphereParams.SetToMaterial(sharedMaterial);
		sharedMaterial.SetTexture(GGPaintableShader._Texture, renderTexture2);
		renderTexture.DiscardContents();
		for (int i = 0; i < this.meshes.Count; i++)
		{
			GGPaintableMesh ggpaintableMesh = this.meshes[i];
			if (!ggpaintableMesh.ignoreForRendering)
			{
				Mesh cachedMesh = ggpaintableMesh.cachedMesh;
				Matrix4x4 localToWorldMatrix = ggpaintableMesh.transform.localToWorldMatrix;
				sharedMaterial.SetVector(GGPaintableShader._Channel, GGPaintableShader.IndexToVector(ggpaintableMesh.uvIndex));
				for (int j = 0; j < cachedMesh.subMeshCount; j++)
				{
					int materialIndex = j;
					sharedMaterial.SetPass(0);
					Graphics.DrawMeshNow(cachedMesh, localToWorldMatrix, materialIndex);
				}
				foreach (Material material in ggpaintableMesh.meshRenderer.materials)
				{
					material.SetTexture("_LayerLerp", renderTexture);
					material.SetFloat("_LayerLerpSlider", 1f);
				}
			}
		}
		GGGraphics.CopyTexture(renderTexture, renderTexture2);
		Material sharedMaterial2 = GGFixIslandEdges.sharedMaterial;
		sharedMaterial2.SetTexture(GGPaintableShader._MainTex, renderTexture2);
		sharedMaterial2.SetTexture(GGPaintableShader._IlsandMap, this.islandMapTexture);
		sharedMaterial2.SetVector(GGPaintableShader._TexelSize, new Vector4(1f / (float)renderTexture2.width, 1f / (float)renderTexture2.height, (float)renderTexture2.width, (float)renderTexture2.height));
		sharedMaterial2.SetPass(0);
		Graphics.Blit(null, renderTexture, sharedMaterial2);
		RenderTexture.active = active;
		this.bufferTexture.ReleaseBuffer(renderTexture2);
	}

	[SerializeField]
	private IntVector2 imageSize = new IntVector2(128, 128);

	[SerializeField]
	private IntVector2 countImageSize = new IntVector2(64, 64);

	[SerializeField]
	private RenderTextureFormat format;

	[SerializeField]
	private List<GGPaintableMesh> meshes = new List<GGPaintableMesh>();

	[NonSerialized]
	private RenderTexture renderTexture_;

	[NonSerialized]
	private RenderTexture islandMapTexture_;

	[NonSerialized]
	private BufferTexture bufferTexture = new BufferTexture();
}
