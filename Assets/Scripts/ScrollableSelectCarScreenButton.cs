using System;
using System.Collections.Generic;
using GGMatch3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollableSelectCarScreenButton : MonoBehaviour
{
	public void Init(CarsDB.Car car)
	{
		this.car = car;
		GGUtil.SetActive(this.widgetsToHide, false);
		if (car.isPassed)
		{
			GGUtil.Show(this.completeStyle);
		}
		else if (car.isLocked)
		{
			GGUtil.Show(this.lockedStyle);
		}
		GGUtil.ChangeText(this.titleLabel, car.displayName);
		GGUtil.ChangeText(this.descriptionLabel, car.description);
		GGUtil.SetSprite(this.mainImage, car.cardSprite);
	}

	public void ShowPassedAnimation()
	{
		GGUtil.SetActive(this.widgetsToHide, false);
		GGUtil.Show(this.passAnimation);
	}

	public void ShowUnlockAnimation()
	{
		GGUtil.SetActive(this.widgetsToHide, false);
		GGUtil.Show(this.unlockAnimation);
	}

	public void ShowOpenNotPassed()
	{
		GGUtil.SetActive(this.widgetsToHide, false);
	}

	public void ShowLocked()
	{
		GGUtil.SetActive(this.widgetsToHide, false);
		GGUtil.Show(this.lockedStyle);
	}

	public void Callback_OnClick()
	{
		UnityEngine.Debug.Log("OPEN ROOM " + this.car.name);
		if (!Application.isEditor && !this.car.isOpen)
		{
			GGSoundSystem.Play(GGSoundSystem.SFXType.CancelPress);
			return;
		}
		GGSoundSystem.Play(GGSoundSystem.SFXType.FlyIn);
		RoomsBackend instance = SingletonInit<RoomsBackend>.instance;
		int selectedRoomIndex = ScriptableObjectSingleton<CarsDB>.instance.IndexOf(this.car);
		instance.selectedRoomIndex = selectedRoomIndex;
		NavigationManager.instance.Pop(true);
	}

	[SerializeField]
	private TextMeshProUGUI titleLabel;

	[SerializeField]
	private TextMeshProUGUI descriptionLabel;

	[SerializeField]
	private List<RectTransform> widgetsToHide = new List<RectTransform>();

	[SerializeField]
	private RectTransform lockedStyle;

	[SerializeField]
	private RectTransform completeStyle;

	[SerializeField]
	private RectTransform unlockAnimation;

	[SerializeField]
	private RectTransform passAnimation;

	[SerializeField]
	public float unlockAnimationDuration;

	[SerializeField]
	public float passAnimationDuration;

	[SerializeField]
	private Image mainImage;

	[NonSerialized]
	public CarsDB.Car car;
}
