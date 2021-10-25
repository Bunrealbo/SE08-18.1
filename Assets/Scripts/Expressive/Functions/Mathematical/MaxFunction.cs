using System;
using System.Collections;
using System.Linq;
using Expressive.Expressions;
using Expressive.Helpers;

namespace Expressive.Functions.Mathematical
{
	internal class MaxFunction : FunctionBase
	{
		public override string Name
		{
			get
			{
				return "Max";
			}
		}

		public override object Evaluate(IExpression[] parameters, ExpressiveOptions options)
		{
			base.ValidateParameterCount(parameters, -1, 1);
			object obj = parameters[0].Evaluate(base.Variables);
			if (obj is IEnumerable)
			{
				obj = this.Max((IEnumerable)obj);
			}
			if (obj == null)
			{
				return null;
			}
			foreach (IExpression expression in Enumerable.Skip<IExpression>(parameters, 1))
			{
				object obj2 = expression.Evaluate(base.Variables);
				IEnumerable enumerable = obj2 as IEnumerable;
				if (enumerable != null)
				{
					obj2 = this.Max(enumerable);
				}
				obj = Numbers.Max(obj, obj2);
				if (obj == null)
				{
					return null;
				}
			}
			return obj;
		}

		private object Max(IEnumerable enumerable)
		{
			object obj = null;
			foreach (object obj2 in enumerable)
			{
				if (obj2 == null)
				{
					return null;
				}
				if (obj == null)
				{
					obj = obj2;
				}
				else
				{
					obj = Numbers.Max(obj, obj2);
				}
			}
			return obj;
		}
	}
}
