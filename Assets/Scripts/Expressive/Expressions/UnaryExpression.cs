using System;
using System.Collections.Generic;
using Expressive.Helpers;

namespace Expressive.Expressions
{
	internal class UnaryExpression : IExpression
	{
		internal UnaryExpression(UnaryExpressionType type, IExpression expression)
		{
			this._expressionType = type;
			this._expression = expression;
		}

		public object Evaluate(IDictionary<string, object> variables)
		{
			switch (this._expressionType)
			{
			case UnaryExpressionType.Minus:
				return Numbers.Subtract(0, this._expression.Evaluate(variables));
			case UnaryExpressionType.Not:
			{
				object obj = this._expression.Evaluate(variables);
				if (obj != null)
				{
					if (obj is bool)
					{
						return !(bool)obj;
					}
					return !Convert.ToBoolean(obj);
				}
				break;
			}
			case UnaryExpressionType.Plus:
				return Numbers.Add(0, this._expression.Evaluate(variables));
			}
			return null;
		}

		private readonly IExpression _expression;

		private readonly UnaryExpressionType _expressionType;
	}
}
