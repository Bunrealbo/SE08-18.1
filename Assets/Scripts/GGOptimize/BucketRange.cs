using System;

namespace GGOptimize
{
	[Serializable]
	public class BucketRange
	{
		public int count
		{
			get
			{
				return this.max - this.min;
			}
		}

		public bool IsAcceptable(int bucket)
		{
			return bucket >= this.min && bucket < this.max;
		}

		public int min;

		public int max = 100;
	}
}
