using System;
using System.Collections.Generic;
using System.Linq;
using Expressive.Expressions;

namespace Expressive.Operators.Additive
{
	internal class SubtractOperator : OperatorBase
	{
		public override string[] Tags
		{
			get
			{
				return new string[]
				{
					"-",
					"−"
				};
			}
		}

		public override IExpression BuildExpression(Token previousToken, IExpression[] expressions, ExpressiveOptions options)
		{
			if (this.IsUnary(previousToken))
			{
				return new UnaryExpression(UnaryExpressionType.Minus, expressions[0] ?? expressions[1]);
			}
			return new BinaryExpression(BinaryExpressionType.Subtract, expressions[0], expressions[1], options);
		}

		public override bool CanGetCaptiveTokens(Token previousToken, Token token, Queue<Token> remainingTokens)
		{
			Queue<Token> remainingTokens2 = new Queue<Token>(remainingTokens.ToArray());
			return Enumerable.Any<Token>(this.GetCaptiveTokens(previousToken, token, remainingTokens2));
		}

		public override Token[] GetInnerCaptiveTokens(Token[] allCaptiveTokens)
		{
			return Enumerable.ToArray<Token>(Enumerable.Skip<Token>(allCaptiveTokens, 1));
		}

		public override OperatorPrecedence GetPrecedence(Token previousToken)
		{
			if (this.IsUnary(previousToken))
			{
				return OperatorPrecedence.UnaryPlus;
			}
			return OperatorPrecedence.Add;
		}

		private bool IsUnary(Token previousToken)
		{
			return string.IsNullOrEmpty((previousToken != null) ? previousToken.CurrentToken : null) || string.Equals(previousToken.CurrentToken, "(", StringComparison.Ordinal) || ExtensionMethods.IsArithmeticOperator(previousToken.CurrentToken);
		}
	}
}
