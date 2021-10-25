using System;
using Expressive.Expressions;
using Expressive.Helpers;

namespace Expressive.Functions.Mathematical
{
	internal class AbsFunction : FunctionBase
	{
		public override string Name
		{
			get
			{
				return "Abs";
			}
		}

		public override object Evaluate(IExpression[] parameters, ExpressiveOptions options)
		{
			base.ValidateParameterCount(parameters, 1, 1);
			object obj = parameters[0].Evaluate(base.Variables);
			if (obj != null)
			{
				switch (TypeHelper.GetTypeCode(obj))
				{
				case TypeCode.SByte:
					return Math.Abs(Convert.ToSByte(obj));
				case TypeCode.Int16:
					return Math.Abs(Convert.ToInt16(obj));
				case TypeCode.UInt16:
					return Math.Abs((int)Convert.ToUInt16(obj));
				case TypeCode.Int32:
					return Math.Abs(Convert.ToInt32(obj));
				case TypeCode.UInt32:
					return Math.Abs((long)((ulong)Convert.ToUInt32(obj)));
				case TypeCode.Int64:
					return Math.Abs(Convert.ToInt64(obj));
				case TypeCode.Single:
					return Math.Abs(Convert.ToSingle(obj));
				case TypeCode.Double:
					return Math.Abs(Convert.ToDouble(obj));
				case TypeCode.Decimal:
					return Math.Abs(Convert.ToDecimal(obj));
				}
			}
			return null;
		}
	}
}
