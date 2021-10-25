using System;
using GGMatch3;
using UnityEngine;

public class LevelEditorGeneratorSetup : MonoBehaviour
{
	public float ChipHeight
	{
		get
		{
			return this.chipsPool.prefabHeight;
		}
	}

	public void Init(GeneratorSetup generatorSetup, Vector3 startPosition)
	{
		float prefabHeight = this.chipsPool.prefabHeight;
		this.chipsPool.Clear(false);
		for (int i = 0; i < generatorSetup.chips.Count; i++)
		{
			GeneratorSetup.GeneratorChipSetup chip = generatorSetup.chips[i];
			LevelEditorGeneratorSetupChip levelEditorGeneratorSetupChip = this.chipsPool.Next<LevelEditorGeneratorSetupChip>(true);
			levelEditorGeneratorSetupChip.Init(chip, generatorSetup);
			GGUtil.Show(levelEditorGeneratorSetupChip);
			Vector3 localPosition = startPosition + Vector3.up * (float)i * prefabHeight;
			levelEditorGeneratorSetupChip.transform.localPosition = localPosition;
		}
		Vector3 localPosition2 = startPosition + Vector3.up * (float)generatorSetup.chips.Count * prefabHeight;
		LevelEditorGeneratorSetupChip levelEditorGeneratorSetupChip2 = this.chipsPool.Next<LevelEditorGeneratorSetupChip>(true);
		levelEditorGeneratorSetupChip2.Init(null, generatorSetup);
		GGUtil.Show(levelEditorGeneratorSetupChip2);
		levelEditorGeneratorSetupChip2.transform.localPosition = localPosition2;
		this.chipsPool.HideNotActive();
	}

	[SerializeField]
	private LevelEditorVisualizer.UIElementPool chipsPool = new LevelEditorVisualizer.UIElementPool();
}
