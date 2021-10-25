using System;
using Expressive.Expressions;

namespace Expressive.Functions.Mathematical
{
	internal class TanFunction : FunctionBase
	{
		public override string Name
		{
			get
			{
				return "Tan";
			}
		}

		public override object Evaluate(IExpression[] parameters, ExpressiveOptions options)
		{
			base.ValidateParameterCount(parameters, 1, 1);
			return Math.Tan(Convert.ToDouble(parameters[0].Evaluate(base.Variables)));
		}
	}
}
