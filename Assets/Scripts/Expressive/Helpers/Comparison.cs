using System;
using System.Collections.Generic;

namespace Expressive.Helpers
{
	internal static class Comparison
	{
		internal static int CompareUsingMostPreciseType(object a, object b, bool ignoreCase)
		{
			Type mostPreciseType = Comparison.GetMostPreciseType(a.GetType(), b.GetType());
			if (mostPreciseType == typeof(string))
			{
				return string.Compare((string)Convert.ChangeType(a, mostPreciseType), (string)Convert.ChangeType(b, mostPreciseType), ignoreCase);
			}
			return Comparison.Compare(a, b, mostPreciseType, ignoreCase);
		}

		internal static Type GetMostPreciseType(Type a, Type b)
		{
			foreach (Type type in Comparison.CommonTypes)
			{
				if (a == type || b == type)
				{
					return type;
				}
			}
			return a;
		}

		private static int Compare(object lhs, object rhs, Type mostPreciseType, bool ignoreCase)
		{
			if (lhs == null && rhs == null)
			{
				return 0;
			}
			if (lhs == null)
			{
				return -1;
			}
			if (rhs == null)
			{
				return 1;
			}
			Type type = lhs.GetType();
			Type type2 = rhs.GetType();
			if (type == type2)
			{
				return Comparer<object>.Default.Compare(lhs, rhs);
			}
			try
			{
				if (type == mostPreciseType)
				{
					rhs = Convert.ChangeType(rhs, mostPreciseType);
				}
				else
				{
					lhs = Convert.ChangeType(lhs, mostPreciseType);
				}
				return Comparer<object>.Default.Compare(lhs, rhs);
			}
			catch (Exception)
			{
			}
			try
			{
				return Comparer<object>.Default.Compare(lhs, Convert.ChangeType(rhs, type));
			}
			catch (Exception)
			{
			}
			try
			{
				return Comparer<object>.Default.Compare(lhs, Convert.ChangeType(rhs, type));
			}
			catch (Exception)
			{
			}
			try
			{
				return string.Compare((string)Convert.ChangeType(lhs, typeof(string)), (string)Convert.ChangeType(rhs, typeof(string)), ignoreCase);
			}
			catch (Exception)
			{
			}
			return 0;
		}

		private static readonly Type[] CommonTypes = new Type[]
		{
			typeof(long),
			typeof(int),
			typeof(double),
			typeof(bool),
			typeof(DateTime),
			typeof(string),
			typeof(decimal)
		};
	}
}
