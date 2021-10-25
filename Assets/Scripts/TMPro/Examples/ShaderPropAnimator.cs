using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace TMPro.Examples
{
	public class ShaderPropAnimator : MonoBehaviour
	{
		private void Awake()
		{
			this.m_Renderer = base.GetComponent<Renderer>();
			this.m_Material = this.m_Renderer.material;
		}

		private void Start()
		{
			base.StartCoroutine(this.AnimateProperties());
		}

		private IEnumerator AnimateProperties()
		{
			return new ShaderPropAnimator._003CAnimateProperties_003Ed__6(0)
			{
				_003C_003E4__this = this
			};
		}

		private Renderer m_Renderer;

		private Material m_Material;

		public AnimationCurve GlowCurve;

		public float m_frame;

		private sealed class _003CAnimateProperties_003Ed__6 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CAnimateProperties_003Ed__6(int _003C_003E1__state)
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
				ShaderPropAnimator shaderPropAnimator = this._003C_003E4__this;
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
					shaderPropAnimator.m_frame = UnityEngine.Random.Range(0f, 1f);
				}
				float value = shaderPropAnimator.GlowCurve.Evaluate(shaderPropAnimator.m_frame);
				shaderPropAnimator.m_Material.SetFloat(ShaderUtilities.ID_GlowPower, value);
				shaderPropAnimator.m_frame += Time.deltaTime * UnityEngine.Random.Range(0.2f, 0.3f);
				this._003C_003E2__current = new WaitForEndOfFrame();
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

			public ShaderPropAnimator _003C_003E4__this;
		}
	}
}
