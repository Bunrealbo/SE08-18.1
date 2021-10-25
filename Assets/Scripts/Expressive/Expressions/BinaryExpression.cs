using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Expressive.Exceptions;
using Expressive.Helpers;

namespace Expressive.Expressions
{
	internal class BinaryExpression : IExpression
	{
		internal BinaryExpression(BinaryExpressionType type, IExpression lhs, IExpression rhs, ExpressiveOptions options)
		{
			this._expressionType = type;
			this._leftHandSide = lhs;
			this._options = options;
			this._rightHandSide = rhs;
		}

		public object Evaluate(IDictionary<string, object> variables)
		{
			if (this._leftHandSide == null)
			{
				throw new MissingParticipantException("The left hand side of the operation is missing.");
			}
			if (this._rightHandSide == null)
			{
				throw new MissingParticipantException("The right hand side of the operation is missing.");
			}
			object obj = this._leftHandSide.Evaluate(variables);
			bool ignoreCase = this._options.HasFlag(ExpressiveOptions.IgnoreCase);
			switch (this._expressionType)
			{
			case BinaryExpressionType.And:
				return this.Evaluate(obj, this._rightHandSide, variables, new Func<object, object, object>(BinaryExpression._003C_003Ec._003C_003E9._003CEvaluate_003Eb__5_0));
			case BinaryExpressionType.Or:
				return this.Evaluate(obj, this._rightHandSide, variables, new Func<object, object, object>(BinaryExpression._003C_003Ec._003C_003E9._003CEvaluate_003Eb__5_1));
			case BinaryExpressionType.NotEqual:
				if (obj == null)
				{
					object obj2 = this._rightHandSide.Evaluate(variables);
					if (obj2 != null)
					{
						return true;
					}
					return false;
				}
				else
				{
					object obj2 = this._rightHandSide.Evaluate(variables);
					if (obj2 == null)
					{
						return true;
					}
					return Comparison.CompareUsingMostPreciseType(obj, obj2, ignoreCase) != 0;
				}
				break;
			case BinaryExpressionType.LessThanOrEqual:
			{
				if (obj == null)
				{
					return null;
				}
				object obj3 = this._rightHandSide.Evaluate(variables);
				if (obj3 == null)
				{
					return null;
				}
				return Comparison.CompareUsingMostPreciseType(obj, obj3, ignoreCase) <= 0;
			}
			case BinaryExpressionType.GreaterThanOrEqual:
			{
				if (obj == null)
				{
					return null;
				}
				object obj4 = this._rightHandSide.Evaluate(variables);
				if (obj4 == null)
				{
					return null;
				}
				return Comparison.CompareUsingMostPreciseType(obj, obj4, ignoreCase) >= 0;
			}
			case BinaryExpressionType.LessThan:
			{
				if (obj == null)
				{
					return null;
				}
				object obj5 = this._rightHandSide.Evaluate(variables);
				if (obj5 == null)
				{
					return null;
				}
				return Comparison.CompareUsingMostPreciseType(obj, obj5, ignoreCase) < 0;
			}
			case BinaryExpressionType.GreaterThan:
			{
				if (obj == null)
				{
					return null;
				}
				object obj6 = this._rightHandSide.Evaluate(variables);
				if (obj6 == null)
				{
					return null;
				}
				return Comparison.CompareUsingMostPreciseType(obj, obj6, ignoreCase) > 0;
			}
			case BinaryExpressionType.Equal:
				if (obj == null)
				{
					if (this._rightHandSide.Evaluate(variables) == null)
					{
						return true;
					}
					return false;
				}
				else
				{
					object obj7 = this._rightHandSide.Evaluate(variables);
					if (obj7 == null)
					{
						return false;
					}
					return Comparison.CompareUsingMostPreciseType(obj, obj7, ignoreCase) == 0;
				}
				break;
			case BinaryExpressionType.Subtract:
				return this.Evaluate(obj, this._rightHandSide, variables, new Func<object, object, object>(BinaryExpression._003C_003Ec._003C_003E9._003CEvaluate_003Eb__5_2));
			case BinaryExpressionType.Add:
				if (obj is string)
				{
					return (string)obj + this._rightHandSide.Evaluate(variables);
				}
				return this.Evaluate(obj, this._rightHandSide, variables, new Func<object, object, object>(BinaryExpression._003C_003Ec._003C_003E9._003CEvaluate_003Eb__5_3));
			case BinaryExpressionType.Modulus:
				return this.Evaluate(obj, this._rightHandSide, variables, new Func<object, object, object>(BinaryExpression._003C_003Ec._003C_003E9._003CEvaluate_003Eb__5_4));
			case BinaryExpressionType.Divide:
				this._rightHandSide.Evaluate(variables);
				return this.Evaluate(obj, this._rightHandSide, variables, new Func<object, object, object>(BinaryExpression._003C_003Ec._003C_003E9._003CEvaluate_003Eb__5_12));
			case BinaryExpressionType.Multiply:
				return this.Evaluate(obj, this._rightHandSide, variables, new Func<object, object, object>(BinaryExpression._003C_003Ec._003C_003E9._003CEvaluate_003Eb__5_5));
			case BinaryExpressionType.BitwiseOr:
				return this.Evaluate(obj, this._rightHandSide, variables, new Func<object, object, object>(BinaryExpression._003C_003Ec._003C_003E9._003CEvaluate_003Eb__5_6));
			case BinaryExpressionType.BitwiseAnd:
				return this.Evaluate(obj, this._rightHandSide, variables, new Func<object, object, object>(BinaryExpression._003C_003Ec._003C_003E9._003CEvaluate_003Eb__5_7));
			case BinaryExpressionType.BitwiseXOr:
				return this.Evaluate(obj, this._rightHandSide, variables, new Func<object, object, object>(BinaryExpression._003C_003Ec._003C_003E9._003CEvaluate_003Eb__5_8));
			case BinaryExpressionType.LeftShift:
				return this.Evaluate(obj, this._rightHandSide, variables, new Func<object, object, object>(BinaryExpression._003C_003Ec._003C_003E9._003CEvaluate_003Eb__5_9));
			case BinaryExpressionType.RightShift:
				return this.Evaluate(obj, this._rightHandSide, variables, new Func<object, object, object>(BinaryExpression._003C_003Ec._003C_003E9._003CEvaluate_003Eb__5_10));
			case BinaryExpressionType.NullCoalescing:
				return this.Evaluate(obj, this._rightHandSide, variables, new Func<object, object, object>(BinaryExpression._003C_003Ec._003C_003E9._003CEvaluate_003Eb__5_11));
			}
			return null;
		}

