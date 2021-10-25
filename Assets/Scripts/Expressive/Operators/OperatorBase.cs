using System;
using System.Collections.Generic;
using Expressive.Expressions;

namespace Expressive.Operators
{
	internal abstract class OperatorBase : IOperator
	{
		public abstract string[] Tags { get; }

		public abstract IExpression BuildExpression(Token previousToken, IExpression[] expressions, ExpressiveOptions options);

		public virtual bool CanGetCaptiveTokens(Token previousToken, Token token, Queue<Token> remainingTokens)
		{
			return true;
		}

		public virtual Token[] GetCaptiveTokens(Token previousToken, Token token, Queue<Token> remainingTokens)
		{
			return new Token[]
			{
				token
			};
		}

		public virtual Token[] GetInnerCaptiveTokens(Token[] allCaptiveTokens)
		{
			return new Token[0];
		}

		public abstract OperatorPrecedence GetPrecedence(Token previousToken);
	}
}
