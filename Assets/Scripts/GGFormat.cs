using System;
using System.Collections.Generic;
using System.Text;

public class GGFormat
{
    public static string JavaScriptStringEncode(string value, bool addDoubleQuotes = false)
    {
        if (string.IsNullOrEmpty(value))
        {
            if (!addDoubleQuotes)
            {
                return string.Empty;
            }
            return "\"\"";
        }
        else
        {
            int length = value.Length;
            bool flag = false;
            for (int i = 0; i < length; i++)
            {
                char c = value[i];
                if ((c >= '\0' && c <= '\u001f') || c == '"' || c == '\'' || c == '<' || c == '>' || c == '\\')
                {
                    flag = true;
                    break;
                }
            }
            if (flag)
            {
                StringBuilder stringBuilder = new StringBuilder();
                if (addDoubleQuotes)
                {
                    stringBuilder.Append('"');
                }
                for (int j = 0; j < length; j++)
                {
                    char c = value[j];
                    if ((c >= '\0' && c <= '\a') || (c == '\v' || (c >= '\u000e' && c <= '\u001f')) || c == '\'' || c == '<' || c == '>')
                    {
                        stringBuilder.AppendFormat("\\u{0:x4}", (int)c);
                    }
                    else
                    {
                        int num = (int)c;
                        switch (num)
                        {
                            case 8:
                                stringBuilder.Append("\\b");
                                goto IL_174;
                            case 9:
                                stringBuilder.Append("\\t");
                                goto IL_174;
                            case 10:
                                stringBuilder.Append("\\n");
                                goto IL_174;
                            case 11:
                                break;
                            case 12:
                                stringBuilder.Append("\\f");
                                goto IL_174;
                            case 13:
                                stringBuilder.Append("\\r");
                                goto IL_174;
                            default:
                                if (num == 34)
                                {
                                    stringBuilder.Append("\\\"");
                                    goto IL_174;
                                }
                                if (num == 92)
                                {
                                    stringBuilder.Append("\\\\");
                                    goto IL_174;
                                }
                                break;
                        }
                        stringBuilder.Append(c);
                    }
                    IL_174:;
                }
                if (addDoubleQuotes)
                {
                    stringBuilder.Append('"');
                }
                return stringBuilder.ToString();
            }
            if (!addDoubleQuotes)
            {
                return value;
            }
            return "\"" + value + "\"";
        }
    }

    public static string FormatPrice(int price, bool rem = false)
    {
        if (price >= 1000000)
        {
            string str = rem ? (price / 1000000).ToString("D3") : (price / 1000000).ToString();
            str += " ";
            int num = price % 1000000;
            if (num >= 1000)
            {
                return str + GGFormat.FormatPrice(num, true);
            }
            if (num % 1000 > 0)
            {
                return str + "000 " + GGFormat.FormatPrice(num % 1000, true);
            }
            return str + "000 000";
        }
        else
        {
            if (price >= 1000)
            {
                string text = rem ? (price / 1000).ToString("D3") : (price / 1000).ToString();
                text = text + " " + (price % 1000).ToString("D3");
                while (price.ToString().Length >= text.Length)
                {
                    text += "0";
                }
                return text;
            }
            if (!rem)
            {
                return price.ToString();
            }
            return price.ToString("D3");
        }
    }

    public static string FormatPrice(long price, bool rem = false)
    {
        if (price >= 1000000L)
        {
            string str = rem ? (price / 1000000L).ToString("D3") : (price / 1000000L).ToString();
            str += " ";
            long num = price % 1000000L;
            if (num >= 1000L)
            {
                return str + GGFormat.FormatPrice(num, true);
            }
            if (num % 1000L > 0L)
            {
                return str + "000 " + GGFormat.FormatPrice(num % 1000L, true);
            }
            return str + "000 000";
        }
        else
        {
            if (price >= 1000L)
            {
                string text = rem ? (price / 1000L).ToString("D3") : (price / 1000L).ToString();
                text = text + " " + (price % 1000L).ToString("D3");
                while (price.ToString().Length >= text.Length)
                {
                    text += "0";
                }
                return text;
            }
            if (!rem)
            {
                return price.ToString();
            }
            return price.ToString("D3");
        }
    }

    public static string FormatPercent(float p)
    {
        return ((int)(p * 100f)).ToString();
    }

    public static string Implode(IEnumerable<string> list, string glue)
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (string value in list)
        {
            if (stringBuilder.Length > 0)
            {
                stringBuilder.Append(glue);
            }
            stringBuilder.Append(value);
        }
        return stringBuilder.ToString();
    }

    public static string FormatTime(int time)
    {
        if (time < 10)
        {
            return "0" + time;
        }
        return time.ToString();
    }

    public static string FormatTimeSpan(TimeSpan span)
    {
        string str = "";
        if (span.TotalDays >= 1.0)
        {
            str = str + GGFormat.FormatTime(span.Days) + ":";
        }
        if (span.TotalHours >= 1.0)
        {
            str = str + GGFormat.FormatTime(span.Hours) + ":";
        }
        return str + GGFormat.FormatTime(span.Minutes) + ":" + GGFormat.FormatTime(span.Seconds);
    }
}
