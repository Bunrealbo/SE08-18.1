using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GGMatch3
{
	public class LevelEditorSlot : MonoBehaviour
	{
		public static void SetActive(List<RectTransform> list, bool active)
		{
			if (list == null)
			{
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				LevelEditorSlot.SetActive(list[i], active);
			}
		}

		public static void SetActive(RectTransform t, bool active)
		{
			if (t == null)
			{
				return;
			}
			GameObject gameObject = t.gameObject;
			if (gameObject == null)
			{
				return;
			}
			gameObject.SetActive(active);
		}

		private static void SetText(Text label, string text)
		{
			if (label == null)
			{
				return;
			}
			if (label.text == text)
			{
				return;
			}
			label.text = text;
		}

		public void Init(LevelDefinition level, LevelDefinition.SlotDefinition slot)
		{
			LevelEditorSlot.SetActive(this.widgetsToHide, false);
			this.SetGeneratorSetup(level, slot);
			LevelEditorSlot.SetActive(this.generatorContainer, slot.generatorSettings.isGeneratorOn);
			LevelEditorSlot.SetText(this.moreMovesLabel, slot.moreMovesCount.ToString());
			string text = slot.generatorSettings.generateOnlyBunnies ? "BUNNIES GEN" : "GEN";
			if (level.GetGeneratorSlotSettings(slot.generatorSettings.slotGeneratorSetupIndex) != null)
			{
				text = text + " - " + slot.generatorSettings.slotGeneratorSetupIndex;
				this.generatorText.color = Color.cyan;
			}
			else
			{
				this.generatorText.color = Color.white;
			}
			LevelEditorSlot.SetText(this.generatorText, text);
			for (int i = 0; i < this.magicHatItemsCount.Count; i++)
			{
				LevelEditorSlot.SetText(this.magicHatItemsCount[i], slot.magicHatItemsCount.ToString());
			}
			LevelEditorSlot.SetActive(this.wallContainer, slot.wallSettings.isWallActive);
			LevelEditorSlot.SetActive(this.wallUp, slot.wallSettings.up);
			LevelEditorSlot.SetActive(this.wallDown, slot.wallSettings.down);
			LevelEditorSlot.SetActive(this.wallLeft, slot.wallSettings.left);
			LevelEditorSlot.SetActive(this.wallRight, slot.wallSettings.right);
			LevelEditorSlot.SetActive(this.downArrow, slot.gravitySettings.down && (slot.gravitySettings.up || slot.gravitySettings.left || slot.gravitySettings.right || slot.gravitySettings.canJumpWithGravity));
			LevelEditorSlot.SetActive(this.upArrow, slot.slotType == SlotType.PlayingSpace && slot.gravitySettings.up);
			LevelEditorSlot.SetActive(this.leftArrow, slot.slotType == SlotType.PlayingSpace && slot.gravitySettings.left);
			LevelEditorSlot.SetActive(this.rightArrow, slot.slotType == SlotType.PlayingSpace && slot.gravitySettings.right);
			LevelEditorSlot.SetActive(this.emptyContainer, slot.slotType == SlotType.Empty);
			LevelEditorSlot.SetActive(this.fillContainer, slot.slotType == SlotType.PlayingSpace);
			LevelEditorSlot.SetActive(this.chipContainer, slot.slotType == SlotType.PlayingSpace);
			LevelEditorSlot.SetActive(this.bubbleContainer, slot.hasBubbles);
			LevelEditorSlot.SetActive(this.snowCoverContainer, slot.hasSnowCover);
			bool flag = slot.chipType == ChipType.Chip || slot.chipType == ChipType.RandomChip;
			for (int j = 0; j < this.chips.Count; j++)
			{
				LevelEditorSlot.ChipDescriptor chipDescriptor = this.chips[j];
				ChipType chipType = flag ? ChipType.Chip : slot.chipType;
				LevelEditorSlot.SetActive(chipDescriptor.container, chipType == chipDescriptor.chipType && chipDescriptor.color == slot.itemColor);
			}
			for (int k = 0; k < this.chipsTypes.Count; k++)
			{
				LevelEditorSlot.ChipTypeDescriptor chipTypeDescriptor = this.chipsTypes[k];
				LevelEditorSlot.SetActive(chipTypeDescriptor.container, chipTypeDescriptor.chipType == slot.chipType);
				if (chipTypeDescriptor.chipType == ChipType.EmptyConveyorSpace && slot.holeBlocker)
				{
					LevelEditorSlot.SetActive(chipTypeDescriptor.container, true);
				}
			}
			Color color = slot.gravitySettings.canJumpWithGravity ? this.jumpGravityColor : this.normalGravityColor;
			for (int l = 0; l < this.coloredGravityImages.Count; l++)
			{
				this.coloredGravityImages[l].color = color;
			}
			if (slot.gravitySettings.noGravity)
			{
				LevelEditorSlot.SetActive(this.downArrow, true);
				LevelEditorSlot.SetActive(this.upArrow, true);
				LevelEditorSlot.SetActive(this.leftArrow, true);
				LevelEditorSlot.SetActive(this.rightArrow, true);
				for (int m = 0; m < this.coloredGravityImages.Count; m++)
				{
					this.coloredGravityImages[m].color = Color.red;
				}
			}
			LevelEditorSlot.SetActive(this.randomChipContainer, slot.chipType == ChipType.RandomChip);
			LevelEditorSlot.SetActive(this.netContainer, slot.hasNet);
			GGUtil.SetActive(this.iceContainer, slot.hasIce);
			this.iceLayerSet.ShowLayer(slot.iceLevel - 1);
			GGUtil.SetActive(this.boxContainer, slot.hasBox);
			this.boxLayerSet.ShowLayer(slot.boxLevel - 1);
			GGUtil.SetActive(this.basketContainer, slot.hasBasket);
			this.basketLayerSet.ShowLayer(slot.basketLevel - 1);
			GGUtil.SetActive(this.chainContainer, slot.hasChain);
			this.chainLayerSet.ShowLayer(slot.chainLevel - 1);
			if (slot.chipType == ChipType.BunnyChip)
			{
				this.bunnyLeyerSet.ShowLayer(slot.itemLevel);
			}
			if (slot.chipType == ChipType.RockBlocker || slot.chipType == ChipType.SnowRockBlocker)
			{
				this.rockBlockLayerSet.ShowLayer(slot.itemLevel);
			}
			if (slot.chipType == ChipType.CookiePickup)
			{
				this.cookiePickupLayerSet.ShowLayer(slot.itemLevel);
			}
			LevelEditorSlot.SetActive(this.exitForFallingChip, slot.isExitForFallingChip);
			LevelEditorSlot.SetActive(this.conveyorContainer, slot.isPartOfConveyor);
			this.SetConveyor(slot);
			this.SetPortals(level, slot);
			GGUtil.SetActive(this.slotColorSlateContainer, slot.hasColorSlate);
			if (slot.hasColorSlate)
			{
				this.slotColorSlateLevelSet.ShowLayer(slot.colorSlateLevel - 1);
			}
			LevelEditorSlot.SetActive(this.carpetContainer, slot.hasCarpet);
		}

		private void SetGeneratorSetup(LevelDefinition level, LevelDefinition.SlotDefinition slot)
		{
			if (this.generatorSetupImage == null)
			{
				return;
			}
			GeneratorSetup generatorSetup = null;
			List<GeneratorSetup> generatorSetups = level.generatorSetups;
			for (int i = 0; i < generatorSetups.Count; i++)
			{
				GeneratorSetup generatorSetup2 = generatorSetups[i];
				if (generatorSetup2.position == slot.position)
				{
					generatorSetup = generatorSetup2;
				}
			}
			if (generatorSetup == null)
			{
				this.generatorSetupImage.color = Color.black;
				return;
			}
			Color color = GGUtil.colorProvider.GetColor(generatorSetup.position.y);
			this.generatorSetupImage.color = color;
		}

		private void SetConveyor(LevelDefinition.SlotDefinition slot)
		{
			if (!slot.isPartOfConveyor)
			{
				return;
			}
			this.conveyorContainer.localScale = new Vector3(1f, 1f, 1f);
			bool flag = slot.conveyorDirection == IntVector2.down || slot.conveyorDirection == IntVector2.right;
			if (!slot.isConveyorDirectionSet)
			{
				LevelEditorSlot.SetActive(this.conveyorContainerUp, false);
				LevelEditorSlot.SetActive(this.conveyorContainerDown, false);
			}
			else
			{
				LevelEditorSlot.SetActive(this.conveyorContainerDown, flag);
				LevelEditorSlot.SetActive(this.conveyorContainerUp, !flag);
			}
			Quaternion localRotation = Quaternion.identity;
			if (slot.conveyorDirection.x != 0)
			{
				localRotation = Quaternion.AngleAxis(90f, Vector3.forward);
			}
			this.conveyorContainer.localRotation = localRotation;
		}

		private void SetPortals(LevelDefinition level, LevelDefinition.SlotDefinition slot)
		{
			if (this.portalContainer == null)
			{
				return;
			}
			this.enterPortal.SetActive(slot.isPortalEntrance);
			this.exitPortal.SetActive(slot.isPortalExit);
			int portalIndexCount = level.portalIndexCount;
			this.enterPortal.Init(slot.portalEntranceIndex, portalIndexCount);
			this.exitPortal.Init(slot.portalExitIndex, portalIndexCount);
			Quaternion localRotation = Quaternion.identity;
			if (slot.gravitySettings.left)
			{
				localRotation = Quaternion.AngleAxis(270f, Vector3.forward);
			}
			if (slot.gravitySettings.right)
			{
				localRotation = Quaternion.AngleAxis(90f, Vector3.forward);
			}
			this.portalContainer.localRotation = localRotation;
		}

		[SerializeField]
		private List<RectTransform> widgetsToHide = new List<RectTransform>();

		[SerializeField]
		private Color normalGravityColor = Color.white;

		[SerializeField]
		private Color jumpGravityColor = Color.green;

		[SerializeField]
		private List<Text> magicHatItemsCount = new List<Text>();

		[SerializeField]
		private List<Image> coloredGravityImages = new List<Image>();

		[SerializeField]
		private RectTransform generatorContainer;

		[SerializeField]
		private Text generatorText;

		[SerializeField]
		private Image generatorSetupImage;

		[SerializeField]
		private RectTransform upArrow;

		[SerializeField]
		private RectTransform downArrow;

		[SerializeField]
		private RectTransform leftArrow;

		[SerializeField]
		private RectTransform rightArrow;

		[SerializeField]
		private RectTransform emptyContainer;

		[SerializeField]
		private RectTransform fillContainer;

		[SerializeField]
		private RectTransform wallContainer;

		[SerializeField]
		private RectTransform wallLeft;

		[SerializeField]
		private RectTransform wallRight;

		[SerializeField]
		private RectTransform wallUp;

		[SerializeField]
		private RectTransform wallDown;

		[SerializeField]
		private RectTransform chipContainer;

		[SerializeField]
		private RectTransform randomChipContainer;

		[SerializeField]
		private RectTransform netContainer;

		[SerializeField]
		private List<RectTransform> boxContainer = new List<RectTransform>();

		[SerializeField]
		private RectTransform iceContainer;

		[SerializeField]
		private LevelEditorSlot.LayerSet iceLayerSet = new LevelEditorSlot.LayerSet();

		[SerializeField]
		private RectTransform conveyorContainer;

		[SerializeField]
		private RectTransform conveyorContainerDown;

		[SerializeField]
		private RectTransform conveyorContainerUp;

		[SerializeField]
		private RectTransform exitForFallingChip;

		[SerializeField]
		private Text moreMovesLabel;

		[SerializeField]
		private RectTransform bubbleContainer;

		[SerializeField]
		private RectTransform snowCoverContainer;

		[SerializeField]
		private List<RectTransform> basketContainer = new List<RectTransform>();

		[SerializeField]
		private LevelEditorSlot.LayerSet basketLayerSet = new LevelEditorSlot.LayerSet();

		[SerializeField]
		private LevelEditorSlot.LayerSet boxLayerSet = new LevelEditorSlot.LayerSet();

		[SerializeField]
		private RectTransform chainContainer;

		[SerializeField]
		private LevelEditorSlot.LayerSet chainLayerSet = new LevelEditorSlot.LayerSet();

		[SerializeField]
		private LevelEditorSlot.LayerSet bunnyLeyerSet = new LevelEditorSlot.LayerSet();

		[SerializeField]
		private LevelEditorSlot.LayerSet rockBlockLayerSet = new LevelEditorSlot.LayerSet();

		[SerializeField]
		private LevelEditorSlot.LayerSet cookiePickupLayerSet = new LevelEditorSlot.LayerSet();

		[SerializeField]
		private RectTransform slotColorSlateContainer;

		[SerializeField]
		private LevelEditorSlot.LayerSet slotColorSlateLevelSet = new LevelEditorSlot.LayerSet();

		[SerializeField]
		private List<LevelEditorSlot.ChipDescriptor> chips = new List<LevelEditorSlot.ChipDescriptor>();

		[SerializeField]
		private List<LevelEditorSlot.ChipTypeDescriptor> chipsTypes = new List<LevelEditorSlot.ChipTypeDescriptor>();

		[SerializeField]
		private RectTransform portalContainer;

		[SerializeField]
		private LevelEditorSlot.PortalDefinition enterPortal = new LevelEditorSlot.PortalDefinition();

		[SerializeField]
		private LevelEditorSlot.PortalDefinition exitPortal = new LevelEditorSlot.PortalDefinition();

		[SerializeField]
		private RectTransform carpetContainer;

		[Serializable]
		public class PortalDefinition
		{
			public void SetActive(bool active)
			{
				GGUtil.SetActive(this.container, active);
			}

			public void Init(int index, int totalCount)
			{
				float t = Mathf.InverseLerp(0f, (float)totalCount, (float)index);
				Color color = Color.Lerp(this.lowColor, this.highColor, t);
				this.coloredImage.color = color;
				string text = string.Format("{0} - {1}", index, this.suffix);
				LevelEditorSlot.SetText(this.label, text);
			}

			public RectTransform container;

			public Color lowColor;

			public Color highColor;

			public Image coloredImage;

			public string suffix;

			public Text label;
		}

		[Serializable]
		public class LayerDesc
		{
			public GameObject objectToShow;
		}

		[Serializable]
		public class LayerSet
		{
			public void ShowLayer(int index)
			{
				int num = Mathf.Clamp(index, 0, this.layers.Count - 1);
				for (int i = 0; i < this.layers.Count; i++)
				{
					LevelEditorSlot.LayerDesc layerDesc = this.layers[i];
					bool active = i == num;
					GGUtil.SetActive(layerDesc.objectToShow, active);
				}
			}

			public List<LevelEditorSlot.LayerDesc> layers = new List<LevelEditorSlot.LayerDesc>();
		}

		[Serializable]
		public class ChipDescriptor
		{
			public ItemColor color;

			public ChipType chipType;

			public RectTransform container;
		}

		[Serializable]
		public class ChipTypeDescriptor
		{
			public ChipType chipType;

			public RectTransform container;
		}
	}
}
