using System;
using Expressive.Expressions;
using Expressive.Helpers;

namespace Expressive.Functions.Mathematical
{
	internal class SignFunction : FunctionBase
	{
		public override string Name
		{
			get
			{
				return "Sign";
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
					return Math.Sign(Convert.ToSByte(obj));
				case TypeCode.Int16:
					return Math.Sign(Convert.ToInt16(obj));
				case TypeCode.UInt16:
					return Math.Sign((int)Convert.ToUInt16(obj));
				case TypeCode.Int32:
					return Math.Sign(Convert.ToInt32(obj));
				case TypeCode.UInt32:
					return Math.Sign((long)((ulong)Convert.ToUInt32(obj)));
				case TypeCode.Int64:
					return Math.Sign(Convert.ToInt64(obj));
				case TypeCode.Single:
					return Math.Sign(Convert.ToSingle(obj));
				case TypeCode.Double:
					return Math.Sign(Convert.ToDouble(obj));
				case TypeCode.Decimal:
					return Math.Sign(Convert.ToDecimal(obj));
				}
			}
			return null;
		}
	}
}
