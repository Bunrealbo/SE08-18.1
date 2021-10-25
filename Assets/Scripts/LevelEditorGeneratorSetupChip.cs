using System;
using System.Collections.Generic;
using GGMatch3;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorGeneratorSetupChip : MonoBehaviour
{
	public void Init(GeneratorSetup.GeneratorChipSetup chip, GeneratorSetup setup)
	{
		this.SetColorCode(setup);
		if (chip == null)
		{
			GGUtil.Hide(this.chipsContainer);
			GGUtil.Show(this.emptyContainer);
			return;
		}
		GGUtil.Hide(this.emptyContainer);
		GGUtil.Show(this.chipsContainer);
		for (int i = 0; i < this.chips.Count; i++)
		{
			LevelEditorGeneratorSetupChip.ChipDescriptor chipDescriptor = this.chips[i];
			GGUtil.SetActive(chipDescriptor.container, chipDescriptor.color == chip.itemColor);
		}
	}

	public void SetColorCode(GeneratorSetup setup)
	{
		if (this.colorCodeImage == null)
		{
			return;
		}
		Color color = GGUtil.colorProvider.GetColor(setup.position.y);
		this.colorCodeImage.color = color;
	}

	[SerializeField]
	private List<LevelEditorGeneratorSetupChip.ChipDescriptor> chips = new List<LevelEditorGeneratorSetupChip.ChipDescriptor>();

	[SerializeField]
	private RectTransform chipsContainer;

	[SerializeField]
	private RectTransform emptyContainer;

	[SerializeField]
	private Image colorCodeImage;

	[Serializable]
	public class ChipDescriptor
	{
		public ItemColor color;

		public RectTransform container;
	}
}
