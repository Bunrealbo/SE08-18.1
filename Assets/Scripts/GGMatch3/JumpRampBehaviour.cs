using System;
using UnityEngine;

namespace GGMatch3
{
	public class JumpRampBehaviour : MonoBehaviour
	{
		public void Init(Vector3 position, IntVector2 direction)
		{
			base.transform.localPosition = position;
			Quaternion localRotation = Quaternion.AngleAxis(GGUtil.VisualRotationAngleUpAxis(direction.ToVector3()) - 90f, Vector3.back);
			this.rotator.localRotation = localRotation;
			GGUtil.SetActive(this, true);
		}

		private void Update()
		{
			if (this.spriteMesh == null)
			{
				return;
			}
			this.spriteOffset += Time.deltaTime * Match3Settings.instance.exitScrollSpeed;
			this.spriteOffset = Mathf.Repeat(this.spriteOffset, 1f);
			this.spriteMesh.sharedMaterial.mainTextureOffset = new Vector2(0f, this.spriteOffset);
		}

		[SerializeField]
		private Transform rotator;

		[SerializeField]
		private MeshRenderer spriteMesh;

		private float spriteOffset;
	}
}
