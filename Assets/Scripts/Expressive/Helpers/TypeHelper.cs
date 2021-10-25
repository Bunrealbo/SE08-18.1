using System;

namespace Expressive.Helpers
{
	internal static class TypeHelper
	{
		internal static TypeCode GetTypeCode(object value)
		{
			return Type.GetTypeCode(value.GetType());
		}
	}
}
