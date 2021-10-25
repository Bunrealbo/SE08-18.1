using System;
using System.Collections;

public class NGUIJson
{
	public static object jsonDecode(string json)
	{
		NGUIJson.lastDecode = json;
		if (json == null)
		{
			return null;
		}
		char[] json2 = json.ToCharArray();
		int num = 0;
		bool flag = true;
		object result = NGUIJson.parseValue(json2, ref num, ref flag);
		if (flag)
		{
			NGUIJson.lastErrorIndex = -1;
			return result;
		}
		NGUIJson.lastErrorIndex = num;
		return result;
	}

	protected static Hashtable parseObject(char[] json, ref int index)
	{
		Hashtable hashtable = new Hashtable();
		NGUIJson.nextToken(json, ref index);
		bool flag = false;
		while (!flag)
		{
			int num = NGUIJson.lookAhead(json, index);
			if (num == 0)
			{
				return null;
			}
			if (num == 6)
			{
				NGUIJson.nextToken(json, ref index);
			}
			else
			{
				if (num == 2)
				{
					NGUIJson.nextToken(json, ref index);
					return hashtable;
				}
				string text = NGUIJson.parseString(json, ref index);
				if (text == null)
				{
					return null;
				}
				num = NGUIJson.nextToken(json, ref index);
				if (num != 5)
				{
					return null;
				}
				bool flag2 = true;
				object value = NGUIJson.parseValue(json, ref index, ref flag2);
				if (!flag2)
				{
					return null;
				}
				hashtable[text] = value;
			}
		}
		return hashtable;
	}

	protected static ArrayList parseArray(char[] json, ref int index)
	{
		ArrayList arrayList = new ArrayList();
		NGUIJson.nextToken(json, ref index);
		bool flag = false;
		while (!flag)
		{
			int num = NGUIJson.lookAhead(json, index);
			if (num == 0)
			{
				return null;
			}
			if (num == 6)
			{
				NGUIJson.nextToken(json, ref index);
			}
			else
			{
				if (num == 4)
				{
					NGUIJson.nextToken(json, ref index);
					break;
				}
				bool flag2 = true;
				object value = NGUIJson.parseValue(json, ref index, ref flag2);
				if (!flag2)
				{
					return null;
				}
				arrayList.Add(value);
			}
		}
		return arrayList;
	}

	protected static object parseValue(char[] json, ref int index, ref bool success)
	{
		switch (NGUIJson.lookAhead(json, index))
		{
		case 1:
			return NGUIJson.parseObject(json, ref index);
		case 3:
			return NGUIJson.parseArray(json, ref index);
		case 7:
			return NGUIJson.parseString(json, ref index);
		case 8:
			return NGUIJson.parseNumber(json, ref index);
		case 9:
			NGUIJson.nextToken(json, ref index);
			return bool.Parse("TRUE");
		case 10:
			NGUIJson.nextToken(json, ref index);
			return bool.Parse("FALSE");
		case 11:
			NGUIJson.nextToken(json, ref index);
			return null;
		}
		success = false;
		return null;
	}

	protected static string parseString(char[] json, ref int index)
	{
		string text = "";
		NGUIJson.eatWhitespace(json, ref index);
		int num = index;
		index = num + 1;
		char c = json[num];
		bool flag = false;
		while (!flag && index != json.Length)
		{
			num = index;
			index = num + 1;
			c = json[num];
			if (c == '"')
			{
				flag = true;
				break;
			}
			if (c == '\\')
			{
				if (index == json.Length)
				{
					break;
				}
				num = index;
				index = num + 1;
				c = json[num];
				if (c == '"')
				{
					text += "\"";
				}
				else if (c == '\\')
				{
					text += "\\";
				}
				else if (c == '/')
				{
					text += "/";
				}
				else if (c == 'b')
				{
					text += "\b";
				}
				else if (c == 'f')
				{
					text += "\f";
				}
				else if (c == 'n')
				{
					text += "\n";
				}
				else if (c == 'r')
				{
					text += "\r";
				}
				else if (c == 't')
				{
					text += "\t";
				}
				else if (c == 'u')
				{
					if (json.Length - index < 4)
					{
						break;
					}
					char[] array = new char[4];
					Array.Copy(json, index, array, 0, 4);
					text = text + "&#x" + new string(array) + ";";
					index += 4;
				}
			}
			else
			{
				text += c.ToString();
			}
		}
		if (!flag)
		{
			return null;
		}
		return text;
	}

	protected static double parseNumber(char[] json, ref int index)
	{
		NGUIJson.eatWhitespace(json, ref index);
		int lastIndexOfNumber = NGUIJson.getLastIndexOfNumber(json, index);
		int num = lastIndexOfNumber - index + 1;
		char[] array = new char[num];
		Array.Copy(json, index, array, 0, num);
		index = lastIndexOfNumber + 1;
		return double.Parse(new string(array));
	}

	protected static int getLastIndexOfNumber(char[] json, int index)
	{
		int num = index;
		while (num < json.Length && "0123456789+-.eE".IndexOf(json[num]) != -1)
		{
			num++;
		}
		return num - 1;
	}

	protected static void eatWhitespace(char[] json, ref int index)
	{
		while (index < json.Length && " \t\n\r".IndexOf(json[index]) != -1)
		{
			index++;
		}
	}

	protected static int lookAhead(char[] json, int index)
	{
		int num = index;
		return NGUIJson.nextToken(json, ref num);
	}

	protected static int nextToken(char[] json, ref int index)
	{
		NGUIJson.eatWhitespace(json, ref index);
		if (index == json.Length)
		{
			return 0;
		}
		char c = json[index];
		index++;
		if (c <= '[')
		{
			switch (c)
			{
			case '"':
				return 7;
			case '#':
			case '$':
			case '%':
			case '&':
			case '\'':
			case '(':
			case ')':
			case '*':
			case '+':
			case '.':
			case '/':
				break;
			case ',':
				return 6;
			case '-':
			case '0':
			case '1':
			case '2':
			case '3':
			case '4':
			case '5':
			case '6':
			case '7':
			case '8':
			case '9':
				return 8;
			case ':':
				return 5;
			default:
				if (c == '[')
				{
					return 3;
				}
				break;
			}
		}
		else
		{
			if (c == ']')
			{
				return 4;
			}
			if (c == '{')
			{
				return 1;
			}
			if (c == '}')
			{
				return 2;
			}
		}
		index--;
		int num = json.Length - index;
		if (num >= 5 && json[index] == 'f' && json[index + 1] == 'a' && json[index + 2] == 'l' && json[index + 3] == 's' && json[index + 4] == 'e')
		{
			index += 5;
			return 10;
		}
		if (num >= 4 && json[index] == 't' && json[index + 1] == 'r' && json[index + 2] == 'u' && json[index + 3] == 'e')
		{
			index += 4;
			return 9;
		}
		if (num >= 4 && json[index] == 'n' && json[index + 1] == 'u' && json[index + 2] == 'l' && json[index + 3] == 'l')
		{
			index += 4;
			return 11;
		}
		return 0;
	}

	protected static int lastErrorIndex = -1;

	protected static string lastDecode = "";
}
