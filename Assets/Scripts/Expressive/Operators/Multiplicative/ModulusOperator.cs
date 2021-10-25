using System;
using Expressive.Expressions;

namespace Expressive.Operators.Multiplicative
{
	internal class ModulusOperator : OperatorBase
	{
		public override string[] Tags
		{
			get
			{
				return new string[]
				{
					"%",
					"mod"
				};
			}
		}

		public override IExpression BuildExpression(Token previousToken, IExpression[] expressions, ExpressiveOptions options)
		{
			return new BinaryExpression(BinaryExpressionType.Modulus, expressions[0], expressions[1], options);
		}

		public override OperatorPrecedence GetPrecedence(Token previousToken)
		{
			return OperatorPrecedence.Modulus;
		}
	}
}
