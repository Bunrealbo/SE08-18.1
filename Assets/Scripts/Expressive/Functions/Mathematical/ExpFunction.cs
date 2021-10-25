using System;
using Expressive.Expressions;

namespace Expressive.Functions.Mathematical
{
	internal class ExpFunction : FunctionBase
	{
		public override string Name
		{
			get
			{
				return "Exp";
			}
		}

		public override object Evaluate(IExpression[] parameters, ExpressiveOptions options)
		{
			base.ValidateParameterCount(parameters, 1, 1);
			return Math.Exp(Convert.ToDouble(parameters[0].Evaluate(base.Variables)));
		}
	}
}
