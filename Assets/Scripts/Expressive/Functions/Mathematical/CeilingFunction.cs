using System;
using Expressive.Expressions;

namespace Expressive.Functions.Mathematical
{
	internal class CeilingFunction : FunctionBase
	{
		public override string Name
		{
			get
			{
				return "Ceiling";
			}
		}

		public override object Evaluate(IExpression[] parameters, ExpressiveOptions options)
		{
			base.ValidateParameterCount(parameters, 1, 1);
			object obj = parameters[0].Evaluate(base.Variables);
			if (obj is double)
			{
				return Math.Ceiling((double)obj);
			}
			if (obj is decimal)
			{
				return Math.Ceiling((decimal)obj);
			}
			return Math.Ceiling(Convert.ToDouble(obj));
		}
	}
}
