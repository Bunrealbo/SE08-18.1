using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CarModelInfo
{
	public CarModelInfo.VariantGroup GetVariantGroup(string name)
	{
		for (int i = 0; i < this.variantGroups.Count; i++)
		{
			CarModelInfo.VariantGroup variantGroup = this.variantGroups[i];
			if (variantGroup.name == name)
			{
				return variantGroup;
			}
		}
		return null;
	}

	public void RemoveVariantGroup(string name)
	{
		CarModelInfo._003C_003Ec__DisplayClass3_0 _003C_003Ec__DisplayClass3_ = new CarModelInfo._003C_003Ec__DisplayClass3_0();
		_003C_003Ec__DisplayClass3_.name = name;
		this.variantGroups.RemoveAll(new Predicate<CarModelInfo.VariantGroup>(_003C_003Ec__DisplayClass3_._003CRemoveVariantGroup_003Eb__0));
	}

	public void AddGroup(CarModelInfo.VariantGroup group)
	{
		this.variantGroups.Add(group);
	}

	public void Init(RoomsBackend.RoomAccessor roomAccessor)
	{
		for (int i = 0; i < this.variantGroups.Count; i++)
		{
			this.variantGroups[i].Init(roomAccessor);
		}
	}

	[SerializeField]
	private List<CarModelInfo.VariantGroup> variantGroups = new List<CarModelInfo.VariantGroup>();

	[Serializable]
	public class VariantGroup
	{
		private RoomsBackend.RoomAccessor backendAccessor
		{
			get
			{
				if (this.backendAccessor_.needsToBeRenewed)
				{
					this.backendAccessor_ = this.backendAccessor_.CreateRenewedAccessor();
				}
				return this.backendAccessor_;
			}
		}

		private RoomsBackend.VariantGroupAccessor variantGroupModel
		{
			get
			{
				if (this.variantGroupModel_ == null)
				{
					this.variantGroupModel_ = this.backendAccessor.GetVariantGroup(this.name);
				}
				if (this.variantGroupModel_.needsToBeRenewed)
				{
					this.variantGroupModel_ = this.backendAccessor.GetVariantGroup(this.name);
				}
				return this.variantGroupModel_;
			}
		}

		public int selectedVariationIndex
		{
			get
			{
				return this.variantGroupModel.variantGroup.selectedVariationIndex;
			}
			set
			{
				this.variantGroupModel.variantGroup.selectedVariationIndex = value;
				this.variantGroupModel.Save();
			}
		}

		public void Init(RoomsBackend.RoomAccessor backendAccessor)
		{
			this.backendAccessor_ = backendAccessor;
		}

		[SerializeField]
		public string name;

		[SerializeField]
		public List<CarModelInfo.VariantGroup.Variation> variations = new List<CarModelInfo.VariantGroup.Variation>();

		[SerializeField]
		public string cameraName;

		private RoomsBackend.RoomAccessor backendAccessor_;

		private RoomsBackend.VariantGroupAccessor variantGroupModel_;

		[Serializable]
		public class Variation
		{
			public Color uiSpriteColor;
		}
	}

	private sealed class _003C_003Ec__DisplayClass3_0
	{
		internal bool _003CRemoveVariantGroup_003Eb__0(CarModelInfo.VariantGroup group)
		{
			return group.name == this.name;
		}

		public string name;
	}
}
