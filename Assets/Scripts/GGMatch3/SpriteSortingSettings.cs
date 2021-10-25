using System;
using UnityEngine;

namespace GGMatch3
{
	[Serializable]
	public class SpriteSortingSettings
	{
		public void Set(SpriteRenderer spriteRenderer)
		{
			if (spriteRenderer == null)
			{
				return;
			}
			spriteRenderer.sortingLayerID = this.sortingLayerId;
			spriteRenderer.sortingOrder = this.sortingOrder;
		}

		public void Set(SkinnedMeshRenderer meshRenderer)
		{
			meshRenderer.sortingLayerID = this.sortingLayerId;
			meshRenderer.sortingOrder = this.sortingOrder;
		}

		public void Set(MeshRenderer meshRenderer)
		{
			meshRenderer.sortingLayerID = this.sortingLayerId;
			meshRenderer.sortingOrder = this.sortingOrder;
		}

		public int sortingLayerId;

		public int sortingOrder;
	}
}
