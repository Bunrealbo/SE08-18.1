using System;
using System.Security.Cryptography;
using System.Text;

public class Hash
{
	public static string getHashSha256(string text)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(text);
		return Convert.ToBase64String(new SHA256Managed().ComputeHash(bytes));
	}
}
