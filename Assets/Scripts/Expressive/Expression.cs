using System;
using System.Collections.Generic;
using Expressive.Exceptions;
using Expressive.Expressions;
using Expressive.Functions;

namespace Expressive
{
	public sealed class Expression
	{
		public Expression(string expression) : this(expression, ExpressiveOptions.None)
		{
		}

		public Expression(string expression, ExpressiveOptions options)
		{
			this._originalExpression = expression;
			this._options = options;
			this._parser = new ExpressionParser(this._options);
		}

		public object Evaluate()
		{
			return this.Evaluate(null);
		}

		public object Evaluate(IDictionary<string, object> variables)
		{
			object result;
			try
			{
				this.CompileExpression();
				if (variables != null && this._options.HasFlag(ExpressiveOptions.IgnoreCase))
				{
					variables = new Dictionary<string, object>(variables, StringComparer.OrdinalIgnoreCase);
				}
				IExpression compiledExpression = this._compiledExpression;
				result = ((compiledExpression != null) ? compiledExpression.Evaluate(variables) : null);
			}
			catch (Exception innerException)
			{
				throw new ExpressiveException(innerException);
			}
			return result;
		}

		public void RegisterFunction(IFunction function)
		{
			this._parser.RegisterFunction(function);
		}

		private void CompileExpression()
		{
			if (this._compiledExpression == null || this._options.HasFlag(ExpressiveOptions.NoCache))
			{
				List<string> list = new List<string>();
				this._compiledExpression = this._parser.CompileExpression(this._originalExpression, list);
				this._variables = list.ToArray();
			}
		}

		private IExpression _compiledExpression;

		private readonly ExpressiveOptions _options;

		private readonly string _originalExpression;

		private readonly ExpressionParser _parser;

		private string[] _variables;
	}
}
