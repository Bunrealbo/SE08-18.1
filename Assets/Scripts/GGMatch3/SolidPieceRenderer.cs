using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGMatch3
{
	public class SolidPieceRenderer : SlotComponentBehavoiour
	{
		public void Init(Chip chip)
		{
			SolidPieceRenderer.ChipTypeSettings chipTypeSettings = null;
			for (int i = 0; i < this.chipTypeSettings.Count; i++)
			{
				SolidPieceRenderer.ChipTypeSettings chipTypeSettings2 = this.chipTypeSettings[i];
				bool flag = chipTypeSettings2.chipType == chip.chipType;
				chipTypeSettings2.obj.SetActive(flag);
				if (flag)
				{
					chipTypeSettings = chipTypeSettings2;
				}
			}
			if (chipTypeSettings != null)
			{
				chipTypeSettings.obj.SetActive(true);
				chipTypeSettings.obj.transform.localRotation = Quaternion.Euler(chipTypeSettings.rotation);
			}
			this.chip = chip;
			chip.SetTransformToMove(base.transform);
			this.UpdateLook();
		}

		public void Init(ChipType chipType)
		{
			SolidPieceRenderer.ChipTypeSettings chipTypeSettings = null;
			for (int i = 0; i < this.chipTypeSettings.Count; i++)
			{
				SolidPieceRenderer.ChipTypeSettings chipTypeSettings2 = this.chipTypeSettings[i];
				bool flag = chipTypeSettings2.chipType == chipType;
				chipTypeSettings2.obj.SetActive(flag);
				if (flag)
				{
					chipTypeSettings = chipTypeSettings2;
				}
			}
			if (chipTypeSettings != null)
			{
				chipTypeSettings.obj.SetActive(true);
				chipTypeSettings.obj.transform.localRotation = Quaternion.Euler(chipTypeSettings.rotation);
			}
		}

		public void ResetVisually()
		{
			if (this.chip != null && this.chip.slot != null)
			{
				base.transform.localPosition = this.chip.slot.localPositionOfCenter;
			}
		}

		public override void RemoveFromGame()
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}

		private void UpdateLook()
		{
			Match3Settings.ChipColorSettings colorSettings = Match3Settings.instance.GetColorSettings();
			if (colorSettings == null)
			{
				return;
			}
			for (int i = 0; i < this.boxSprites.Count; i++)
			{
				this.boxSprites[i].gameObject.SetActive(colorSettings.hasBoxes);
			}
		}

		private void Update()
		{
			bool isEditor = Application.isEditor;
		}

		[SerializeField]
		private List<SolidPieceRenderer.ChipTypeSettings> chipTypeSettings = new List<SolidPieceRenderer.ChipTypeSettings>();

		[SerializeField]
		private List<SpriteRenderer> boxSprites = new List<SpriteRenderer>();

		[SerializeField]
		private Transform rotator;

		[NonSerialized]
		private Chip chip;

		[Serializable]
		public class ChipTypeSettings
		{
			public ChipType chipType;

			public GameObject obj;

			public Vector3 rotation;
		}
	}
}
