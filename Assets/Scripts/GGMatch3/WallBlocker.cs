using System;

namespace GGMatch3
{
	public class WallBlocker : SlotComponent
	{
		public void Init(IntVector2 direction)
		{
			this.blockDirection = direction;
		}

		public override bool isMovingWithConveyor
		{
			get
			{
				return true;
			}
		}

		public override bool isBlockingDirection(IntVector2 direction)
		{
			return this.slot != null && ((this.blockDirection.y != 0 && direction.y == this.blockDirection.y) || (this.blockDirection.x != 0 && direction.x == this.blockDirection.x));
		}

		private IntVector2 blockDirection;
	}
}
