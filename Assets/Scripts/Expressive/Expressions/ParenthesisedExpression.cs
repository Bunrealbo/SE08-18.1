using System;
using System.Collections.Generic;
using Expressive.Exceptions;

namespace Expressive.Expressions
{
	internal class ParenthesisedExpression : IExpression
	{
		internal ParenthesisedExpression(IExpression innerExpression)
		{
			this._innerExpression = innerExpression;
		}

		public object Evaluate(IDictionary<string, object> variables)
		{
			if (this._innerExpression == null)
			{
				throw new MissingParticipantException("Missing contents inside ().");
			}
			return this._innerExpression.Evaluate(variables);
		}

		private readonly IExpression _innerExpression;
	}
}
