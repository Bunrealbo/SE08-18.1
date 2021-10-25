using System;
using Expressive.Expressions;

namespace Expressive.Functions.Date
{
	internal sealed class YearOfFunction : FunctionBase
	{
		public override string Name
		{
			get
			{
				return "YearOf";
			}
		}

		public override object Evaluate(IExpression[] parameters, ExpressiveOptions options)
		{
			base.ValidateParameterCount(parameters, 1, 1);
			object obj = parameters[0].Evaluate(base.Variables);
			if (obj == null)
			{
				return null;
			}
			return Convert.ToDateTime(obj).Year;
		}
	}
}
