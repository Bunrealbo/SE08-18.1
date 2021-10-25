using System;
using Expressive.Expressions;

namespace Expressive.Functions.Mathematical
{
	internal class RoundFunction : FunctionBase
	{
		public override string Name
		{
			get
			{
				return "Round";
			}
		}

		public override object Evaluate(IExpression[] parameters, ExpressiveOptions options)
		{
			base.ValidateParameterCount(parameters, 2, 2);
			return Math.Round(Convert.ToDouble(parameters[0].Evaluate(base.Variables)), Convert.ToInt32(parameters[1].Evaluate(base.Variables)));
		}
	}
}
