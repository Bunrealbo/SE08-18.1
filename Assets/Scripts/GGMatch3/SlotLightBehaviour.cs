using System;
using UnityEngine;

namespace GGMatch3
{
	public class SlotLightBehaviour : SlotComponentBehavoiour
	{
		public void InitWithSlotComponent(LightSlotComponent slotComponent)
		{
			SlotLightBehaviour.SetActive(this.lightSprite, false);
			slotComponent.Init(this);
			SlotLightBehaviour.SetActive(this, true);
		}

		public void Init(Slot slot, bool isBackPatternEnabled)
		{
			LightSlotComponent lightSlotComponent = new LightSlotComponent();
			SlotLightBehaviour.SetActive(this.lightSprite, false);
			lightSlotComponent.Init(this);
			slot.AddComponent(lightSlotComponent);
			SlotLightBehaviour.SetActive(this, true);
			SlotLightBehaviour.SetActive(this.slotBckSprite, isBackPatternEnabled);
		}

		public static void SetActive(SpriteRenderer beh, bool active)
		{
			if (beh == null)
			{
				return;
			}
			GameObject gameObject = beh.gameObject;
			if (gameObject.activeSelf == active)
			{
				return;
			}
			gameObject.SetActive(active);
		}

		public static void SetActive(MonoBehaviour beh, bool active)
		{
			if (beh == null)
			{
				return;
			}
			GameObject gameObject = beh.gameObject;
			if (gameObject.activeSelf == active)
			{
				return;
			}
			gameObject.SetActive(active);
		}

		public void SetLight(float intensity)
		{
			SlotLightBehaviour.SetActive(this.lightSprite, intensity > 0f);
			Color color = this.lightSprite.color;
			color.a = intensity;
			this.lightSprite.color = color;
		}

		[SerializeField]
		private SpriteRenderer lightSprite;

		[SerializeField]
		private SpriteRenderer slotBckSprite;
	}
}
