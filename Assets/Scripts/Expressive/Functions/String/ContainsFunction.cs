using System;
using Expressive.Expressions;

namespace Expressive.Functions.String
{
	internal class ContainsFunction : FunctionBase
	{
		public override string Name
		{
			get
			{
				return "Contains";
			}
		}

		public override object Evaluate(IExpression[] parameters, ExpressiveOptions options)
		{
			base.ValidateParameterCount(parameters, 2, 2);
			string text = (string)parameters[0].Evaluate(base.Variables);
			string text2 = (string)parameters[1].Evaluate(base.Variables);
			if (text2 == null)
			{
				return false;
			}
			if (options.HasFlag(ExpressiveOptions.IgnoreCase))
			{
				text = ((text != null) ? text.ToLower() : null);
				text2 = ((text2 != null) ? text2.ToLower() : null);
			}
			return text != null && text.Contains(text2);
		}
	}
}
