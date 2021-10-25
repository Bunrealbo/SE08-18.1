using System;
using System.Collections.Generic;
using Expressive.Expressions;

namespace Expressive.Functions
{
	public interface IFunction
	{
		IDictionary<string, object> Variables { set; }

		string Name { get; }

		object Evaluate(IExpression[] parameters, ExpressiveOptions options);
	}
}
