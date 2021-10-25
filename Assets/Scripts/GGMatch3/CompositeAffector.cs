using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGMatch3
{
	public class CompositeAffector : PlayerInput.AffectorBase
	{
		public override void ReleaseLocks()
		{
			for (int i = 0; i < this.chipAffectors.Count; i++)
			{
				this.chipAffectors[i].ReleaseLocks();
			}
		}

		public override void ApplyLocks()
		{
			for (int i = 0; i < this.chipAffectors.Count; i++)
			{
				this.chipAffectors[i].ApplyLocks();
			}
		}

		public void Init(CompositeAffector.InitArguments initArguments)
		{
			this.Clear();
			this.initArguments = initArguments;
			Match3Game game = initArguments.game;
			this.ignoreSlots.Clear();
			List<CompositeAffector.SwipedSlot> swipedSlots = initArguments.swipedSlots;
			for (int i = 0; i < swipedSlots.Count; i++)
			{
				CompositeAffector.SwipedSlot swipedSlot = swipedSlots[i];
				this.ignoreSlots.Add(swipedSlot.slot);
				this.ignoreSlots.AddRange(swipedSlot.matchingSlots);
			}
			int j = 0;
			while (j < swipedSlots.Count)
			{
				CompositeAffector.SwipedSlot swipedSlot2 = swipedSlots[j];
				if (swipedSlot2.isDiscoCombine)
				{
					DiscoChipAffector discoChipAffector = new DiscoChipAffector();
					discoChipAffector.Init(swipedSlot2.slot, swipedSlot2.mixSlot, game);
					this.chipAffectors.Add(discoChipAffector);
					goto IL_31B;
				}
				if (swipedSlot2.isMix)
				{
					CombineChipAffectors combineChipAffectors = new CombineChipAffectors();
					combineChipAffectors.Init(swipedSlot2, game);
					this.chipAffectors.Add(combineChipAffectors);
					if (swipedSlot2.mixClass.CountOfType(ChipType.DiscoBall) == 1)
					{
						Chip chip = swipedSlot2.mixClass.GetChip(ChipType.DiscoBall);
						Chip otherChip = swipedSlot2.mixClass.GetOtherChip(ChipType.DiscoBall);
						ItemColor itemColor = game.BestItemColorForDiscoBomb(true);
						List<Slot> slots = game.SlotsThatCanParticipateInDiscoBallAffectedArea(itemColor, true);
						DiscoChipCombineWithPowerupAffector discoChipCombineWithPowerupAffector = new DiscoChipCombineWithPowerupAffector();
						discoChipCombineWithPowerupAffector.Init(chip.lastConnectedSlot, otherChip.lastConnectedSlot, otherChip.chipType, game, slots);
						this.chipAffectors.Add(discoChipCombineWithPowerupAffector);
						goto IL_31B;
					}
					if (swipedSlot2.mixClass.CountOfType(ChipType.HorizontalRocket, ChipType.VerticalRocket) == 2)
					{
						PowerCrossChipAffector powerCrossChipAffector = new PowerCrossChipAffector();
						powerCrossChipAffector.Init(swipedSlot2.mixSlot, game, 0);
						this.chipAffectors.Add(powerCrossChipAffector);
						goto IL_31B;
					}
					if (swipedSlot2.mixClass.CountOfType(ChipType.HorizontalRocket, ChipType.VerticalRocket) == 1 && swipedSlot2.mixClass.CountOfType(ChipType.Bomb) == 1)
					{
						PowerCrossChipAffector powerCrossChipAffector2 = new PowerCrossChipAffector();
						powerCrossChipAffector2.Init(swipedSlot2.mixSlot, game, 1);
						this.chipAffectors.Add(powerCrossChipAffector2);
						goto IL_31B;
					}
					if (swipedSlot2.mixClass.CountOfType(ChipType.Bomb) == 2)
					{
						BombChipAffector bombChipAffector = new BombChipAffector();
						bombChipAffector.Init(swipedSlot2.mixSlot, game, 3, false, BombChipAffector.PowerupType.Block);
						this.chipAffectors.Add(bombChipAffector);
						goto IL_31B;
					}
					if (swipedSlot2.mixClass.CountOfType(ChipType.SeekingMissle) == 1)
					{
						BombChipAffector bombChipAffector2 = new BombChipAffector();
						bombChipAffector2.Init(swipedSlot2.mixSlot, game, 1, true, BombChipAffector.PowerupType.Seeking);
						this.chipAffectors.Add(bombChipAffector2);
						goto IL_31B;
					}
					goto IL_31B;
				}
				else
				{
					if (!swipedSlot2.isPowerup)
					{
						goto IL_31B;
					}
					if (swipedSlot2.chipType == ChipType.SeekingMissle)
					{
						BombChipAffector bombChipAffector3 = new BombChipAffector();
						bombChipAffector3.Init(swipedSlot2.slot, game, 1, true, BombChipAffector.PowerupType.Seeking);
						this.chipAffectors.Add(bombChipAffector3);
					}
					else if (swipedSlot2.chipType == ChipType.Bomb)
					{
						BombChipAffector bombChipAffector4 = new BombChipAffector();
						bombChipAffector4.Init(swipedSlot2.slot, game, 2, false, BombChipAffector.PowerupType.Bomb);
						this.chipAffectors.Add(bombChipAffector4);
					}
					else if (swipedSlot2.chipType == ChipType.VerticalRocket || swipedSlot2.chipType == ChipType.HorizontalRocket)
					{
						LineChipAffector lineChipAffector = new LineChipAffector();
						IntVector2 direction = (swipedSlot2.chipType == ChipType.VerticalRocket) ? IntVector2.up : IntVector2.right;
						lineChipAffector.Init(swipedSlot2.slot, game, direction);
						this.chipAffectors.Add(lineChipAffector);
					}
				}
				IL_3B8:
				j++;
				continue;
				IL_31B:
				if (swipedSlot2.isMatching)
				{
					MatchChipAffector.InitArguments initArguments2 = new MatchChipAffector.InitArguments();
					initArguments2.game = game;
					initArguments2.cameFromPositionSet = true;
					initArguments2.cameFromPosition = swipedSlot2.cameFromPosition;
					initArguments2.otherChipToMove = swipedSlot2.otherChipToMove;
					initArguments2.isCreatingPowerup = swipedSlot2.isCreatingPowerup;
					initArguments2.originSlot = swipedSlot2.slot;
					initArguments2.matchingSlots.AddRange(swipedSlot2.matchingSlots);
					initArguments2.ignoreSlots.AddRange(this.ignoreSlots);
					MatchChipAffector matchChipAffector = new MatchChipAffector();
					matchChipAffector.Init(initArguments2);
					this.chipAffectors.Add(matchChipAffector);
					goto IL_3B8;
				}
				goto IL_3B8;
			}
		}

		public override bool wantsToEnd
		{
			get
			{
				if (!Match3Settings.instance.swipeAffectorSettings.hasMaxAffectorDuration)
				{
					return false;
				}
				List<CompositeAffector.SwipedSlot> swipedSlots = this.initArguments.swipedSlots;
				for (int i = 0; i < swipedSlots.Count; i++)
				{
					if (swipedSlots[i].isMix)
					{
						return false;
					}
				}
				return this.affectorDuration > Match3Settings.instance.swipeAffectorSettings.maxMaxAffectorDuration;
			}
		}

		public override bool canFinish
		{
			get
			{
				if (this.affectorDuration < this.minAffectorDuration)
				{
					return false;
				}
				for (int i = 0; i < this.chipAffectors.Count; i++)
				{
					if (!this.chipAffectors[i].canFinish)
					{
						return false;
					}
				}
				return true;
			}
		}

		public override float minAffectorDuration
		{
			get
			{
				float num = 0f;
				List<CompositeAffector.SwipedSlot> swipedSlots = this.initArguments.swipedSlots;
				for (int i = 0; i < swipedSlots.Count; i++)
				{
					float num2 = 0f;
					CompositeAffector.SwipedSlot swipedSlot = swipedSlots[i];
					if (swipedSlot.isPowerup && swipedSlot.chipType == ChipType.DiscoBall)
					{
						num2 = Mathf.Max(num2, Match3Settings.instance.discoBallAffectorSettings.minDuration);
					}
					else if (swipedSlot.isMix)
					{
						num2 = Match3Settings.instance.swipeAffectorSettings.minAffectorDurationMix;
					}
					else if (swipedSlot.isPowerup)
					{
						if (swipedSlot.chipType == ChipType.Bomb && Match3Settings.instance.playerInputSettings.disableBombLighting)
						{
							num2 = 0f;
						}
						else
						{
							num2 = Mathf.Max(num2, Match3Settings.instance.seekingMissleAffectorSettings.minAffectorDuration);
						}
					}
					else if (swipedSlot.isMatching)
					{
						if (swipedSlot.isCreatingPowerup)
						{
							num2 = Mathf.Max(num2, Match3Settings.instance.swipeAffectorSettings.minAffectorDurationPowerup);
						}
						else
						{
							num2 = Mathf.Max(num2, Match3Settings.instance.swipeAffectorSettings.minAffectorDuration);
						}
					}
					num = Mathf.Max(num, num2);
				}
				return num;
			}
		}

		public override void AddToSwitchSlotArguments(ref Match3Game.SwitchSlotsArguments switchSlotsArguments)
		{
			base.AddToSwitchSlotArguments(ref switchSlotsArguments);
			switchSlotsArguments.bolts = new List<LightingBolt>();
			for (int i = 0; i < this.chipAffectors.Count; i++)
			{
				ChipAffectorBase chipAffectorBase = this.chipAffectors[i];
				chipAffectorBase.AddToInputAffectorExport(switchSlotsArguments.affectorExport);
				switchSlotsArguments.affectorExport.AddChipAffector(chipAffectorBase);
			}
		}

		public override void Clear()
		{
			this.affectorDuration = 0f;
			for (int i = 0; i < this.chipAffectors.Count; i++)
			{
				this.chipAffectors[i].Clear();
			}
			this.chipAffectors.Clear();
		}

		public override void OnBeforeDestroy()
		{
		}

		public override void OnUpdate(PlayerInput.AffectorUpdateParams updateParams)
		{
			this.affectorDuration += Time.deltaTime;
			for (int i = 0; i < this.chipAffectors.Count; i++)
			{
				this.chipAffectors[i].Update();
			}
		}

		private CompositeAffector.InitArguments initArguments;

		private List<ChipAffectorBase> chipAffectors = new List<ChipAffectorBase>();

		private List<Slot> ignoreSlots = new List<Slot>();

		public class SwipedSlot
		{
			public bool isMix
			{
				get
				{
					return this.mixSlot != null;
				}
			}

			public bool isPowerup
			{
				get
				{
					Chip slotComponent = this.slot.GetSlotComponent<Chip>();
					return slotComponent != null && slotComponent.isPowerup;
				}
			}

			public ChipType chipType
			{
				get
				{
					Chip slotComponent = this.slot.GetSlotComponent<Chip>();
					if (slotComponent == null)
					{
						return ChipType.Unknown;
					}
					return slotComponent.chipType;
				}
			}

			public bool isMatching
			{
				get
				{
					return this.matchingSlots.Count > 1;
				}
			}

			public void SetMix(Slot mixSlot)
			{
				this.mixSlot = mixSlot;
				this.mixClass = new PlayerInput.MixClass();
				this.mixClass.TryAdd(this.slot.GetSlotComponent<Chip>());
				this.mixClass.TryAdd(mixSlot.GetSlotComponent<Chip>());
			}

			public void SetOtherChipMatch(Slot otherSlot, Island otherIsland)
			{
				if (otherIsland != null)
				{
					return;
				}
				if (otherSlot == null)
				{
					return;
				}
				this.otherChipToMove = otherSlot.GetSlotComponent<Chip>();
			}

			public void SetMatch(Island island)
			{
				this.matchingSlots.Clear();
				if (island == null)
				{
					return;
				}
				List<Slot> allSlots = island.allSlots;
				for (int i = 0; i < allSlots.Count; i++)
				{
					Slot item = allSlots[i];
					this.matchingSlots.Add(item);
				}
				this.isCreatingPowerup = island.isCreatingPowerup;
			}

			public IntVector2 cameFromPosition;

			public Chip otherChipToMove;

			public Slot slot;

			public bool isCreatingPowerup;

			public Slot mixSlot;

			public PlayerInput.MixClass mixClass;

			public bool isDiscoCombine;

			public List<Slot> matchingSlots = new List<Slot>();
		}

		public class InitArguments
		{
			public CompositeAffector.SwipedSlot AddSwipedSlot(Slot slot)
			{
				CompositeAffector.SwipedSlot swipedSlot = new CompositeAffector.SwipedSlot();
				swipedSlot.slot = slot;
				this.swipedSlots.Add(swipedSlot);
				return swipedSlot;
			}

			public Match3Game game;

			public List<CompositeAffector.SwipedSlot> swipedSlots = new List<CompositeAffector.SwipedSlot>();
		}
	}
}
