using System;
using System.Collections;
using Expressive.Expressions;

namespace Expressive.Functions.Mathematical
{
	internal class CountFunction : FunctionBase
	{
		public override string Name
		{
			get
			{
				return "Count";
			}
		}

		public override object Evaluate(IExpression[] parameters, ExpressiveOptions options)
		{
			base.ValidateParameterCount(parameters, -1, 1);
			int num = 0;
			foreach (IExpression expression in parameters)
			{
				int num2 = 1;
				IEnumerable enumerable = expression.Evaluate(base.Variables) as IEnumerable;
				if (enumerable != null)
				{
					int num3 = 0;
					foreach (object obj in enumerable)
					{
						num3++;
					}
					num2 = num3;
				}
				num += num2;
			}
			return num;
		}
	}
}
