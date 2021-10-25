using System;
using Expressive.Expressions;

namespace Expressive.Functions.Mathematical
{
	internal class SqrtFunction : FunctionBase
	{
		public override string Name
		{
			get
			{
				return "Sqrt";
			}
		}

		public override object Evaluate(IExpression[] parameters, ExpressiveOptions options)
		{
			base.ValidateParameterCount(parameters, 1, 1);
			return Math.Sqrt(Convert.ToDouble(parameters[0].Evaluate(base.Variables)));
		}
	}
}
