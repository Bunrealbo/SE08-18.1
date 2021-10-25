using System;
using UnityEngine;

public class PartTemplate : MonoBehaviour
{
	public string linkName
	{
		get
		{
			string[] array = this.path.Split(new char[]
			{
				'/'
			});
			if (array.Length == 0)
			{
				return "";
			}
			return array[0].Replace("[", "").Replace("]", "");
		}
	}

	[SerializeField]
	public string path;

	[SerializeField]
	public PartTemplate.CopyType copyType;

	public enum CopyType
	{
		CopyAllChildren,
		CopyRoot
	}
}
