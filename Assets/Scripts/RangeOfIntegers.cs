using System;

namespace DigitalRuby.ThunderAndLightning
{
	[Serializable]
	public struct RangeOfIntegers
	{
		public int Random(Random r)
		{
			return r.Next(this.Minimum, this.Maximum + 1);
		}

		public int Minimum;

		public int Maximum;
	}
}
