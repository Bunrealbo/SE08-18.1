using System;
using Expressive.Expressions;

namespace Expressive.Functions.Mathematical
{
	internal class RandomFunction : FunctionBase
	{
		public override string Name
		{
			get
			{
				return "Random";
			}
		}

		public override object Evaluate(IExpression[] parameters, ExpressiveOptions options)
		{
			base.ValidateParameterCount(parameters, 2, 2);
			object obj = parameters[0].Evaluate(base.Variables);
			object obj2 = parameters[1].Evaluate(base.Variables);
			Random random = new Random(DateTime.UtcNow.Millisecond);
			if (obj is int && obj2 is int)
			{
				return random.Next((int)obj, (int)obj2);
			}
			if (obj is double || obj2 is double)
			{
				double num = random.NextDouble();
				double num2 = Convert.ToDouble(obj);
				double num3 = Convert.ToDouble(obj2) - num2;
				return num2 + num3 * num;
			}
			return null;
		}
	}
}
