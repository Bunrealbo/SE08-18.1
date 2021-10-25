using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace TMPro.Examples
{
	public class VertexShakeA : MonoBehaviour
	{
		private void Awake()
		{
			this.m_TextComponent = base.GetComponent<TMP_Text>();
		}

		private void OnEnable()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(new Action<UnityEngine.Object>(this.ON_TEXT_CHANGED));
		}

		private void OnDisable()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(new Action<UnityEngine.Object>(this.ON_TEXT_CHANGED));
		}

		private void Start()
		{
			base.StartCoroutine(this.AnimateVertexColors());
		}

		private void ON_TEXT_CHANGED(UnityEngine.Object obj)
		{
			if (this.m_TextComponent)
			{
				this.hasTextChanged = true;
			}
		}

		private IEnumerator AnimateVertexColors()
		{
			return new VertexShakeA._003CAnimateVertexColors_003Ed__11(0)
			{
				_003C_003E4__this = this
			};
		}

		public float AngleMultiplier = 1f;

		public float SpeedMultiplier = 1f;

		public float ScaleMultiplier = 1f;

		public float RotationMultiplier = 1f;

		private TMP_Text m_TextComponent;

		private bool hasTextChanged;

		private sealed class _003CAnimateVertexColors_003Ed__11 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CAnimateVertexColors_003Ed__11(int _003C_003E1__state)
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
				VertexShakeA vertexShakeA = this._003C_003E4__this;
				switch (num)
				{
				case 0:
					this._003C_003E1__state = -1;
					vertexShakeA.m_TextComponent.ForceMeshUpdate();
					this._003CtextInfo_003E5__2 = vertexShakeA.m_TextComponent.textInfo;
					this._003CcopyOfVertices_003E5__3 = new Vector3[0][];
					vertexShakeA.hasTextChanged = true;
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
				if (vertexShakeA.hasTextChanged)
				{
					if (this._003CcopyOfVertices_003E5__3.Length < this._003CtextInfo_003E5__2.meshInfo.Length)
					{
						this._003CcopyOfVertices_003E5__3 = new Vector3[this._003CtextInfo_003E5__2.meshInfo.Length][];
					}
					for (int i = 0; i < this._003CtextInfo_003E5__2.meshInfo.Length; i++)
					{
						int num2 = this._003CtextInfo_003E5__2.meshInfo[i].vertices.Length;
						this._003CcopyOfVertices_003E5__3[i] = new Vector3[num2];
					}
					vertexShakeA.hasTextChanged = false;
				}
				if (this._003CtextInfo_003E5__2.characterCount == 0)
				{
					this._003C_003E2__current = new WaitForSeconds(0.25f);
					this._003C_003E1__state = 1;
					return true;
				}
				int lineCount = this._003CtextInfo_003E5__2.lineCount;
				for (int j = 0; j < lineCount; j++)
				{
					int firstCharacterIndex = this._003CtextInfo_003E5__2.lineInfo[j].firstCharacterIndex;
					int lastCharacterIndex = this._003CtextInfo_003E5__2.lineInfo[j].lastCharacterIndex;
					Vector3 b = (this._003CtextInfo_003E5__2.characterInfo[firstCharacterIndex].bottomLeft + this._003CtextInfo_003E5__2.characterInfo[lastCharacterIndex].topRight) / 2f;
					Quaternion q = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-0.25f, 0.25f) * vertexShakeA.RotationMultiplier);
					for (int k = firstCharacterIndex; k <= lastCharacterIndex; k++)
					{
						if (this._003CtextInfo_003E5__2.characterInfo[k].isVisible)
						{
							int materialReferenceIndex = this._003CtextInfo_003E5__2.characterInfo[k].materialReferenceIndex;
							int vertexIndex = this._003CtextInfo_003E5__2.characterInfo[k].vertexIndex;
							Vector3[] vertices = this._003CtextInfo_003E5__2.meshInfo[materialReferenceIndex].vertices;
							this._003CcopyOfVertices_003E5__3[materialReferenceIndex][vertexIndex] = vertices[vertexIndex] - b;
							this._003CcopyOfVertices_003E5__3[materialReferenceIndex][vertexIndex + 1] = vertices[vertexIndex + 1] - b;
							this._003CcopyOfVertices_003E5__3[materialReferenceIndex][vertexIndex + 2] = vertices[vertexIndex + 2] - b;
							this._003CcopyOfVertices_003E5__3[materialReferenceIndex][vertexIndex + 3] = vertices[vertexIndex + 3] - b;
							float d = UnityEngine.Random.Range(0.995f - 0.001f * vertexShakeA.ScaleMultiplier, 1.005f + 0.001f * vertexShakeA.ScaleMultiplier);
							Matrix4x4 matrix4x = Matrix4x4.TRS(Vector3.one, q, Vector3.one * d);
							this._003CcopyOfVertices_003E5__3[materialReferenceIndex][vertexIndex] = matrix4x.MultiplyPoint3x4(this._003CcopyOfVertices_003E5__3[materialReferenceIndex][vertexIndex]);
							this._003CcopyOfVertices_003E5__3[materialReferenceIndex][vertexIndex + 1] = matrix4x.MultiplyPoint3x4(this._003CcopyOfVertices_003E5__3[materialReferenceIndex][vertexIndex + 1]);
							this._003CcopyOfVertices_003E5__3[materialReferenceIndex][vertexIndex + 2] = matrix4x.MultiplyPoint3x4(this._003CcopyOfVertices_003E5__3[materialReferenceIndex][vertexIndex + 2]);
							this._003CcopyOfVertices_003E5__3[materialReferenceIndex][vertexIndex + 3] = matrix4x.MultiplyPoint3x4(this._003CcopyOfVertices_003E5__3[materialReferenceIndex][vertexIndex + 3]);
							this._003CcopyOfVertices_003E5__3[materialReferenceIndex][vertexIndex] += b;
							this._003CcopyOfVertices_003E5__3[materialReferenceIndex][vertexIndex + 1] += b;
							this._003CcopyOfVertices_003E5__3[materialReferenceIndex][vertexIndex + 2] += b;
							this._003CcopyOfVertices_003E5__3[materialReferenceIndex][vertexIndex + 3] += b;
						}
					}
				}
				for (int l = 0; l < this._003CtextInfo_003E5__2.meshInfo.Length; l++)
				{
					this._003CtextInfo_003E5__2.meshInfo[l].mesh.vertices = this._003CcopyOfVertices_003E5__3[l];
					vertexShakeA.m_TextComponent.UpdateGeometry(this._003CtextInfo_003E5__2.meshInfo[l].mesh, l);
				}
				this._003C_003E2__current = new WaitForSeconds(0.1f);
				this._003C_003E1__state = 2;
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

			public VertexShakeA _003C_003E4__this;

			private TMP_TextInfo _003CtextInfo_003E5__2;

			private Vector3[][] _003CcopyOfVertices_003E5__3;
		}
	}
}
