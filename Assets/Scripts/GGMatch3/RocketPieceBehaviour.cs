using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GGMatch3
{
	public class RocketPieceBehaviour : MonoBehaviour
	{
		public void SetDirection(IntVector2 direction)
		{
			Vector2 direction2 = new Vector2((float)direction.x, (float)direction.y);
			this.SetDirection(direction2);
		}

		public void SetDirection(Vector2 direction)
		{
			float num = Vector3.Angle(Vector3.up, direction);
			if (direction.x < 0f || direction.y > 0f)
			{
				num += 180f;
			}
			this.rotatorTransform.localRotation = Quaternion.AngleAxis(num, Vector3.forward);
			GGUtil.SetActive(this.verticalWidgets, Mathf.Abs(direction.y) > 0f);
			GGUtil.SetActive(this.horizontalWidgets, Mathf.Abs(direction.x) > 0f);
		}

		public void Init()
		{
			base.gameObject.SetActive(true);
		}

		public Vector3 localPosition
		{
			get
			{
				return base.transform.localPosition;
			}
			set
			{
				base.transform.localPosition = value;
			}
		}

		public Vector3 localScale
		{
			get
			{
				return base.transform.localScale;
			}
			set
			{
				base.transform.localScale = value;
			}
		}

		public void RemoveFromGameAfter(float duration)
		{
			GGUtil.Hide(this.rocketTransform);
			base.StartCoroutine(this.DoRemoveFromGameAfter(duration));
		}

		private IEnumerator DoRemoveFromGameAfter(float duration)
		{
			return new RocketPieceBehaviour._003CDoRemoveFromGameAfter_003Ed__14(0)
			{
				_003C_003E4__this = this,
				duration = duration
			};
		}

		public void RemoveFromGame()
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}

		[SerializeField]
		private Transform rotatorTransform;

		[SerializeField]
		private Transform rocketTransform;

		[SerializeField]
		private List<Transform> verticalWidgets = new List<Transform>();

		[SerializeField]
		private List<Transform> horizontalWidgets = new List<Transform>();

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
				RocketPieceBehaviour rocketPieceBehaviour = this._003C_003E4__this;
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
					GGUtil.Hide(rocketPieceBehaviour.rocketTransform);
				}
				if (this._003Ctime_003E5__2 >= this.duration)
				{
					rocketPieceBehaviour.RemoveFromGame();
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

			public RocketPieceBehaviour _003C_003E4__this;

			public float duration;

			private float _003Ctime_003E5__2;
		}
	}
}
