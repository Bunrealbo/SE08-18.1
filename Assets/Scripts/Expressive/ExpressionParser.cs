using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Expressive.Exceptions;
using Expressive.Expressions;
using Expressive.Functions;
using Expressive.Functions.Conversion;
using Expressive.Functions.Date;
using Expressive.Functions.Logical;
using Expressive.Functions.Mathematical;
using Expressive.Functions.Statistical;
using Expressive.Functions.String;
using Expressive.Operators;
using Expressive.Operators.Additive;
using Expressive.Operators.Bitwise;
using Expressive.Operators.Conditional;
using Expressive.Operators.Grouping;
using Expressive.Operators.Logic;
using Expressive.Operators.Multiplicative;
using Expressive.Operators.Relational;

namespace Expressive
{
    internal sealed class ExpressionParser
    {
        internal ExpressionParser(ExpressiveOptions options)
        {
            this._options = options;
            this._stringComparer = (this._options.HasFlag(ExpressiveOptions.IgnoreCase) ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);
            this._decimalSeparator = Convert.ToChar(CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);
            this._registeredFunctions = new Dictionary<string, Func<IExpression[], IDictionary<string, object>, object>>(this.GetDictionaryComparer(options));
            this._registeredOperators = new Dictionary<string, IOperator>(this.GetDictionaryComparer(options));
            this.RegisterOperator(new PlusOperator());
            this.RegisterOperator(new SubtractOperator());
            this.RegisterOperator(new BitwiseAndOperator());
            this.RegisterOperator(new BitwiseOrOperator());
            this.RegisterOperator(new BitwiseXOrOperator());
            this.RegisterOperator(new LeftShiftOperator());
            this.RegisterOperator(new RightShiftOperator());
            this.RegisterOperator(new NullCoalescingOperator());
            this.RegisterOperator(new ParenthesisCloseOperator());
            this.RegisterOperator(new ParenthesisOpenOperator());
            this.RegisterOperator(new AndOperator());
            this.RegisterOperator(new NotOperator());
            this.RegisterOperator(new OrOperator());
            this.RegisterOperator(new DivideOperator());
            this.RegisterOperator(new ModulusOperator());
            this.RegisterOperator(new MultiplyOperator());
            this.RegisterOperator(new EqualOperator());
            this.RegisterOperator(new GreaterThanOperator());
            this.RegisterOperator(new GreaterThanOrEqualOperator());
            this.RegisterOperator(new LessThanOperator());
            this.RegisterOperator(new LessThanOrEqualOperator());
            this.RegisterOperator(new NotEqualOperator());
            this.RegisterFunction(new DateFunction());
            this.RegisterFunction(new DecimalFunction());
            this.RegisterFunction(new DoubleFunction());
            this.RegisterFunction(new IntegerFunction());
            this.RegisterFunction(new LongFunction());
            this.RegisterFunction(new StringFunction());
            this.RegisterFunction(new AddDaysFunction());
            this.RegisterFunction(new AddHoursFunction());
            this.RegisterFunction(new AddMillisecondsFunction());
            this.RegisterFunction(new AddMinutesFunction());
            this.RegisterFunction(new AddMonthsFunction());
            this.RegisterFunction(new AddSecondsFunction());
            this.RegisterFunction(new AddYearsFunction());
            this.RegisterFunction(new DayOfFunction());
            this.RegisterFunction(new DaysBetweenFunction());
            this.RegisterFunction(new HourOfFunction());
            this.RegisterFunction(new HoursBetweenFunction());
            this.RegisterFunction(new MillisecondOfFunction());
            this.RegisterFunction(new MillisecondsBetweenFunction());
            this.RegisterFunction(new MinuteOfFunction());
            this.RegisterFunction(new MinutesBetweenFunction());
            this.RegisterFunction(new MonthOfFunction());
            this.RegisterFunction(new SecondOfFunction());
            this.RegisterFunction(new SecondsBetweenFunction());
            this.RegisterFunction(new YearOfFunction());
            this.RegisterFunction(new AbsFunction());
            this.RegisterFunction(new AcosFunction());
            this.RegisterFunction(new AsinFunction());
            this.RegisterFunction(new AtanFunction());
            this.RegisterFunction(new CeilingFunction());
            this.RegisterFunction(new CosFunction());
            this.RegisterFunction(new CountFunction());
            this.RegisterFunction(new ExpFunction());
            this.RegisterFunction(new FloorFunction());
            this.RegisterFunction(new IEEERemainderFunction());
            this.RegisterFunction(new Log10Function());
            this.RegisterFunction(new LogFunction());
            this.RegisterFunction(new MaxFunction());
            this.RegisterFunction(new MinFunction());
            this.RegisterFunction(new PowFunction());
            this.RegisterFunction(new RandomFunction());
            this.RegisterFunction(new RoundFunction());
            this.RegisterFunction(new SignFunction());
            this.RegisterFunction(new SinFunction());
            this.RegisterFunction(new SqrtFunction());
            this.RegisterFunction(new SumFunction());
            this.RegisterFunction(new TanFunction());
            this.RegisterFunction(new TruncateFunction());
            this.RegisterFunction(new IfFunction());
            this.RegisterFunction(new InFunction());
            this.RegisterFunction(new AverageFunction());
            this.RegisterFunction(new MeanFunction());
            this.RegisterFunction(new MedianFunction());
            this.RegisterFunction(new ModeFunction());
            this.RegisterFunction(new ContainsFunction());
            this.RegisterFunction(new EndsWithFunction());
            this.RegisterFunction(new LengthFunction());
            this.RegisterFunction(new PadLeftFunction());
            this.RegisterFunction(new PadRightFunction());
            this.RegisterFunction(new RegexFunction());
            this.RegisterFunction(new StartsWithFunction());
            this.RegisterFunction(new SubstringFunction());
        }

