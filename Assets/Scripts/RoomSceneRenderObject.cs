using System;
using UnityEngine;

public class RoomSceneRenderObject : MonoBehaviour
{
	public float animationAlpha
	{
		get
		{
			return this.animationAlpha_;
		}
		set
		{
			this.animationAlpha_ = value;
			this.ApplyAlpha(this.alpha, this.animationAlpha_);
		}
	}

	public void SetAlpha(float alpha)
	{
		this.alpha = alpha;
		this.alphaChange.isActive = false;
	}

	public void AnimateAlphaTo(float endAlpha, float duration, DecorateRoomScreen screen)
	{
		this.alphaChange.isActive = true;
		this.alphaChange.screen = screen;
		this.alphaChange.startAlpha = this.alpha;
		this.alphaChange.endAlpha = endAlpha;
		this.alphaChange.duration = duration;
		this.alphaChange.time = 0f;
	}

	private float alpha
	{
		get
		{
			return this.alpha_;
		}
		set
		{
			this.alpha_ = value;
			this.ApplyAlpha(this.alpha_, this.animationAlpha);
		}
	}

	private void ApplyAlpha(float alpha, float animationAlpha)
	{
		float a = alpha * animationAlpha;
		if (this.textureMaterial == null)
		{
			return;
		}
		Color color = this.textureMaterial.color;
		color.a = a;
		this.textureMaterial.color = color;
	}

	private void Update()
	{
		if (!this.alphaChange.isActive)
		{
			return;
		}
		this.alphaChange.time = this.alphaChange.time + Time.deltaTime;
		float num = Mathf.InverseLerp(0f, this.alphaChange.duration, this.alphaChange.time);
		this.alpha = Mathf.Lerp(this.alphaChange.startAlpha, this.alphaChange.endAlpha, num);
		if (this.alphaChange.screen != null)
		{
			this.alphaChange.screen.SetSpeachBubbleAlpha(this.alpha);
		}
		if (num >= 1f)
		{
			this.alphaChange.isActive = false;
		}
	}

	[SerializeField]
	private Material textureMaterial;

	[NonSerialized]
	private float animationAlpha_ = 1f;

	[NonSerialized]
	private float alpha_ = 1f;

	private RoomSceneRenderObject.AlphaChange alphaChange;

	private struct AlphaChange
	{
		public bool isActive;

		public float startAlpha;

		public float endAlpha;

		public float duration;

		public float time;

		public DecorateRoomScreen screen;
	}
}
