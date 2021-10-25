using System;
using GGMatch3;
using TMPro;
using UnityEngine;

public class MonsterElementBehaviour : MonoBehaviour
{
	public void Init(LevelDefinition.MonsterElement monsterElement)
	{
		GGUtil.SetScale(this.scalerTransform, new Vector3((float)monsterElement.size.x, (float)monsterElement.size.y, 1f));
		Match3Settings.MonsterColorSettings monsterColorSettings = Match3Settings.instance.GeMonsterColorSettings(monsterElement.itemColor);
		if (monsterColorSettings != null)
		{
			this.monsterSprite.material = monsterColorSettings.material;
		}
	}

	public void SetCount(int countRemaining)
	{
		if (this.label == null)
		{
			return;
		}
		GGUtil.SetActive(this.label.transform, countRemaining > 0);
		this.label.text = countRemaining.ToString();
	}

	public void DoEatAnimation()
	{
		if (this.monsterAnimator == null)
		{
			return;
		}
		this.monsterAnimator.SetTrigger("eat");
	}

	[SerializeField]
	private Transform scalerTransform;

	[SerializeField]
	private SpriteRenderer monsterSprite;

	[SerializeField]
	private TextMeshPro label;

	[SerializeField]
	private Animator monsterAnimator;
}
