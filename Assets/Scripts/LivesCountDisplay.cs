using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class LivesCountDisplay : MonoBehaviour
{
	private void OnEnable()
	{
		this.UpdateVisualy();
	}

	private void Update()
	{
		if (this.updateEnumerator == null)
		{
			this.updateEnumerator = this.CheckLivesRegeneration();
		}
		this.updateEnumerator.MoveNext();
	}

	private void UpdateVisualy()
	{
		EnergyManager instance = BehaviourSingleton<EnergyManager>.instance;
		int ownedPlayCoins = instance.ownedPlayCoins;
		this.visibility.Clear();
		this.visibility.SetActive(this.widgetsToHide, false);
		if (instance.isLimitedFreeEnergyActive)
		{
			this.infiniteEnergyCountStyle.Apply(this.visibility);
			GGUtil.ChangeText(this.livesText, GGFormat.FormatTimeSpan(instance.limitedEnergyTimespanLeft));
		}
		else if (ownedPlayCoins <= 0)
		{
			this.livesCountStyle.Apply(this.visibility);
			TimeSpan span = TimeSpan.FromSeconds((double)BehaviourSingleton<EnergyManager>.instance.secToNextCoin);
			GGUtil.ChangeText(this.livesText, GGFormat.FormatTimeSpan(span));
		}
		else if (instance.isFullLives)
		{
			this.fullLivesCountStyle.Apply(this.visibility);
			GGUtil.ChangeText(this.heartsLivesText, ownedPlayCoins.ToString());
			GGUtil.ChangeText(this.livesText, "Full");
		}
		else
		{
			this.livesCountStyle.Apply(this.visibility);
			GGUtil.ChangeText(this.livesText, ownedPlayCoins.ToString());
		}
		this.visibility.Complete();
	}

	public IEnumerator CheckLivesRegeneration()
	{
		return new LivesCountDisplay._003CCheckLivesRegeneration_003Ed__12(0)
		{
			_003C_003E4__this = this
		};
	}

	[SerializeField]
	private List<Transform> widgetsToHide = new List<Transform>();

	[SerializeField]
	private VisualStyleSet livesCountStyle = new VisualStyleSet();

	[SerializeField]
	private VisualStyleSet infiniteEnergyCountStyle = new VisualStyleSet();

	[SerializeField]
	private VisualStyleSet fullLivesCountStyle = new VisualStyleSet();

	[SerializeField]
	private TextMeshProUGUI livesText;

	[SerializeField]
	private TextMeshProUGUI heartsLivesText;

	[SerializeField]
	public float regenerationIntervalSeconds = 1f;

	private VisibilityHelper visibility = new VisibilityHelper();

	private IEnumerator updateEnumerator;

	private sealed class _003CCheckLivesRegeneration_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CCheckLivesRegeneration_003Ed__12(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		bool IEnumerator.MoveNext()
		{
			int num = this._003C_003E1__state;
			LivesCountDisplay livesCountDisplay = this._003C_003E4__this;
			switch (num)
			{
			case 0:
				this._003C_003E1__state = -1;
				break;
			case 1:
				this._003C_003E1__state = -1;
				this._003Ctimer_003E5__2 += Time.deltaTime;
				goto IL_5F;
			case 2:
				this._003C_003E1__state = -1;
				break;
			default:
				return false;
			}
			this._003Ctimer_003E5__2 = 0f;
			IL_5F:
			if (this._003Ctimer_003E5__2 >= livesCountDisplay.regenerationIntervalSeconds)
			{
				livesCountDisplay.UpdateVisualy();
				this._003C_003E2__current = null;
				this._003C_003E1__state = 2;
				return true;
			}
			this._003C_003E2__current = null;
			this._003C_003E1__state = 1;
			return true;
		}

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this._003C_003E2__current;
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this._003C_003E2__current;
			}
		}

		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public LivesCountDisplay _003C_003E4__this;

		private float _003Ctimer_003E5__2;
	}
}