		private static bool IsReal(object value)
		{
			TypeCode typeCode = TypeHelper.GetTypeCode(value);
			return typeCode == TypeCode.Decimal || typeCode == TypeCode.Double || typeCode == TypeCode.Single;
		}

		private object Evaluate(object lhsResult, IExpression rhs, IDictionary<string, object> variables, Func<object, object, object> resultSelector)
		{
			IList<object> list = new List<object>();
			IList<object> list2 = new List<object>();
			object obj = rhs.Evaluate(variables);
			if (!(lhsResult is ICollection) && !(obj is ICollection))
			{
				return resultSelector(lhsResult, obj);
			}
			if (lhsResult is ICollection)
			{
				foreach (object item in ((ICollection)lhsResult))
				{
					list.Add(item);
				}
			}
			if (obj is ICollection)
			{
				foreach (object item2 in ((ICollection)obj))
				{
					list2.Add(item2);
				}
			}
			object[] result = null;
			if (list.Count == list2.Count)
			{
				IList<object> list3 = new List<object>();
				for (int i = 0; i < list.Count; i++)
				{
					list3.Add(resultSelector(list[i], list2[i]));
				}
				result = Enumerable.ToArray<object>(list3);
			}
			else if (list.Count == 0)
			{
				IList<object> list4 = new List<object>();
				for (int j = 0; j < list2.Count; j++)
				{
					list4.Add(resultSelector(lhsResult, list2[j]));
				}
				result = Enumerable.ToArray<object>(list4);
			}
			else if (list2.Count == 0)
			{
				IList<object> list5 = new List<object>();
				for (int k = 0; k < list.Count; k++)
				{
					list5.Add(resultSelector(list[k], obj));
				}
				result = Enumerable.ToArray<object>(list5);
			}
			return result;
		}

