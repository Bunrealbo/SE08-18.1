using System;
using System.Collections.Generic;

namespace GGMatch3
{
	[Serializable]
	public class GeneratorSlotSettings
	{
		public GeneratorSlotSettings Clone()
		{
			GeneratorSlotSettings generatorSlotSettings = new GeneratorSlotSettings();
			generatorSlotSettings.weight = this.weight;
			generatorSlotSettings.hasIce = this.hasIce;
			generatorSlotSettings.iceLevel = this.iceLevel;
			for (int i = 0; i < this.chipSettings.Count; i++)
			{
				LevelDefinition.ChipGenerationSettings.ChipSetting chipSetting = this.chipSettings[i];
				generatorSlotSettings.chipSettings.Add(chipSetting.Clone());
			}
			return generatorSlotSettings;
		}

		public float weight;

		public bool hasIce;

		public int iceLevel;

		public List<LevelDefinition.ChipGenerationSettings.ChipSetting> chipSettings = new List<LevelDefinition.ChipGenerationSettings.ChipSetting>();
	}
}
