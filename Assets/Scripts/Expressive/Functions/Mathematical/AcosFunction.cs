using System;
using Expressive.Expressions;

namespace Expressive.Functions.Mathematical
{
	internal class AcosFunction : FunctionBase
	{
		public override string Name
		{
			get
			{
				return "Acos";
			}
		}

		public override object Evaluate(IExpression[] parameters, ExpressiveOptions options)
		{
			base.ValidateParameterCount(parameters, 1, 1);
			return Math.Acos(Convert.ToDouble(parameters[0].Evaluate(base.Variables)));
		}
	}
}
