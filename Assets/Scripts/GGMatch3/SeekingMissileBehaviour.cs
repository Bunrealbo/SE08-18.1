using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GGMatch3
{
	public class SeekingMissileBehaviour : MonoBehaviour
	{
		public void SetDirection(IntVector2 direction)
		{
			Vector2 direction2 = new Vector2((float)direction.x, (float)direction.y);
			this.SetDirection(direction2);
		}

		public void SetDirection(Vector2 direction)
		{
			float angle = GGUtil.SignedAngle(Vector2.down, direction);
			this.rotatorTransform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}

		public void Init()
		{
			base.gameObject.SetActive(true);
			this.trailRenderer.sortingLayerID = this.sortingLayer.sortingLayerId;
			this.trailRenderer.sortingOrder = this.sortingLayer.sortingOrder;
		}

		public Vector3 localScale
		{
			set
			{
				base.transform.localScale = value;
			}
		}

		public Vector3 localPosition
		{
			get
			{
				return base.transform.localPosition;
			}
			set
			{
				if (float.IsNaN(value.x) || float.IsNaN(value.y) || float.IsNaN(value.z))
				{
					return;
				}
				base.transform.localPosition = value;
			}
		}

		public void RemoveFromGame()
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}

		public void RemoveFromGameAfter(float duration)
		{
			GGUtil.Hide(this.rocketTransform);
			base.StartCoroutine(this.DoRemoveFromGameAfter(duration));
		}

		private IEnumerator DoRemoveFromGameAfter(float duration)
		{
			return new SeekingMissileBehaviour._003CDoRemoveFromGameAfter_003Ed__14(0)
			{
				_003C_003E4__this = this,
				duration = duration
			};
		}

		public void ClearTrail()
		{
			this.trailRenderer.Clear();
		}

		[SerializeField]
		private Transform rotatorTransform;

		[SerializeField]
		private TrailRenderer trailRenderer;

		[SerializeField]
		private SpriteSortingSettings sortingLayer = new SpriteSortingSettings();

		[SerializeField]
		private Transform rocketTransform;

		private sealed class _003CDoRemoveFromGameAfter_003Ed__14 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CDoRemoveFromGameAfter_003Ed__14(int _003C_003E1__state)
			{
				this._003C_003E1__state = _003C_003E1__state;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			bool IEnumerator.MoveNext()
			{
				int num = this._003C_003E1__state;
				SeekingMissileBehaviour seekingMissileBehaviour = this._003C_003E4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					this._003C_003E1__state = -1;
				}
				else
				{
					this._003C_003E1__state = -1;
					this._003Ctime_003E5__2 = 0f;
					GGUtil.Hide(seekingMissileBehaviour.rocketTransform);
				}
				if (this._003Ctime_003E5__2 >= this.duration)
				{
					seekingMissileBehaviour.RemoveFromGame();
					return false;
				}
				this._003Ctime_003E5__2 += Time.deltaTime;
				this._003C_003E2__current = null;
				this._003C_003E1__state = 1;
				return true;
			}

			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this._003C_003E2__current;
				}
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this._003C_003E2__current;
				}
			}

			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public SeekingMissileBehaviour _003C_003E4__this;

			public float duration;

			private float _003Ctime_003E5__2;
		}
	}
}
