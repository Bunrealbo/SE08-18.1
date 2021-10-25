using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CarPartInfo
{
	public bool hasSomethingToSay
	{
		get
		{
			return !string.IsNullOrWhiteSpace(this.thingToSay);
		}
	}

	public string displayName
	{
		get
		{
			if (!string.IsNullOrWhiteSpace(this.displayName_))
			{
				return this.displayName_;
			}
			return this.name;
		}
	}

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

	private RoomsBackend.VisualObjectAccessor visualObjectModel
	{
		get
		{
			if (this.visualObjectModel_ == null)
			{
				this.visualObjectModel_ = this.backendAccessor.GetVisualObject(this.name);
			}
			if (this.visualObjectModel_.needsToBeRenewed)
			{
				this.visualObjectModel_ = this.backendAccessor.GetVisualObject(this.name);
			}
			return this.visualObjectModel_;
		}
	}

	public int selectedVariation
	{
		set
		{
			this.visualObjectModel.visualObject.selectedVariationIndex = value;
			this.visualObjectModel.Save();
		}
	}

	public bool isOwned
	{
		get
		{
			return this.isDefault || this.visualObjectModel.visualObject.isOwned;
		}
		set
		{
			this.visualObjectModel.visualObject.isOwned = value;
			this.visualObjectModel.Save();
		}
	}

	public bool isUnlocked
	{
		get
		{
			return this.groupIndex <= 0 || this.carModel.IsAllElementsPickedUpInGroup(this.groupIndex - 1);
		}
	}

	public void InitForRuntime(CarModel carModel, RoomsBackend.RoomAccessor backend)
	{
		this.carModel = carModel;
		this.backendAccessor_ = backend;
	}

	[SerializeField]
	public List<CarModelPart> hideWhenAnyActive = new List<CarModelPart>();

	[SerializeField]
	public bool isDefault;

	[SerializeField]
	public string name;

	[SerializeField]
	private string displayName_;

	[SerializeField]
	public string thingToSay;

	[SerializeField]
	public string cameraName;

	[SerializeField]
	public List<string> toSayBefore = new List<string>();

	[SerializeField]
	public int groupIndex;

	[SerializeField]
	public int explodeGroupIndex;

	[SerializeField]
	public CarPartInfo.AnimType animType;

	[SerializeField]
	public Vector3 moveOffset;

	[SerializeField]
	public bool suspendExploding;

	[SerializeField]
	public bool confirmEachSubpart;

	[SerializeField]
	public float delaySubpartAnimation;

	[SerializeField]
	public string variantGroupToShowAfterPurchase;

	[SerializeField]
	public List<string> animateChangeWithVariations = new List<string>();

	[NonSerialized]
	private CarModel carModel;

	private RoomsBackend.RoomAccessor backendAccessor_;

	private RoomsBackend.VisualObjectAccessor visualObjectModel_;

	public enum AnimType
	{
		ScaleBounce,
		MoveOffset
	}
}
