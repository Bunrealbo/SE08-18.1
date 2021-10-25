using System;

namespace GGOptimize
{
	[Serializable]
	public class NamedProperty
	{
		public int GetInt()
		{
			return this.intVal;
		}

		public bool GetBool()
		{
			return this.boolVal;
		}

		public string GetString()
		{
			return this.strVal;
		}

		public string name;

		public NamedPropertyDataType dataType;

		public int intVal;

		public bool boolVal;

		public string strVal;

		public float floatVal;
	}
}
