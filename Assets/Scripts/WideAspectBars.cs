using System;
using UnityEngine;

public class WideAspectBars : MonoBehaviour
{
	public void Hide()
	{
		this.SetAlpha(0f);
		GGUtil.SetActive(base.gameObject, false);
	}

	public void SetAlpha(float alpha)
	{
		this.state = default(WideAspectBars.AlphaState);
		this.canvasGroup.alpha = alpha;
	}

	public void AnimateShow()
	{
		this.state = default(WideAspectBars.AlphaState);
		this.canvasGroup.alpha = 0f;
		this.state.startAlpha = this.canvasGroup.alpha;
		this.state.endAlpha = 1f;
		GGUtil.SetActive(this, true);
		this.state.duration = 1f;
		this.state.isActive = true;
	}

	public void AnimateHide()
	{
		this.state = default(WideAspectBars.AlphaState);
		this.state.startAlpha = this.canvasGroup.alpha;
		this.state.endAlpha = 0f;
		GGUtil.SetActive(this, true);
		this.state.duration = 1f;
		this.state.hideAtEnd = true;
		this.state.isActive = true;
	}

	private void Update()
	{
		if (this.state.notActive)
		{
			return;
		}
		this.state.time = this.state.time + Time.deltaTime;
		float t = Mathf.InverseLerp(0f, this.state.duration, this.state.time);
		float alpha = Mathf.Lerp(this.state.startAlpha, this.state.endAlpha, t);
		this.canvasGroup.alpha = alpha;
		if (this.state.time >= this.state.duration)
		{
			this.state.isActive = false;
			if (this.state.hideAtEnd)
			{
				this.Hide();
			}
		}
	}

	[SerializeField]
	private CanvasGroup canvasGroup;

	private WideAspectBars.AlphaState state;

	private struct AlphaState
	{
		public bool notActive
		{
			get
			{
				return !this.isActive;
			}
		}

		public bool isActive;

		public float startAlpha;

		public float endAlpha;

		public float time;

		public float duration;

		public bool hideAtEnd;
	}
}
