using System;
using Expressive.Expressions;

namespace Expressive.Functions.Date
{
	internal sealed class MillisecondsBetweenFunction : FunctionBase
	{
		public override string Name
		{
			get
			{
				return "MillisecondsBetween";
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
			DateTime d = Convert.ToDateTime(obj);
			return (Convert.ToDateTime(obj2) - d).TotalMilliseconds;
		}
	}
}