        internal IExpression CompileExpression(string expression, IList<string> variables)
        {
            if (string.IsNullOrWhiteSpace(expression))
            {
                throw new ExpressiveException("An Expression cannot be empty.");
            }
            IList<Token> list = this.Tokenise(expression);
            int num = Enumerable.Count<string>(Enumerable.Select<Token, string>(list, new Func<Token, string>(ExpressionParser._003C_003Ec._003C_003E9._003CCompileExpression_003Eb__8_0)), new Func<string, bool>(ExpressionParser._003C_003Ec._003C_003E9._003CCompileExpression_003Eb__8_1));
            int num2 = Enumerable.Count<string>(Enumerable.Select<Token, string>(list, new Func<Token, string>(ExpressionParser._003C_003Ec._003C_003E9._003CCompileExpression_003Eb__8_2)), new Func<string, bool>(ExpressionParser._003C_003Ec._003C_003E9._003CCompileExpression_003Eb__8_3));
            if (num > num2)
            {
                throw new ArgumentException(string.Concat(new object[]
                {
                    "There aren't enough ')' symbols. Expected ",
                    num,
                    " but there is only ",
                    num2
                }));
            }
            if (num < num2)
            {
                throw new ArgumentException(string.Concat(new object[]
                {
                    "There are too many ')' symbols. Expected ",
                    num,
                    " but there is ",
                    num2
                }));
            }
            return this.CompileExpression(new Queue<Token>(list), OperatorPrecedence.Minimum, variables, false);
        }

        internal void RegisterFunction(IFunction function)
        {
            ExpressionParser._003C_003Ec__DisplayClass10_0 _003C_003Ec__DisplayClass10_ = new ExpressionParser._003C_003Ec__DisplayClass10_0();
            _003C_003Ec__DisplayClass10_.function = function;
            _003C_003Ec__DisplayClass10_._003C_003E4__this = this;
            this.CheckForExistingFunctionName(_003C_003Ec__DisplayClass10_.function.Name);
            this._registeredFunctions.Add(_003C_003Ec__DisplayClass10_.function.Name, new Func<IExpression[], IDictionary<string, object>, object>(_003C_003Ec__DisplayClass10_._003CRegisterFunction_003Eb__0));
        }

        internal void RegisterOperator(IOperator op)
        {
            foreach (string key in op.Tags)
            {
                this._registeredOperators.Add(key, op);
            }
        }

