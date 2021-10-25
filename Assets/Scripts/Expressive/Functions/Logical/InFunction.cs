using System;
using Expressive.Expressions;
using Expressive.Helpers;

namespace Expressive.Functions.Logical
{
	internal class InFunction : FunctionBase
	{
		public override string Name
		{
			get
			{
				return "In";
			}
		}

		public override object Evaluate(IExpression[] parameters, ExpressiveOptions options)
		{
			base.ValidateParameterCount(parameters, -1, 2);
			bool flag = false;
			object a = parameters[0].Evaluate(base.Variables);
			for (int i = 1; i < parameters.Length; i++)
			{
				if (Comparison.CompareUsingMostPreciseType(a, parameters[i].Evaluate(base.Variables), options.HasFlag(ExpressiveOptions.IgnoreCase)) == 0)
				{
					flag = true;
					break;
				}
			}
			return flag;
		}
	}
}
