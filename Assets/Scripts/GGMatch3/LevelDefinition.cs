using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGMatch3
{
	[Serializable]
	public class LevelDefinition
	{
		public LevelDefinition.ConveyorBelts GetConveyorBelts()
		{
			LevelDefinition.ConveyorBelts conveyorBelts = new LevelDefinition.ConveyorBelts();
			conveyorBelts.Init(this);
			return conveyorBelts;
		}

		public List<LevelDefinition.Portal> GetAllPortals()
		{
			List<LevelDefinition.Portal> list = new List<LevelDefinition.Portal>();
			for (int i = 0; i < this.slots.Count; i++)
			{
				LevelDefinition.SlotDefinition slotDefinition = this.slots[i];
				if (slotDefinition.isPortalExit)
				{
					int portalExitIndex = slotDefinition.portalExitIndex;
					LevelDefinition.Portal portal = null;
					for (int j = 0; j < list.Count; j++)
					{
						LevelDefinition.Portal portal2 = list[j];
						if (portal2.HasEntranceIndex(portalExitIndex))
						{
							portal = portal2;
							break;
						}
					}
					if (portal == null)
					{
						portal = new LevelDefinition.Portal();
						list.Add(portal);
					}
					portal.exitSlot = slotDefinition;
				}
				if (slotDefinition.isPortalEntrance)
				{
					int portalEntranceIndex = slotDefinition.portalEntranceIndex;
					LevelDefinition.Portal portal3 = null;
					for (int k = 0; k < list.Count; k++)
					{
						LevelDefinition.Portal portal4 = list[k];
						if (portal4.HasExitIndex(portalEntranceIndex))
						{
							portal3 = portal4;
							break;
						}
					}
					if (portal3 == null)
					{
						portal3 = new LevelDefinition.Portal();
						list.Add(portal3);
					}
					portal3.entranceSlot = slotDefinition;
				}
			}
			return list;
		}

		public int portalIndexCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.slots.Count; i++)
				{
					num = Mathf.Max(this.slots[i].portalEntranceIndex + 1, num);
				}
				return num;
			}
		}

		public GeneratorSlotSettings GetGeneratorSlotSettings(int index)
		{
			if (index < 0)
			{
				return null;
			}
			if (index >= this.generatorSlotSettings.Count)
			{
				return null;
			}
			return this.generatorSlotSettings[index];
		}

		public LevelDefinition.SlotDefinition GetSlot(IntVector2 position)
		{
			if (position.x < 0 || position.y < 0 || position.x >= this.size.width || position.y >= this.size.height)
			{
				return null;
			}
			int index = this.size.width * position.y + position.x;
			return this.slots[index];
		}

		public void SetSlot(IntVector2 position, LevelDefinition.SlotDefinition slot)
		{
			int index = this.size.width * position.y + position.x;
			this.slots[index] = slot;
		}

		public void ExchangeBurriedElementsForSmallOnes()
		{
			List<LevelDefinition.BurriedElement> list = new List<LevelDefinition.BurriedElement>();
			list.AddRange(this.burriedElements.elements);
			this.burriedElements.elements.Clear();
			for (int i = 0; i < list.Count; i++)
			{
				LevelDefinition.BurriedElement burriedElement = list[i];
				IntVector2 position = burriedElement.position;
				IntVector2 oppositeCornerPosition = burriedElement.oppositeCornerPosition;
				int num = Mathf.Min(position.x, oppositeCornerPosition.x);
				int num2 = Mathf.Max(position.x, oppositeCornerPosition.x);
				int num3 = Mathf.Min(position.y, oppositeCornerPosition.y);
				int num4 = Mathf.Max(position.y, oppositeCornerPosition.y);
				for (int j = num; j <= num2; j++)
				{
					for (int k = num3; k <= num4; k++)
					{
						LevelDefinition.BurriedElement burriedElement2 = new LevelDefinition.BurriedElement();
						burriedElement2.position = new IntVector2(j, k);
						burriedElement2.size = new IntVector2(1, 1);
						burriedElement2.orientation = LevelDefinition.BurriedElement.Orientation.Vertical;
						this.burriedElements.elements.Add(burriedElement2);
					}
				}
			}
		}

		public LevelDefinition Clone()
		{
			LevelDefinition levelDefinition = new LevelDefinition();
			levelDefinition.name = this.name;
			levelDefinition.nextLevelName = this.nextLevelName;
			levelDefinition.prevLevelName = this.prevLevelName;
			levelDefinition.size = this.size.Clone();
			levelDefinition.numChips = this.numChips;
			levelDefinition.generationSettings = this.generationSettings.Clone();
			levelDefinition.suggestMoveType = this.suggestMoveType;
			levelDefinition.suggestMoveSetting = this.suggestMoveSetting;
			levelDefinition.isPowerupPlacementSuspended = this.isPowerupPlacementSuspended;
			levelDefinition.isPreventingGeneratorChipMatching = this.isPreventingGeneratorChipMatching;
			levelDefinition.chanceToNotPreventChipMatching = this.chanceToNotPreventChipMatching;
			levelDefinition.useChanceToNotPreventChipMatching = this.useChanceToNotPreventChipMatching;
			levelDefinition.burriedElements = this.burriedElements.Clone();
			levelDefinition.monsterElements = this.monsterElements.Clone();
			levelDefinition.goals = this.goals.Clone();
			levelDefinition.extraFallingElements = this.extraFallingElements.Clone();
			for (int i = 0; i < this.generatorSlotSettings.Count; i++)
			{
				GeneratorSlotSettings generatorSlotSettings = this.generatorSlotSettings[i];
				levelDefinition.generatorSlotSettings.Add(generatorSlotSettings.Clone());
			}
			for (int j = 0; j < this.tutorialMatches.Count; j++)
			{
				LevelDefinition.TutorialMatch tutorialMatch = this.tutorialMatches[j];
				levelDefinition.tutorialMatches.Add(tutorialMatch.Clone());
			}
			for (int k = 0; k < this.slots.Count; k++)
			{
				LevelDefinition.SlotDefinition slotDefinition = this.slots[k];
				levelDefinition.slots.Add(slotDefinition.Clone());
			}
			for (int l = 0; l < this.generatorSetups.Count; l++)
			{
				GeneratorSetup generatorSetup = this.generatorSetups[l];
				levelDefinition.generatorSetups.Add(generatorSetup.Clone());
			}
			return levelDefinition;
		}

		public void EnsureSizeAndInit()
		{
			for (int i = 0; i < this.size.height; i++)
			{
				for (int j = 0; j < this.size.width; j++)
				{
					int num = this.size.width * i + j;
					if (num >= this.slots.Count)
					{
						LevelDefinition.SlotDefinition slotDefinition = new LevelDefinition.SlotDefinition();
						slotDefinition.gravitySettings.down = true;
						slotDefinition.slotType = SlotType.Empty;
						this.slots.Add(slotDefinition);
					}
					this.slots[num].position = new IntVector2(j, i);
				}
			}
		}

		public int CountChips(ChipType type)
		{
			int num = 0;
			for (int i = 0; i < this.slots.Count; i++)
			{
				LevelDefinition.SlotDefinition slotDefinition = this.slots[i];
				if (slotDefinition.chipType == type)
				{
					num++;
				}
				if (type == ChipType.Carpet && !slotDefinition.hasCarpet)
				{
					num++;
				}
			}
			if (type == ChipType.FallingGingerbreadMan)
			{
				num += this.extraFallingElements.fallingElementsList.Count;
			}
			if (type == ChipType.BurriedElement)
			{
				num = this.burriedElements.CountAllElements();
			}
			return num;
		}

		[SerializeField]
		public string name;

		[SerializeField]
		public string nextLevelName;

		[SerializeField]
		public string prevLevelName;

		[SerializeField]
		public bool lockLevelForEditing;

		[SerializeField]
		private string description;

		[SerializeField]
		private string notes;

		[SerializeField]
		public string tags;

		public long versionIndex;

		public LevelDefinition.Size size = new LevelDefinition.Size();

		public SuggestMoveType suggestMoveType;

		public ShowPotentialMatchSetting suggestMoveSetting;

		public int numChips = 5;

		[SerializeField]
		public LevelDefinition.ChipGenerationSettings generationSettings = new LevelDefinition.ChipGenerationSettings();

		public bool isPowerupPlacementSuspended;

		public List<LevelDefinition.TutorialMatch> tutorialMatches = new List<LevelDefinition.TutorialMatch>();

		public bool isPreventingGeneratorChipMatching;

		public bool useChanceToNotPreventChipMatching;

		public float chanceToNotPreventChipMatching = 50f;

		public List<LevelDefinition.SlotDefinition> slots = new List<LevelDefinition.SlotDefinition>();

		public LevelDefinition.BurriedElements burriedElements = new LevelDefinition.BurriedElements();

		public LevelDefinition.MonsterElements monsterElements = new LevelDefinition.MonsterElements();

		public GoalsDefinition goals = new GoalsDefinition();

		public ExtraFallingElements extraFallingElements = new ExtraFallingElements();

		public List<GeneratorSetup> generatorSetups = new List<GeneratorSetup>();

		public List<GeneratorSlotSettings> generatorSlotSettings = new List<GeneratorSlotSettings>();

		[Serializable]
		public class BurriedElement
		{
			public IntVector2 oppositeCornerPosition
			{
				get
				{
					IntVector2 b = new IntVector2(this.size.x, -this.size.y);
					if (this.orientation == LevelDefinition.BurriedElement.Orientation.Horizontal)
					{
						b = new IntVector2(this.size.y, -this.size.x);
					}
					return this.position + b + new IntVector2(-1, 1);
				}
			}

			public bool ContainsPosition(IntVector2 positionToCheck)
			{
				IntVector2 oppositeCornerPosition = this.oppositeCornerPosition;
				FloatRange floatRange = new FloatRange((float)Mathf.Min(this.position.x, oppositeCornerPosition.x), (float)Mathf.Max(this.position.x, oppositeCornerPosition.x));
				FloatRange floatRange2 = new FloatRange((float)Mathf.Min(this.position.y, oppositeCornerPosition.y), (float)Mathf.Max(this.position.y, oppositeCornerPosition.y));
				return (float)positionToCheck.x >= floatRange.min && (float)positionToCheck.x <= floatRange.max && (float)positionToCheck.y >= floatRange2.min && (float)positionToCheck.y <= floatRange2.max;
			}

			public LevelDefinition.BurriedElement Clone()
			{
				return new LevelDefinition.BurriedElement
				{
					position = this.position,
					size = this.size,
					orientation = this.orientation
				};
			}

			public IntVector2 position;

			public IntVector2 size;

			public LevelDefinition.BurriedElement.Orientation orientation;

			public enum Orientation
			{
				Horizontal,
				Vertical
			}
		}

		[Serializable]
		public class MonsterElement
		{
			public IntVector2 oppositeCornerPosition
			{
				get
				{
					IntVector2 b = new IntVector2(this.size.x, -this.size.y);
					return this.position + b + new IntVector2(-1, 1);
				}
			}

			public bool ContainsPosition(IntVector2 positionToCheck)
			{
				IntVector2 oppositeCornerPosition = this.oppositeCornerPosition;
				FloatRange floatRange = new FloatRange((float)Mathf.Min(this.position.x, oppositeCornerPosition.x), (float)Mathf.Max(this.position.x, oppositeCornerPosition.x));
				FloatRange floatRange2 = new FloatRange((float)Mathf.Min(this.position.y, oppositeCornerPosition.y), (float)Mathf.Max(this.position.y, oppositeCornerPosition.y));
				return (float)positionToCheck.x >= floatRange.min && (float)positionToCheck.x <= floatRange.max && (float)positionToCheck.y >= floatRange2.min && (float)positionToCheck.y <= floatRange2.max;
			}

			public LevelDefinition.MonsterElement Clone()
			{
				return new LevelDefinition.MonsterElement
				{
					position = this.position,
					size = this.size,
					itemColor = this.itemColor,
					numberToCollect = this.numberToCollect
				};
			}

			public IntVector2 position;

			public IntVector2 size;

			public ItemColor itemColor;

			public int numberToCollect;
		}

		[Serializable]
		public class BurriedElements
		{
			public int CountAllElements()
			{
				int num = 0;
				for (int i = 0; i < this.elements.Count; i++)
				{
					LevelDefinition.BurriedElement burriedElement = this.elements[i];
					num += burriedElement.size.x * burriedElement.size.y;
				}
				return num;
			}

			public void MoveByOffset(IntVector2 offset)
			{
				for (int i = 0; i < this.elements.Count; i++)
				{
					this.elements[i].position += offset;
				}
			}

			public bool HasElementsUnderPosition(IntVector2 position)
			{
				for (int i = 0; i < this.elements.Count; i++)
				{
					if (this.elements[i].ContainsPosition(position))
					{
						return true;
					}
				}
				return false;
			}

			public LevelDefinition.BurriedElements Clone()
			{
				LevelDefinition.BurriedElements burriedElements = new LevelDefinition.BurriedElements();
				for (int i = 0; i < this.elements.Count; i++)
				{
					LevelDefinition.BurriedElement burriedElement = this.elements[i];
					burriedElements.elements.Add(burriedElement.Clone());
				}
				return burriedElements;
			}

			public List<LevelDefinition.BurriedElement> elements = new List<LevelDefinition.BurriedElement>();
		}

		[Serializable]
		public class MonsterElements
		{
			public void MoveByOffset(IntVector2 offset)
			{
				for (int i = 0; i < this.elements.Count; i++)
				{
					this.elements[i].position += offset;
				}
			}

			public bool HasElementsUnderPosition(IntVector2 position)
			{
				for (int i = 0; i < this.elements.Count; i++)
				{
					if (this.elements[i].ContainsPosition(position))
					{
						return true;
					}
				}
				return false;
			}

			public LevelDefinition.MonsterElements Clone()
			{
				LevelDefinition.MonsterElements monsterElements = new LevelDefinition.MonsterElements();
				for (int i = 0; i < this.elements.Count; i++)
				{
					LevelDefinition.MonsterElement monsterElement = this.elements[i];
					monsterElements.elements.Add(monsterElement.Clone());
				}
				return monsterElements;
			}

			public List<LevelDefinition.MonsterElement> elements = new List<LevelDefinition.MonsterElement>();
		}

		[Serializable]
		public class Size
		{
			public LevelDefinition.Size Clone()
			{
				return new LevelDefinition.Size
				{
					width = this.width,
					height = this.height
				};
			}

			public int width = 12;

			public int height = 12;
		}

		[Serializable]
		public class GeneratorSettings
		{
			public LevelDefinition.GeneratorSettings Clone()
			{
				return new LevelDefinition.GeneratorSettings
				{
					isGeneratorOn = this.isGeneratorOn,
					generateOnlyBunnies = this.generateOnlyBunnies,
					maxFallingElementsToGenerate = this.maxFallingElementsToGenerate,
					chipTag = this.chipTag,
					slotGeneratorSetupIndex = this.slotGeneratorSetupIndex
				};
			}

			public bool isGeneratorOn;

			public bool generateOnlyBunnies;

			public int maxFallingElementsToGenerate;

			public int chipTag;

			public int slotGeneratorSetupIndex = -1;
		}

		[Serializable]
		public class WallSettings
		{
			public bool isWallActive
			{
				get
				{
					return !this.noWall;
				}
			}

			public bool noWall
			{
				get
				{
					return !this.up && !this.down && !this.left && !this.right;
				}
			}

			public LevelDefinition.WallSettings Clone()
			{
				return new LevelDefinition.WallSettings
				{
					up = this.up,
					down = this.down,
					left = this.left,
					right = this.right
				};
			}

			public bool up;

			public bool down;

			public bool left;

			public bool right;
		}

		[Serializable]
		public class GravitySettings
		{
			public bool noGravity
			{
				get
				{
					return !this.up && !this.down && !this.left && !this.right;
				}
			}

			public LevelDefinition.GravitySettings Clone()
			{
				return new LevelDefinition.GravitySettings
				{
					up = this.up,
					down = this.down,
					left = this.left,
					right = this.right,
					canJumpWithGravity = this.canJumpWithGravity
				};
			}

			public bool up;

			public bool down = true;

			public bool left;

			public bool right;

			public bool canJumpWithGravity;
		}

		[Serializable]
		public class SlotDefinition
		{
			public bool hasHoleInSlot
			{
				get
				{
					return this.chipType == ChipType.EmptyConveyorSpace || this.holeBlocker;
				}
			}

			public bool hasSnowCover
			{
				get
				{
					return this.snowCoverLevel > 0;
				}
			}

			public int moreMovesCount
			{
				get
				{
					if (this.chipType != ChipType.MoreMovesChip)
					{
						return 0;
					}
					return this.itemLevel + 1;
				}
			}

			public bool needsToBeGenerated
			{
				get
				{
					return this.chipType == ChipType.RandomChip || (this.chipType == ChipType.MonsterChip && this.itemColor == ItemColor.RandomColor);
				}
			}

			public bool isFormingMatchesSuspended(LevelDefinition level)
			{
				if (this.IsMonsterInSlot(level))
				{
					return true;
				}
				bool result = this.chipType != ChipType.Chip && this.chipType != ChipType.RandomChip && this.chipType != ChipType.MonsterChip;
				if (this.hasBox || this.hasBubbles || this.hasSnowCover)
				{
					result = true;
				}
				return result;
			}

			public bool isMoveSuspended(LevelDefinition level)
			{
				return this.IsMonsterInSlot(level) || this.hasBox || this.hasChain || this.hasNet || this.hasIce || this.hasBubbles || this.hasSnowCover;
			}

			public bool IsMonsterInSlot(LevelDefinition level)
			{
				return level.monsterElements.HasElementsUnderPosition(this.position);
			}

			public bool isPortalEntrance
			{
				get
				{
					return this.portalEntranceIndex >= 0;
				}
			}

			public bool isPortalExit
			{
				get
				{
					return this.portalExitIndex >= 0;
				}
			}

			public bool isConveyorDirectionSet
			{
				get
				{
					return this.conveyorDirection != IntVector2.zero && this.isPartOfConveyor;
				}
			}

			public bool hasChain
			{
				get
				{
					return this.chainLevel > 0;
				}
			}

			public bool hasBox
			{
				get
				{
					return this.boxLevel > 0;
				}
			}

			public bool hasBasket
			{
				get
				{
					return this.basketLevel > 0;
				}
			}

			public bool hasIce
			{
				get
				{
					return this.iceLevel > 0 && this.chipType != ChipType.EmptyChipSlot;
				}
			}

			public bool hasNet
			{
				get
				{
					return this.netLevel > 0;
				}
			}

			public bool hasColorSlate
			{
				get
				{
					return this.colorSlateLevel > 0;
				}
			}

			public LevelDefinition.SlotDefinition Clone()
			{
				return new LevelDefinition.SlotDefinition
				{
					position = this.position,
					generatorSettings = this.generatorSettings.Clone(),
					gravitySettings = this.gravitySettings.Clone(),
					wallSettings = this.wallSettings.Clone(),
					slotType = this.slotType,
					chipType = this.chipType,
					magicHatItemsCount = this.magicHatItemsCount,
					itemColor = this.itemColor,
					netLevel = this.netLevel,
					boxLevel = this.boxLevel,
					hasBubbles = this.hasBubbles,
					snowCoverLevel = this.snowCoverLevel,
					basketLevel = this.basketLevel,
					iceLevel = this.iceLevel,
					chainLevel = this.chainLevel,
					itemLevel = this.itemLevel,
					isPartOfConveyor = this.isPartOfConveyor,
					conveyorDirection = this.conveyorDirection,
					portalEntranceIndex = this.portalEntranceIndex,
					portalExitIndex = this.portalExitIndex,
					isExitForFallingChip = this.isExitForFallingChip,
					colorSlateLevel = this.colorSlateLevel,
					colorSlateColor = this.colorSlateColor,
					colorSlateSpriteName = this.colorSlateSpriteName,
					hasCarpet = this.hasCarpet,
					chipTag = this.chipTag,
					holeBlocker = this.holeBlocker
				};
			}

			public IntVector2 position;

			public LevelDefinition.GeneratorSettings generatorSettings = new LevelDefinition.GeneratorSettings();

			public LevelDefinition.GravitySettings gravitySettings = new LevelDefinition.GravitySettings();

			public LevelDefinition.WallSettings wallSettings = new LevelDefinition.WallSettings();

			public SlotType slotType;

			public ChipType chipType = ChipType.RandomChip;

			public int chipTag;

			public ItemColor itemColor = ItemColor.Unknown;

			public int netLevel;

			public int magicHatItemsCount;

			public int boxLevel;

			public bool hasBubbles;

			public bool holeBlocker;

			public int snowCoverLevel;

			public int basketLevel;

			public int iceLevel;

			public bool hasCarpet;

			public int chainLevel;

			public int itemLevel;

			public bool isPartOfConveyor;

			public int portalEntranceIndex = -1;

			public int portalExitIndex = -1;

			public IntVector2 conveyorDirection;

			public bool isExitForFallingChip;

			public int colorSlateLevel;

			public Color colorSlateColor;

			public string colorSlateSpriteName;
		}

		[Serializable]
		public class ChipGenerationSettings
		{
			public LevelDefinition.ChipGenerationSettings Clone()
			{
				LevelDefinition.ChipGenerationSettings chipGenerationSettings = new LevelDefinition.ChipGenerationSettings();
				for (int i = 0; i < this.chipSettings.Count; i++)
				{
					LevelDefinition.ChipGenerationSettings.ChipSetting chipSetting = this.chipSettings[i];
					chipGenerationSettings.chipSettings.Add(chipSetting.Clone());
				}
				return chipGenerationSettings;
			}

			public bool isConfigured
			{
				get
				{
					return this.chipSettings.Count > 0;
				}
			}

			public List<LevelDefinition.ChipGenerationSettings.ChipSetting> chipSettings = new List<LevelDefinition.ChipGenerationSettings.ChipSetting>();

			[Serializable]
			public class ChipSetting
			{
				public LevelDefinition.ChipGenerationSettings.ChipSetting Clone()
				{
					return new LevelDefinition.ChipGenerationSettings.ChipSetting
					{
						chipType = this.chipType,
						itemColor = this.itemColor,
						weight = this.weight
					};
				}

				public ChipType chipType;

				public ItemColor itemColor;

				public float weight;
			}
		}

		[Serializable]
		public class TutorialMatch
		{
			public void OffsetAllSlots(IntVector2 offset)
			{
				this.exchangeSlot += offset;
				this.slotToSwipe += offset;
				for (int i = 0; i < this.matchingSlots.Count; i++)
				{
					List<IntVector2> list = this.matchingSlots;
					int index = i;
					list[index] += offset;
				}
			}

			public bool Contains(IntVector2 slot)
			{
				return this.matchingSlots.Contains(slot) || this.exchangeSlot == slot || this.slotToSwipe == slot;
			}

			public LevelDefinition.TutorialMatch Clone()
			{
				LevelDefinition.TutorialMatch tutorialMatch = new LevelDefinition.TutorialMatch();
				tutorialMatch.isEnabled = this.isEnabled;
				tutorialMatch.matchingSlots.AddRange(this.matchingSlots);
				tutorialMatch.slotToSwipe = this.slotToSwipe;
				tutorialMatch.exchangeSlot = this.exchangeSlot;
				return tutorialMatch;
			}

			public bool isEnabled;

			public List<IntVector2> matchingSlots = new List<IntVector2>();

			public IntVector2 slotToSwipe;

			public IntVector2 exchangeSlot;
		}

		public class Portal
		{
			public bool HasEntranceIndex(int index)
			{
				return this.entranceSlot != null && this.entranceSlot.portalEntranceIndex == index;
			}

			public bool HasExitIndex(int index)
			{
				return this.exitSlot != null && this.exitSlot.portalExitIndex == index;
			}

			public bool isValid
			{
				get
				{
					return this.entranceSlot != null && this.exitSlot != null;
				}
			}

			public LevelDefinition.SlotDefinition entranceSlot;

			public LevelDefinition.SlotDefinition exitSlot;
		}

		public class ConveyorBeltLinearSegment
		{
			public void FillLinearSegmentFromSlot(LevelDefinition.SlotDefinition slot, LevelDefinition level)
			{
				LevelDefinition.SlotDefinition slotDefinition = slot;
				for (;;)
				{
					LevelDefinition.SlotDefinition slot2 = level.GetSlot(slotDefinition.position - slotDefinition.conveyorDirection);
					if (slot2 == null || !slot2.isConveyorDirectionSet || slot2.conveyorDirection != slotDefinition.conveyorDirection)
					{
						break;
					}
					slotDefinition = slot2;
				}
				LevelDefinition.SlotDefinition slotDefinition2 = slotDefinition;
				for (;;)
				{
					this.slotList.Add(slotDefinition2);
					LevelDefinition.SlotDefinition slot3 = level.GetSlot(slotDefinition2.position + slotDefinition2.conveyorDirection);
					if (slot3 == null || !slot3.isConveyorDirectionSet || slot3.conveyorDirection != slot.conveyorDirection)
					{
						break;
					}
					slotDefinition2 = slot3;
				}
			}

			public IntVector2 direction
			{
				get
				{
					if (this.enterSlot == null)
					{
						return IntVector2.zero;
					}
					return this.enterSlot.conveyorDirection;
				}
			}

			public LevelDefinition.SlotDefinition enterSlot
			{
				get
				{
					if (this.slotList.Count == 0)
					{
						return null;
					}
					return this.slotList[0];
				}
			}

			public LevelDefinition.SlotDefinition exitSlot
			{
				get
				{
					if (this.slotList.Count == 0)
					{
						return null;
					}
					return this.slotList[this.slotList.Count - 1];
				}
			}

			public IntVector2 exitPosition
			{
				get
				{
					LevelDefinition.SlotDefinition exitSlot = this.exitSlot;
					if (exitSlot == null)
					{
						return IntVector2.zero;
					}
					return exitSlot.position + exitSlot.conveyorDirection;
				}
			}

			public bool IsContaining(IntVector2 position)
			{
				for (int i = 0; i < this.slotList.Count; i++)
				{
					if (this.slotList[i].position == position)
					{
						return true;
					}
				}
				return false;
			}

			public bool IsContaining(LevelDefinition.SlotDefinition slot)
			{
				for (int i = 0; i < this.slotList.Count; i++)
				{
					if (this.slotList[i] == slot)
					{
						return true;
					}
				}
				return false;
			}

			public List<LevelDefinition.SlotDefinition> slotList = new List<LevelDefinition.SlotDefinition>();

			private List<IntVector2> possibleEntryPositions_ = new List<IntVector2>();
		}

		public class ConveyorBelts
		{
			private LevelDefinition.ConveyorBeltLinearSegment SegmentPriorTo(LevelDefinition.ConveyorBeltLinearSegment nextSegment)
			{
				for (int i = 0; i < this.linearSegmentsList.Count; i++)
				{
					LevelDefinition.ConveyorBeltLinearSegment conveyorBeltLinearSegment = this.linearSegmentsList[i];
					if (nextSegment.IsContaining(conveyorBeltLinearSegment.exitPosition))
					{
						return conveyorBeltLinearSegment;
					}
				}
				return null;
			}

			private LevelDefinition.ConveyorBeltLinearSegment SegmentAfter(LevelDefinition.ConveyorBeltLinearSegment prevSegment)
			{
				for (int i = 0; i < this.linearSegmentsList.Count; i++)
				{
					LevelDefinition.ConveyorBeltLinearSegment conveyorBeltLinearSegment = this.linearSegmentsList[i];
					if (conveyorBeltLinearSegment.IsContaining(prevSegment.exitPosition))
					{
						return conveyorBeltLinearSegment;
					}
				}
				return null;
			}

			public bool IsPartOfConveyor(IntVector2 position)
			{
				for (int i = 0; i < this.conveyorBeltList.Count; i++)
				{
					if (this.conveyorBeltList[i].IsContaining(position))
					{
						return true;
					}
				}
				return false;
			}

			public bool IsPartOfConveyor(LevelDefinition.ConveyorBeltLinearSegment segment)
			{
				for (int i = 0; i < this.conveyorBeltList.Count; i++)
				{
					if (this.conveyorBeltList[i].IsContaining(segment))
					{
						return true;
					}
				}
				return false;
			}

			public bool IsPartOfLinearSegmentList(LevelDefinition.SlotDefinition slot)
			{
				for (int i = 0; i < this.linearSegmentsList.Count; i++)
				{
					if (this.linearSegmentsList[i].IsContaining(slot))
					{
						return true;
					}
				}
				return false;
			}

			public void Init(LevelDefinition level)
			{
				new List<LevelDefinition.ConveyorBelt>();
				List<LevelDefinition.SlotDefinition> slots = level.slots;
				this.linearSegmentsList.Clear();
				for (int i = 0; i < slots.Count; i++)
				{
					LevelDefinition.SlotDefinition slotDefinition = slots[i];
					if (slotDefinition != null && slotDefinition.isPartOfConveyor && slotDefinition.isConveyorDirectionSet && !this.IsPartOfLinearSegmentList(slotDefinition))
					{
						LevelDefinition.ConveyorBeltLinearSegment conveyorBeltLinearSegment = new LevelDefinition.ConveyorBeltLinearSegment();
						conveyorBeltLinearSegment.FillLinearSegmentFromSlot(slotDefinition, level);
						this.linearSegmentsList.Add(conveyorBeltLinearSegment);
					}
				}
				this.CreateConveyorBeltsFromSegments(level);
			}

			private void CreateConveyorBeltsFromSegments(LevelDefinition level)
			{
				for (int i = 0; i < this.linearSegmentsList.Count; i++)
				{
					LevelDefinition.ConveyorBeltLinearSegment conveyorBeltLinearSegment = this.linearSegmentsList[i];
					if (!this.IsPartOfConveyor(conveyorBeltLinearSegment))
					{
						LevelDefinition.ConveyorBelt conveyorBelt = new LevelDefinition.ConveyorBelt();
						bool flag = false;
						LevelDefinition.ConveyorBeltLinearSegment conveyorBeltLinearSegment2 = conveyorBeltLinearSegment;
						int num = 0;
						for (;;)
						{
							LevelDefinition.ConveyorBeltLinearSegment conveyorBeltLinearSegment3 = this.SegmentPriorTo(conveyorBeltLinearSegment2);
							if (conveyorBeltLinearSegment3 == null)
							{
								break;
							}
							conveyorBeltLinearSegment2 = conveyorBeltLinearSegment3;
							if (conveyorBeltLinearSegment2 == conveyorBeltLinearSegment)
							{
								break;
							}
							num++;
							if (num > this.linearSegmentsList.Count)
							{
								goto Block_4;
							}
						}
						IL_68:
						num = 0;
						LevelDefinition.ConveyorBeltLinearSegment conveyorBeltLinearSegment4 = conveyorBeltLinearSegment2;
						for (;;)
						{
							conveyorBelt.segmentList.Add(conveyorBeltLinearSegment4);
							LevelDefinition.ConveyorBeltLinearSegment conveyorBeltLinearSegment5 = this.SegmentAfter(conveyorBeltLinearSegment4);
							if (conveyorBeltLinearSegment5 == null)
							{
								break;
							}
							if (conveyorBeltLinearSegment5 == conveyorBeltLinearSegment2)
							{
								goto Block_6;
							}
							conveyorBeltLinearSegment4 = conveyorBeltLinearSegment5;
							num++;
							if (num > this.linearSegmentsList.Count)
							{
								goto Block_7;
							}
						}
						IL_CB:
						if (!flag)
						{
							this.conveyorBeltList.Add(conveyorBelt);
							goto IL_DD;
						}
						goto IL_DD;
						Block_6:
						conveyorBelt.isLoop = true;
						goto IL_CB;
						Block_7:
						conveyorBelt.segmentList.Clear();
						UnityEngine.Debug.LogError("CONVEYOR LIST HAS MULTIPLE ENTRIES");
						flag = true;
						goto IL_CB;
						Block_4:
						UnityEngine.Debug.LogError("CONVEYOR LIST HAS MULTIPLE ENTRIES");
						flag = true;
						goto IL_68;
					}
					IL_DD:;
				}
			}

			public List<LevelDefinition.ConveyorBelt> conveyorBeltList = new List<LevelDefinition.ConveyorBelt>();

			private List<LevelDefinition.ConveyorBeltLinearSegment> linearSegmentsList = new List<LevelDefinition.ConveyorBeltLinearSegment>();
		}

		public class ConveyorBelt
		{
			public LevelDefinition.ConveyorBeltLinearSegment firstSegment
			{
				get
				{
					return this.segmentList[0];
				}
			}

			public LevelDefinition.ConveyorBeltLinearSegment lastSegment
			{
				get
				{
					return this.segmentList[this.segmentList.Count - 1];
				}
			}

			public IntVector2 firstPosition
			{
				get
				{
					return this.firstSegment.enterSlot.position;
				}
			}

			public IntVector2 lastPosition
			{
				get
				{
					return this.lastSegment.exitSlot.position;
				}
			}

			public bool IsContaining(IntVector2 position)
			{
				for (int i = 0; i < this.segmentList.Count; i++)
				{
					if (this.segmentList[i].IsContaining(position))
					{
						return true;
					}
				}
				return false;
			}

			public bool IsContaining(LevelDefinition.ConveyorBeltLinearSegment segment)
			{
				for (int i = 0; i < this.segmentList.Count; i++)
				{
					if (this.segmentList[i] == segment)
					{
						return true;
					}
				}
				return false;
			}

			public List<LevelDefinition.ConveyorBeltLinearSegment> segmentList = new List<LevelDefinition.ConveyorBeltLinearSegment>();

			public bool isLoop;
		}
	}
}