        private IExpression CompileExpression(Queue<Token> tokens, OperatorPrecedence minimumPrecedence, IList<string> variables, bool isWithinFunction)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException("tokens", "You must call Tokenise before compiling");
            }
            IExpression expression = null;
            Token token = ExtensionMethods.PeekOrDefault<Token>(tokens);
            Token previousToken = null;
            while (token != null)
            {
                Func<IExpression[], IDictionary<string, object>, object> function = null;
                IOperator @operator = null;
                if (this._registeredOperators.TryGetValue(token.CurrentToken, out @operator))
                {
                    OperatorPrecedence precedence = @operator.GetPrecedence(previousToken);
                    if (precedence <= minimumPrecedence)
                    {
                        break;
                    }
                    tokens.Dequeue();
                    if (!@operator.CanGetCaptiveTokens(previousToken, token, tokens))
                    {
                        @operator.GetCaptiveTokens(previousToken, token, tokens);
                        break;
                    }
                    Token[] captiveTokens = @operator.GetCaptiveTokens(previousToken, token, tokens);
                    IExpression expression2;
                    if (captiveTokens.Length > 1)
                    {
                        Token[] innerCaptiveTokens = @operator.GetInnerCaptiveTokens(captiveTokens);
                        expression2 = this.CompileExpression(new Queue<Token>(innerCaptiveTokens), OperatorPrecedence.Minimum, variables, isWithinFunction);
                        token = captiveTokens[captiveTokens.Length - 1];
                    }
                    else
                    {
                        expression2 = this.CompileExpression(tokens, precedence, variables, isWithinFunction);
                        token = new Token(")", -1);
                    }
                    expression = @operator.BuildExpression(previousToken, new IExpression[]
                    {
                        expression,
                        expression2
                    }, this._options);
                }
                else if (this._registeredFunctions.TryGetValue(token.CurrentToken, out function))
                {
                    this.CheckForExistingParticipant(expression, token, isWithinFunction);
                    List<IExpression> list = new List<IExpression>();
                    Queue<Token> queue = new Queue<Token>();
                    int num = 0;
                    tokens.Dequeue();
                    while (tokens.Count > 0)
                    {
                        Token token2 = tokens.Dequeue();
                        if (string.Equals(token2.CurrentToken, "(", StringComparison.Ordinal))
                        {
                            num++;
                        }
                        else if (string.Equals(token2.CurrentToken, ")", StringComparison.Ordinal))
                        {
                            num--;
                        }
                        if ((num != 1 || !(token2.CurrentToken == "(")) && (num != 0 || !(token2.CurrentToken == ")")))
                        {
                            queue.Enqueue(token2);
                        }
                        if (num == 0 && Enumerable.Any<Token>(queue))
                        {
                            list.Add(this.CompileExpression(queue, OperatorPrecedence.Minimum, variables, true));
                            queue.Clear();
                        }
                        else if (string.Equals(token2.CurrentToken, ','.ToString(), StringComparison.Ordinal) && num == 1)
                        {
                            list.Add(this.CompileExpression(queue, OperatorPrecedence.Minimum, variables, true));
                            queue.Clear();
                        }
                        if (num <= 0)
                        {
                            break;
                        }
                    }
                    expression = new FunctionExpression(token.CurrentToken, function, list.ToArray());
                }
                else if (ExtensionMethods.IsNumeric(token.CurrentToken))
                {
                    this.CheckForExistingParticipant(expression, token, isWithinFunction);
                    tokens.Dequeue();
                    int num2 = 0;
                    decimal num3 = 0.0m;
                    double num4 = 0.0;
                    float num5 = 0f;
                    long num6 = 0L;
                    if (int.TryParse(token.CurrentToken, out num2))
                    {
                        expression = new ConstantValueExpression(ConstantValueExpressionType.Integer, num2);
                    }
                    else if (decimal.TryParse(token.CurrentToken, out num3))
                    {
                        expression = new ConstantValueExpression(ConstantValueExpressionType.Decimal, num3);
                    }
                    else if (double.TryParse(token.CurrentToken, out num4))
                    {
                        expression = new ConstantValueExpression(ConstantValueExpressionType.Double, num4);
                    }
                    else if (float.TryParse(token.CurrentToken, out num5))
                    {
                        expression = new ConstantValueExpression(ConstantValueExpressionType.Float, num5);
                    }
                    else if (long.TryParse(token.CurrentToken, out num6))
                    {
                        expression = new ConstantValueExpression(ConstantValueExpressionType.Long, num6);
                    }
                }
                else if (token.CurrentToken.StartsWith("[") && token.CurrentToken.EndsWith("]"))
                {
                    this.CheckForExistingParticipant(expression, token, isWithinFunction);
                    tokens.Dequeue();
                    string text = token.CurrentToken.Replace("[", "").Replace("]", "");
                    expression = new VariableExpression(text);
                    if (!Enumerable.Contains<string>(variables, text, this._stringComparer))
                    {
                        variables.Add(text);
                    }
                }
                else if (string.Equals(token.CurrentToken, "true", StringComparison.OrdinalIgnoreCase))
                {
                    this.CheckForExistingParticipant(expression, token, isWithinFunction);
                    tokens.Dequeue();
                    expression = new ConstantValueExpression(ConstantValueExpressionType.Boolean, true);
                }
                else if (string.Equals(token.CurrentToken, "false", StringComparison.OrdinalIgnoreCase))
                {
                    this.CheckForExistingParticipant(expression, token, isWithinFunction);
                    tokens.Dequeue();
                    expression = new ConstantValueExpression(ConstantValueExpressionType.Boolean, false);
                }
                else if (string.Equals(token.CurrentToken, "null", StringComparison.OrdinalIgnoreCase))
                {
                    this.CheckForExistingParticipant(expression, token, isWithinFunction);
                    tokens.Dequeue();
                    expression = new ConstantValueExpression(ConstantValueExpressionType.Null, null);
                }
                else if (token.CurrentToken.StartsWith('#'.ToString()) && token.CurrentToken.EndsWith('#'.ToString()))
                {
                    this.CheckForExistingParticipant(expression, token, isWithinFunction);
                    tokens.Dequeue();
                    string text2 = token.CurrentToken.Replace('#'.ToString(), "");
                    DateTime dateTime = DateTime.MinValue;
                    if (!DateTime.TryParse(text2, out dateTime))
                    {
                        if (string.Equals("TODAY", text2, StringComparison.OrdinalIgnoreCase))
                        {
                            dateTime = DateTime.Today;
                        }
                        else
                        {
                            if (!string.Equals("NOW", text2, StringComparison.OrdinalIgnoreCase))
                            {
                                throw new UnrecognisedTokenException(text2);
                            }
                            dateTime = DateTime.Now;
                        }
                    }
                    expression = new ConstantValueExpression(ConstantValueExpressionType.DateTime, dateTime);
                }
                else if ((token.CurrentToken.StartsWith("'") && token.CurrentToken.EndsWith("'")) || (token.CurrentToken.StartsWith("\"") && token.CurrentToken.EndsWith("\"")))
                {
                    this.CheckForExistingParticipant(expression, token, isWithinFunction);
                    tokens.Dequeue();
                    expression = new ConstantValueExpression(ConstantValueExpressionType.String, ExpressionParser.CleanString(token.CurrentToken.Substring(1, token.Length - 2)));
                }
                else
                {
                    if (!string.Equals(token.CurrentToken, ','.ToString(), StringComparison.Ordinal))
                    {
                        tokens.Dequeue();
                        throw new UnrecognisedTokenException(token.CurrentToken);
                    }
                    if (!isWithinFunction)
                    {
                        throw new ExpressiveException(string.Format("Unexpected token '{0}'", token));
                    }
                    tokens.Dequeue();
                }
                previousToken = token;
                token = ExtensionMethods.PeekOrDefault<Token>(tokens);
            }
            return expression;
        }

        private static string CleanString(string input)
        {
            if (input.Length <= 1)
            {
                return input;
            }
            char[] array = new char[input.Length];
            int length = 0;
            int i = 0;
            while (i < input.Length)
            {
                char c = input[i];
                if (c != '\\' || i >= input.Length - 1)
                {
                    goto IL_A1;
                }
                char c2 = input[i + 1];
                if (c2 <= 'n')
                {
                    if (c2 != '\'')
                    {
                        if (c2 != 'n')
                        {
                            goto IL_A1;
                        }
                        array[length++] = '\n';
                        i++;
                    }
                    else
                    {
                        array[length++] = '\'';
                        i++;
                    }
                }
                else if (c2 != 'r')
                {
                    if (c2 != 't')
                    {
                        goto IL_A1;
                    }
                    array[length++] = '\t';
                    i++;
                }
                else
                {
                    array[length++] = '\r';
                    i++;
                }
                IL_A9:
                i++;
                continue;
                IL_A1:
                array[length++] = c;
                goto IL_A9;
            }
            return new string(array, 0, length);
        }

        private static bool CanExtractValue(string expression, int expressionLength, int index, string value)
        {
            return string.Equals(value, ExpressionParser.ExtractValue(expression, expressionLength, index, value), StringComparison.OrdinalIgnoreCase);
        }

        private static bool CanGetString(string expression, int startIndex, char quoteCharacter)
        {
            return !string.IsNullOrWhiteSpace(ExpressionParser.GetString(expression, startIndex, quoteCharacter));
        }

        private void CheckForExistingFunctionName(string functionName)
        {
            if (this._registeredFunctions.ContainsKey(functionName))
            {
                throw new FunctionNameAlreadyRegisteredException(functionName);
            }
        }

        private void CheckForExistingParticipant(IExpression participant, Token token, bool isWithinFunction)
        {
            if (participant == null)
            {
                return;
            }
            if (isWithinFunction)
            {
                throw new MissingTokenException("Missing token, expecting ','.", ',');
            }
            throw new ExpressiveException(string.Format("Unexpected token '{0}' at index {1}", token.CurrentToken, token.StartIndex));
        }

        private static bool CheckForTag(string tag, string lookAhead, ExpressiveOptions options)
        {
            return (options.HasFlag(ExpressiveOptions.IgnoreCase) && string.Equals(lookAhead, tag, StringComparison.OrdinalIgnoreCase)) || string.Equals(lookAhead, tag, StringComparison.Ordinal);
        }

        private static string ExtractValue(string expression, int expressionLength, int index, string expectedValue)
        {
            string result = null;
            int length = expectedValue.Length;
            if (expressionLength >= index + length)
            {
                string text = expression.Substring(index, length);
                bool flag = true;
                if (expressionLength > index + length)
                {
                    flag = !char.IsLetterOrDigit(expression[index + length]);
                }
                if (string.Equals(text, expectedValue, StringComparison.OrdinalIgnoreCase) && flag)
                {
                    result = text;
                }
            }
            return result;
        }

        private StringComparer GetDictionaryComparer(ExpressiveOptions options)
        {
            if (!options.HasFlag(ExpressiveOptions.IgnoreCase))
            {
                return StringComparer.Ordinal;
            }
            return StringComparer.OrdinalIgnoreCase;
        }

        private string GetNumber(string expression, int startIndex)
        {
            bool flag = false;
            int num = startIndex;
            char c = expression[num];
            while (num < expression.Length && (char.IsDigit(c) || (!flag && c == this._decimalSeparator)))
            {
                if (!flag && c == this._decimalSeparator)
                {
                    flag = true;
                }
                num++;
                if (num == expression.Length)
                {
                    break;
                }
                c = expression[num];
            }
            return expression.Substring(startIndex, num - startIndex);
        }

        private static string GetString(string expression, int startIndex, char quoteCharacter)
        {
            int num = startIndex;
            bool flag = false;
            char c = expression[num];
            char c2 = '\0';
            while (num < expression.Length && !flag)
            {
                if (num != startIndex && c == quoteCharacter && c2 != '\\')
                {
                    flag = true;
                }
                c2 = c;
                num++;
                if (num == expression.Length)
                {
                    break;
                }
                c = expression[num];
            }
            if (flag)
            {
                return expression.Substring(startIndex, num - startIndex);
            }
            return null;
        }

        private static bool IsQuote(char character)
        {
            return character == '\'' || character == '"';
        }

        private IList<Token> Tokenise(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
            {
                return null;
            }
            int length = expression.Length;
            IOrderedEnumerable<KeyValuePair<string, IOperator>> orderedEnumerable = Enumerable.OrderByDescending<KeyValuePair<string, IOperator>, int>(this._registeredOperators, new Func<KeyValuePair<string, IOperator>, int>(ExpressionParser._003C_003Ec._003C_003E9._003CTokenise_003Eb__25_0));
            List<Token> list = new List<Token>();
            IList<char> list2 = null;
            int i;
            int num;
            for (i = 0; i < length; i += ((num == 0) ? 1 : num))
            {
                num = 0;
                bool flag = false;
                foreach (KeyValuePair<string, Func<IExpression[], IDictionary<string, object>, object>> keyValuePair in Enumerable.OrderByDescending<KeyValuePair<string, Func<IExpression[], IDictionary<string, object>, object>>, int>(this._registeredFunctions, new Func<KeyValuePair<string, Func<IExpression[], IDictionary<string, object>, object>>, int>(ExpressionParser._003C_003Ec._003C_003E9._003CTokenise_003Eb__25_1)))
                {
                    string text = expression.Substring(i, Math.Min(keyValuePair.Key.Length, length - i));
                    if (ExpressionParser.CheckForTag(keyValuePair.Key, text, this._options))
                    {
                        ExpressionParser.CheckForUnrecognised(list2, list, i);
                        num = keyValuePair.Key.Length;
                        list.Add(new Token(text, i));
                        break;
                    }
                }
                if (num == 0)
                {
                    foreach (KeyValuePair<string, IOperator> keyValuePair2 in orderedEnumerable)
                    {
                        string text2 = expression.Substring(i, Math.Min(keyValuePair2.Key.Length, length - i));
                        if (ExpressionParser.CheckForTag(keyValuePair2.Key, text2, this._options))
                        {
                            ExpressionParser.CheckForUnrecognised(list2, list, i);
                            num = keyValuePair2.Key.Length;
                            list.Add(new Token(text2, i));
                            break;
                        }
                    }
                }
                if (num == 0)
                {
                    char c = expression[i];
                    if (c == '[')
                    {
                        char c2 = ']';
                        if (!ExpressionParser.CanGetString(expression, i, c2))
                        {
                            throw new MissingTokenException(string.Format("Missing closing token '{0}'", c2), c2);
                        }
                        string text3 = ExtensionMethods.SubstringUpTo(expression, i, c2);
                        ExpressionParser.CheckForUnrecognised(list2, list, i);
                        list.Add(new Token(text3, i));
                        num = text3.Length;
                    }
                    else if (char.IsDigit(c))
                    {
                        string number = this.GetNumber(expression, i);
                        ExpressionParser.CheckForUnrecognised(list2, list, i);
                        list.Add(new Token(number, i));
                        num = number.Length;
                    }
                    else if (ExpressionParser.IsQuote(c))
                    {
                        if (!ExpressionParser.CanGetString(expression, i, c))
                        {
                            throw new MissingTokenException(string.Format("Missing closing token '{0}'", c), c);
                        }
                        string @string = ExpressionParser.GetString(expression, i, c);
                        ExpressionParser.CheckForUnrecognised(list2, list, i);
                        list.Add(new Token(@string, i));
                        num = @string.Length;
                    }
                    else if (c == '#')
                    {
                        if (!ExpressionParser.CanGetString(expression, i, c))
                        {
                            throw new MissingTokenException(string.Format("Missing closing token '{0}'", c), c);
                        }
                        string text4 = "#" + ExtensionMethods.SubstringUpTo(expression, i + 1, '#');
                        ExpressionParser.CheckForUnrecognised(list2, list, i);
                        list.Add(new Token(text4, i));
                        num = text4.Length;
                    }
                    else if (c == ',')
                    {
                        ExpressionParser.CheckForUnrecognised(list2, list, i);
                        list.Add(new Token(c.ToString(), i));
                        num = 1;
                    }
                    else if ((c == 't' || c == 'T') && ExpressionParser.CanExtractValue(expression, length, i, "true"))
                    {
                        ExpressionParser.CheckForUnrecognised(list2, list, i);
                        string text5 = ExpressionParser.ExtractValue(expression, length, i, "true");
                        if (!string.IsNullOrWhiteSpace(text5))
                        {
                            list.Add(new Token(text5, i));
                            num = 4;
                        }
                    }
                    else if ((c == 'f' || c == 'F') && ExpressionParser.CanExtractValue(expression, length, i, "false"))
                    {
                        ExpressionParser.CheckForUnrecognised(list2, list, i);
                        string text6 = ExpressionParser.ExtractValue(expression, length, i, "false");
                        if (!string.IsNullOrWhiteSpace(text6))
                        {
                            list.Add(new Token(text6, i));
                            num = 5;
                        }
                    }
                    else if ((c == 'n' || c == 'N') && ExpressionParser.CanExtractValue(expression, length, i, "null"))
                    {
                        ExpressionParser.CheckForUnrecognised(list2, list, i);
                        string text7 = ExpressionParser.ExtractValue(expression, length, i, "null");
                        if (!string.IsNullOrWhiteSpace(text7))
                        {
                            list.Add(new Token(text7, i));
                            num = 4;
                        }
                    }
                    else if (!char.IsWhiteSpace(c))
                    {
                        if (list2 == null)
                        {
                            list2 = new List<char>();
                        }
                        flag = true;
                        list2.Add(c);
                    }
                }
                if (!flag)
                {
                    ExpressionParser.CheckForUnrecognised(list2, list, i);
                    list2 = null;
                }
            }
            ExpressionParser.CheckForUnrecognised(list2, list, i);
            return list;
        }

        private static void CheckForUnrecognised(IList<char> unrecognised, IList<Token> tokens, int index)
        {
            if (unrecognised != null)
            {
                string text = new string(Enumerable.ToArray<char>(unrecognised));
                tokens.Add(new Token(text, index - text.Length));
            }
        }

        private readonly char _decimalSeparator;

        private readonly ExpressiveOptions _options;

        private readonly IDictionary<string, Func<IExpression[], IDictionary<string, object>, object>> _registeredFunctions;

        private readonly IDictionary<string, IOperator> _registeredOperators;

        private readonly StringComparer _stringComparer;

        [Serializable]
        private sealed class _003C_003Ec
        {
            internal string _003CCompileExpression_003Eb__8_0(Token t)
            {
                return t.CurrentToken;
            }

            internal bool _003CCompileExpression_003Eb__8_1(string t)
            {
                return string.Equals(t, "(", StringComparison.Ordinal);
            }

            internal string _003CCompileExpression_003Eb__8_2(Token t)
            {
                return t.CurrentToken;
            }

            internal bool _003CCompileExpression_003Eb__8_3(string t)
            {
                return string.Equals(t, ")", StringComparison.Ordinal);
            }

            internal int _003CTokenise_003Eb__25_0(KeyValuePair<string, IOperator> op)
            {
                return op.Key.Length;
            }

            internal int _003CTokenise_003Eb__25_1(KeyValuePair<string, Func<IExpression[], IDictionary<string, object>, object>> f)
            {
                return f.Key.Length;
            }

            public static readonly ExpressionParser._003C_003Ec _003C_003E9 = new ExpressionParser._003C_003Ec();

            public static Func<Token, string> _003C_003E9__8_0;

            public static Func<string, bool> _003C_003E9__8_1;

            public static Func<Token, string> _003C_003E9__8_2;

            public static Func<string, bool> _003C_003E9__8_3;

            public static Func<KeyValuePair<string, IOperator>, int> _003C_003E9__25_0;

            public static Func<KeyValuePair<string, Func<IExpression[], IDictionary<string, object>, object>>, int> _003C_003E9__25_1;
        }

        private sealed class _003C_003Ec__DisplayClass10_0
        {
            internal object _003CRegisterFunction_003Eb__0(IExpression[] p, IDictionary<string, object> a)
            {
                this.function.Variables = a;
                return this.function.Evaluate(p, this._003C_003E4__this._options);
            }

            public IFunction function;

            public ExpressionParser _003C_003E4__this;
        }
    }
}
