using System;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceTexturesHelper : MonoBehaviour
{
	public GameObject rootGameObject;

	public List<ReplaceTexturesHelper.ReplaceTexture> toReplace = new List<ReplaceTexturesHelper.ReplaceTexture>();

	[Serializable]
	public class ReplaceTexture
	{
		public Texture findTexture;

		public Texture replaceTexture;
	}
}
