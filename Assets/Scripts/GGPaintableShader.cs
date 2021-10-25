using System;
using UnityEngine;

public class GGPaintableShader
{
	public static Shader Load(string shaderName)
	{
		Shader shader = Shader.Find(shaderName);
		if (shader == null)
		{
			throw new Exception("Failed to find shader called: " + shaderName);
		}
		return shader;
	}

	public static Material Build(Shader shader)
	{
		return new Material(shader);
	}

	public static Vector4 IndexToVector(int index)
	{
		switch (index)
		{
		case 0:
			return new Vector4(1f, 0f, 0f, 0f);
		case 1:
			return new Vector4(0f, 1f, 0f, 0f);
		case 2:
			return new Vector4(0f, 0f, 1f, 0f);
		case 3:
			return new Vector4(0f, 0f, 0f, 1f);
		default:
			return default(Vector4);
		}
	}

	public static int _Channel = Shader.PropertyToID("_Channel");

	public static int _Position = Shader.PropertyToID("_Position");

	public static int _BrushSize = Shader.PropertyToID("_BrushSize");

	public static int _BrushHardness = Shader.PropertyToID("_BrushHardness");

	public static int _Color = Shader.PropertyToID("_Color");

	public static int _Texture = Shader.PropertyToID("_Texture");

	public static int _MainTex = Shader.PropertyToID("_MainTex");

	public static int _IlsandMap = Shader.PropertyToID("_IlsandMap");

	public static int _TexelSize = Shader.PropertyToID("_TexelSize");
}
