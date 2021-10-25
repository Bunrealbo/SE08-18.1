using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using GGMatch3;
using UnityEngine;

public class PlacePowerupAction : BoardAction
{
	private PlacePowerupAction.Settings settings
	{
		get
		{
			return Match3Settings.instance.placePowerupActionSettings;
		}
	}

	public static Slot GetSlotThatCanBeReplacedWithPowerup(Match3Game game, ChipType powerup)
	{
		List<Slot> slots = new List<Slot>(game.board.slots);
		PlacePowerupAction.ConstraintsFilter constraintsFilter = new PlacePowerupAction.ConstraintsFilter();
		List<Slot> list;
		if (powerup == ChipType.DiscoBall)
		{
			list = constraintsFilter.GetSlotsThatCanBeReplacedWithPowerup(slots);
			list = constraintsFilter.GetSwappableSlotsForDiscoBomb(list);
		}
		else
		{
			list = constraintsFilter.GetSlotsThatCanBeReplacedWithPowerup(slots);
			list = constraintsFilter.GetTappableSlots(list);
		}
		if (list.Count == 0)
		{
			if (powerup == ChipType.DiscoBall)
			{
				list = constraintsFilter.GetSwappableSlotsForDiscoBomb(slots);
			}
			else
			{
				list = constraintsFilter.GetTappableSlots(slots);
			}
		}
		if (list.Count == 0)
		{
			list = constraintsFilter.GetNoPowerupPlacementSlots(slots);
		}
		if (list.Count == 0)
		{
			return null;
		}
		return list[game.RandomRange(0, list.Count)];
	}

	public void Init(PlacePowerupAction.Parameters parameters)
	{
		this.parameters = parameters;
		this.slotLock = this.lockContainer.NewLock();
		this.slot = parameters.slotWhereToPlacePowerup;
		if (this.slot == null)
		{
			List<Slot> slots = new List<Slot>(parameters.game.board.slots);
			PlacePowerupAction.ConstraintsFilter constraintsFilter = new PlacePowerupAction.ConstraintsFilter();
			List<Slot> list;
			if (parameters.powerup == ChipType.DiscoBall)
			{
				list = constraintsFilter.GetSlotsThatCanBeReplacedWithPowerup(slots);
				list = constraintsFilter.GetSwappableSlotsForDiscoBomb(list);
			}
			else
			{
				list = constraintsFilter.GetSlotsThatCanBeReplacedWithPowerup(slots);
				list = constraintsFilter.GetTappableSlots(list);
			}
			if (list.Count == 0)
			{
				if (parameters.powerup == ChipType.DiscoBall)
				{
					list = constraintsFilter.GetSwappableSlotsForDiscoBomb(slots);
				}
				else
				{
					list = constraintsFilter.GetTappableSlots(slots);
				}
			}
			if (list.Count == 0)
			{
				list = constraintsFilter.GetNoPowerupPlacementSlots(slots);
			}
			if (list.Count == 0)
			{
				return;
			}
			this.slot = list[parameters.game.RandomRange(0, list.Count)];
		}
		this.slotLock.isPowerupReplacementSuspended = true;
		this.slotLock.isDestroySuspended = true;
		this.slotLock.isSlotGravitySuspended = true;
		this.slotLock.isSlotMatchingSuspended = true;
		this.slotLock.isAvailableForSeekingMissileSuspended = true;
		this.slotLock.isAvailableForDiscoBombSuspended = true;
		this.slotLock.LockSlot(this.slot);
	}

	public override void OnUpdate(float deltaTime)
	{
		this.deltaTime = deltaTime;
		base.OnUpdate(deltaTime);
		if (this.replaceChipWithPowerup == null)
		{
			this.replaceChipWithPowerup = this.DoReplaceChipWithPowerup();
		}
		this.replaceChipWithPowerup.MoveNext();
	}

	public IEnumerator DoReplaceChipWithPowerup()
	{
		return new PlacePowerupAction._003CDoReplaceChipWithPowerup_003Ed__12(0)
		{
			_003C_003E4__this = this
		};
	}

	private IEnumerator PowerupCreation(IntVector2 positionWherePowerupWillBeCreated)
	{
		return new PlacePowerupAction._003CPowerupCreation_003Ed__13(0)
		{
			_003C_003E4__this = this,
			positionWherePowerupWillBeCreated = positionWherePowerupWillBeCreated
		};
	}

	private Slot slot;

	private PlacePowerupAction.Parameters parameters;

	private Lock slotLock;

	private IEnumerator replaceChipWithPowerup;

