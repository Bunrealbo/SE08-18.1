using System;
using Expressive.Expressions;

namespace Expressive.Functions.String
{
	internal class SubstringFunction : FunctionBase
	{
		public override string Name
		{
			get
			{
				return "Substring";
			}
		}

		public override object Evaluate(IExpression[] parameters, ExpressiveOptions options)
		{
			base.ValidateParameterCount(parameters, 3, 3);
			object obj = parameters[0].Evaluate(base.Variables);
			if (obj == null)
			{
				return null;
			}
			string text;
			if (obj is string)
			{
				text = (string)obj;
			}
			else
			{
				text = obj.ToString();
			}
			int startIndex = (int)parameters[1].Evaluate(base.Variables);
			int length = (int)parameters[2].Evaluate(base.Variables);
			if (text == null)
			{
				return null;
			}
			return text.Substring(startIndex, length);
		}
	}
}
