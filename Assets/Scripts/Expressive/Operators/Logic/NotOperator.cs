using System;
using Expressive.Expressions;

namespace Expressive.Operators.Logic
{
	internal class NotOperator : OperatorBase
	{
		public override string[] Tags
		{
			get
			{
				return new string[]
				{
					"!",
					"not"
				};
			}
		}

		public override IExpression BuildExpression(Token previousToken, IExpression[] expressions, ExpressiveOptions options)
		{
			return new UnaryExpression(UnaryExpressionType.Not, expressions[0] ?? expressions[1]);
		}

		public override OperatorPrecedence GetPrecedence(Token previousToken)
		{
			return OperatorPrecedence.Not;
		}
	}
}
