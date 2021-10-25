using System;
using System.Collections.Generic;

namespace Expressive.Expressions
{
	internal class FunctionExpression : IExpression
	{
		internal FunctionExpression(string name, Func<IExpression[], IDictionary<string, object>, object> function, IExpression[] parameters)
		{
			this._name = name;
			this._function = function;
			this._parameters = parameters;
		}

		public object Evaluate(IDictionary<string, object> variables)
		{
			return this._function(this._parameters, variables);
		}

		private readonly Func<IExpression[], IDictionary<string, object>, object> _function;

		private readonly string _name;

		private readonly IExpression[] _parameters;
	}
}
