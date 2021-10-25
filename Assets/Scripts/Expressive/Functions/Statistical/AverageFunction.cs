using System;
using System.Collections;
using Expressive.Expressions;
using Expressive.Helpers;

namespace Expressive.Functions.Statistical
{
	internal class AverageFunction : FunctionBase
	{
		public override string Name
		{
			get
			{
				return "Average";
			}
		}

		public override object Evaluate(IExpression[] parameters, ExpressiveOptions options)
		{
			base.ValidateParameterCount(parameters, -1, 1);
			int num = 0;
			object obj = 0;
			foreach (IExpression expression in parameters)
			{
				int num2 = 1;
				object obj2 = expression.Evaluate(base.Variables);
				IEnumerable enumerable = obj2 as IEnumerable;
				if (enumerable != null)
				{
					int num3 = 0;
					object obj3 = 0;
					foreach (object b in enumerable)
					{
						num3++;
						obj3 = Numbers.Add(obj3, b);
					}
					num2 = num3;
					obj2 = obj3;
				}
				obj = Numbers.Add(obj, obj2);
				num += num2;
			}
			return Convert.ToDouble(obj) / (double)num;
		}
	}
}
