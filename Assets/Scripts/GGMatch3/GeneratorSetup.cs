using System;
using System.Collections.Generic;

namespace GGMatch3
{
	[Serializable]
	public class GeneratorSetup
	{
		public GeneratorSetup Clone()
		{
			GeneratorSetup generatorSetup = new GeneratorSetup();
			generatorSetup.position = this.position;
			for (int i = 0; i < this.chips.Count; i++)
			{
				GeneratorSetup.GeneratorChipSetup generatorChipSetup = this.chips[i];
				generatorSetup.chips.Add(generatorChipSetup.Clone());
			}
			return generatorSetup;
		}

		public IntVector2 position;

		public List<GeneratorSetup.GeneratorChipSetup> chips = new List<GeneratorSetup.GeneratorChipSetup>();

		[Serializable]
		public class GeneratorChipSetup
		{
			public GeneratorSetup.GeneratorChipSetup Clone()
			{
				return new GeneratorSetup.GeneratorChipSetup
				{
					itemColor = this.itemColor
				};
			}

			public ItemColor itemColor;
		}
	}
}