	private float deltaTime;

	[Serializable]
	public class Settings
	{
		public float delayBetweenPowerups = 0.5f;

		public float durationForPowerup = 1f;

		public AnimationCurve powerupCurve;

		public float startScale = 2f;

		public float startAlpha;

		public float normalizedTimeForParticles;
	}

	public class Parameters
	{
		public Match3Game game;

		public ChipType powerup;

		public float initialDelay;

		public int addCoins;

		public Slot slotWhereToPlacePowerup;

		public bool internalAnimation;

		public Action onComplete;
	}

	public class ConstraintsFilter
	{
		public List<Slot> GetSlotsThatCanBeReplacedWithPowerup(List<Slot> slots)
		{
			List<Slot> list = new List<Slot>();
			for (int i = 0; i < slots.Count; i++)
			{
				Slot slot = slots[i];
				if (slot != null)
				{
					Chip slotComponent = slot.GetSlotComponent<Chip>();
					if (slotComponent != null && slotComponent.chipType == ChipType.Chip)
					{
						list.Add(slot);
					}
				}
			}
			return list;
		}

		public List<Slot> GetTappableSlots(List<Slot> slots)
		{
			List<Slot> list = new List<Slot>();
			for (int i = 0; i < slots.Count; i++)
			{
				Slot slot = slots[i];
				if (slot != null && !slot.isTapToActivateSuspended && !slot.isPowerupReplacementSuspended)
				{
					list.Add(slot);
				}
			}
			return list;
		}

		public List<Slot> GetSwappableSlotsForDiscoBomb(List<Slot> slots)
		{
			List<Slot> list = new List<Slot>();
			for (int i = 0; i < slots.Count; i++)
			{
				Slot slot = slots[i];
				if (slot != null && !slot.isSlotSwapSuspended && !slot.isPowerupReplacementSuspended)
				{
					List<Slot> neigbourSlots = slot.neigbourSlots;
					for (int j = 0; j < neigbourSlots.Count; j++)
					{
						Slot slot2 = neigbourSlots[j];
						Chip slotComponent = slot2.GetSlotComponent<Chip>();
						if (slotComponent != null && (slotComponent.canFormColorMatches || slotComponent.isPowerup) && !slot2.isSlotSwapSuspended)
						{
							list.Add(slot);
							break;
						}
					}
				}
			}
			return list;
		}

		public List<Slot> GetNoPowerupPlacementSlots(List<Slot> slots)
		{
			List<Slot> list = new List<Slot>();
			for (int i = 0; i < slots.Count; i++)
			{
				Slot slot = slots[i];
				if (slot != null && !slot.isPowerupReplacementSuspended)
				{
					list.Add(slot);
				}
			}
			return list;
		}
	}

	private sealed class _003CDoReplaceChipWithPowerup_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoReplaceChipWithPowerup_003Ed__12(int _003C_003E1__state)
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
			PlacePowerupAction placePowerupAction = this._003C_003E4__this;
			switch (num)
			{
			case 0:
				this._003C_003E1__state = -1;
				if (placePowerupAction.slot == null)
				{
					placePowerupAction.isAlive = false;
					return false;
				}
				if (placePowerupAction.parameters.initialDelay <= 0f)
				{
					goto IL_9A;
				}
				this._003Ctime_003E5__2 = 0f;
				break;
			case 1:
				this._003C_003E1__state = -1;
				break;
			case 2:
				this._003C_003E1__state = -1;
				goto IL_EC;
			case 3:
				this._003C_003E1__state = -1;
				goto IL_195;
			default:
				return false;
			}
			if (this._003Ctime_003E5__2 < placePowerupAction.parameters.initialDelay)
			{
				this._003Ctime_003E5__2 += placePowerupAction.deltaTime;
				this._003C_003E2__current = null;
				this._003C_003E1__state = 1;
				return true;
			}
			IL_9A:
			Chip slotComponent = placePowerupAction.slot.GetSlotComponent<Chip>();
			if (slotComponent != null)
			{
				slotComponent.RemoveFromGame();
			}
			if (!placePowerupAction.parameters.internalAnimation)
			{
				CreatePowerupAction createPowerupAction = new CreatePowerupAction();
				createPowerupAction.Init(new CreatePowerupAction.CreateParams
				{
					positionWherePowerupWillBeCreated = placePowerupAction.slot.position,
					powerupToCreate = placePowerupAction.parameters.powerup,
					game = placePowerupAction.parameters.game,
					addCoins = placePowerupAction.parameters.addCoins
				});
				placePowerupAction.parameters.game.board.actionManager.AddAction(createPowerupAction);
				this._003C_003E2__current = null;
				this._003C_003E1__state = 3;
				return true;
			}
			this._003Cenumerator_003E5__3 = placePowerupAction.PowerupCreation(placePowerupAction.slot.position);
			IL_EC:
			if (this._003Cenumerator_003E5__3.MoveNext())
			{
				this._003C_003E2__current = null;
				this._003C_003E1__state = 2;
				return true;
			}
			this._003Cenumerator_003E5__3 = null;
			IL_195:
			placePowerupAction.slotLock.UnlockAll();
			if (placePowerupAction.parameters.onComplete != null)
			{
				placePowerupAction.parameters.onComplete();
			}
			placePowerupAction.isAlive = false;
			return false;
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

