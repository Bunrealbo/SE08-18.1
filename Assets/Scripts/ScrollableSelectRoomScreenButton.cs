using System;
using System.Collections.Generic;
using GGMatch3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollableSelectRoomScreenButton : MonoBehaviour
{
	public void Init(RoomsDB.Room room)
	{
		this.room = room;
		GGUtil.SetActive(this.widgetsToHide, false);
		if (room.isPassed)
		{
			GGUtil.Show(this.completeStyle);
		}
		else if (room.isLocked)
		{
			GGUtil.Show(this.lockedStyle);
		}
		GGUtil.ChangeText(this.titleLabel, room.displayName);
		GGUtil.ChangeText(this.descriptionLabel, room.description);
		GGUtil.SetSprite(this.mainImage, room.cardSprite);
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
		UnityEngine.Debug.Log("OPEN ROOM " + this.room.name);
		if (!ConfigBase.instance.debug && !this.room.isOpen)
		{
			GGSoundSystem.Play(GGSoundSystem.SFXType.CancelPress);
			return;
		}
		GGSoundSystem.Play(GGSoundSystem.SFXType.FlyIn);
		RoomsBackend instance = SingletonInit<RoomsBackend>.instance;
		int selectedRoomIndex = ScriptableObjectSingleton<RoomsDB>.instance.IndexOf(this.room);
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
	public RoomsDB.Room room;
}
