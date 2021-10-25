using System;
using System.Collections.Generic;

namespace GGMatch3
{
	[Serializable]
	public class ExtraFallingElements
	{
		public ExtraFallingElements Clone()
		{
			ExtraFallingElements extraFallingElements = new ExtraFallingElements();
			for (int i = 0; i < this.fallingElementsList.Count; i++)
			{
				ExtraFallingElements.ExtraFallingElement extraFallingElement = this.fallingElementsList[i];
				extraFallingElements.fallingElementsList.Add(extraFallingElement.Clone());
			}
			return extraFallingElements;
		}

		public List<ExtraFallingElements.ExtraFallingElement> fallingElementsList = new List<ExtraFallingElements.ExtraFallingElement>();

		[Serializable]
		public class ExtraFallingElement
		{
			public ExtraFallingElements.ExtraFallingElement Clone()
			{
				return new ExtraFallingElements.ExtraFallingElement
				{
					minMovesBeforeActive = this.minMovesBeforeActive
				};
			}

			public int minMovesBeforeActive;

			public int minCreatedChipsBeforeReactivate;
		}
	}
}
