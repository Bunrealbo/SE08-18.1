using System;
using System.Collections.Generic;
using System.Linq;
using Expressive.Expressions;

namespace Expressive.Operators.Grouping
{
	internal class ParenthesisOpenOperator : IOperator
	{
		public string[] Tags
		{
			get
			{
				return new string[]
				{
					"("
				};
			}
		}

		public IExpression BuildExpression(Token previousToken, IExpression[] expressions, ExpressiveOptions options)
		{
			return new ParenthesisedExpression(expressions[0] ?? expressions[1]);
		}

		public bool CanGetCaptiveTokens(Token previousToken, Token token, Queue<Token> remainingTokens)
		{
			Queue<Token> remainingTokens2 = new Queue<Token>(remainingTokens.ToArray());
			return Enumerable.Any<Token>(this.GetCaptiveTokens(previousToken, token, remainingTokens2));
		}

		public Token[] GetCaptiveTokens(Token previousToken, Token token, Queue<Token> remainingTokens)
		{
			IList<Token> list = new List<Token>();
			list.Add(token);
			int num = 1;
			while (Enumerable.Any<Token>(remainingTokens))
			{
				Token token2 = remainingTokens.Dequeue();
				list.Add(token2);
				if (string.Equals(token2.CurrentToken, "(", StringComparison.Ordinal))
				{
					num++;
				}
				else if (string.Equals(token2.CurrentToken, ")", StringComparison.Ordinal))
				{
					num--;
				}
				if (num <= 0)
				{
					break;
				}
			}
			return Enumerable.ToArray<Token>(list);
		}

		public Token[] GetInnerCaptiveTokens(Token[] allCaptiveTokens)
		{
			return Enumerable.ToArray<Token>(Enumerable.Take<Token>(Enumerable.Skip<Token>(allCaptiveTokens, 1), allCaptiveTokens.Length - 2));
		}

		public OperatorPrecedence GetPrecedence(Token previousToken)
		{
			return OperatorPrecedence.ParenthesisOpen;
		}
	}
}
