using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace TMPro.Examples
{
	public class WarpTextExample : MonoBehaviour
	{
		private void Awake()
		{
			this.m_TextComponent = base.gameObject.GetComponent<TMP_Text>();
		}

		private void Start()
		{
			base.StartCoroutine(this.WarpText());
		}

		private AnimationCurve CopyAnimationCurve(AnimationCurve curve)
		{
			return new AnimationCurve
			{
				keys = curve.keys
			};
		}

		private IEnumerator WarpText()
		{
			return new WarpTextExample._003CWarpText_003Ed__8(0)
			{
				_003C_003E4__this = this
			};
		}

		private TMP_Text m_TextComponent;

		public AnimationCurve VertexCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.25f, 2f),
			new Keyframe(0.5f, 0f),
			new Keyframe(0.75f, 2f),
			new Keyframe(1f, 0f)
		});

		public float AngleMultiplier = 1f;

		public float SpeedMultiplier = 1f;

		public float CurveScale = 1f;

		private sealed class _003CWarpText_003Ed__8 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CWarpText_003Ed__8(int _003C_003E1__state)
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
				WarpTextExample warpTextExample = this._003C_003E4__this;
				switch (num)
				{
				case 0:
					this._003C_003E1__state = -1;
					warpTextExample.VertexCurve.preWrapMode = WrapMode.Once;
					warpTextExample.VertexCurve.postWrapMode = WrapMode.Once;
					warpTextExample.m_TextComponent.havePropertiesChanged = true;
					warpTextExample.CurveScale *= 10f;
					this._003Cold_CurveScale_003E5__2 = warpTextExample.CurveScale;
					this._003Cold_curve_003E5__3 = warpTextExample.CopyAnimationCurve(warpTextExample.VertexCurve);
					break;
				case 1:
					this._003C_003E1__state = -1;
					break;
				case 2:
					this._003C_003E1__state = -1;
					break;
				default:
					return false;
				}
				while (warpTextExample.m_TextComponent.havePropertiesChanged || this._003Cold_CurveScale_003E5__2 != warpTextExample.CurveScale || this._003Cold_curve_003E5__3.keys[1].value != warpTextExample.VertexCurve.keys[1].value)
				{
					this._003Cold_CurveScale_003E5__2 = warpTextExample.CurveScale;
					this._003Cold_curve_003E5__3 = warpTextExample.CopyAnimationCurve(warpTextExample.VertexCurve);
					warpTextExample.m_TextComponent.ForceMeshUpdate();
					TMP_TextInfo textInfo = warpTextExample.m_TextComponent.textInfo;
					int characterCount = textInfo.characterCount;
					if (characterCount != 0)
					{
						float x = warpTextExample.m_TextComponent.bounds.min.x;
						float x2 = warpTextExample.m_TextComponent.bounds.max.x;
						for (int i = 0; i < characterCount; i++)
						{
							if (textInfo.characterInfo[i].isVisible)
							{
								int vertexIndex = textInfo.characterInfo[i].vertexIndex;
								int materialReferenceIndex = textInfo.characterInfo[i].materialReferenceIndex;
								Vector3[] vertices = textInfo.meshInfo[materialReferenceIndex].vertices;
								Vector3 vector = new Vector2((vertices[vertexIndex].x + vertices[vertexIndex + 2].x) / 2f, textInfo.characterInfo[i].baseLine);
								vertices[vertexIndex] += -vector;
								vertices[vertexIndex + 1] += -vector;
								vertices[vertexIndex + 2] += -vector;
								vertices[vertexIndex + 3] += -vector;
								float num2 = (vector.x - x) / (x2 - x);
								float num3 = num2 + 0.0001f;
								float y = warpTextExample.VertexCurve.Evaluate(num2) * warpTextExample.CurveScale;
								float y2 = warpTextExample.VertexCurve.Evaluate(num3) * warpTextExample.CurveScale;
								Vector3 lhs = new Vector3(1f, 0f, 0f);
								Vector3 rhs = new Vector3(num3 * (x2 - x) + x, y2) - new Vector3(vector.x, y);
								float num4 = Mathf.Acos(Vector3.Dot(lhs, rhs.normalized)) * 57.29578f;
								float z = (Vector3.Cross(lhs, rhs).z > 0f) ? num4 : (360f - num4);
								Matrix4x4 matrix4x = Matrix4x4.TRS(new Vector3(0f, y, 0f), Quaternion.Euler(0f, 0f, z), Vector3.one);
								vertices[vertexIndex] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex]);
								vertices[vertexIndex + 1] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex + 1]);
								vertices[vertexIndex + 2] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex + 2]);
								vertices[vertexIndex + 3] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex + 3]);
								vertices[vertexIndex] += vector;
								vertices[vertexIndex + 1] += vector;
								vertices[vertexIndex + 2] += vector;
								vertices[vertexIndex + 3] += vector;
							}
						}
						warpTextExample.m_TextComponent.UpdateVertexData();
						this._003C_003E2__current = new WaitForSeconds(0.025f);
						this._003C_003E1__state = 2;
						return true;
					}
				}
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

			public WarpTextExample _003C_003E4__this;

			private float _003Cold_CurveScale_003E5__2;

			private AnimationCurve _003Cold_curve_003E5__3;
		}
	}
}
