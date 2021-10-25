using System;
using System.Collections.Generic;

namespace Expressive.Expressions
{
	internal class ConstantValueExpression : IExpression
	{
		internal ConstantValueExpression(ConstantValueExpressionType type, object value)
		{
			this._expressionType = type;
			this._value = value;
		}

		public object Evaluate(IDictionary<string, object> variables)
		{
			return this._value;
		}

		private readonly ConstantValueExpressionType _expressionType;

		private readonly object _value;
	}
}
