using System;
using System.Collections.Generic;
using System.Linq;
using Expressive.Exceptions;
using Expressive.Expressions;

namespace Expressive.Functions
{
	internal abstract class FunctionBase : IFunction
	{
		public IDictionary<string, object> Variables
		{
			get
			{
				return this._003CVariables_003Ek__BackingField;
			}
			set
			{
				this._003CVariables_003Ek__BackingField = value;
			}
		}

		public abstract string Name { get; }

		public abstract object Evaluate(IExpression[] parameters, ExpressiveOptions options);

		protected bool ValidateParameterCount(IExpression[] parameters, int expectedCount, int minimumCount)
		{
			if (expectedCount != -1 && (parameters == null || !Enumerable.Any<IExpression>(parameters) || parameters.Length != expectedCount))
			{
				throw new ParameterCountMismatchException(string.Concat(new object[]
				{
					this.Name,
					"() takes only ",
					expectedCount,
					" argument(s)"
				}));
			}
			if (minimumCount > 0 && (parameters == null || !Enumerable.Any<IExpression>(parameters) || parameters.Length < minimumCount))
			{
				throw new ParameterCountMismatchException(string.Concat(new object[]
				{
					this.Name,
					"() expects at least ",
					minimumCount,
					" argument(s)"
				}));
			}
			return true;
		}

		private IDictionary<string, object> _003CVariables_003Ek__BackingField;
	}
}