		private readonly BinaryExpressionType _expressionType;

		private readonly IExpression _leftHandSide;

		private readonly ExpressiveOptions _options;

		private readonly IExpression _rightHandSide;

		[Serializable]
		private sealed class _003C_003Ec
		{
			internal object _003CEvaluate_003Eb__5_0(object l, object r)
			{
				return Convert.ToBoolean(l) && Convert.ToBoolean(r);
			}

			internal object _003CEvaluate_003Eb__5_1(object l, object r)
			{
				return Convert.ToBoolean(l) || Convert.ToBoolean(r);
			}

			internal object _003CEvaluate_003Eb__5_2(object l, object r)
			{
				return Numbers.Subtract(l, r);
			}

			internal object _003CEvaluate_003Eb__5_3(object l, object r)
			{
				return Numbers.Add(l, r);
			}

			internal object _003CEvaluate_003Eb__5_4(object l, object r)
			{
				return Numbers.Modulus(l, r);
			}

			internal object _003CEvaluate_003Eb__5_12(object l, object r)
			{
				if (l != null && r != null && !BinaryExpression.IsReal(l) && !BinaryExpression.IsReal(r))
				{
					return Numbers.Divide(Convert.ToDouble(l), r);
				}
				return Numbers.Divide(l, r);
			}

			internal object _003CEvaluate_003Eb__5_5(object l, object r)
			{
				return Numbers.Multiply(l, r);
			}

			internal object _003CEvaluate_003Eb__5_6(object l, object r)
			{
				return (int)(Convert.ToUInt16(l) | Convert.ToUInt16(r));
			}

			internal object _003CEvaluate_003Eb__5_7(object l, object r)
			{
				return (int)(Convert.ToUInt16(l) & Convert.ToUInt16(r));
			}

			internal object _003CEvaluate_003Eb__5_8(object l, object r)
			{
				return (int)(Convert.ToUInt16(l) ^ Convert.ToUInt16(r));
			}

			internal object _003CEvaluate_003Eb__5_9(object l, object r)
			{
				return (int)Convert.ToUInt16(l) << (int)Convert.ToUInt16(r);
			}

			internal object _003CEvaluate_003Eb__5_10(object l, object r)
			{
				return Convert.ToUInt16(l) >> (int)Convert.ToUInt16(r);
			}

			internal object _003CEvaluate_003Eb__5_11(object l, object r)
			{
				return l ?? r;
			}

			public static readonly BinaryExpression._003C_003Ec _003C_003E9 = new BinaryExpression._003C_003Ec();

			public static Func<object, object, object> _003C_003E9__5_0;

			public static Func<object, object, object> _003C_003E9__5_1;

			public static Func<object, object, object> _003C_003E9__5_2;

			public static Func<object, object, object> _003C_003E9__5_3;

			public static Func<object, object, object> _003C_003E9__5_4;

			public static Func<object, object, object> _003C_003E9__5_12;

			public static Func<object, object, object> _003C_003E9__5_5;

			public static Func<object, object, object> _003C_003E9__5_6;

			public static Func<object, object, object> _003C_003E9__5_7;

			public static Func<object, object, object> _003C_003E9__5_8;

			public static Func<object, object, object> _003C_003E9__5_9;

			public static Func<object, object, object> _003C_003E9__5_10;

			public static Func<object, object, object> _003C_003E9__5_11;
		}
	}
}
