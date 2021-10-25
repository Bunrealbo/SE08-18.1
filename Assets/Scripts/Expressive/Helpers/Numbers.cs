using System;

namespace Expressive.Helpers
{
	internal class Numbers
	{
		private static object ConvertIfString(object s)
		{
			if (s is string || s is char)
			{
				return decimal.Parse(s.ToString());
			}
			return s;
		}

		internal static object Add(object a, object b)
		{
			if (a == null || b == null)
			{
				return null;
			}
			a = Numbers.ConvertIfString(a);
			b = Numbers.ConvertIfString(b);
			if (a is double && double.IsNaN((double)a))
			{
				return a;
			}
			if (b is double && double.IsNaN((double)b))
			{
				return b;
			}
			TypeCode typeCode = TypeHelper.GetTypeCode(a);
			TypeCode typeCode2 = TypeHelper.GetTypeCode(b);
			switch (typeCode)
			{
			case TypeCode.Boolean:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'bool'");
				case TypeCode.SByte:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.Byte:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.Int16:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.UInt16:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.Int32:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.UInt32:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.Int64:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.Single:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.Double:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.Decimal:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
				}
				break;
			case TypeCode.SByte:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'sbyte' and 'bool'");
				case TypeCode.SByte:
					return (int)((sbyte)a + (sbyte)b);
				case TypeCode.Byte:
					return (int)((sbyte)a + (sbyte)((byte)b));
				case TypeCode.Int16:
					return (int)((short)((sbyte)a) + (short)b);
				case TypeCode.UInt16:
					return (int)((ushort)((sbyte)a) + (ushort)b);
				case TypeCode.Int32:
					return (int)((sbyte)a) + (int)b;
				case TypeCode.UInt32:
					return (long)((sbyte)a) + (long)((ulong)((uint)b));
				case TypeCode.Int64:
					return (long)((sbyte)a) + (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'sbyte' and 'ulong'");
				case TypeCode.Single:
					return (float)((sbyte)a) + (float)b;
				case TypeCode.Double:
					return (double)((sbyte)a) + (double)b;
				case TypeCode.Decimal:
					return (sbyte)a + (decimal)b;
				}
				break;
			case TypeCode.Byte:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'byte' and 'bool'");
				case TypeCode.SByte:
					return (int)((byte)a + (byte)((sbyte)b));
				case TypeCode.Byte:
					return (int)((byte)a + (byte)b);
				case TypeCode.Int16:
					return (int)((short)((byte)a) + (short)b);
				case TypeCode.UInt16:
					return (int)((ushort)((byte)a) + (ushort)b);
				case TypeCode.Int32:
					return (int)((byte)a) + (int)b;
				case TypeCode.UInt32:
					return (uint)((byte)a) + (uint)b;
				case TypeCode.Int64:
					return (long)((ulong)((byte)a) + (ulong)((long)b));
				case TypeCode.UInt64:
					return (ulong)((byte)a) + (ulong)b;
				case TypeCode.Single:
					return (float)((byte)a) + (float)b;
				case TypeCode.Double:
					return (double)((byte)a) + (double)b;
				case TypeCode.Decimal:
					return (byte)a + (decimal)b;
				}
				break;
			case TypeCode.Int16:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'short' and 'bool'");
				case TypeCode.SByte:
					return (int)((short)a + (short)((sbyte)b));
				case TypeCode.Byte:
					return (int)((short)a + (short)((byte)b));
				case TypeCode.Int16:
					return (int)((short)a + (short)b);
				case TypeCode.UInt16:
					return (int)((short)a + (short)((ushort)b));
				case TypeCode.Int32:
					return (int)((short)a) + (int)b;
				case TypeCode.UInt32:
					return (long)((short)a) + (long)((ulong)((uint)b));
				case TypeCode.Int64:
					return (long)((short)a) + (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'short' and 'ulong'");
				case TypeCode.Single:
					return (float)((short)a) + (float)b;
				case TypeCode.Double:
					return (double)((short)a) + (double)b;
				case TypeCode.Decimal:
					return (short)a + (decimal)b;
				}
				break;
			case TypeCode.UInt16:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'ushort' and 'bool'");
				case TypeCode.SByte:
					return (int)((ushort)a + (ushort)((sbyte)b));
				case TypeCode.Byte:
					return (int)((ushort)a + (ushort)((byte)b));
				case TypeCode.Int16:
					return (int)((ushort)a + (ushort)((short)b));
				case TypeCode.UInt16:
					return (int)((ushort)a + (ushort)b);
				case TypeCode.Int32:
					return (int)((ushort)a) + (int)b;
				case TypeCode.UInt32:
					return (uint)((ushort)a) + (uint)b;
				case TypeCode.Int64:
					return (long)((ulong)((ushort)a) + (ulong)((long)b));
				case TypeCode.UInt64:
					return (ulong)((ushort)a) + (ulong)b;
				case TypeCode.Single:
					return (float)((ushort)a) + (float)b;
				case TypeCode.Double:
					return (double)((ushort)a) + (double)b;
				case TypeCode.Decimal:
					return (ushort)a + (decimal)b;
				}
				break;
			case TypeCode.Int32:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'int' and 'bool'");
				case TypeCode.SByte:
					return (int)a + (int)((sbyte)b);
				case TypeCode.Byte:
					return (int)a + (int)((byte)b);
				case TypeCode.Int16:
					return (int)a + (int)((short)b);
				case TypeCode.UInt16:
					return (int)a + (int)((ushort)b);
				case TypeCode.Int32:
					return (int)a + (int)b;
				case TypeCode.UInt32:
					return (long)((int)a) + (long)((ulong)((uint)b));
				case TypeCode.Int64:
					return (long)((int)a) + (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'int' and 'ulong'");
				case TypeCode.Single:
					return (float)((int)a) + (float)b;
				case TypeCode.Double:
					return (double)((int)a) + (double)b;
				case TypeCode.Decimal:
					return (int)a + (decimal)b;
				}
				break;
			case TypeCode.UInt32:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'unit' and 'bool'");
				case TypeCode.SByte:
					return (long)((ulong)((uint)a) + (ulong)((long)((sbyte)b)));
				case TypeCode.Byte:
					return (uint)a + (uint)((byte)b);
				case TypeCode.Int16:
					return (long)((ulong)((uint)a) + (ulong)((long)((short)b)));
				case TypeCode.UInt16:
					return (uint)a + (uint)((ushort)b);
				case TypeCode.Int32:
					return (long)((ulong)((uint)a) + (ulong)((long)((int)b)));
				case TypeCode.UInt32:
					return (uint)a + (uint)b;
				case TypeCode.Int64:
					return (long)((ulong)((uint)a) + (ulong)((long)b));
				case TypeCode.UInt64:
					return (ulong)((uint)a) + (ulong)b;
				case TypeCode.Single:
					return (uint)a + (float)b;
				case TypeCode.Double:
					return (uint)a + (double)b;
				case TypeCode.Decimal:
					return (uint)a + (decimal)b;
				}
				break;
			case TypeCode.Int64:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'long' and 'bool'");
				case TypeCode.SByte:
					return (long)a + (long)((sbyte)b);
				case TypeCode.Byte:
					return (long)a + (long)((ulong)((byte)b));
				case TypeCode.Int16:
					return (long)a + (long)((short)b);
				case TypeCode.UInt16:
					return (long)a + (long)((ulong)((ushort)b));
				case TypeCode.Int32:
					return (long)a + (long)((int)b);
				case TypeCode.UInt32:
					return (long)a + (long)((ulong)((uint)b));
				case TypeCode.Int64:
					return (long)a + (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'long' and 'ulong'");
				case TypeCode.Single:
					return (float)((long)a) + (float)b;
				case TypeCode.Double:
					return (double)((long)a) + (double)b;
				case TypeCode.Decimal:
					return (long)a + (decimal)b;
				}
				break;
			case TypeCode.UInt64:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'ulong' and 'bool'");
				case TypeCode.SByte:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'ulong' and 'sbyte'");
				case TypeCode.Byte:
					return (ulong)a + (ulong)((byte)b);
				case TypeCode.Int16:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'ulong' and 'short'");
				case TypeCode.UInt16:
					return (ulong)a + (ulong)((ushort)b);
				case TypeCode.Int32:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'ulong' and 'int'");
				case TypeCode.UInt32:
					return (ulong)a + (ulong)((uint)b);
				case TypeCode.Int64:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'ulong' and 'long'");
				case TypeCode.UInt64:
					return (ulong)a + (ulong)b;
				case TypeCode.Single:
					return (ulong)a + (float)b;
				case TypeCode.Double:
					return (ulong)a + (double)b;
				case TypeCode.Decimal:
					return (ulong)a + (decimal)b;
				}
				break;
			case TypeCode.Single:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'float' and 'bool'");
				case TypeCode.SByte:
					return (float)a + (float)((sbyte)b);
				case TypeCode.Byte:
					return (float)a + (float)((byte)b);
				case TypeCode.Int16:
					return (float)a + (float)((short)b);
				case TypeCode.UInt16:
					return (float)a + (float)((ushort)b);
				case TypeCode.Int32:
					return (float)a + (float)((int)b);
				case TypeCode.UInt32:
					return (float)a + (uint)b;
				case TypeCode.Int64:
					return (float)a + (float)((long)b);
				case TypeCode.UInt64:
					return (float)a + (ulong)b;
				case TypeCode.Single:
					return (float)a + (float)b;
				case TypeCode.Double:
					return (double)((float)a) + (double)b;
				case TypeCode.Decimal:
					return Convert.ToDecimal(a) + (decimal)b;
				}
				break;
			case TypeCode.Double:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'double' and 'bool'");
				case TypeCode.SByte:
					return (double)a + (double)((sbyte)b);
				case TypeCode.Byte:
					return (double)a + (double)((byte)b);
				case TypeCode.Int16:
					return (double)a + (double)((short)b);
				case TypeCode.UInt16:
					return (double)a + (double)((ushort)b);
				case TypeCode.Int32:
					return (double)a + (double)((int)b);
				case TypeCode.UInt32:
					return (double)a + (uint)b;
				case TypeCode.Int64:
					return (double)a + (double)((long)b);
				case TypeCode.UInt64:
					return (double)a + (ulong)b;
				case TypeCode.Single:
					return (double)a + (double)((float)b);
				case TypeCode.Double:
					return (double)a + (double)b;
				case TypeCode.Decimal:
					return Convert.ToDecimal(a) + (decimal)b;
				}
				break;
			case TypeCode.Decimal:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'decimal' and 'bool'");
				case TypeCode.SByte:
					return (decimal)a + (sbyte)b;
				case TypeCode.Byte:
					return (decimal)a + (byte)b;
				case TypeCode.Int16:
					return (decimal)a + (short)b;
				case TypeCode.UInt16:
					return (decimal)a + (ushort)b;
				case TypeCode.Int32:
					return (decimal)a + (int)b;
				case TypeCode.UInt32:
					return (decimal)a + (uint)b;
				case TypeCode.Int64:
					return (decimal)a + (long)b;
				case TypeCode.UInt64:
					return (decimal)a + (ulong)b;
				case TypeCode.Single:
					return (decimal)a + Convert.ToDecimal(b);
				case TypeCode.Double:
					return (decimal)a + Convert.ToDecimal(b);
				case TypeCode.Decimal:
					return (decimal)a + (decimal)b;
				}
				break;
			}
			return null;
		}

		internal static object Divide(object a, object b)
		{
			if (a == null || b == null)
			{
				return null;
			}
			a = Numbers.ConvertIfString(a);
			b = Numbers.ConvertIfString(b);
			if (a is double && double.IsNaN((double)a))
			{
				return a;
			}
			if (b is double && double.IsNaN((double)b))
			{
				return b;
			}
			TypeCode typeCode = TypeHelper.GetTypeCode(a);
			TypeCode typeCode2 = TypeHelper.GetTypeCode(b);
			switch (typeCode)
			{
			case TypeCode.SByte:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'sbyte' and 'bool'");
				case TypeCode.SByte:
					return (int)((sbyte)a / (sbyte)b);
				case TypeCode.Int16:
					return (int)((short)((sbyte)a) / (short)b);
				case TypeCode.UInt16:
					return (int)((ushort)((sbyte)a) / (ushort)b);
				case TypeCode.Int32:
					return (int)((sbyte)a) / (int)b;
				case TypeCode.UInt32:
					return (long)((sbyte)a) / (long)((ulong)((uint)b));
				case TypeCode.Int64:
					return (long)((sbyte)a) / (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'sbyte' and 'ulong'");
				case TypeCode.Single:
					return (float)((sbyte)a) / (float)b;
				case TypeCode.Double:
					return (double)((sbyte)a) / (double)b;
				case TypeCode.Decimal:
					return (sbyte)a / (decimal)b;
				}
				break;
			case TypeCode.Byte:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'byte' and 'bool'");
				case TypeCode.SByte:
					return (int)((byte)a / (byte)((sbyte)b));
				case TypeCode.Int16:
					return (int)((short)((byte)a) / (short)b);
				case TypeCode.UInt16:
					return (int)((ushort)((byte)a) / (ushort)b);
				case TypeCode.Int32:
					return (int)((byte)a) / (int)b;
				case TypeCode.UInt32:
					return (uint)((byte)a) / (uint)b;
				case TypeCode.Int64:
					return (long)((ulong)((byte)a) / (ulong)((long)b));
				case TypeCode.UInt64:
					return (ulong)((byte)a) / (ulong)b;
				case TypeCode.Single:
					return (float)((byte)a) / (float)b;
				case TypeCode.Double:
					return (double)((byte)a) / (double)b;
				case TypeCode.Decimal:
					return (byte)a / (decimal)b;
				}
				break;
			case TypeCode.Int16:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'short' and 'bool'");
				case TypeCode.SByte:
					return (int)((short)a / (short)((sbyte)b));
				case TypeCode.Int16:
					return (int)((short)a / (short)b);
				case TypeCode.UInt16:
					return (int)((short)a / (short)((ushort)b));
				case TypeCode.Int32:
					return (int)((short)a) / (int)b;
				case TypeCode.UInt32:
					return (long)((short)a) / (long)((ulong)((uint)b));
				case TypeCode.Int64:
					return (long)((short)a) / (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'short' and 'ulong'");
				case TypeCode.Single:
					return (float)((short)a) / (float)b;
				case TypeCode.Double:
					return (double)((short)a) / (double)b;
				case TypeCode.Decimal:
					return (short)a / (decimal)b;
				}
				break;
			case TypeCode.UInt16:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'ushort' and 'bool'");
				case TypeCode.SByte:
					return (int)((ushort)a / (ushort)((sbyte)b));
				case TypeCode.Int16:
					return (int)((ushort)a / (ushort)((short)b));
				case TypeCode.UInt16:
					return (int)((ushort)a / (ushort)b);
				case TypeCode.Int32:
					return (int)((ushort)a) / (int)b;
				case TypeCode.UInt32:
					return (uint)((ushort)a) / (uint)b;
				case TypeCode.Int64:
					return (long)((ulong)((ushort)a) / (ulong)((long)b));
				case TypeCode.UInt64:
					return (ulong)((ushort)a) / (ulong)b;
				case TypeCode.Single:
					return (float)((ushort)a) / (float)b;
				case TypeCode.Double:
					return (double)((ushort)a) / (double)b;
				case TypeCode.Decimal:
					return (ushort)a / (decimal)b;
				}
				break;
			case TypeCode.Int32:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'int' and 'bool'");
				case TypeCode.SByte:
					return (int)a / (int)((sbyte)b);
				case TypeCode.Int16:
					return (int)a / (int)((short)b);
				case TypeCode.UInt16:
					return (int)a / (int)((ushort)b);
				case TypeCode.Int32:
					return (int)a / (int)b;
				case TypeCode.UInt32:
					return (long)((int)a) / (long)((ulong)((uint)b));
				case TypeCode.Int64:
					return (long)((int)a) / (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'int' and 'ulong'");
				case TypeCode.Single:
					return (float)((int)a) / (float)b;
				case TypeCode.Double:
					return (double)((int)a) / (double)b;
				case TypeCode.Decimal:
					return (int)a / (decimal)b;
				}
				break;
			case TypeCode.UInt32:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'uint' and 'bool'");
				case TypeCode.SByte:
					return (long)((ulong)((uint)a) / (ulong)((long)((sbyte)b)));
				case TypeCode.Int16:
					return (long)((ulong)((uint)a) / (ulong)((long)((short)b)));
				case TypeCode.UInt16:
					return (uint)a / (uint)((ushort)b);
				case TypeCode.Int32:
					return (long)((ulong)((uint)a) / (ulong)((long)((int)b)));
				case TypeCode.UInt32:
					return (uint)a / (uint)b;
				case TypeCode.Int64:
					return (long)((ulong)((uint)a) / (ulong)((long)b));
				case TypeCode.UInt64:
					return (ulong)((uint)a) / (ulong)b;
				case TypeCode.Single:
					return (uint)a / (float)b;
				case TypeCode.Double:
					return (uint)a / (double)b;
				case TypeCode.Decimal:
					return (uint)a / (decimal)b;
				}
				break;
			case TypeCode.Int64:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'long' and 'bool'");
				case TypeCode.SByte:
					return (long)a / (long)((sbyte)b);
				case TypeCode.Int16:
					return (long)a / (long)((short)b);
				case TypeCode.UInt16:
					return (long)a / (long)((ulong)((ushort)b));
				case TypeCode.Int32:
					return (long)a / (long)((int)b);
				case TypeCode.UInt32:
					return (long)a / (long)((ulong)((uint)b));
				case TypeCode.Int64:
					return (long)a / (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'long' and 'ulong'");
				case TypeCode.Single:
					return (float)((long)a) / (float)b;
				case TypeCode.Double:
					return (double)((long)a) / (double)b;
				case TypeCode.Decimal:
					return (long)a / (decimal)b;
				}
				break;
			case TypeCode.UInt64:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ulong' and 'bool'");
				case TypeCode.SByte:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'ulong' and 'sbyte'");
				case TypeCode.Int16:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'ulong' and 'short'");
				case TypeCode.UInt16:
					return (ulong)a / (ulong)((ushort)b);
				case TypeCode.Int32:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'ulong' and 'int'");
				case TypeCode.UInt32:
					return (ulong)a / (ulong)((uint)b);
				case TypeCode.Int64:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'ulong' and 'long'");
				case TypeCode.UInt64:
					return (ulong)a / (ulong)b;
				case TypeCode.Single:
					return (ulong)a / (float)b;
				case TypeCode.Double:
					return (ulong)a / (double)b;
				case TypeCode.Decimal:
					return (ulong)a / (decimal)b;
				}
				break;
			case TypeCode.Single:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'float' and 'bool'");
				case TypeCode.SByte:
					return (float)a / (float)((sbyte)b);
				case TypeCode.Int16:
					return (float)a / (float)((short)b);
				case TypeCode.UInt16:
					return (float)a / (float)((ushort)b);
				case TypeCode.Int32:
					return (float)a / (float)((int)b);
				case TypeCode.UInt32:
					return (float)a / (uint)b;
				case TypeCode.Int64:
					return (float)a / (float)((long)b);
				case TypeCode.UInt64:
					return (float)a / (ulong)b;
				case TypeCode.Single:
					return (float)a / (float)b;
				case TypeCode.Double:
					return (double)((float)a) / (double)b;
				case TypeCode.Decimal:
					return Convert.ToDecimal(a) / (decimal)b;
				}
				break;
			case TypeCode.Double:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'double' and 'bool'");
				case TypeCode.SByte:
					return (double)a / (double)((sbyte)b);
				case TypeCode.Int16:
					return (double)a / (double)((short)b);
				case TypeCode.UInt16:
					return (double)a / (double)((ushort)b);
				case TypeCode.Int32:
					return (double)a / (double)((int)b);
				case TypeCode.UInt32:
					return (double)a / (uint)b;
				case TypeCode.Int64:
					return (double)a / (double)((long)b);
				case TypeCode.UInt64:
					return (double)a / (ulong)b;
				case TypeCode.Single:
					return (double)a / (double)((float)b);
				case TypeCode.Double:
					return (double)a / (double)b;
				case TypeCode.Decimal:
					return Convert.ToDecimal(a) / (decimal)b;
				}
				break;
			case TypeCode.Decimal:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'decimal' and 'bool'");
				case TypeCode.SByte:
					return (decimal)a / (sbyte)b;
				case TypeCode.Int16:
					return (decimal)a / (short)b;
				case TypeCode.UInt16:
					return (decimal)a / (ushort)b;
				case TypeCode.Int32:
					return (decimal)a / (int)b;
				case TypeCode.UInt32:
					return (decimal)a / (uint)b;
				case TypeCode.Int64:
					return (decimal)a / (long)b;
				case TypeCode.UInt64:
					return (decimal)a / (ulong)b;
				case TypeCode.Single:
					return (decimal)a / Convert.ToDecimal(b);
				case TypeCode.Double:
					return (decimal)a / Convert.ToDecimal(b);
				case TypeCode.Decimal:
					return (decimal)a / (decimal)b;
				}
				break;
			}
			return null;
		}

		internal static object Multiply(object a, object b)
		{
			if (a == null || b == null)
			{
				return null;
			}
			a = Numbers.ConvertIfString(a);
			b = Numbers.ConvertIfString(b);
			if (a is double && double.IsNaN((double)a))
			{
				return a;
			}
			if (b is double && double.IsNaN((double)b))
			{
				return b;
			}
			TypeCode typeCode = TypeHelper.GetTypeCode(a);
			TypeCode typeCode2 = TypeHelper.GetTypeCode(b);
			switch (typeCode)
			{
			case TypeCode.SByte:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'sbyte' and 'bool'");
				case TypeCode.SByte:
					return (int)((sbyte)a * (sbyte)b);
				case TypeCode.Int16:
					return (int)((short)((sbyte)a) * (short)b);
				case TypeCode.UInt16:
					return (int)((ushort)((sbyte)a) * (ushort)b);
				case TypeCode.Int32:
					return (int)((sbyte)a) * (int)b;
				case TypeCode.UInt32:
					return (long)((sbyte)a) * (long)((ulong)((uint)b));
				case TypeCode.Int64:
					return (long)((sbyte)a) * (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'sbyte' and 'ulong'");
				case TypeCode.Single:
					return (float)((sbyte)a) * (float)b;
				case TypeCode.Double:
					return (double)((sbyte)a) * (double)b;
				case TypeCode.Decimal:
					return (sbyte)a * (decimal)b;
				}
				break;
			case TypeCode.Byte:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'byte' and 'bool'");
				case TypeCode.SByte:
					return (int)((byte)a * (byte)((sbyte)b));
				case TypeCode.Int16:
					return (int)((short)((byte)a) * (short)b);
				case TypeCode.UInt16:
					return (int)((ushort)((byte)a) * (ushort)b);
				case TypeCode.Int32:
					return (int)((byte)a) * (int)b;
				case TypeCode.UInt32:
					return (uint)((byte)a) * (uint)b;
				case TypeCode.Int64:
					return (long)((ulong)((byte)a) * (ulong)((long)b));
				case TypeCode.UInt64:
					return (ulong)((byte)a) * (ulong)b;
				case TypeCode.Single:
					return (float)((byte)a) * (float)b;
				case TypeCode.Double:
					return (double)((byte)a) * (double)b;
				case TypeCode.Decimal:
					return (byte)a * (decimal)b;
				}
				break;
			case TypeCode.Int16:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'short' and 'bool'");
				case TypeCode.SByte:
					return (int)((short)a * (short)((sbyte)b));
				case TypeCode.Int16:
					return (int)((short)a * (short)b);
				case TypeCode.UInt16:
					return (int)((short)a * (short)((ushort)b));
				case TypeCode.Int32:
					return (int)((short)a) * (int)b;
				case TypeCode.UInt32:
					return (long)((short)a) * (long)((ulong)((uint)b));
				case TypeCode.Int64:
					return (long)((short)a) * (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'short' and 'ulong'");
				case TypeCode.Single:
					return (float)((short)a) * (float)b;
				case TypeCode.Double:
					return (double)((short)a) * (double)b;
				case TypeCode.Decimal:
					return (short)a * (decimal)b;
				}
				break;
			case TypeCode.UInt16:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'ushort' and 'bool'");
				case TypeCode.SByte:
					return (int)((ushort)a * (ushort)((sbyte)b));
				case TypeCode.Int16:
					return (int)((ushort)a * (ushort)((short)b));
				case TypeCode.UInt16:
					return (int)((ushort)a * (ushort)b);
				case TypeCode.Int32:
					return (int)((ushort)a) * (int)b;
				case TypeCode.UInt32:
					return (uint)((ushort)a) * (uint)b;
				case TypeCode.Int64:
					return (long)((ulong)((ushort)a) * (ulong)((long)b));
				case TypeCode.UInt64:
					return (ulong)((ushort)a) * (ulong)b;
				case TypeCode.Single:
					return (float)((ushort)a) * (float)b;
				case TypeCode.Double:
					return (double)((ushort)a) * (double)b;
				case TypeCode.Decimal:
					return (ushort)a * (decimal)b;
				}
				break;
			case TypeCode.Int32:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'int' and 'bool'");
				case TypeCode.SByte:
					return (int)a * (int)((sbyte)b);
				case TypeCode.Int16:
					return (int)a * (int)((short)b);
				case TypeCode.UInt16:
					return (int)a * (int)((ushort)b);
				case TypeCode.Int32:
					return (int)a * (int)b;
				case TypeCode.UInt32:
					return (long)((int)a) * (long)((ulong)((uint)b));
				case TypeCode.Int64:
					return (long)((int)a) * (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'int' and 'ulong'");
				case TypeCode.Single:
					return (float)((int)a) * (float)b;
				case TypeCode.Double:
					return (double)((int)a) * (double)b;
				case TypeCode.Decimal:
					return (int)a * (decimal)b;
				}
				break;
			case TypeCode.UInt32:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'uint' and 'bool'");
				case TypeCode.SByte:
					return (long)((ulong)((uint)a) * (ulong)((long)((sbyte)b)));
				case TypeCode.Int16:
					return (long)((ulong)((uint)a) * (ulong)((long)((short)b)));
				case TypeCode.UInt16:
					return (uint)a * (uint)((ushort)b);
				case TypeCode.Int32:
					return (long)((ulong)((uint)a) * (ulong)((long)((int)b)));
				case TypeCode.UInt32:
					return (uint)a * (uint)b;
				case TypeCode.Int64:
					return (long)((ulong)((uint)a) * (ulong)((long)b));
				case TypeCode.UInt64:
					return (ulong)((uint)a) * (ulong)b;
				case TypeCode.Single:
					return (uint)a * (float)b;
				case TypeCode.Double:
					return (uint)a * (double)b;
				case TypeCode.Decimal:
					return (uint)a * (decimal)b;
				}
				break;
			case TypeCode.Int64:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'long' and 'bool'");
				case TypeCode.SByte:
					return (long)a * (long)((sbyte)b);
				case TypeCode.Int16:
					return (long)a * (long)((short)b);
				case TypeCode.UInt16:
					return (long)a * (long)((ulong)((ushort)b));
				case TypeCode.Int32:
					return (long)a * (long)((int)b);
				case TypeCode.UInt32:
					return (long)a * (long)((ulong)((uint)b));
				case TypeCode.Int64:
					return (long)a * (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'long' and 'ulong'");
				case TypeCode.Single:
					return (float)((long)a) * (float)b;
				case TypeCode.Double:
					return (double)((long)a) * (double)b;
				case TypeCode.Decimal:
					return (long)a * (decimal)b;
				}
				break;
			case TypeCode.UInt64:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'ulong' and 'bool'");
				case TypeCode.SByte:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'ulong' and 'sbyte'");
				case TypeCode.Int16:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'ulong' and 'short'");
				case TypeCode.UInt16:
					return (ulong)a * (ulong)((ushort)b);
				case TypeCode.Int32:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'ulong' and 'int'");
				case TypeCode.UInt32:
					return (ulong)a * (ulong)((uint)b);
				case TypeCode.Int64:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'ulong' and 'long'");
				case TypeCode.UInt64:
					return (ulong)a * (ulong)b;
				case TypeCode.Single:
					return (ulong)a * (float)b;
				case TypeCode.Double:
					return (ulong)a * (double)b;
				case TypeCode.Decimal:
					return (ulong)a * (decimal)b;
				}
				break;
			case TypeCode.Single:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'float' and 'bool'");
				case TypeCode.SByte:
					return (float)a * (float)((sbyte)b);
				case TypeCode.Int16:
					return (float)a * (float)((short)b);
				case TypeCode.UInt16:
					return (float)a * (float)((ushort)b);
				case TypeCode.Int32:
					return (float)a * (float)((int)b);
				case TypeCode.UInt32:
					return (float)a * (uint)b;
				case TypeCode.Int64:
					return (float)a * (float)((long)b);
				case TypeCode.UInt64:
					return (float)a * (ulong)b;
				case TypeCode.Single:
					return (float)a * (float)b;
				case TypeCode.Double:
					return (double)((float)a) * (double)b;
				case TypeCode.Decimal:
					return Convert.ToDecimal(a) * (decimal)b;
				}
				break;
			case TypeCode.Double:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'double' and 'bool'");
				case TypeCode.SByte:
					return (double)a * (double)((sbyte)b);
				case TypeCode.Int16:
					return (double)a * (double)((short)b);
				case TypeCode.UInt16:
					return (double)a * (double)((ushort)b);
				case TypeCode.Int32:
					return (double)a * (double)((int)b);
				case TypeCode.UInt32:
					return (double)a * (uint)b;
				case TypeCode.Int64:
					return (double)a * (double)((long)b);
				case TypeCode.UInt64:
					return (double)a * (ulong)b;
				case TypeCode.Single:
					return (double)a * (double)((float)b);
				case TypeCode.Double:
					return (double)a * (double)b;
				case TypeCode.Decimal:
					return Convert.ToDecimal(a) * (decimal)b;
				}
				break;
			case TypeCode.Decimal:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'decimal' and 'bool'");
				case TypeCode.SByte:
					return (decimal)a * (sbyte)b;
				case TypeCode.Int16:
					return (decimal)a * (short)b;
				case TypeCode.UInt16:
					return (decimal)a * (ushort)b;
				case TypeCode.Int32:
					return (decimal)a * (int)b;
				case TypeCode.UInt32:
					return (decimal)a * (uint)b;
				case TypeCode.Int64:
					return (decimal)a * (long)b;
				case TypeCode.UInt64:
					return (decimal)a * (ulong)b;
				case TypeCode.Single:
					return (decimal)a * Convert.ToDecimal(b);
				case TypeCode.Double:
					return (decimal)a * Convert.ToDecimal(b);
				case TypeCode.Decimal:
					return (decimal)a * (decimal)b;
				}
				break;
			}
			return null;
		}

		internal static object Subtract(object a, object b)
		{
			if (a == null || b == null)
			{
				return null;
			}
			a = Numbers.ConvertIfString(a);
			b = Numbers.ConvertIfString(b);
			if (a is double && double.IsNaN((double)a))
			{
				return a;
			}
			if (b is double && double.IsNaN((double)b))
			{
				return b;
			}
			TypeCode typeCode = TypeHelper.GetTypeCode(a);
			TypeCode typeCode2 = TypeHelper.GetTypeCode(b);
			switch (typeCode)
			{
			case TypeCode.Boolean:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'bool'");
				case TypeCode.SByte:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.Byte:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.Int16:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.UInt16:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.Int32:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.UInt32:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.Int64:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.Single:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.Double:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.Decimal:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
				}
				break;
			case TypeCode.SByte:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'sbyte' and 'bool'");
				case TypeCode.SByte:
					return (int)((sbyte)a - (sbyte)b);
				case TypeCode.Int16:
					return (int)((short)((sbyte)a) - (short)b);
				case TypeCode.UInt16:
					return (int)((ushort)((sbyte)a) - (ushort)b);
				case TypeCode.Int32:
					return (int)((sbyte)a) - (int)b;
				case TypeCode.UInt32:
					return (long)((sbyte)a) - (long)((ulong)((uint)b));
				case TypeCode.Int64:
					return (long)((sbyte)a) - (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'sbyte' and 'ulong'");
				case TypeCode.Single:
					return (float)((sbyte)a) - (float)b;
				case TypeCode.Double:
					return (double)((sbyte)a) - (double)b;
				case TypeCode.Decimal:
					return (sbyte)a - (decimal)b;
				}
				break;
			case TypeCode.Byte:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'byte' and 'bool'");
				case TypeCode.SByte:
					return (int)((byte)a - (byte)((sbyte)b));
				case TypeCode.Int16:
					return (int)((short)((byte)a) - (short)b);
				case TypeCode.UInt16:
					return (int)((ushort)((byte)a) - (ushort)b);
				case TypeCode.Int32:
					return (int)((byte)a) - (int)b;
				case TypeCode.UInt32:
					return (uint)((byte)a) - (uint)b;
				case TypeCode.Int64:
					return (long)((ulong)((byte)a) - (ulong)((long)b));
				case TypeCode.UInt64:
					return (ulong)((byte)a) - (ulong)b;
				case TypeCode.Single:
					return (float)((byte)a) - (float)b;
				case TypeCode.Double:
					return (double)((byte)a) - (double)b;
				case TypeCode.Decimal:
					return (byte)a - (decimal)b;
				}
				break;
			case TypeCode.Int16:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'short' and 'bool'");
				case TypeCode.SByte:
					return (int)((short)a - (short)((sbyte)b));
				case TypeCode.Int16:
					return (int)((short)a - (short)b);
				case TypeCode.UInt16:
					return (int)((short)a - (short)((ushort)b));
				case TypeCode.Int32:
					return (int)((short)a) - (int)b;
				case TypeCode.UInt32:
					return (long)((short)a) - (long)((ulong)((uint)b));
				case TypeCode.Int64:
					return (long)((short)a) - (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'short' and 'ulong'");
				case TypeCode.Single:
					return (float)((short)a) - (float)b;
				case TypeCode.Double:
					return (double)((short)a) - (double)b;
				case TypeCode.Decimal:
					return (short)a - (decimal)b;
				}
				break;
			case TypeCode.UInt16:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ushort' and 'bool'");
				case TypeCode.SByte:
					return (int)((ushort)a - (ushort)((sbyte)b));
				case TypeCode.Int16:
					return (int)((ushort)a - (ushort)((short)b));
				case TypeCode.UInt16:
					return (int)((ushort)a - (ushort)b);
				case TypeCode.Int32:
					return (int)((ushort)a) - (int)b;
				case TypeCode.UInt32:
					return (uint)((ushort)a) - (uint)b;
				case TypeCode.Int64:
					return (long)((ulong)((ushort)a) - (ulong)((long)b));
				case TypeCode.UInt64:
					return (ulong)((ushort)a) - (ulong)b;
				case TypeCode.Single:
					return (float)((ushort)a) - (float)b;
				case TypeCode.Double:
					return (double)((ushort)a) - (double)b;
				case TypeCode.Decimal:
					return (ushort)a - (decimal)b;
				}
				break;
			case TypeCode.Int32:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'int' and 'bool'");
				case TypeCode.SByte:
					return (int)a - (int)((sbyte)b);
				case TypeCode.Int16:
					return (int)a - (int)((short)b);
				case TypeCode.UInt16:
					return (int)a - (int)((ushort)b);
				case TypeCode.Int32:
					return (int)a - (int)b;
				case TypeCode.UInt32:
					return (long)((int)a) - (long)((ulong)((uint)b));
				case TypeCode.Int64:
					return (long)((int)a) - (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'int' and 'ulong'");
				case TypeCode.Single:
					return (float)((int)a) - (float)b;
				case TypeCode.Double:
					return (double)((int)a) - (double)b;
				case TypeCode.Decimal:
					return (int)a - (decimal)b;
				}
				break;
			case TypeCode.UInt32:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'uint' and 'bool'");
				case TypeCode.SByte:
					return (long)((ulong)((uint)a) - (ulong)((long)((sbyte)b)));
				case TypeCode.Int16:
					return (long)((ulong)((uint)a) - (ulong)((long)((short)b)));
				case TypeCode.UInt16:
					return (uint)a - (uint)((ushort)b);
				case TypeCode.Int32:
					return (long)((ulong)((uint)a) - (ulong)((long)((int)b)));
				case TypeCode.UInt32:
					return (uint)a - (uint)b;
				case TypeCode.Int64:
					return (long)((ulong)((uint)a) - (ulong)((long)b));
				case TypeCode.UInt64:
					return (ulong)((uint)a) - (ulong)b;
				case TypeCode.Single:
					return (uint)a - (float)b;
				case TypeCode.Double:
					return (uint)a - (double)b;
				case TypeCode.Decimal:
					return (uint)a - (decimal)b;
				}
				break;
			case TypeCode.Int64:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'long' and 'bool'");
				case TypeCode.SByte:
					return (long)a - (long)((sbyte)b);
				case TypeCode.Int16:
					return (long)a - (long)((short)b);
				case TypeCode.UInt16:
					return (long)a - (long)((ulong)((ushort)b));
				case TypeCode.Int32:
					return (long)a - (long)((int)b);
				case TypeCode.UInt32:
					return (long)a - (long)((ulong)((uint)b));
				case TypeCode.Int64:
					return (long)a - (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'long' and 'ulong'");
				case TypeCode.Single:
					return (float)((long)a) - (float)b;
				case TypeCode.Double:
					return (double)((long)a) - (double)b;
				case TypeCode.Decimal:
					return (long)a - (decimal)b;
				}
				break;
			case TypeCode.UInt64:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ulong' and 'bool'");
				case TypeCode.SByte:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ulong' and 'double'");
				case TypeCode.Int16:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ulong' and 'short'");
				case TypeCode.UInt16:
					return (ulong)a - (ulong)((ushort)b);
				case TypeCode.Int32:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ulong' and 'int'");
				case TypeCode.UInt32:
					return (ulong)a - (ulong)((uint)b);
				case TypeCode.Int64:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ulong' and 'long'");
				case TypeCode.UInt64:
					return (ulong)a - (ulong)b;
				case TypeCode.Single:
					return (ulong)a - (float)b;
				case TypeCode.Double:
					return (ulong)a - (double)b;
				case TypeCode.Decimal:
					return (ulong)a - (decimal)b;
				}
				break;
			case TypeCode.Single:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'float' and 'bool'");
				case TypeCode.SByte:
					return (float)a - (float)((sbyte)b);
				case TypeCode.Int16:
					return (float)a - (float)((short)b);
				case TypeCode.UInt16:
					return (float)a - (float)((ushort)b);
				case TypeCode.Int32:
					return (float)a - (float)((int)b);
				case TypeCode.UInt32:
					return (float)a - (uint)b;
				case TypeCode.Int64:
					return (float)a - (float)((long)b);
				case TypeCode.UInt64:
					return (float)a - (ulong)b;
				case TypeCode.Single:
					return (float)a - (float)b;
				case TypeCode.Double:
					return (double)((float)a) - (double)b;
				case TypeCode.Decimal:
					return Convert.ToDecimal(a) - (decimal)b;
				}
				break;
			case TypeCode.Double:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'double' and 'bool'");
				case TypeCode.SByte:
					return (double)a - (double)((sbyte)b);
				case TypeCode.Int16:
					return (double)a - (double)((short)b);
				case TypeCode.UInt16:
					return (double)a - (double)((ushort)b);
				case TypeCode.Int32:
					return (double)a - (double)((int)b);
				case TypeCode.UInt32:
					return (double)a - (uint)b;
				case TypeCode.Int64:
					return (double)a - (double)((long)b);
				case TypeCode.UInt64:
					return (double)a - (ulong)b;
				case TypeCode.Single:
					return (double)a - (double)((float)b);
				case TypeCode.Double:
					return (double)a - (double)b;
				case TypeCode.Decimal:
					return Convert.ToDecimal(a) - (decimal)b;
				}
				break;
			case TypeCode.Decimal:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'decimal' and 'bool'");
				case TypeCode.SByte:
					return (decimal)a - (sbyte)b;
				case TypeCode.Int16:
					return (decimal)a - (short)b;
				case TypeCode.UInt16:
					return (decimal)a - (ushort)b;
				case TypeCode.Int32:
					return (decimal)a - (int)b;
				case TypeCode.UInt32:
					return (decimal)a - (uint)b;
				case TypeCode.Int64:
					return (decimal)a - (long)b;
				case TypeCode.UInt64:
					return (decimal)a - (ulong)b;
				case TypeCode.Single:
					return (decimal)a - Convert.ToDecimal(b);
				case TypeCode.Double:
					return (decimal)a - Convert.ToDecimal(b);
				case TypeCode.Decimal:
					return (decimal)a - (decimal)b;
				}
				break;
			}
			return null;
		}

		internal static object Modulus(object a, object b)
		{
			if (a == null || b == null)
			{
				return null;
			}
			a = Numbers.ConvertIfString(a);
			b = Numbers.ConvertIfString(b);
			if (a is double && double.IsNaN((double)a))
			{
				return a;
			}
			if (b is double && double.IsNaN((double)b))
			{
				return b;
			}
			TypeCode typeCode = TypeHelper.GetTypeCode(a);
			TypeCode typeCode2 = TypeHelper.GetTypeCode(b);
			switch (typeCode)
			{
			case TypeCode.SByte:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'sbyte' and 'bool'");
				case TypeCode.SByte:
					return (int)((sbyte)a % (sbyte)b);
				case TypeCode.Int16:
					return (int)((short)((sbyte)a) % (short)b);
				case TypeCode.UInt16:
					return (int)((ushort)((sbyte)a) % (ushort)b);
				case TypeCode.Int32:
					return (int)((sbyte)a) % (int)b;
				case TypeCode.UInt32:
					return (long)((sbyte)a) % (long)((ulong)((uint)b));
				case TypeCode.Int64:
					return (long)((sbyte)a) % (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'sbyte' and 'ulong'");
				case TypeCode.Single:
					return (float)((sbyte)a) % (float)b;
				case TypeCode.Double:
					return (double)((sbyte)a) % (double)b;
				case TypeCode.Decimal:
					return (sbyte)a % (decimal)b;
				}
				break;
			case TypeCode.Byte:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'byte' and 'bool'");
				case TypeCode.SByte:
					return (int)((byte)a % (byte)((sbyte)b));
				case TypeCode.Int16:
					return (int)((short)((byte)a) % (short)b);
				case TypeCode.UInt16:
					return (int)((ushort)((byte)a) % (ushort)b);
				case TypeCode.Int32:
					return (int)((byte)a) % (int)b;
				case TypeCode.UInt32:
					return (uint)((byte)a) % (uint)b;
				case TypeCode.Int64:
					return (long)((ulong)((byte)a) % (ulong)((long)b));
				case TypeCode.UInt64:
					return (ulong)((byte)a) % (ulong)b;
				case TypeCode.Single:
					return (float)((byte)a) % (float)b;
				case TypeCode.Double:
					return (double)((byte)a) % (double)b;
				case TypeCode.Decimal:
					return (byte)a % (decimal)b;
				}
				break;
			case TypeCode.Int16:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'short' and 'bool'");
				case TypeCode.SByte:
					return (int)((short)a % (short)((sbyte)b));
				case TypeCode.Int16:
					return (int)((short)a % (short)b);
				case TypeCode.UInt16:
					return (int)((short)a % (short)((ushort)b));
				case TypeCode.Int32:
					return (int)((short)a) % (int)b;
				case TypeCode.UInt32:
					return (long)((short)a) % (long)((ulong)((uint)b));
				case TypeCode.Int64:
					return (long)((short)a) % (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'short' and 'ulong'");
				case TypeCode.Single:
					return (float)((short)a) % (float)b;
				case TypeCode.Double:
					return (double)((short)a) % (double)b;
				case TypeCode.Decimal:
					return (short)a % (decimal)b;
				}
				break;
			case TypeCode.UInt16:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'ushort' and 'bool'");
				case TypeCode.SByte:
					return (int)((ushort)a % (ushort)((sbyte)b));
				case TypeCode.Int16:
					return (int)((ushort)a % (ushort)((short)b));
				case TypeCode.UInt16:
					return (int)((ushort)a % (ushort)b);
				case TypeCode.Int32:
					return (int)((ushort)a) % (int)b;
				case TypeCode.UInt32:
					return (uint)((ushort)a) % (uint)b;
				case TypeCode.Int64:
					return (long)((ulong)((ushort)a) % (ulong)((long)b));
				case TypeCode.UInt64:
					return (ulong)((ushort)a) % (ulong)b;
				case TypeCode.Single:
					return (float)((ushort)a) % (float)b;
				case TypeCode.Double:
					return (double)((ushort)a) % (double)b;
				case TypeCode.Decimal:
					return (ushort)a % (decimal)b;
				}
				break;
			case TypeCode.Int32:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'int' and 'bool'");
				case TypeCode.SByte:
					return (int)a % (int)((sbyte)b);
				case TypeCode.Int16:
					return (int)a % (int)((short)b);
				case TypeCode.UInt16:
					return (int)a % (int)((ushort)b);
				case TypeCode.Int32:
					return (int)a % (int)b;
				case TypeCode.UInt32:
					return (long)((int)a) % (long)((ulong)((uint)b));
				case TypeCode.Int64:
					return (long)((int)a) % (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'int' and 'ulong'");
				case TypeCode.Single:
					return (float)((int)a) % (float)b;
				case TypeCode.Double:
					return (double)((int)a) % (double)b;
				case TypeCode.Decimal:
					return (int)a % (decimal)b;
				}
				break;
			case TypeCode.UInt32:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'uint' and 'bool'");
				case TypeCode.SByte:
					return (long)((ulong)((uint)a) % (ulong)((long)((sbyte)b)));
				case TypeCode.Int16:
					return (long)((ulong)((uint)a) % (ulong)((long)((short)b)));
				case TypeCode.UInt16:
					return (uint)a % (uint)((ushort)b);
				case TypeCode.Int32:
					return (long)((ulong)((uint)a) % (ulong)((long)((int)b)));
				case TypeCode.UInt32:
					return (uint)a % (uint)b;
				case TypeCode.Int64:
					return (long)((ulong)((uint)a) % (ulong)((long)b));
				case TypeCode.UInt64:
					return (ulong)((uint)a) % (ulong)b;
				case TypeCode.Single:
					return (uint)a % (float)b;
				case TypeCode.Double:
					return (uint)a % (double)b;
				case TypeCode.Decimal:
					return (uint)a % (decimal)b;
				}
				break;
			case TypeCode.Int64:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'long' and 'bool'");
				case TypeCode.SByte:
					return (long)a % (long)((sbyte)b);
				case TypeCode.Int16:
					return (long)a % (long)((short)b);
				case TypeCode.UInt16:
					return (long)a % (long)((ulong)((ushort)b));
				case TypeCode.Int32:
					return (long)a % (long)((int)b);
				case TypeCode.UInt32:
					return (long)a % (long)((ulong)((uint)b));
				case TypeCode.Int64:
					return (long)a % (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'long' and 'ulong'");
				case TypeCode.Single:
					return (float)((long)a) % (float)b;
				case TypeCode.Double:
					return (double)((long)a) % (double)b;
				case TypeCode.Decimal:
					return (long)a % (decimal)b;
				}
				break;
			case TypeCode.UInt64:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'ulong' and 'bool'");
				case TypeCode.SByte:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'ulong' and 'sbyte'");
				case TypeCode.Int16:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'ulong' and 'short'");
				case TypeCode.UInt16:
					return (ulong)a % (ulong)((ushort)b);
				case TypeCode.Int32:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'ulong' and 'int'");
				case TypeCode.UInt32:
					return (ulong)a % (ulong)((uint)b);
				case TypeCode.Int64:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'ulong' and 'long'");
				case TypeCode.UInt64:
					return (ulong)a % (ulong)b;
				case TypeCode.Single:
					return (ulong)a % (float)b;
				case TypeCode.Double:
					return (ulong)a % (double)b;
				case TypeCode.Decimal:
					return (ulong)a % (decimal)b;
				}
				break;
			case TypeCode.Single:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'float' and 'bool'");
				case TypeCode.SByte:
					return (float)a % (float)((sbyte)b);
				case TypeCode.Int16:
					return (float)a % (float)((short)b);
				case TypeCode.UInt16:
					return (float)a % (float)((ushort)b);
				case TypeCode.Int32:
					return (float)a % (float)((int)b);
				case TypeCode.UInt32:
					return (float)a % (uint)b;
				case TypeCode.Int64:
					return (float)a % (float)((long)b);
				case TypeCode.UInt64:
					return (float)a % (ulong)b;
				case TypeCode.Single:
					return (float)a % (float)b;
				case TypeCode.Double:
					return (double)((float)a) % (double)b;
				case TypeCode.Decimal:
					return Convert.ToDecimal(a) % (decimal)b;
				}
				break;
			case TypeCode.Double:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'double' and 'bool'");
				case TypeCode.SByte:
					return (double)a % (double)((sbyte)b);
				case TypeCode.Int16:
					return (double)a % (double)((short)b);
				case TypeCode.UInt16:
					return (double)a % (double)((ushort)b);
				case TypeCode.Int32:
					return (double)a % (double)((int)b);
				case TypeCode.UInt32:
					return (double)a % (uint)b;
				case TypeCode.Int64:
					return (double)a % (double)((long)b);
				case TypeCode.UInt64:
					return (double)a % (ulong)b;
				case TypeCode.Single:
					return (double)a % (double)((float)b);
				case TypeCode.Double:
					return (double)a % (double)b;
				case TypeCode.Decimal:
					return Convert.ToDecimal(a) % (decimal)b;
				}
				break;
			case TypeCode.Decimal:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'decimal' and 'bool'");
				case TypeCode.SByte:
					return (decimal)a % (sbyte)b;
				case TypeCode.Int16:
					return (decimal)a % (short)b;
				case TypeCode.UInt16:
					return (decimal)a % (ushort)b;
				case TypeCode.Int32:
					return (decimal)a % (int)b;
				case TypeCode.UInt32:
					return (decimal)a % (uint)b;
				case TypeCode.Int64:
					return (decimal)a % (long)b;
				case TypeCode.UInt64:
					return (decimal)a % (ulong)b;
				case TypeCode.Single:
					return (decimal)a % Convert.ToDecimal(b);
				case TypeCode.Double:
					return (decimal)a % Convert.ToDecimal(b);
				case TypeCode.Decimal:
					return (decimal)a % (decimal)b;
				}
				break;
			}
			return null;
		}

		internal static object Max(object a, object b)
		{
			a = Numbers.ConvertIfString(a);
			b = Numbers.ConvertIfString(b);
			if (a == null || b == null)
			{
				return null;
			}
			switch (TypeHelper.GetTypeCode(a))
			{
			case TypeCode.SByte:
				return Math.Max((sbyte)a, Convert.ToSByte(b));
			case TypeCode.Byte:
				return Math.Max((byte)a, Convert.ToByte(b));
			case TypeCode.Int16:
				return Math.Max((short)a, Convert.ToInt16(b));
			case TypeCode.UInt16:
				return Math.Max((ushort)a, Convert.ToUInt16(b));
			case TypeCode.Int32:
				return Math.Max((int)a, Convert.ToInt32(b));
			case TypeCode.UInt32:
				return Math.Max((uint)a, Convert.ToUInt32(b));
			case TypeCode.Int64:
				return Math.Max((long)a, Convert.ToInt64(b));
			case TypeCode.UInt64:
				return Math.Max((ulong)a, Convert.ToUInt64(b));
			case TypeCode.Single:
				return Math.Max((float)a, Convert.ToSingle(b));
			case TypeCode.Double:
				return Math.Max((double)a, Convert.ToDouble(b));
			case TypeCode.Decimal:
				return Math.Max((decimal)a, Convert.ToDecimal(b));
			default:
				return null;
			}
		}

		internal static object Min(object a, object b)
		{
			a = Numbers.ConvertIfString(a);
			b = Numbers.ConvertIfString(b);
			if (a == null && b == null)
			{
				return null;
			}
			if (a == null)
			{
				return b;
			}
			if (b == null)
			{
				return a;
			}
			switch (TypeHelper.GetTypeCode(a))
			{
			case TypeCode.SByte:
				return Math.Min((sbyte)a, Convert.ToSByte(b));
			case TypeCode.Byte:
				return Math.Min((byte)a, Convert.ToByte(b));
			case TypeCode.Int16:
				return Math.Min((short)a, Convert.ToInt16(b));
			case TypeCode.UInt16:
				return Math.Min((ushort)a, Convert.ToUInt16(b));
			case TypeCode.Int32:
				return Math.Min((int)a, Convert.ToInt32(b));
			case TypeCode.UInt32:
				return Math.Min((uint)a, Convert.ToUInt32(b));
			case TypeCode.Int64:
				return Math.Min((long)a, Convert.ToInt64(b));
			case TypeCode.UInt64:
				return Math.Min((ulong)a, Convert.ToUInt64(b));
			case TypeCode.Single:
				return Math.Min((float)a, Convert.ToSingle(b));
			case TypeCode.Double:
				return Math.Min((double)a, Convert.ToDouble(b));
			case TypeCode.Decimal:
				return Math.Min((decimal)a, Convert.ToDecimal(b));
			default:
				return null;
			}
		}
	}
}
