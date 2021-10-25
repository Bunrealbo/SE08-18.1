using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GGUtil
{
	public static void SetScale(Transform transform, Vector3 scale)
	{
		if (transform == null)
		{
			return;
		}
		transform.localScale = scale;
	}

	public static void SetFill(Image image, float fillAmount)
	{
		if (image == null)
		{
			return;
		}
		image.fillAmount = fillAmount;
	}

	public static void SetSprite(Image image, Sprite sprite)
	{
		if (image == null)
		{
			return;
		}
		image.sprite = sprite;
	}

	public static void SetAlpha(Image image, float alpha)
	{
		if (image == null)
		{
			return;
		}
		Color color = image.color;
		color.a = alpha;
		image.color = color;
	}

	public static void SetColor(Image image, Color color)
	{
		if (image == null)
		{
			return;
		}
		image.color = color;
	}

	public static void SetAlpha(CanvasGroup canvasGroup, float alpha)
	{
		if (canvasGroup == null)
		{
			return;
		}
		canvasGroup.alpha = alpha;
	}

	public static void Call(Action action)
	{
		if (action != null)
		{
			action();
		}
	}

	public static bool HasText(string text)
	{
		return !string.IsNullOrWhiteSpace(text);
	}

	public static void ChangeText(TextMeshProUGUI label, int text)
	{
		if (label == null)
		{
			return;
		}
		GGUtil.ChangeText(label, text.ToString());
	}

	public static void ChangeText(TextMeshProUGUI label, long text)
	{
		if (label == null)
		{
			return;
		}
		GGUtil.ChangeText(label, text.ToString());
	}

	public static void ChangeText(TextMeshProUGUI label, string text)
	{
		if (label == null)
		{
			return;
		}
		if (label.text == text)
		{
			return;
		}
		label.text = text;
	}

	public static void SetActive(List<Transform> transform, bool active)
	{
		if (transform == null)
		{
			return;
		}
		for (int i = 0; i < transform.Count; i++)
		{
			GGUtil.SetActive(transform[i], active);
		}
	}

	public static void SetActive(Transform[] transform, bool active)
	{
		if (transform == null)
		{
			return;
		}
		for (int i = 0; i < transform.Length; i++)
		{
			GGUtil.SetActive(transform[i], active);
		}
	}

	public static void SetActive(Transform transform, bool active)
	{
		if (transform == null)
		{
			return;
		}
		GGUtil.SetActive(transform.gameObject, active);
	}

	public static void SetActive(ParticleSystem ps, bool active)
	{
		if (ps == null)
		{
			return;
		}
		GGUtil.SetActive(ps.gameObject, active);
	}

	public static void SetActive(MonoBehaviour beh, bool active)
	{
		if (beh == null)
		{
			return;
		}
		GGUtil.SetActive(beh.gameObject, active);
	}

	public static void Hide(List<Transform> list)
	{
		GGUtil.SetActive(list, false);
	}

	public static void Hide(Transform[] list)
	{
		GGUtil.SetActive(list, false);
	}

	public static void Hide(List<RectTransform> list)
	{
		GGUtil.SetActive(list, false);
	}

	public static void Hide(GameObject beh)
	{
		GGUtil.SetActive(beh, false);
	}

	public static void Hide(ParticleSystem particleSystem)
	{
		GGUtil.SetActive(particleSystem, false);
	}

	public static void Hide(MonoBehaviour beh)
	{
		GGUtil.SetActive(beh, false);
	}

	public static void Hide(Transform t)
	{
		GGUtil.SetActive(t, false);
	}

	public static void Hide(RectTransform t)
	{
		GGUtil.SetActive(t, false);
	}

	public static void Show(Transform trans)
	{
		GGUtil.SetActive(trans, true);
	}

	public static void Show(ParticleSystem ps)
	{
		GGUtil.SetActive(ps, true);
	}

	public static void Show(TrailRenderer trail)
	{
		GGUtil.SetActive(trail, true);
	}

	public static void SetActive(TrailRenderer trail, bool active)
	{
		if (trail == null)
		{
			return;
		}
		trail.gameObject.SetActive(active);
	}

	public static void Show(GameObject trans)
	{
		GGUtil.SetActive(trans, true);
	}

	public static void Show(RectTransform trans)
	{
		GGUtil.SetActive(trans, true);
	}

	public static void Show(MonoBehaviour beh)
	{
		GGUtil.SetActive(beh, true);
	}

	public static void SetActive(List<RectTransform> transList, bool active)
	{
		if (transList == null)
		{
			return;
		}
		for (int i = 0; i < transList.Count; i++)
		{
			GGUtil.SetActive(transList[i], active);
		}
	}

	public static void SetActive(RectTransform trans, bool active)
	{
		if (trans == null)
		{
			return;
		}
		GGUtil.SetActive(trans.gameObject, active);
	}

	public static bool isPartOfHierarchy(GameObject go)
	{
		return !(go == null) && go.transform.parent != null;
	}

	public static void SetActive(GameObject go, bool active)
	{
		if (go == null)
		{
			return;
		}
		if (go.activeSelf == active)
		{
			return;
		}
		go.SetActive(active);
	}

	public static float VisualRotationAngleUpAxis(Vector3 vec)
	{
		float num = Vector3.Angle(Vector3.up, vec);
		if (vec.x < 0f || vec.y < 0f)
		{
			num *= -1f;
		}
		return num;
	}

	public static float SignedAngle(Vector2 from, Vector2 to)
	{
		float num = Vector2.Angle(from, to);
		float num2 = Mathf.Sign(from.x * to.y - from.y * to.x);
		return num * num2;
	}

	public static float SignedArea(List<Vector2> orderedVertices)
	{
		float num = 0f;
		for (int i = 0; i < orderedVertices.Count - 1; i++)
		{
			Vector2 vector = orderedVertices[i];
			Vector2 vector2 = orderedVertices[i + 1];
			num += (vector2.x - vector.x) * (vector2.y + vector.y);
		}
		if (orderedVertices.Count > 1)
		{
			Vector2 vector3 = orderedVertices[orderedVertices.Count - 1];
			Vector2 vector4 = orderedVertices[0];
			num += (vector4.x - vector3.x) * (vector4.y + vector3.y);
		}
		return num * 0.5f;
	}

	public static void Shuffle<T>(List<T> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			T value = list[i];
			int index = UnityEngine.Random.Range(0, list.Count);
			list[i] = list[index];
			list[index] = value;
		}
	}

	public static void Shuffle<T>(List<T> list, RandomProvider randomProvider)
	{
		for (int i = 0; i < list.Count; i++)
		{
			T value = list[i];
			int index = randomProvider.Range(0, list.Count);
			list[i] = list[index];
			list[index] = value;
		}
	}

	public static void Intersection<T>(List<T> a, List<T> b, List<T> resultIn) where T : struct
	{
		resultIn.Clear();
		for (int i = 0; i < a.Count; i++)
		{
			T item = a[i];
			for (int j = 0; j < b.Count; j++)
			{
				T t = b[j];
				if (item.Equals(t))
				{
					resultIn.Add(item);
				}
			}
		}
	}

	public static string FirstCharToUpper(string s)
	{
		if (string.IsNullOrEmpty(s))
		{
			return string.Empty;
		}
		return char.ToUpper(s[0]).ToString() + s.Substring(1);
	}

	public static int ToLayer(LayerMask layer)
	{
		int i = layer.value;
		int num = (i > 0) ? 0 : 31;
		while (i > 1)
		{
			i >>= 1;
			num++;
		}
		return num;
	}

	public static void SetLayerRecursively(GameObject obj, LayerMask newLayer)
	{
		GGUtil.SetLayerRecursively(obj, GGUtil.ToLayer(newLayer));
	}

	public static void SetLayerRecursively(GameObject obj, int newLayer)
	{
		if (null == obj)
		{
			return;
		}
		obj.layer = newLayer;
		foreach (object obj2 in obj.transform)
		{
			Transform transform = (Transform)obj2;
			if (!(null == transform))
			{
				GGUtil.SetLayerRecursively(transform.gameObject, newLayer);
			}
		}
	}

	public static void CopyWorldTransform(Transform from, Transform to)
	{
		to.position = from.position;
		to.localScale = from.localScale;
		to.rotation = from.rotation;
	}

	public static GGUtil.ColorProvider colorProvider = new GGUtil.ColorProvider();

	public static GGUtil.UIUtil uiUtil = new GGUtil.UIUtil();

	public class ColorProvider
	{
		public Color GetColor(int index)
		{
			if (index < 0)
			{
				return this.colors[0];
			}
			int num = index % this.colors.Length;
			return this.colors[num];
		}

		private Color[] colors = new Color[]
		{
			Color.red,
			Color.green,
			Color.blue,
			Color.cyan,
			Color.grey,
			Color.magenta,
			Color.white,
			Color.yellow
		};
	}

	public class UIUtil
	{
		public Vector2 GetWorldDimensions(RectTransform trans)
		{
			trans.GetWorldCorners(this.worldCorners);
			float x = this.worldCorners[2].x - this.worldCorners[1].x;
			float y = this.worldCorners[1].y - this.worldCorners[0].y;
			return new Vector2(x, y);
		}

		public Pair<Vector2, Vector2> GetAABB(List<RectTransform> transforms)
		{
			Vector2 vector = Vector2.one * float.PositiveInfinity;
			Vector2 vector2 = Vector2.one * float.NegativeInfinity;
			for (int i = 0; i < transforms.Count; i++)
			{
				RectTransform rectTransform = transforms[i];
				Vector3 b = this.GetWorldDimensions(rectTransform) * 0.5f;
				Vector3 position = rectTransform.position;
				Vector3 vector3 = position - b;
				Vector3 vector4 = position + b;
				if (vector3.x < vector.x)
				{
					vector.x = vector3.x;
				}
				if (vector3.y < vector.y)
				{
					vector.y = vector3.y;
				}
				if (vector4.x > vector2.x)
				{
					vector2.x = vector4.x;
				}
				if (vector4.y > vector2.y)
				{
					vector2.y = vector4.y;
				}
			}
			return new Pair<Vector2, Vector2>(vector, vector2);
		}

		public void PositionRectInsideRect(RectTransform constrainRect, RectTransform rect, Vector2 screenSpacePosition)
		{
			constrainRect.GetWorldCorners(this.worldCorners);
			Vector2 vector = screenSpacePosition;
			vector.x /= constrainRect.rect.size.x;
			vector.y /= constrainRect.rect.size.y;
			Vector2 v = new Vector2(Mathf.LerpUnclamped(this.worldCorners[0].x, this.worldCorners[3].x, vector.x), Mathf.LerpUnclamped(this.worldCorners[0].y, this.worldCorners[1].y, vector.y));
			rect.position = v;
		}

		public void RestrictRectTransform(RectTransform rectTrans, RectTransform constrainingRectTrans)
		{
			rectTrans.GetLocalCorners(this.worldCorners);
			float num = this.worldCorners[3].x - this.worldCorners[0].x;
			float num2 = this.worldCorners[1].y - this.worldCorners[0].y;
			constrainingRectTrans.GetLocalCorners(this.worldCorners);
			float x = this.worldCorners[0].x;
			float x2 = this.worldCorners[3].x;
			float y = this.worldCorners[1].y;
			float y2 = this.worldCorners[0].y;
			Vector3 position = rectTrans.position;
			position.x = Mathf.Clamp(position.x, x + num * 0.5f, x2 - num * 0.5f);
			position.y = Mathf.Clamp(position.y, y2 + num2 * 0.5f, y - num2 * 0.5f);
			position.z = 0f;
			rectTrans.position = position;
		}

		public void AnchorRectInsideScreen(RectTransform rectTrans, Camera camera)
		{
			RectTransform rectTransform = rectTrans.parent as RectTransform;
			Vector2 worldDimensions = this.GetWorldDimensions(rectTransform);
			Vector3 b = camera.ViewportToWorldPoint(Vector2.zero);
			Vector3 vector = camera.ViewportToWorldPoint(Vector2.one) - b;
			rectTrans.anchorMin = Vector2.zero;
			rectTrans.anchorMax = new Vector2(vector.x / worldDimensions.x, vector.y / worldDimensions.y);
			rectTrans.anchoredPosition = rectTransform.anchoredPosition;
			rectTrans.offsetMax = Vector2.zero;
			rectTrans.offsetMin = Vector2.zero;
		}

		private Vector3[] worldCorners = new Vector3[4];
	}
}
