using System;
using Expressive.Expressions;

namespace Expressive.Functions.Date
{
	internal sealed class DayOfFunction : FunctionBase
	{
		public override string Name
		{
			get
			{
				return "DayOf";
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
			return Convert.ToDateTime(obj).Day;
		}
	}
}
