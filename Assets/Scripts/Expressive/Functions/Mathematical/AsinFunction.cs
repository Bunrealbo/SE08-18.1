using System;
using Expressive.Expressions;

namespace Expressive.Functions.Mathematical
{
	internal class AsinFunction : FunctionBase
	{
		public override string Name
		{
			get
			{
				return "Asin";
			}
		}

		public override object Evaluate(IExpression[] parameters, ExpressiveOptions options)
		{
			base.ValidateParameterCount(parameters, 1, 1);
			return Math.Asin(Convert.ToDouble(parameters[0].Evaluate(base.Variables)));
		}
	}
}
