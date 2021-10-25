using System;
using Expressive.Expressions;

namespace Expressive.Functions.Mathematical
{
	internal class FloorFunction : FunctionBase
	{
		public override string Name
		{
			get
			{
				return "Floor";
			}
		}

		public override object Evaluate(IExpression[] parameters, ExpressiveOptions options)
		{
			base.ValidateParameterCount(parameters, 1, 1);
			return Math.Floor(Convert.ToDouble(parameters[0].Evaluate(base.Variables)));
		}
	}
}
