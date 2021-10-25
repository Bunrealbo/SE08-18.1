using System;
using Expressive.Expressions;

namespace Expressive.Functions.String
{
	internal class LengthFunction : FunctionBase
	{
		public override string Name
		{
			get
			{
				return "Length";
			}
		}

		public override object Evaluate(IExpression[] parameters, ExpressiveOptions options)
		{
			base.ValidateParameterCount(parameters, 1, 1);
			object obj = parameters[0].Evaluate(base.Variables);
			if (obj == null)
			{
				return null;
			}
			string text = obj as string;
			if (text != null)
			{
				return text.Length;
			}
			return obj.ToString().Length;
		}
	}
}
