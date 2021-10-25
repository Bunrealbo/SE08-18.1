using System;
using Expressive.Expressions;

namespace Expressive.Functions.Mathematical
{
	internal class PowFunction : FunctionBase
	{
		public override string Name
		{
			get
			{
				return "Pow";
			}
		}

		public override object Evaluate(IExpression[] parameters, ExpressiveOptions options)
		{
			base.ValidateParameterCount(parameters, 2, 2);
			return Math.Pow(Convert.ToDouble(parameters[0].Evaluate(base.Variables)), Convert.ToDouble(parameters[1].Evaluate(base.Variables)));
		}
	}
}