		public PlacePowerupAction _003C_003E4__this;

		private float _003Ctime_003E5__2;

		private IEnumerator _003Cenumerator_003E5__3;
	}

	private sealed class _003CPowerupCreation_003Ed__13 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CPowerupCreation_003Ed__13(int _003C_003E1__state)
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
			PlacePowerupAction placePowerupAction = this._003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				this._003C_003E1__state = -1;
			}
			else
			{
				this._003C_003E1__state = -1;
				this._003Cgame_003E5__2 = placePowerupAction.parameters.game;
				this._003Ctime_003E5__3 = 0f;
				this._003CpowerupSlot_003E5__4 = this._003Cgame_003E5__2.GetSlot(this.positionWherePowerupWillBeCreated);
				Chip slotComponent = this._003CpowerupSlot_003E5__4.GetSlotComponent<Chip>();
				if (slotComponent != null)
				{
					slotComponent.RemoveFromGame();
				}
				Chip chip = this._003Cgame_003E5__2.CreatePowerupInSlot(this._003CpowerupSlot_003E5__4, placePowerupAction.parameters.powerup);
				chip.carriesCoins = placePowerupAction.parameters.addCoins;
				this._003CchipTransform_003E5__5 = chip.GetComponentBehaviour<TransformBehaviour>();
				this._003Ctime_003E5__3 = 0f;
				this._003Cduration_003E5__6 = placePowerupAction.settings.durationForPowerup;
				this._003Ccurve_003E5__7 = placePowerupAction.settings.powerupCurve;
				this._003Cgame_003E5__2.particles.CreateParticles(this._003CpowerupSlot_003E5__4, Match3Particles.PositionType.BombCreate);
			}
			if (this._003Ctime_003E5__3 > this._003Cduration_003E5__6)
			{
				if (placePowerupAction.settings.normalizedTimeForParticles >= 1f)
				{
					this._003Cgame_003E5__2.particles.CreateParticles(this._003CpowerupSlot_003E5__4, Match3Particles.PositionType.BombCreate);
				}
				return false;
			}
			float num2 = Mathf.InverseLerp(0f, this._003Cduration_003E5__6, this._003Ctime_003E5__3);
			this._003Ctime_003E5__3 += placePowerupAction.deltaTime;
			float num3 = Mathf.InverseLerp(0f, this._003Cduration_003E5__6, this._003Ctime_003E5__3);
			float t = this._003Ccurve_003E5__7.Evaluate(num3);
			Vector3 localScale = Vector3.LerpUnclamped(new Vector3(placePowerupAction.settings.startScale, placePowerupAction.settings.startScale, 1f), Vector3.one, t);
			float alpha = Mathf.Lerp(placePowerupAction.settings.startAlpha, 1f, t);
			if (this._003CchipTransform_003E5__5 != null)
			{
				this._003CchipTransform_003E5__5.localScale = localScale;
				this._003CchipTransform_003E5__5.SetAlpha(alpha);
			}
			if (num2 <= placePowerupAction.settings.normalizedTimeForParticles && num3 > placePowerupAction.settings.normalizedTimeForParticles)
			{
				this._003Cgame_003E5__2.particles.CreateParticles(this._003CpowerupSlot_003E5__4, Match3Particles.PositionType.BombCreate);
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

		public PlacePowerupAction _003C_003E4__this;

		public IntVector2 positionWherePowerupWillBeCreated;

		private Match3Game _003Cgame_003E5__2;

		private float _003Ctime_003E5__3;

		private Slot _003CpowerupSlot_003E5__4;

		private TransformBehaviour _003CchipTransform_003E5__5;

		private float _003Cduration_003E5__6;

		private AnimationCurve _003Ccurve_003E5__7;
	}
}
