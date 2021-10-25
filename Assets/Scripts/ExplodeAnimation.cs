using System;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeAnimation
{
	public bool hasParts
	{
		get
		{
			return this.partDefinitions.Count > 0;
		}
	}

	private ExplodeAnimation.PartDefinition GetOrCreatePartDefinition(int sortingGroupIndex)
	{
		for (int i = 0; i < this.partDefinitions.Count; i++)
		{
			ExplodeAnimation.PartDefinition partDefinition = this.partDefinitions[i];
			if (partDefinition.sortingGroupIndex == sortingGroupIndex)
			{
				return partDefinition;
			}
		}
		ExplodeAnimation.PartDefinition partDefinition2 = new ExplodeAnimation.PartDefinition();
		partDefinition2.sortingGroupIndex = sortingGroupIndex;
		this.partDefinitions.Add(partDefinition2);
		return partDefinition2;
	}

	public void Init(CarModel model)
	{
		this.model = model;
		this.partDefinitions.Clear();
		List<CarModelPart> parts = model.parts;
		for (int i = 0; i < parts.Count; i++)
		{
			CarModelPart carModelPart = parts[i];
			if (!carModelPart.partInfo.suspendExploding && carModelPart.partInfo.isOwned)
			{
				this.GetOrCreatePartDefinition(carModelPart.partInfo.explodeGroupIndex).parts.Add(carModelPart);
			}
		}
		this.partDefinitions.Sort(new Comparison<ExplodeAnimation.PartDefinition>(ExplodeAnimation._003C_003Ec._003C_003E9._003CInit_003Eb__6_0));
		if (this.partDefinitions.Count == 0)
		{
			return;
		}
		float num = 1f / (float)this.partDefinitions.Count;
		for (int j = 0; j < this.partDefinitions.Count; j++)
		{
			ExplodeAnimation.PartDefinition partDefinition = this.partDefinitions[j];
			partDefinition.startTime = (float)j * num;
			partDefinition.endTime = (float)(j + 1) * num;
			this.partDefinitions[j] = partDefinition;
		}
	}

	public float ClosestFullTime(float time, float changeDirection)
	{
		if (this.partDefinitions.Count == 0)
		{
			return 0f;
		}
		float num = 1f / (float)this.partDefinitions.Count;
		float num2 = (float)Mathf.FloorToInt(time / num);
		float num3 = (time - num2 * num) / num;
		float num4 = ScriptableObjectSingleton<CarsDB>.instance.explosionSettings.minValueWhenSwitch;
		if (changeDirection < 0f)
		{
			num4 = 1f - num4;
		}
		float value = num2 * num;
		if (num3 > num4)
		{
			value = (num2 + 1f) * num;
		}
		return Mathf.Clamp01(value);
	}

	public void SetTimeTo(float time)
	{
		for (int i = 0; i < this.partDefinitions.Count; i++)
		{
			ExplodeAnimation.PartDefinition partDefinition = this.partDefinitions[i];
			float explodeOffset = Mathf.InverseLerp(partDefinition.startTime, partDefinition.endTime, time);
			for (int j = 0; j < partDefinition.parts.Count; j++)
			{
				partDefinition.parts[j].SetExplodeOffset(explodeOffset);
			}
		}
	}

	[NonSerialized]
	private CarModel model;

	private List<ExplodeAnimation.PartDefinition> partDefinitions = new List<ExplodeAnimation.PartDefinition>();

	public class PartDefinition
	{
		public float startTime;

		public float endTime;

		public int sortingGroupIndex;

		public List<CarModelPart> parts = new List<CarModelPart>();
	}

	[Serializable]
	private sealed class _003C_003Ec
	{
		internal int _003CInit_003Eb__6_0(ExplodeAnimation.PartDefinition a, ExplodeAnimation.PartDefinition b)
		{
			return b.sortingGroupIndex.CompareTo(a.sortingGroupIndex);
		}

		public static readonly ExplodeAnimation._003C_003Ec _003C_003E9 = new ExplodeAnimation._003C_003Ec();

		public static Comparison<ExplodeAnimation.PartDefinition> _003C_003E9__6_0;
	}
}
