using System;

namespace Expressive.Expressions
{
	internal enum BinaryExpressionType
	{
		Unknown,
		And,
		Or,
		NotEqual,
		LessThanOrEqual,
		GreaterThanOrEqual,
		LessThan,
		GreaterThan,
		Equal,
		Subtract,
		Add,
		Modulus,
		Divide,
		Multiply,
		BitwiseOr,
		BitwiseAnd,
		BitwiseXOr,
		LeftShift,
		RightShift,
		NullCoalescing
	}
}
