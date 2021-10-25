using System;
using GGMatch3;
using UnityEngine;

public class DailyGiftsScreen : MonoBehaviour
{
	private void OnEnable()
	{
		this.Init();
	}

	private void Init()
	{
		Vector2 prefabSizeDelta = this.smallGiftPanels.prefabSizeDelta;
		Vector2 prefabSizeDelta2 = this.bigGiftPanels.prefabSizeDelta;
		float num = prefabSizeDelta.x * 3f + prefabSizeDelta2.x + (float)(3 * this.padding);
		float y = prefabSizeDelta2.y;
		this.smallGiftPanels.Clear();
		this.bigGiftPanels.Clear();
		GiftsDefinitionDB.DailyGifts dailyGifts = ScriptableObjectSingleton<GiftsDefinitionDB>.instance.dailyGifts;
		RectTransform component = base.GetComponent<RectTransform>();
		Vector2 vector = new Vector2(component.rect.width, component.rect.height) * this.screenSizePercent;
		float num2 = vector.x / num;
		float num3 = vector.y / y;
		float d = Mathf.Min(new float[]
		{
			num2,
			num3,
			1f
		});
		this.scalingTransform.localScale = Vector3.one * d;
		Vector2 a = new Vector2(-num * 0.5f, -y * 0.5f);
		for (int i = 0; i < 6; i++)
		{
			ComponentPool componentPool = this.smallGiftPanels;
			int num4 = i / 3;
			int num5 = i % 3;
			DailyGiftsScreenGiftCard dailyGiftsScreenGiftCard = componentPool.Next<DailyGiftsScreenGiftCard>(false);
			dailyGiftsScreenGiftCard.Init(i, 1f, dailyGifts.IsSelected(i));
			GGUtil.Show(dailyGiftsScreenGiftCard);
			Vector2 b = new Vector2(((float)num5 + 0.5f) * prefabSizeDelta.x + (float)((num5 - 1) * this.padding), ((float)(-(float)num4) + 0.5f) * prefabSizeDelta.y - (float)((num4 - 1) * this.padding));
			dailyGiftsScreenGiftCard.transform.localPosition = a + Vector2.up * y * 0.5f + b;
		}
		ComponentPool componentPool2 = this.bigGiftPanels;
		int num6 = 3;
		int num7 = 6;
		DailyGiftsScreenGiftCard dailyGiftsScreenGiftCard2 = componentPool2.Next<DailyGiftsScreenGiftCard>(false);
		dailyGiftsScreenGiftCard2.Init(num7, 1f, dailyGifts.IsSelected(num7));
		GGUtil.Show(dailyGiftsScreenGiftCard2);
		Vector2 vector2 = new Vector2((float)num6 * prefabSizeDelta.x + (float)((num6 - 1) * this.padding), 0f);
		vector2 += prefabSizeDelta2 * 0.5f;
		dailyGiftsScreenGiftCard2.transform.localPosition = a + vector2;
		this.smallGiftPanels.HideNotUsed();
		this.bigGiftPanels.HideNotUsed();
	}

	public void ButtonCallback_OnBackgroundClick()
	{
		DailyGiftsScreen._003C_003Ec__DisplayClass7_0 _003C_003Ec__DisplayClass7_ = new DailyGiftsScreen._003C_003Ec__DisplayClass7_0();
		_003C_003Ec__DisplayClass7_.nav = NavigationManager.instance;
		GiftBoxScreen @object = _003C_003Ec__DisplayClass7_.nav.GetObject<GiftBoxScreen>();
		GiftsDefinitionDB.DailyGifts dailyGifts = ScriptableObjectSingleton<GiftsDefinitionDB>.instance.dailyGifts;
		GiftsDefinitionDB.DailyGifts.DailyGift currentDailyGift = dailyGifts.currentDailyGift;
		if (currentDailyGift == null)
		{
			_003C_003Ec__DisplayClass7_.nav.Pop(true);
			return;
		}
		_003C_003Ec__DisplayClass7_.nav.Pop(false);
		@object.Show(new GiftBoxScreen.ShowArguments
		{
			giftsDefinition = currentDailyGift.gifts,
			title = string.Format("Day {0} gift", currentDailyGift.index + 1),
			onComplete = new Action(_003C_003Ec__DisplayClass7_._003CButtonCallback_OnBackgroundClick_003Eb__0)
		});
		dailyGifts.OnClaimedDailyCoins();
	}

	[SerializeField]
	private ComponentPool smallGiftPanels = new ComponentPool();

	[SerializeField]
	private ComponentPool bigGiftPanels = new ComponentPool();

	[SerializeField]
	private Transform scalingTransform;

	[SerializeField]
	private float screenSizePercent = 0.9f;

	[SerializeField]
	private int padding = 1;

	private sealed class _003C_003Ec__DisplayClass7_0
	{
		internal void _003CButtonCallback_OnBackgroundClick_003Eb__0()
		{
			this.nav.Pop(true);
		}

		public NavigationManager nav;
	}
}
