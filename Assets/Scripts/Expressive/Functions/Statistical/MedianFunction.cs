using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Expressive.Expressions;

namespace Expressive.Functions.Statistical
{
    internal class MedianFunction : FunctionBase
    {
        public override string Name
        {
            get
            {
                return "Median";
            }
        }

        public override object Evaluate(IExpression[] parameters, ExpressiveOptions options)
        {
            base.ValidateParameterCount(parameters, -1, 1);
            IList<decimal> list = new List<decimal>();
            int i = 0;
            while (i < parameters.Length)
            {
                object obj = parameters[i].Evaluate(base.Variables);
                IEnumerable enumerable = obj as IEnumerable;
                if (enumerable != null)
                {

                    while (enumerable.GetEnumerator().MoveNext())
                    {
                        object value = enumerable.GetEnumerator().Current;
                        list.Add(Convert.ToDecimal(value));
                    }
                    i++;

                    list.Add(Convert.ToDecimal(obj));
                }

            }
            return this.Median(Enumerable.ToArray<decimal>(list));
        }

        private decimal Median(decimal[] xs)
        {
            List<decimal> list = Enumerable.ToList<decimal>(Enumerable.OrderBy<decimal, decimal>(xs, new Func<decimal, decimal>(MedianFunction._003C_003Ec._003C_003E9._003CMedian_003Eb__3_0)));
            double num = (double)(list.Count - 1) / 2.0;
            return (list[(int)num] + list[(int)(num + 0.5)]) / 2m;
        }

        [Serializable]
        private sealed class _003C_003Ec
        {
            internal decimal _003CMedian_003Eb__3_0(decimal x)
            {
                return x;
            }

            public static readonly MedianFunction._003C_003Ec _003C_003E9 = new MedianFunction._003C_003Ec();

            public static Func<decimal, decimal> _003C_003E9__3_0;
        }
    }
}
