using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class EnvMapAnimator : MonoBehaviour
{
	private void Awake()
	{
		this.m_textMeshPro = base.GetComponent<TMP_Text>();
		this.m_material = this.m_textMeshPro.fontSharedMaterial;
	}

	private IEnumerator Start()
	{
		return new EnvMapAnimator._003CStart_003Ed__4(0)
		{
			_003C_003E4__this = this
		};
	}

	public Vector3 RotationSpeeds;

	private TMP_Text m_textMeshPro;

	private Material m_material;

	private sealed class _003CStart_003Ed__4 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CStart_003Ed__4(int _003C_003E1__state)
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
			EnvMapAnimator envMapAnimator = this._003C_003E4__this;
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
				this._003Cmatrix_003E5__2 = default(Matrix4x4);
			}
			this._003Cmatrix_003E5__2.SetTRS(Vector3.zero, Quaternion.Euler(Time.time * envMapAnimator.RotationSpeeds.x, Time.time * envMapAnimator.RotationSpeeds.y, Time.time * envMapAnimator.RotationSpeeds.z), Vector3.one);
			envMapAnimator.m_material.SetMatrix("_EnvMatrix", this._003Cmatrix_003E5__2);
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

		public EnvMapAnimator _003C_003E4__this;

		private Matrix4x4 _003Cmatrix_003E5__2;
	}
}
