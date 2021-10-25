using System;

namespace GGMatch3
{
	public class SwapParams
	{
		public Match3Game.InputAffectorExport affectorExport
		{
			get
			{
				if (this.affectorExport_ == null)
				{
					this.affectorExport_ = new Match3Game.InputAffectorExport();
				}
				return this.affectorExport_;
			}
			set
			{
				this.affectorExport_ = value;
			}
		}

		public IntVector2 startPosition;

		public IntVector2 swipeToPosition;

		private Match3Game.InputAffectorExport affectorExport_;
	}
}
