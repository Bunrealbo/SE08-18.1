using System;

namespace Expressive
{
	[Flags]
	public enum ExpressiveOptions
	{
		None = 1,
		IgnoreCase = 2,
		NoCache = 4,
		RoundAwayFromZero = 8,
		All = 14
	}
}
