using System;
using Expressive.Expressions;

namespace Expressive.Functions.Conversion
{
	internal sealed class StringFunction : FunctionBase
	{
		public override string Name
		{
			get
			{
				return "String";
			}
		}

		public override object Evaluate(IExpression[] parameters, ExpressiveOptions options)
		{
			base.ValidateParameterCount(parameters, -1, 1);
			object obj = parameters[0].Evaluate(base.Variables);
			if (obj == null)
			{
				return null;
			}
			string str;
			if (parameters.Length > 1 && (str = (parameters[1].Evaluate(base.Variables) as string)) != null)
			{
				return string.Format("{0:" + str + "}", obj);
			}
			return obj.ToString();
		}
	}
}
