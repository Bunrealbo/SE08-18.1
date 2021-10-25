using System;
using UnityEngine;
using UnityEngine.UI;

public class StarConsumeAnimationStar : MonoBehaviour
{
	public void Init()
	{
		GGUtil.SetActive(this.whiteOutImage, false);
		this.mainGroup.alpha = 1f;
	}

	[SerializeField]
	public Image whiteOutImage;

	[SerializeField]
	public CanvasGroup mainGroup;
}
