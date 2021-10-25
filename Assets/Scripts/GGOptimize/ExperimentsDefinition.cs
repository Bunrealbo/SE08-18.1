using System;
using System.Collections.Generic;

namespace GGOptimize
{
	[Serializable]
	public class ExperimentsDefinition
	{
		public Variation defaultProperties = new Variation("Default_Properties");

		public List<Experiment> experiments = new List<Experiment>();
	}
}
