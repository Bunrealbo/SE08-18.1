using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Expressive.Expressions;

namespace Expressive.Functions.Statistical
{
    internal class ModeFunction : FunctionBase
    {
        public override string Name
        {
            get
            {
                return "Mode";
            }
        }

        public override object Evaluate(IExpression[] parameters, ExpressiveOptions options)
        {
            ModeFunction._003C_003Ec__DisplayClass2_0 _003C_003Ec__DisplayClass2_ = new ModeFunction._003C_003Ec__DisplayClass2_0();
            base.ValidateParameterCount(parameters, -1, 1);
            IList<object> list = new List<object>();
            int i = 0;
            while (i < parameters.Length)
            {
                object obj = parameters[i].Evaluate(base.Variables);
                IEnumerable enumerable = obj as IEnumerable;
                if (enumerable != null)
                {

                    while (enumerable.GetEnumerator().MoveNext())
                    {
                        object item = enumerable.GetEnumerator().Current;
                        list.Add(item);
                    }
                    i++;

                    list.Add(obj);
                }

            }
            IEnumerable<IGrouping<object, object>> source = Enumerable.GroupBy<object, object>(list, new Func<object, object>(ModeFunction._003C_003Ec._003C_003E9._003CEvaluate_003Eb__2_0));
            _003C_003Ec__DisplayClass2_.maxCount = Enumerable.Max<IGrouping<object, object>>(source, new Func<IGrouping<object, object>, int>(ModeFunction._003C_003Ec._003C_003E9._003CEvaluate_003Eb__2_1));
            return Enumerable.First<IGrouping<object, object>>(source, new Func<IGrouping<object, object>, bool>(_003C_003Ec__DisplayClass2_._003CEvaluate_003Eb__2)).Key;
        }

        private sealed class _003C_003Ec__DisplayClass2_0
        {
            internal bool _003CEvaluate_003Eb__2(IGrouping<object, object> g)
            {
                return Enumerable.Count<object>(g) == this.maxCount;
            }

            public int maxCount;
        }

        [Serializable]
        private sealed class _003C_003Ec
        {
            internal object _003CEvaluate_003Eb__2_0(object v)
            {
                return v;
            }

            internal int _003CEvaluate_003Eb__2_1(IGrouping<object, object> g)
            {
                return Enumerable.Count<object>(g);
            }

            public static readonly ModeFunction._003C_003Ec _003C_003E9 = new ModeFunction._003C_003Ec();

            public static Func<object, object> _003C_003E9__2_0;

            public static Func<IGrouping<object, object>, int> _003C_003E9__2_1;
        }
    }
}
