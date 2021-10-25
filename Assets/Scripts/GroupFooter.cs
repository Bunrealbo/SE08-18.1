using System;
using TMPro;
using UnityEngine;

public class GroupFooter : MonoBehaviour
{
	public void Init(DecoratingScene scene)
	{
		GGUtil.Hide(this);
		if (!this.enableFooter)
		{
			return;
		}
		DecoratingScene.GroupDefinition groupDefinition = scene.CurrentGroup();
		if (groupDefinition == null)
		{
			return;
		}
		if (string.IsNullOrWhiteSpace(groupDefinition.title))
		{
			return;
		}
		GGUtil.Show(this);
		this.label.text = groupDefinition.title;
	}

	[SerializeField]
	private TextMeshProUGUI label;

	[SerializeField]
	private bool enableFooter;
}
