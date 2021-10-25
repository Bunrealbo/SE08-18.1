using System;
using Expressive.Expressions;

namespace Expressive.Functions.Mathematical
{
	internal class SinFunction : FunctionBase
	{
		public override string Name
		{
			get
			{
				return "Sin";
			}
		}

		public override object Evaluate(IExpression[] parameters, ExpressiveOptions options)
		{
			base.ValidateParameterCount(parameters, 1, 1);
			return Math.Sin(Convert.ToDouble(parameters[0].Evaluate(base.Variables)));
		}
	}
}
