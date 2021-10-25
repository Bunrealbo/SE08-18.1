using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GGMatch3
{
	public class SwapToMatchAction : BoardAction
	{
		public SwapToMatchAction.Settings settings
		{
			get
			{
				return Match3Settings.instance.swapToMatchActionSettings;
			}
		}

		public void Init(SwapToMatchAction.SwapActionProperties swapProperties)
		{
			this.slot1 = swapProperties.slot1;
			this.slot2 = swapProperties.slot2;
			this.swapProperties = swapProperties;
			this.slotLock = this.lockContainer.NewLock();
			this.slotLock.isChipGravitySuspended = true;
			this.slotLock.isSlotGravitySuspended = true;
			this.slotLock.isDestroySuspended = true;
			this.slotLock.isSlotMatchingSuspended = true;
			this.slotLock.LockSlot(this.slot1);
			this.slotLock.LockSlot(this.slot2);
		}

		private IEnumerator DoSwapToMixPowerups(SwapToMatchAction.PowerupList powerupList)
		{
			return new SwapToMatchAction._003CDoSwapToMixPowerups_003Ed__11(0)
			{
				_003C_003E4__this = this,
				powerupList = powerupList
			};
		}

		private IEnumerator DoSwap()
		{
			return new SwapToMatchAction._003CDoSwap_003Ed__13(0)
			{
				_003C_003E4__this = this
			};
		}

		public override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);
			if (this.swapEnumerator == null)
			{
				this.swapEnumerator = this.DoSwap();
			}
			this.swapEnumerator.MoveNext();
		}

		private Slot slot1;

		private Slot slot2;

		private Lock slotLock;

		private IEnumerator swapEnumerator;

		private SwapToMatchAction.SwapActionProperties swapProperties;

		private List<Slot> allSlotsInMatch = new List<Slot>();

		[Serializable]
		public class Settings
		{
			public float slot1LightIntensity = 2f;

			public float slot2LightIntensity = 1f;
		}

		public struct SwapActionProperties
		{
			public Slot slot1;

			public Slot slot2;

			public bool isInstant;

			public List<LightingBolt> bolts;

			public Match3Game.SwitchSlotsArguments switchSlotsArgument;
		}

		public class PowerupList
		{
			public Chip FirstPowerup
			{
				get
				{
					return this.powerupList[0];
				}
			}

			public bool isActivatingPowerup
			{
				get
				{
					return this.isMixingDiscoBallWithColorElement || this.isContainingSingleActivateablePowerup || this.isMixingPowerups;
				}
			}

			public Chip OtherPowerup(ChipType chipType)
			{
				for (int i = 0; i < this.powerupList.Count; i++)
				{
					Chip chip = this.powerupList[i];
					if (chip.chipType != chipType)
					{
						return chip;
					}
				}
				return null;
			}

			public Chip PowerupOfType(ChipType chipType)
			{
				for (int i = 0; i < this.powerupList.Count; i++)
				{
					Chip chip = this.powerupList[i];
					if (chip.chipType == chipType)
					{
						return chip;
					}
				}
				return null;
			}

			public int CountChipTypes(ChipType chipType1, ChipType chipType2)
			{
				int num = 0;
				for (int i = 0; i < this.powerupList.Count; i++)
				{
					Chip chip = this.powerupList[i];
					if (chip.chipType == chipType1)
					{
						num++;
					}
					else if (chip.chipType == chipType2)
					{
						num++;
					}
				}
				return num;
			}

			public int CountChipType(ChipType chipType)
			{
				int num = 0;
				for (int i = 0; i < this.powerupList.Count; i++)
				{
					if (this.powerupList[i].chipType == chipType)
					{
						num++;
					}
				}
				return num;
			}

			public bool isMixingPowerups
			{
				get
				{
					return this.powerupList.Count >= 2;
				}
			}

			public bool isMixingDiscoBallWithColorElement
			{
				get
				{
					return this.CountChipType(ChipType.DiscoBall) == 1 && this.coloredChips.Count == 1;
				}
			}

			public bool isContainingSingleActivateablePowerup
			{
				get
				{
					return this.powerupList.Count == 1 && this.FirstPowerup.chipType != ChipType.DiscoBall;
				}
			}

			public ItemColor mixingColor
			{
				get
				{
					return this.coloredChips[0].itemColor;
				}
			}

			public void Add(Chip chip)
			{
				if (chip == null)
				{
					return;
				}
				if (chip.isPowerup)
				{
					this.powerupList.Add(chip);
				}
				if (chip.canFormColorMatches)
				{
					this.coloredChips.Add(chip);
				}
			}

			public List<Chip> powerupList = new List<Chip>();

			private List<Chip> coloredChips = new List<Chip>();
		}

		private sealed class _003C_003Ec__DisplayClass11_0
		{
			internal void _003CDoSwapToMixPowerups_003Eb__0()
			{
				this.swapForwardComplete = true;
			}

			public bool swapForwardComplete;
		}

		private sealed class _003CDoSwapToMixPowerups_003Ed__11 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CDoSwapToMixPowerups_003Ed__11(int _003C_003E1__state)
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
				SwapToMatchAction swapToMatchAction = this._003C_003E4__this;
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
					this._003C_003E8__1 = new SwapToMatchAction._003C_003Ec__DisplayClass11_0();
					bool isInstant = swapToMatchAction.swapProperties.isInstant;
					this._003Cgame_003E5__2 = swapToMatchAction.slot1.game;
					this._003C_003E8__1.swapForwardComplete = false;
					SwapChipsAction swapChipsAction = new SwapChipsAction();
					SwapChipsAction.SwapChipsParams swapChipsParams = default(SwapChipsAction.SwapChipsParams);
					swapChipsParams.slot1 = swapToMatchAction.slot1;
					swapChipsParams.slot2 = swapToMatchAction.slot2;
					swapChipsParams.onComplete = new Action(this._003C_003E8__1._003CDoSwapToMixPowerups_003Eb__0);
					swapChipsParams.switchSlots = true;
					swapChipsParams.game = this._003Cgame_003E5__2;
					swapChipsParams.moveToSpecificPos = true;
					swapChipsParams.positionToMoveSlot1 = swapChipsParams.slot2.position;
					swapChipsParams.positionToMoveSlot2 = swapChipsParams.slot2.position;
					this._003ChasCarpet_003E5__3 = false;
					if (swapToMatchAction.slot1.canCarpetSpreadFromHere || swapToMatchAction.slot2.canCarpetSpreadFromHere)
					{
						this._003ChasCarpet_003E5__3 = true;
					}
					if (!isInstant)
					{
						swapChipsAction.Init(swapChipsParams);
						this._003Cgame_003E5__2.board.actionManager.AddAction(swapChipsAction);
					}
					else
					{
						TransformBehaviour transformBehaviour = null;
						TransformBehaviour transformBehaviour2 = null;
						Chip chip = this.powerupList.powerupList[0];
						Chip chip2 = this.powerupList.powerupList[1];
						if (chip != null)
						{
							transformBehaviour = chip.GetComponentBehaviour<TransformBehaviour>();
						}
						if (chip2 != null)
						{
							transformBehaviour2 = chip2.GetComponentBehaviour<TransformBehaviour>();
						}
						if (transformBehaviour != null)
						{
							transformBehaviour.localPosition = swapToMatchAction.slot2.localPositionOfCenter;
						}
						if (transformBehaviour2 != null)
						{
							transformBehaviour2.localPosition = swapToMatchAction.slot1.localPositionOfCenter;
							goto IL_1C2;
						}
						goto IL_1C2;
					}
				}
				if (!this._003C_003E8__1.swapForwardComplete)
				{
					this._003C_003E2__current = null;
					this._003C_003E1__state = 1;
					return true;
				}
				IL_1C2:
				SwapParams swapParams = new SwapParams();
				swapParams.startPosition = swapToMatchAction.slot1.position;
				swapParams.swipeToPosition = swapToMatchAction.slot2.position;
				swapParams.affectorExport = swapToMatchAction.swapProperties.switchSlotsArgument.affectorExport;
				this._003Cgame_003E5__2.particles.CreateParticles(swapToMatchAction.slot2.localPositionOfCenter, Match3Particles.PositionType.BombCombine);
				if (this.powerupList.CountChipType(ChipType.SeekingMissle) == 2)
				{
					ComboSeekingMissileAction comboSeekingMissileAction = new ComboSeekingMissileAction();
					comboSeekingMissileAction.Init(new ComboSeekingMissileAction.Parameters
					{
						game = this._003Cgame_003E5__2,
						rocketsCount = 3,
						startSlot = swapToMatchAction.slot2,
						isHavingCarpet = this._003ChasCarpet_003E5__3
					});
					for (int i = 0; i < this.powerupList.powerupList.Count; i++)
					{
						this.powerupList.powerupList[i].RemoveFromGame();
					}
					swapToMatchAction.slotLock.UnlockAll();
					this._003Cgame_003E5__2.board.actionManager.AddAction(comboSeekingMissileAction);
				}
				else if (this.powerupList.CountChipType(ChipType.SeekingMissle) == 1 && (this.powerupList.CountChipType(ChipType.Bomb) == 1 || this.powerupList.CountChipTypes(ChipType.HorizontalRocket, ChipType.VerticalRocket) == 1))
				{
					SeekingMissileAction seekingMissileAction = new SeekingMissileAction();
					SeekingMissileAction.Parameters parameters = new SeekingMissileAction.Parameters();
					this.powerupList.PowerupOfType(ChipType.SeekingMissle).RemoveFromGame();
					parameters.game = this._003Cgame_003E5__2;
					parameters.startSlot = swapToMatchAction.slot2;
					parameters.hasOtherChip = true;
					parameters.otherChipType = this.powerupList.OtherPowerup(ChipType.SeekingMissle).chipType;
					parameters.doCrossExplosion = true;
					parameters.isHavingCarpet = this._003ChasCarpet_003E5__3;
					this.powerupList.OtherPowerup(ChipType.SeekingMissle).RemoveFromGame();
					seekingMissileAction.Init(parameters);
					this._003Cgame_003E5__2.board.actionManager.AddAction(seekingMissileAction);
				}
				else if (this.powerupList.CountChipTypes(ChipType.HorizontalRocket, ChipType.VerticalRocket) == 1 && this.powerupList.CountChipType(ChipType.Bomb) == 1)
				{
					FlyCrossRocketAction flyCrossRocketAction = new FlyCrossRocketAction();
					FlyCrossRocketAction.FlyParams flyParams = default(FlyCrossRocketAction.FlyParams);
					flyParams.game = this._003Cgame_003E5__2;
					for (int j = 0; j < this.powerupList.powerupList.Count; j++)
					{
						Chip item = this.powerupList.powerupList[j];
						flyParams.bombChips.Add(item);
					}
					flyParams.prelockAll = true;
					flyParams.originPosition = swapToMatchAction.slot2.position;
					flyParams.rows = 3;
					flyParams.columns = 3;
					flyParams.useDelayBetweenRowsAndColumns = true;
					flyParams.isHavingCarpet = this._003ChasCarpet_003E5__3;
					flyParams.affectorExport = swapParams.affectorExport;
					flyCrossRocketAction.Init(flyParams);
					this._003Cgame_003E5__2.board.actionManager.AddAction(flyCrossRocketAction);
				}
				else if (this.powerupList.CountChipTypes(ChipType.HorizontalRocket, ChipType.VerticalRocket) == 2)
				{
					FlyCrossRocketAction flyCrossRocketAction2 = new FlyCrossRocketAction();
					FlyCrossRocketAction.FlyParams flyParams2 = default(FlyCrossRocketAction.FlyParams);
					flyParams2.game = this._003Cgame_003E5__2;
					for (int k = 0; k < this.powerupList.powerupList.Count; k++)
					{
						Chip item2 = this.powerupList.powerupList[k];
						flyParams2.bombChips.Add(item2);
					}
					flyParams2.prelockAll = true;
					flyParams2.originPosition = swapToMatchAction.slot2.position;
					flyParams2.rows = 1;
					flyParams2.columns = 1;
					flyParams2.isHavingCarpet = this._003ChasCarpet_003E5__3;
					flyParams2.useDelayBetweenRowsAndColumns = false;
					flyCrossRocketAction2.Init(flyParams2);
					this._003Cgame_003E5__2.board.actionManager.AddAction(flyCrossRocketAction2);
				}
				else if (this.powerupList.CountChipType(ChipType.Bomb) == 2)
				{
					ExplosionAction explosionAction = new ExplosionAction();
					ExplosionAction.ExplosionSettings settings = default(ExplosionAction.ExplosionSettings);
					settings.position = swapToMatchAction.slot2.position;
					settings.radius = 4;
					settings.bombChip = null;
					settings.isUsingBombAreaOfEffect = false;
					explosionAction.Init(this._003Cgame_003E5__2, settings);
					for (int l = 0; l < this.powerupList.powerupList.Count; l++)
					{
						this.powerupList.powerupList[l].RemoveFromGame();
					}
					this._003Cgame_003E5__2.board.actionManager.AddAction(explosionAction);
				}
				else if (this.powerupList.CountChipType(ChipType.DiscoBall) == 2)
				{
					ExplosionAction explosionAction2 = new ExplosionAction();
					ExplosionAction.ExplosionSettings settings2 = default(ExplosionAction.ExplosionSettings);
					settings2.position = swapToMatchAction.slot2.position;
					settings2.radius = Mathf.Max(this._003Cgame_003E5__2.board.size.x, this._003Cgame_003E5__2.board.size.y);
					settings2.bombChip = null;
					settings2.isHavingCarpet = this._003ChasCarpet_003E5__3;
					explosionAction2.Init(this._003Cgame_003E5__2, settings2);
					for (int m = 0; m < this.powerupList.powerupList.Count; m++)
					{
						this.powerupList.powerupList[m].RemoveFromGame();
					}
					this._003Cgame_003E5__2.board.actionManager.AddAction(explosionAction2);
				}
				else if (this.powerupList.CountChipType(ChipType.DiscoBall) == 1)
				{
					Chip chip3 = this.powerupList.OtherPowerup(ChipType.DiscoBall);
					DiscoBallDestroyAction discoBallDestroyAction = new DiscoBallDestroyAction();
					DiscoBallDestroyAction.DiscoParams discoParams = new DiscoBallDestroyAction.DiscoParams();
					Chip chip4 = this.powerupList.PowerupOfType(ChipType.DiscoBall);
					Slot lastConnectedSlot = chip4.lastConnectedSlot;
					discoParams.replaceWithBombs = true;
					discoParams.InitWithItemColor(lastConnectedSlot, this._003Cgame_003E5__2, this._003Cgame_003E5__2.BestItemColorForDiscoBomb(discoParams.replaceWithBombs), discoParams.replaceWithBombs);
					discoParams.bombType = chip3.chipType;
					discoParams.otherBomb = chip3;
					discoParams.originBomb = chip4;
					discoParams.isHavingCarpet = this._003ChasCarpet_003E5__3;
					discoBallDestroyAction.Init(discoParams);
					this._003Cgame_003E5__2.board.actionManager.AddAction(discoBallDestroyAction);
				}
				else
				{
					SlotDestroyParams slotDestroyParams = new SlotDestroyParams();
					slotDestroyParams.isFromSwap = true;
					slotDestroyParams.swapParams = swapParams;
					for (int n = 0; n < this.powerupList.powerupList.Count; n++)
					{
						this.powerupList.powerupList[n].OnDestroySlotComponent(slotDestroyParams);
					}
				}
				swapToMatchAction.slotLock.UnlockAll();
				swapToMatchAction.isAlive = false;
				this._003Cgame_003E5__2.OnUserMadeMove();
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

			public SwapToMatchAction _003C_003E4__this;

			private SwapToMatchAction._003C_003Ec__DisplayClass11_0 _003C_003E8__1;

			public SwapToMatchAction.PowerupList powerupList;

			private Match3Game _003Cgame_003E5__2;

			private bool _003ChasCarpet_003E5__3;
		}

		private sealed class _003C_003Ec__DisplayClass13_0
		{
			internal void _003CDoSwap_003Eb__0()
			{
				this.swapForwardComplete = true;
			}

			public bool swapForwardComplete;
		}

		private sealed class _003C_003Ec__DisplayClass13_1
		{
			internal void _003CDoSwap_003Eb__1()
			{
				this.swapBackComplete = true;
			}

			public bool swapBackComplete;
		}

		private sealed class _003CDoSwap_003Ed__13 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CDoSwap_003Ed__13(int _003C_003E1__state)
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
				SwapToMatchAction swapToMatchAction = this._003C_003E4__this;
				switch (num)
				{
				case 0:
				{
					this._003C_003E1__state = -1;
					this._003C_003E8__1 = new SwapToMatchAction._003C_003Ec__DisplayClass13_0();
					this._003Cgame_003E5__2 = swapToMatchAction.slot1.game;
					if (swapToMatchAction.swapProperties.switchSlotsArgument.affectorExport.hasActions)
					{
						swapToMatchAction.slotLock.UnlockAll();
						this._003Cgame_003E5__2.OnUserMadeMove();
						swapToMatchAction.swapProperties.switchSlotsArgument.affectorExport.ExecuteActions();
						swapToMatchAction.isAlive = false;
						return false;
					}
					this._003Cchip1_003E5__3 = swapToMatchAction.slot1.GetSlotComponent<Chip>();
					this._003Cchip2_003E5__4 = swapToMatchAction.slot2.GetSlotComponent<Chip>();
					SwapToMatchAction.Settings settings = swapToMatchAction.settings;
					swapToMatchAction.slot1.light.AddLight(settings.slot1LightIntensity);
					swapToMatchAction.slot2.light.AddLight(settings.slot2LightIntensity);
					this._003CpowerupList_003E5__5 = new SwapToMatchAction.PowerupList();
					this._003CpowerupList_003E5__5.Add(this._003Cchip1_003E5__3);
					this._003CpowerupList_003E5__5.Add(this._003Cchip2_003E5__4);
					if (this._003CpowerupList_003E5__5.isMixingPowerups)
					{
						this._003CenumList_003E5__8 = new EnumeratorsList();
						this._003CenumList_003E5__8.Add(swapToMatchAction.DoSwapToMixPowerups(this._003CpowerupList_003E5__5), 0f, null, null, false);
					}
					else
					{
						this._003C_003E8__1.swapForwardComplete = false;
						this._003CisInstant_003E5__6 = swapToMatchAction.swapProperties.isInstant;
						this._003Cp_003E5__7 = default(SwapChipsAction.SwapChipsParams);
						this._003Cp_003E5__7.slot1 = swapToMatchAction.slot1;
						this._003Cp_003E5__7.slot2 = swapToMatchAction.slot2;
						this._003Cp_003E5__7.onComplete = new Action(this._003C_003E8__1._003CDoSwap_003Eb__0);
						this._003Cp_003E5__7.switchSlots = true;
						this._003Cp_003E5__7.game = this._003Cgame_003E5__2;
						if (this._003CpowerupList_003E5__5.isMixingPowerups)
						{
							this._003Cp_003E5__7.moveToSpecificPos = true;
							this._003Cp_003E5__7.positionToMoveSlot1 = this._003Cp_003E5__7.slot2.position;
							this._003Cp_003E5__7.positionToMoveSlot2 = this._003Cp_003E5__7.slot2.position;
						}
						if (swapToMatchAction.swapProperties.switchSlotsArgument.isAlreadySwitched)
						{
							goto IL_365;
						}
						if (!this._003CisInstant_003E5__6)
						{
							SwapChipsAction swapChipsAction = new SwapChipsAction();
							swapChipsAction.Init(this._003Cp_003E5__7);
							this._003Cgame_003E5__2.board.actionManager.AddAction(swapChipsAction);
							goto IL_2AA;
						}
						TransformBehaviour transformBehaviour = null;
						TransformBehaviour transformBehaviour2 = null;
						if (this._003Cchip1_003E5__3 != null)
						{
							transformBehaviour = this._003Cchip1_003E5__3.GetComponentBehaviour<TransformBehaviour>();
						}
						if (this._003Cchip2_003E5__4 != null)
						{
							transformBehaviour2 = this._003Cchip2_003E5__4.GetComponentBehaviour<TransformBehaviour>();
						}
						if (transformBehaviour != null)
						{
							transformBehaviour.localPosition = swapToMatchAction.slot2.localPositionOfCenter;
						}
						if (transformBehaviour2 != null)
						{
							transformBehaviour2.localPosition = swapToMatchAction.slot1.localPositionOfCenter;
							goto IL_321;
						}
						goto IL_321;
					}
					break;
				}
				case 1:
					this._003C_003E1__state = -1;
					break;
				case 2:
					this._003C_003E1__state = -1;
					goto IL_2AA;
				case 3:
					this._003C_003E1__state = -1;
					goto IL_779;
				default:
					return false;
				}
				if (!this._003CenumList_003E5__8.Update())
				{
					return false;
				}
				this._003C_003E2__current = null;
				this._003C_003E1__state = 1;
				return true;
				IL_2AA:
				if (!this._003C_003E8__1.swapForwardComplete)
				{
					this._003C_003E2__current = null;
					this._003C_003E1__state = 2;
					return true;
				}
				IL_321:
				swapToMatchAction.slot1.RemoveComponent(this._003Cchip1_003E5__3);
				swapToMatchAction.slot2.RemoveComponent(this._003Cchip2_003E5__4);
				swapToMatchAction.slot1.AddComponent(this._003Cchip2_003E5__4);
				swapToMatchAction.slot2.AddComponent(this._003Cchip1_003E5__3);
				IL_365:
				swapToMatchAction.slotLock.Unlock(swapToMatchAction.slot1);
				swapToMatchAction.slotLock.Unlock(swapToMatchAction.slot2);
				Matches matches = this._003Cgame_003E5__2.board.findMatchesOutside.FindAllMatches();
				Island island = matches.GetIsland(swapToMatchAction.slot1.position);
				Island island2 = matches.GetIsland(swapToMatchAction.slot2.position);
				swapToMatchAction.allSlotsInMatch.Clear();
				if (island != null)
				{
					swapToMatchAction.allSlotsInMatch.AddRange(island.allSlots);
				}
				if (island2 != null)
				{
					swapToMatchAction.allSlotsInMatch.AddRange(island2.allSlots);
				}
				bool flag = island != null || island2 != null;
				bool isActivatingPowerup = this._003CpowerupList_003E5__5.isActivatingPowerup;
				bool flag2 = !flag && !isActivatingPowerup;
				SwapParams swapParams = new SwapParams();
				swapParams.startPosition = swapToMatchAction.slot1.position;
				swapParams.swipeToPosition = swapToMatchAction.slot2.position;
				swapParams.affectorExport = swapToMatchAction.swapProperties.switchSlotsArgument.affectorExport;
				if (!flag2)
				{
					swapToMatchAction.allSlotsInMatch.Add(swapToMatchAction.slot1);
					swapToMatchAction.allSlotsInMatch.Add(swapToMatchAction.slot2);
				}
				if (this._003CisInstant_003E5__6 && !flag2)
				{
					for (int i = 0; i < swapToMatchAction.allSlotsInMatch.Count; i++)
					{
						Slot slot = swapToMatchAction.allSlotsInMatch[i];
						slot.offsetPosition = Vector3.zero;
						slot.positionIntegrator.SetPosition(Vector3.zero);
						slot.prevOffsetPosition = Vector3.zero;
						Chip slotComponent = slot.GetSlotComponent<Chip>();
						if (slotComponent != null)
						{
							TransformBehaviour componentBehaviour = slotComponent.GetComponentBehaviour<TransformBehaviour>();
							if (!(componentBehaviour == null))
							{
								componentBehaviour.slotOffsetPosition = slot.offsetPosition;
							}
						}
					}
				}
				if (!flag2)
				{
					this._003Cgame_003E5__2.OnUserMadeMove();
				}
				if (flag)
				{
					if (island != null)
					{
						island.isFromSwap = true;
					}
					if (island2 != null)
					{
						island2.isFromSwap = true;
					}
					this._003Cgame_003E5__2.ProcessMatches(matches, swapParams);
				}
				if (this._003CpowerupList_003E5__5.isMixingDiscoBallWithColorElement)
				{
					DiscoBallDestroyAction discoBallDestroyAction = new DiscoBallDestroyAction();
					DiscoBallDestroyAction.DiscoParams discoParams = new DiscoBallDestroyAction.DiscoParams();
					Slot lastConnectedSlot = this._003CpowerupList_003E5__5.FirstPowerup.lastConnectedSlot;
					discoParams.replaceWithBombs = false;
					discoParams.InitWithItemColor(lastConnectedSlot, this._003Cgame_003E5__2, this._003CpowerupList_003E5__5.mixingColor, discoParams.replaceWithBombs);
					discoParams.originBomb = this._003CpowerupList_003E5__5.FirstPowerup;
					discoParams.isInstant = this._003CisInstant_003E5__6;
					discoParams.bolts = swapToMatchAction.swapProperties.bolts;
					discoParams.affectorDuration = swapToMatchAction.swapProperties.switchSlotsArgument.affectorDuration;
					discoBallDestroyAction.Init(discoParams);
					this._003Cgame_003E5__2.board.actionManager.AddAction(discoBallDestroyAction);
					swapToMatchAction.swapProperties.bolts = null;
					goto IL_7F7;
				}
				if (this._003CpowerupList_003E5__5.isContainingSingleActivateablePowerup)
				{
					SlotDestroyParams slotDestroyParams = new SlotDestroyParams();
					slotDestroyParams.isFromSwap = true;
					slotDestroyParams.swapParams = swapParams;
					this._003CpowerupList_003E5__5.FirstPowerup.lastConnectedSlot.OnDestroySlot(slotDestroyParams);
					goto IL_7F7;
				}
				if (!flag2 || swapToMatchAction.swapProperties.switchSlotsArgument.isAlreadySwitched)
				{
					goto IL_7F7;
				}
				swapToMatchAction.slotLock.LockSlot(swapToMatchAction.slot1);
				swapToMatchAction.slotLock.LockSlot(swapToMatchAction.slot2);
				swapToMatchAction.slot1.RemoveComponent(this._003Cchip2_003E5__4);
				swapToMatchAction.slot2.RemoveComponent(this._003Cchip1_003E5__3);
				swapToMatchAction.slot1.AddComponent(this._003Cchip1_003E5__3);
				swapToMatchAction.slot2.AddComponent(this._003Cchip2_003E5__4);
				if (!this._003CisInstant_003E5__6)
				{
					this._003C_003E8__2 = new SwapToMatchAction._003C_003Ec__DisplayClass13_1();
					this._003C_003E8__2.swapBackComplete = false;
					SwapChipsAction swapChipsAction2 = new SwapChipsAction();
					this._003Cp_003E5__7.switchSlots = false;
					this._003Cp_003E5__7.onComplete = new Action(this._003C_003E8__2._003CDoSwap_003Eb__1);
					swapChipsAction2.Init(this._003Cp_003E5__7);
					this._003Cgame_003E5__2.board.actionManager.AddAction(swapChipsAction2);
				}
				else
				{
					TransformBehaviour transformBehaviour3 = null;
					TransformBehaviour transformBehaviour4 = null;
					if (this._003Cchip1_003E5__3 != null)
					{
						transformBehaviour3 = this._003Cchip1_003E5__3.GetComponentBehaviour<TransformBehaviour>();
					}
					if (this._003Cchip2_003E5__4 != null)
					{
						transformBehaviour4 = this._003Cchip2_003E5__4.GetComponentBehaviour<TransformBehaviour>();
					}
					if (transformBehaviour3 != null)
					{
						transformBehaviour3.localPosition = swapToMatchAction.slot1.localPositionOfCenter;
					}
					if (transformBehaviour4 != null)
					{
						transformBehaviour4.localPosition = swapToMatchAction.slot2.localPositionOfCenter;
						goto IL_7F7;
					}
					goto IL_7F7;
				}
				IL_779:
				if (!this._003C_003E8__2.swapBackComplete)
				{
					this._003C_003E2__current = null;
					this._003C_003E1__state = 3;
					return true;
				}
				this._003C_003E8__2 = null;
				IL_7F7:
				DiscoBallAffector.RemoveFromGame(swapToMatchAction.swapProperties.bolts);
				swapToMatchAction.lockContainer.UnlockAll();
				swapToMatchAction.isAlive = false;
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

			public SwapToMatchAction _003C_003E4__this;

			private SwapToMatchAction._003C_003Ec__DisplayClass13_0 _003C_003E8__1;

			private SwapToMatchAction._003C_003Ec__DisplayClass13_1 _003C_003E8__2;

			private Match3Game _003Cgame_003E5__2;

			private Chip _003Cchip1_003E5__3;

			private Chip _003Cchip2_003E5__4;

			private SwapToMatchAction.PowerupList _003CpowerupList_003E5__5;

			private bool _003CisInstant_003E5__6;

			private SwapChipsAction.SwapChipsParams _003Cp_003E5__7;

			private EnumeratorsList _003CenumList_003E5__8;
		}
	}
}
