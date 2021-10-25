using System;
using System.Collections.Generic;
using GGMatch3;
using UnityEngine;

public class SlotsRendererPool : MonoBehaviour
{
	public void ReturnRenderer(TilesBorderRenderer renderer)
	{
		if (renderer == null)
		{
			return;
		}
		if (this.availableBorderRenderers.Contains(renderer))
		{
			return;
		}
		GGUtil.Hide(renderer);
		this.availableBorderRenderers.Add(renderer);
	}

	private TilesBorderRenderer CreateFromPrefab()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.prefab, this.parent);
		if (this.resetScale)
		{
			gameObject.transform.localScale = Vector3.one;
		}
		if (this.resetPosition)
		{
			gameObject.transform.localPosition = Vector3.zero;
		}
		return gameObject.GetComponent<TilesBorderRenderer>();
	}

	public TilesBorderRenderer Next()
	{
		TilesBorderRenderer tilesBorderRenderer = null;
		if (this.availableBorderRenderers.Count > 0)
		{
			int index = this.availableBorderRenderers.Count - 1;
			TilesBorderRenderer tilesBorderRenderer2 = this.availableBorderRenderers[index];
			this.availableBorderRenderers.RemoveAt(index);
			tilesBorderRenderer = tilesBorderRenderer2;
		}
		if (tilesBorderRenderer == null)
		{
			tilesBorderRenderer = this.CreateFromPrefab();
		}
		GGUtil.Show(tilesBorderRenderer);
		return tilesBorderRenderer;
	}

	[SerializeField]
	private Transform parent;

	[SerializeField]
	private GameObject prefab;

	[SerializeField]
	private bool resetScale = true;

	[SerializeField]
	private bool resetPosition = true;

	private List<TilesBorderRenderer> availableBorderRenderers = new List<TilesBorderRenderer>();
}
