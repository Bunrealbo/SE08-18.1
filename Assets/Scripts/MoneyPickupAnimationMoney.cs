using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyPickupAnimationMoney : MonoBehaviour
{
	private MoneyPickupAnimationMoney.StylePool GetStylePool(MoneyPickupAnimationMoney.Style style)
	{
		for (int i = 0; i < this.stylePools.Count; i++)
		{
			MoneyPickupAnimationMoney.StylePool stylePool = this.stylePools[i];
			if (stylePool.style == style)
			{
				return stylePool;
			}
		}
		return null;
	}

	public void SetStyle(MoneyPickupAnimationMoney.Style style)
	{
		GGUtil.SetActive(this.starStyle, style == MoneyPickupAnimationMoney.Style.Star);
		GGUtil.SetActive(this.coinStyle, style == MoneyPickupAnimationMoney.Style.Coin);
		this.style = style;
	}

	public void Init(int index, Vector3 startLocalPosition)
	{
		this.index = index;
		this.startLocalPosition = startLocalPosition;
		base.transform.localPosition = startLocalPosition;
	}

	public void Init(int index, Vector3 startLocalPosition, MoneyPickupAnimationMoney.Style style, int count)
	{
		this.index = index;
		this.startLocalPosition = startLocalPosition;
		base.transform.localPosition = startLocalPosition;
		if (style == MoneyPickupAnimationMoney.Style.Star)
		{
			this.bottomLabel.text = "Design Star";
		}
		else
		{
			this.bottomLabel.text = count.ToString();
		}
		this.SetStyle(style);
		ComponentPool pool = this.GetStylePool(style).pool;
		RectTransform component = pool.parent.GetComponent<RectTransform>();
		pool.Clear();
		for (int i = 0; i < count; i++)
		{
			RectTransform component2 = pool.Next(true).GetComponent<RectTransform>();
			if (count == 1)
			{
				component2.localPosition = Vector3.zero;
			}
			else
			{
				component2.localPosition = new Vector3(UnityEngine.Random.Range(-component.sizeDelta.x * 0.5f, component.sizeDelta.x * 0.5f), UnityEngine.Random.Range(-component.sizeDelta.y * 0.5f, component.sizeDelta.y * 0.5f), 0f);
			}
		}
		pool.HideNotUsed();
	}

	[SerializeField]
	private RectTransform starStyle;

	[SerializeField]
	private RectTransform coinStyle;

	[SerializeField]
	private List<MoneyPickupAnimationMoney.StylePool> stylePools = new List<MoneyPickupAnimationMoney.StylePool>();

	[NonSerialized]
	public int index;

	[NonSerialized]
	public MoneyPickupAnimationMoney.Style style;

	[SerializeField]
	private TextMeshProUGUI bottomLabel;

	[NonSerialized]
	public Vector3 startLocalPosition;

	[NonSerialized]
	public Vector3 startTravelScale;

	public enum Style
	{
		Coin,
		Star
	}

	[Serializable]
	public class StylePool
	{
		public MoneyPickupAnimationMoney.Style style;

		public ComponentPool pool = new ComponentPool();
	}
}
