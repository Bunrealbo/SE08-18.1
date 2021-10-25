using System;
using System.Collections;
using Expressive.Expressions;
using Expressive.Helpers;

namespace Expressive.Functions.Mathematical
{
	internal class SumFunction : FunctionBase
	{
		public override string Name
		{
			get
			{
				return "Sum";
			}
		}

		public override object Evaluate(IExpression[] parameters, ExpressiveOptions options)
		{
			base.ValidateParameterCount(parameters, -1, 1);
			object obj = 0;
			for (int i = 0; i < parameters.Length; i++)
			{
				object obj2 = parameters[i].Evaluate(base.Variables);
				IEnumerable enumerable = obj2 as IEnumerable;
				if (enumerable != null)
				{
					object obj3 = 0;
					foreach (object obj4 in enumerable)
					{
						obj3 = Numbers.Add(obj3 ?? 0, obj4 ?? 0);
					}
					obj2 = obj3;
				}
				obj = Numbers.Add(obj ?? 0, obj2 ?? 0);
			}
			return obj;
		}
	}
}
