using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class VisualStyleSet
{
	public void Apply()
	{
		for (int i = 0; i < this.objects.Count; i++)
		{
			this.objects[i].Apply();
		}
	}

	public void Apply(VisibilityHelper visibility)
	{
		for (int i = 0; i < this.objects.Count; i++)
		{
			this.objects[i].Apply(visibility);
		}
	}

	[SerializeField]
	private List<VisualStyleSet.ObjectStyle> objects = new List<VisualStyleSet.ObjectStyle>();

	[Serializable]
	public class ObjectStyle
	{
		public void Apply()
		{
			bool active = this.action == VisualStyleSet.ObjectStyle.Action.Show;
			GGUtil.SetActive(this.visual, active);
		}

		public void Apply(VisibilityHelper visibility)
		{
			bool isVisible = this.action == VisualStyleSet.ObjectStyle.Action.Show;
			visibility.SetActive(this.visual, isVisible);
		}

		public VisualStyleSet.ObjectStyle.Action action;

		public Transform visual;

		public enum Action
		{
			Show,
			Hide
		}
	}
}
