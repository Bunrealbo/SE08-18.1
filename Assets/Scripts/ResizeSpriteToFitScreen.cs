using System;
using UnityEngine;

public class ResizeSpriteToFitScreen : MonoBehaviour
{
	private void OnEnable()
	{
		this.ResizeSpriteToScreen();
	}

	private void ResizeSpriteToScreen()
	{
		SpriteRenderer component = base.GetComponent<SpriteRenderer>();
		if (component == null)
		{
			return;
		}
		base.transform.localScale = new Vector3(1f, 1f, 1f);
		float x = component.sprite.bounds.size.x;
		float y = component.sprite.bounds.size.y;
		if (Camera.main == null)
		{
			return;
		}
		float num = Camera.main.orthographicSize * 2f;
		float num2 = num / (float)Screen.height * (float)Screen.width;
		base.transform.localScale = new Vector3(num2 / x, num / y);
	}
}
