using System;
using System.Collections.Generic;

namespace Expressive.Expressions
{
	internal class VariableExpression : IExpression
	{
		internal VariableExpression(string variableName)
		{
			this._variableName = variableName;
		}

		public object Evaluate(IDictionary<string, object> variables)
		{
			if (variables == null || !variables.ContainsKey(this._variableName))
			{
				throw new ArgumentException("The variable '" + this._variableName + "' has not been supplied.");
			}
			Expression expression = variables[this._variableName] as Expression;
			if (expression != null)
			{
				return expression.Evaluate(variables);
			}
			return variables[this._variableName];
		}

		private readonly string _variableName;
	}
}
