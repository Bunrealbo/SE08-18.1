using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CarNut : MonoBehaviour
{
	public void Init()
	{
		this.screwInTransform.localRotation = Quaternion.identity;
	}

	public void SetRotation(Quaternion rotation)
	{
		base.transform.rotation = rotation * Quaternion.Euler(0f, 90f, 90f);
	}

	public float nutSize
	{
		get
		{
			return Vector3.Distance(this.headTransfrom.position, this.tailTransfrom.position);
		}
	}

	public void SetRotateIn(Vector3 fromPosition, Vector3 toPosition, float n)
	{
		this.screwInTransform.localRotation = Quaternion.identity;
		base.transform.position = fromPosition;
		float num = Vector3.Distance(fromPosition, toPosition) / this.nutSize * this.rotationsTillLength;
		Vector3 position = Vector3.Lerp(fromPosition, toPosition, n);
		float angle = Mathf.Lerp(0f, num * 360f, n);
		this.screwInTransform.localRotation = Quaternion.AngleAxis(angle, Vector3.up);
		base.transform.position = position;
	}

	public IEnumerator DoRotateIn(Vector3 fromPosition, Vector3 toPosition, float duration)
	{
		return new CarNut._003CDoRotateIn_003Ed__9(0)
		{
			_003C_003E4__this = this,
			fromPosition = fromPosition,
			toPosition = toPosition,
			duration = duration
		};
	}

	[SerializeField]
	private Transform screwInTransform;

	[SerializeField]
	private Transform tailTransfrom;

	[SerializeField]
	private Transform headTransfrom;

	[SerializeField]
	private float rotationsTillLength = 20f;

	private sealed class _003CDoRotateIn_003Ed__9 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoRotateIn_003Ed__9(int _003C_003E1__state)
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
			CarNut carNut = this._003C_003E4__this;
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
				carNut.screwInTransform.localRotation = Quaternion.identity;
				carNut.transform.position = this.fromPosition;
				this._003Ctime_003E5__2 = 0f;
				this._003Crotations_003E5__3 = Vector3.Distance(this.fromPosition, this.toPosition) / carNut.nutSize * carNut.rotationsTillLength;
			}
			if (this._003Ctime_003E5__2 > this.duration)
			{
				return false;
			}
			this._003Ctime_003E5__2 += Time.deltaTime;
			float t = Mathf.InverseLerp(0f, this.duration, this._003Ctime_003E5__2);
			Vector3 position = Vector3.Lerp(this.fromPosition, this.toPosition, t);
			float angle = Mathf.Lerp(0f, this._003Crotations_003E5__3 * 360f, t);
			carNut.screwInTransform.localRotation = Quaternion.AngleAxis(angle, Vector3.up);
			carNut.transform.position = position;
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

		public CarNut _003C_003E4__this;

		public Vector3 fromPosition;

		public Vector3 toPosition;

		public float duration;

		private float _003Ctime_003E5__2;

		private float _003Crotations_003E5__3;
	}
}
