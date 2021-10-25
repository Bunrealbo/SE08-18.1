using System;
using System.Collections.Generic;
using GGMatch3;
using UnityEngine;

public class IceBehaviour : MonoBehaviour
{
	public void DoOnDestroy(Chip chip)
	{
		for (int i = 0; i < this.graphics.Count; i++)
		{
			this.graphics[i].Hide();
		}
		ChipBehaviour chipBehaviour = null;
		if (chip != null)
		{
			chipBehaviour = chip.GetComponentBehaviour<ChipBehaviour>();
		}
		if (chipBehaviour != null)
		{
			chipBehaviour.SetActive(true);
		}
	}

	public void TryInitIfDifferent(Chip chip, int level)
	{
		ChipType chipType = ChipType.Unknown;
		ItemColor itemColor = ItemColor.Unknown;
		if (chip != null)
		{
			chipType = chip.chipType;
			itemColor = chip.itemColor;
		}
		if (chipType == this.shownType && level == this.shownLevel && itemColor == this.shownColor)
		{
			return;
		}
		this.Init(chip, level);
	}

	public void Init(Chip chip, int level)
	{
		this.shownType = ChipType.Unknown;
		this.shownColor = ItemColor.Unknown;
		if (chip != null)
		{
			this.shownType = chip.chipType;
			this.shownColor = chip.itemColor;
		}
		this.shownLevel = level;
		for (int i = 0; i < this.graphics.Count; i++)
		{
			this.graphics[i].Hide();
		}
		ChipBehaviour chipBehaviour = null;
		if (chip != null)
		{
			chipBehaviour = chip.GetComponentBehaviour<ChipBehaviour>();
		}
		if (level <= 0)
		{
			chipBehaviour.SetActive(false);
			return;
		}
		IceBehaviour.GraphicsDefinition graphicsDefinition = this.DefinitionForChip(chip);
		if (graphicsDefinition == null)
		{
			return;
		}
		graphicsDefinition.ShowLevel(level - 1);
		chipBehaviour = chip.GetComponentBehaviour<ChipBehaviour>();
		if (chipBehaviour == null)
		{
			return;
		}
		chipBehaviour.SetActive(graphicsDefinition.isChipActive);
	}

	private IceBehaviour.GraphicsDefinition DefinitionForChip(Chip chip)
	{
		ChipType chipType = ChipType.Unknown;
		ItemColor itemColor = ItemColor.Unknown;
		if (chip != null)
		{
			chipType = chip.chipType;
			itemColor = chip.itemColor;
		}
		for (int i = 0; i < this.graphics.Count; i++)
		{
			IceBehaviour.GraphicsDefinition graphicsDefinition = this.graphics[i];
			if (graphicsDefinition.ContainsType(chipType, itemColor))
			{
				return graphicsDefinition;
			}
		}
		return null;
	}

	private ChipType shownType;

	private ItemColor shownColor;

	private int shownLevel;

	[SerializeField]
	private List<IceBehaviour.GraphicsDefinition> graphics = new List<IceBehaviour.GraphicsDefinition>();

	[Serializable]
	public class GraphicsDefinition
	{
		public bool ContainsType(ChipType chipType, ItemColor itemColor)
		{
			if (this.isForAnyChip)
			{
				return true;
			}
			for (int i = 0; i < this.types.Count; i++)
			{
				if (this.types[i].ContainsType(chipType, itemColor))
				{
					return true;
				}
			}
			return false;
		}

		public void ShowLevel(int level)
		{
			if (this.levels.Count == 0)
			{
				return;
			}
			GGUtil.SetActive(this.levels[Mathf.Clamp(level, 0, this.levels.Count - 1)], true);
		}

		public void Hide()
		{
			GGUtil.SetActive(this.container, false);
			for (int i = 0; i < this.levels.Count; i++)
			{
				GGUtil.SetActive(this.levels[i], false);
			}
		}

		public List<IceBehaviour.GraphicsDefinition.TypeDefinition> types = new List<IceBehaviour.GraphicsDefinition.TypeDefinition>();

		public Transform container;

		public List<Transform> levels = new List<Transform>();

		public bool isChipActive;

		public bool isForAnyChip;

		[Serializable]
		public class TypeDefinition
		{
			public bool ContainsType(ChipType chipType, ItemColor itemColor)
			{
				return this.chipType == chipType && (chipType != ChipType.Chip || this.itemColor == itemColor);
			}

			public ChipType chipType;

			public ItemColor itemColor;
		}
	}
}
