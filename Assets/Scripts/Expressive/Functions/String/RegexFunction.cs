using System;
using System.Text.RegularExpressions;
using Expressive.Expressions;

namespace Expressive.Functions.String
{
	internal class RegexFunction : FunctionBase
	{
		public override string Name
		{
			get
			{
				return "Regex";
			}
		}

		public override object Evaluate(IExpression[] parameters, ExpressiveOptions options)
		{
			base.ValidateParameterCount(parameters, 2, 2);
			return new Regex(parameters[1].Evaluate(base.Variables) as string).IsMatch(parameters[0].Evaluate(base.Variables) as string);
		}
	}
}
