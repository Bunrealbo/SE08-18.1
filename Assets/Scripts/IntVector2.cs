using System;
using UnityEngine;

namespace GGMatch3
{
	[Serializable]
	public struct IntVector2
	{
		public IntVector2(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public IntVector2 orthogonal
		{
			get
			{
				return new IntVector2(this.y, this.x);
			}
		}

		public static IntVector2 up
		{
			get
			{
				return new IntVector2(0, 1);
			}
		}

		public static IntVector2 down
		{
			get
			{
				return new IntVector2(0, -1);
			}
		}

		public static IntVector2 zero
		{
			get
			{
				return new IntVector2(0, 0);
			}
		}

		public static IntVector2 left
		{
			get
			{
				return new IntVector2(-1, 0);
			}
		}

		public static IntVector2 right
		{
			get
			{
				return new IntVector2(1, 0);
			}
		}

		public Vector3 ToVector3()
		{
			return new Vector3((float)this.x, (float)this.y, 0f);
		}

		public static IntVector2 operator +(IntVector2 a, IntVector2 b)
		{
			return new IntVector2
			{
				x = a.x + b.x,
				y = a.y + b.y
			};
		}

		public static IntVector2 operator -(IntVector2 a, IntVector2 b)
		{
			return new IntVector2
			{
				x = a.x - b.x,
				y = a.y - b.y
			};
		}

		public static bool operator ==(IntVector2 a, IntVector2 b)
		{
			return a.x == b.x && a.y == b.y;
		}

		public static IntVector2 operator *(IntVector2 a, int b)
		{
			return new IntVector2
			{
				x = a.x * b,
				y = a.y * b
			};
		}

		public static IntVector2 operator *(int b, IntVector2 a)
		{
			return new IntVector2
			{
				x = a.x * b,
				y = a.y * b
			};
		}

		public static bool operator !=(IntVector2 a, IntVector2 b)
		{
			return a.x != b.x || a.y != b.y;
		}

		public static IntVector2 operator -(IntVector2 a)
		{
			return new IntVector2
			{
				x = -a.x,
				y = -a.y
			};
		}

		public override string ToString()
		{
			return string.Format("({0},{1})", this.x, this.y);
		}

		public int x;

		public int y;

		public static IntVector2[] upDownLeftRight = new IntVector2[]
		{
			IntVector2.up,
			IntVector2.down,
			IntVector2.left,
			IntVector2.right
		};

		public static IntVector2[] upDown = new IntVector2[]
		{
			IntVector2.up,
			IntVector2.down
		};

		public static IntVector2[] leftRight = new IntVector2[]
		{
			IntVector2.left,
			IntVector2.right
		};
	}
}
