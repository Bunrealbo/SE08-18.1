using System;
using UnityEngine;

public class GameScreenMenu : MonoBehaviour
{
	public void Show()
	{
		this.backgroundOutAnimation.Stop();
		this.backgroundInAnimation.Init();
		this.backgroundInAnimation.Play(0f, null);
		this.exitButtonOutAnimation.Stop();
		this.exitButtonInAnimation.Init();
		this.exitButtonInAnimation.Play(0.15f, null);
		GGUtil.SetActive(this.backgroundButton, true);
	}

	public void OnEnable()
	{
		this.state = GameScreenMenu.State.Closed;
		this.exitButtonInAnimation.Stop();
		this.exitButtonOutAnimation.Stop();
		this.backgroundOutAnimation.Stop();
		this.backgroundInAnimation.Stop();
		this.backgroundInAnimation.Init();
		this.exitButtonInAnimation.Init();
		GGUtil.SetActive(this.backgroundButton, false);
	}

	public void Hide()
	{
		this.backgroundInAnimation.Stop();
		this.backgroundOutAnimation.Init();
		this.backgroundOutAnimation.Play(0.15f, null);
		this.exitButtonInAnimation.Stop();
		this.exitButtonOutAnimation.Init();
		this.exitButtonOutAnimation.Play(0f, null);
		GGUtil.SetActive(this.backgroundButton, false);
	}

	public void ButtonCallback_OnMenuButtonClicked()
	{
		if (this.state == GameScreenMenu.State.Closed)
		{
			this.Show();
			this.state = GameScreenMenu.State.Open;
			GGSoundSystem.Play(GGSoundSystem.SFXType.ButtonPress);
			return;
		}
		this.Hide();
		this.state = GameScreenMenu.State.Closed;
		GGSoundSystem.Play(GGSoundSystem.SFXType.CancelPress);
	}

	[SerializeField]
	private CurrencyPrefabAnimation backgroundInAnimation;

	[SerializeField]
	private CurrencyPrefabAnimation backgroundOutAnimation;

	[SerializeField]
	private CurrencyPrefabAnimation exitButtonInAnimation;

	[SerializeField]
	private CurrencyPrefabAnimation exitButtonOutAnimation;

	[SerializeField]
	private RectTransform backgroundButton;

	private GameScreenMenu.State state = GameScreenMenu.State.Closed;

	public enum State
	{
		Open,
		Closed
	}
}
