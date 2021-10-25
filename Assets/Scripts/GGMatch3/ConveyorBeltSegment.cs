using System;
using UnityEngine;

namespace GGMatch3
{
	public class ConveyorBeltSegment : MonoBehaviour
	{
		public void SetTile(float tile)
		{
			this.tiledSegment.meshRenderer.sharedMaterial.mainTextureOffset = new Vector2(0f, -tile);
		}

		public void SetColor(Color color)
		{
			this.tiledSegment.meshRenderer.sharedMaterial.color = color;
		}

		public Color GetColor()
		{
			return this.tiledSegment.meshRenderer.sharedMaterial.color;
		}

		public void Init(int numSlots, Vector2 slotSize)
		{
			float num = slotSize.x * 0.5f;
			float num2 = slotSize.y * 0.5f;
			this.tiledSegment.ClearAndInit(1);
			Rect pos = default(Rect);
			pos.xMin = -num;
			pos.xMax = num;
			pos.yMin = -num2;
			pos.yMax = ((float)numSlots - 0.5f) * slotSize.y;
			Rect uv = new Rect(0f, 0f, 1f, (float)numSlots);
			this.tiledSegment.DrawRectangle(pos, uv);
			this.tiledSegment.CopyToMesh();
			if (this.start != null)
			{
				this.SetScaleToMatch(this.start);
				Bounds bounds = this.start.bounds;
				this.start.transform.localPosition = new Vector3(0f, -num2 + bounds.extents.y, 0f);
			}
			if (this.end != null)
			{
				this.SetScaleToMatch(this.end);
				Bounds bounds2 = this.end.bounds;
				this.end.transform.localPosition = new Vector3(0f, (float)numSlots * slotSize.y - bounds2.extents.y, 0f);
			}
		}

		private void SetScaleToMatch(SpriteRenderer sprite)
		{
			float num = 1f / sprite.bounds.extents.x;
			sprite.transform.localScale = Vector3.Scale(sprite.transform.localScale, new Vector3(num, num, 1f));
		}

		[SerializeField]
		private TiledSpriteRenderer tiledSegment;

		[SerializeField]
		private SpriteRenderer start;

		[SerializeField]
		private SpriteRenderer end;
	}
}
