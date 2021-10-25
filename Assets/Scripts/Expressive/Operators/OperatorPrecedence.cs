using System;

namespace Expressive.Operators
{
	internal enum OperatorPrecedence
	{
		Minimum,
		Or,
		And,
		Equal,
		NotEqual,
		LessThan,
		GreaterThan,
		LessThanOrEqual,
		GreaterThanOrEqual,
		Not,
		BitwiseOr,
		BitwiseXOr,
		BitwiseAnd,
		LeftShift,
		RightShift,
		Add,
		Subtract,
		Multiply,
		Modulus,
		Divide,
		NullCoalescing,
		Conditional = 20,
		UnaryPlus,
		UnaryMinus,
		ParenthesisOpen,
		ParenthesisClose
	}
}
