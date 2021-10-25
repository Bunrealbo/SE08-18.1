using System;

namespace GGMatch3
{
	public class ConveyorBeltPlate : SlotComponent
	{
		public override bool isMovingWithConveyor
		{
			get
			{
				return true;
			}
		}
	}
}
