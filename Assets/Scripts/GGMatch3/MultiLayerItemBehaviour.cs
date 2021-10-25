using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGMatch3
{
	public class MultiLayerItemBehaviour : SlotComponentBehavoiour
	{
		public void SetHasEmptyChip(bool hasEmptyChip)
		{
			GGUtil.SetActive(this.emptyChipTransform, hasEmptyChip);
		}

		private MultiLayerItemBehaviour.LayerSet GetLayerSet(ChipType chipType)
		{
			for (int i = 0; i < this.layerSetList.Count; i++)
			{
				MultiLayerItemBehaviour.LayerSet layerSet = this.layerSetList[i];
				if (layerSet.chipType == chipType)
				{
					return layerSet;
				}
			}
			return null;
		}

		public void HideAllLayers()
		{
			for (int i = 0; i < this.layerSetList.Count; i++)
			{
				this.layerSetList[i].HideAllLayers();
			}
		}

		public void SetLayerIndex(int layerIndex)
		{
			MultiLayerItemBehaviour.LayerSet layerSet = this.GetLayerSet(this.chipType);
			if (layerSet == null)
			{
				return;
			}
			layerSet.ShowLayer(layerIndex);
		}

		public void SetPattern(Slot slot)
		{
			GGUtil.SetActive(this.slotPatterTransform, slot.isBackgroundPatternActive);
		}

		public bool HasChipType(ChipType chipType)
		{
			return this.GetLayerSet(chipType) != null;
		}

		public void Init(ChipType chipType, int layerIndex)
		{
			for (int i = 0; i < this.layerSetList.Count; i++)
			{
				MultiLayerItemBehaviour.LayerSet layerSet = this.layerSetList[i];
				GGUtil.SetActive(layerSet.container, false);
				layerSet.HideAllLayers();
			}
			this.chipType = chipType;
			this.SetLayerIndex(layerIndex);
			GGUtil.SetActive(this, true);
		}

		public override void RemoveFromGame()
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}

		[SerializeField]
		private List<MultiLayerItemBehaviour.LayerSet> layerSetList = new List<MultiLayerItemBehaviour.LayerSet>();

		[SerializeField]
		private Transform emptyChipTransform;

		[SerializeField]
		private Transform slotPatterTransform;

		private ChipType chipType;

		[Serializable]
		public class LayerSet
		{
			public void HideAllLayers()
			{
				for (int i = 0; i < this.layers.Count; i++)
				{
					GGUtil.SetActive(this.layers[i].objectToShow, false);
				}
			}

			public void ShowLayer(int index)
			{
				GGUtil.SetActive(this.container, true);
				int num = Mathf.Clamp(index, 0, this.layers.Count - 1);
				for (int i = 0; i < this.layers.Count; i++)
				{
					MultiLayerItemBehaviour.LayerDesc layerDesc = this.layers[i];
					bool active = i == num;
					GGUtil.SetActive(layerDesc.objectToShow, active);
				}
			}

			public ChipType chipType;

			public GameObject container;

			public List<MultiLayerItemBehaviour.LayerDesc> layers = new List<MultiLayerItemBehaviour.LayerDesc>();
		}

		[Serializable]
		public class LayerDesc
		{
			public GameObject objectToShow;
		}
	}
}
