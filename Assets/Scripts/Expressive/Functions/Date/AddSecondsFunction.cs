using System;
using Expressive.Expressions;

namespace Expressive.Functions.Date
{
	internal sealed class AddSecondsFunction : FunctionBase
	{
		public override string Name
		{
			get
			{
				return "AddSeconds";
			}
		}

		public override object Evaluate(IExpression[] parameters, ExpressiveOptions options)
		{
			base.ValidateParameterCount(parameters, 2, 2);
			object obj = parameters[0].Evaluate(base.Variables);
			object obj2 = parameters[1].Evaluate(base.Variables);
			if (obj == null || obj2 == null)
			{
				return null;
			}
			DateTime dateTime = Convert.ToDateTime(obj);
			double value = Convert.ToDouble(obj2);
			return dateTime.AddSeconds(value);
		}
	}
}
