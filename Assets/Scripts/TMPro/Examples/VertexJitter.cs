using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace TMPro.Examples
{
	public class VertexJitter : MonoBehaviour
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
			if (obj == this.m_TextComponent)
			{
				this.hasTextChanged = true;
			}
		}

		private IEnumerator AnimateVertexColors()
		{
			return new VertexJitter._003CAnimateVertexColors_003Ed__11(0)
			{
				_003C_003E4__this = this
			};
		}

		public float AngleMultiplier = 1f;

		public float SpeedMultiplier = 1f;

		public float CurveScale = 1f;

		private TMP_Text m_TextComponent;

		private bool hasTextChanged;

		private struct VertexAnim
		{
			public float angleRange;

			public float angle;

			public float speed;
		}

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
				VertexJitter vertexJitter = this._003C_003E4__this;
				switch (num)
				{
				case 0:
					this._003C_003E1__state = -1;
					vertexJitter.m_TextComponent.ForceMeshUpdate();
					this._003CtextInfo_003E5__2 = vertexJitter.m_TextComponent.textInfo;
					this._003CloopCount_003E5__3 = 0;
					vertexJitter.hasTextChanged = true;
					this._003CvertexAnim_003E5__4 = new VertexJitter.VertexAnim[1024];
					for (int i = 0; i < 1024; i++)
					{
						this._003CvertexAnim_003E5__4[i].angleRange = UnityEngine.Random.Range(10f, 25f);
						this._003CvertexAnim_003E5__4[i].speed = UnityEngine.Random.Range(1f, 3f);
					}
					this._003CcachedMeshInfo_003E5__5 = this._003CtextInfo_003E5__2.CopyMeshInfoVertexData();
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
				if (vertexJitter.hasTextChanged)
				{
					this._003CcachedMeshInfo_003E5__5 = this._003CtextInfo_003E5__2.CopyMeshInfoVertexData();
					vertexJitter.hasTextChanged = false;
				}
				int characterCount = this._003CtextInfo_003E5__2.characterCount;
				if (characterCount == 0)
				{
					this._003C_003E2__current = new WaitForSeconds(0.25f);
					this._003C_003E1__state = 1;
					return true;
				}
				for (int j = 0; j < characterCount; j++)
				{
					if (this._003CtextInfo_003E5__2.characterInfo[j].isVisible)
					{
						VertexJitter.VertexAnim vertexAnim = this._003CvertexAnim_003E5__4[j];
						int materialReferenceIndex = this._003CtextInfo_003E5__2.characterInfo[j].materialReferenceIndex;
						int vertexIndex = this._003CtextInfo_003E5__2.characterInfo[j].vertexIndex;
						Vector3[] vertices = this._003CcachedMeshInfo_003E5__5[materialReferenceIndex].vertices;
						Vector3 b = (vertices[vertexIndex] + vertices[vertexIndex + 2]) / 2f;
						Vector3[] vertices2 = this._003CtextInfo_003E5__2.meshInfo[materialReferenceIndex].vertices;
						vertices2[vertexIndex] = vertices[vertexIndex] - b;
						vertices2[vertexIndex + 1] = vertices[vertexIndex + 1] - b;
						vertices2[vertexIndex + 2] = vertices[vertexIndex + 2] - b;
						vertices2[vertexIndex + 3] = vertices[vertexIndex + 3] - b;
						vertexAnim.angle = Mathf.SmoothStep(-vertexAnim.angleRange, vertexAnim.angleRange, Mathf.PingPong((float)this._003CloopCount_003E5__3 / 25f * vertexAnim.speed, 1f));
						Matrix4x4 matrix4x = Matrix4x4.TRS(new Vector3(UnityEngine.Random.Range(-0.25f, 0.25f), UnityEngine.Random.Range(-0.25f, 0.25f), 0f) * vertexJitter.CurveScale, Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-5f, 5f) * vertexJitter.AngleMultiplier), Vector3.one);
						vertices2[vertexIndex] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex]);
						vertices2[vertexIndex + 1] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex + 1]);
						vertices2[vertexIndex + 2] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex + 2]);
						vertices2[vertexIndex + 3] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex + 3]);
						vertices2[vertexIndex] += b;
						vertices2[vertexIndex + 1] += b;
						vertices2[vertexIndex + 2] += b;
						vertices2[vertexIndex + 3] += b;
						this._003CvertexAnim_003E5__4[j] = vertexAnim;
					}
				}
				for (int k = 0; k < this._003CtextInfo_003E5__2.meshInfo.Length; k++)
				{
					this._003CtextInfo_003E5__2.meshInfo[k].mesh.vertices = this._003CtextInfo_003E5__2.meshInfo[k].vertices;
					vertexJitter.m_TextComponent.UpdateGeometry(this._003CtextInfo_003E5__2.meshInfo[k].mesh, k);
				}
				this._003CloopCount_003E5__3++;
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

			public VertexJitter _003C_003E4__this;

			private TMP_TextInfo _003CtextInfo_003E5__2;

			private int _003CloopCount_003E5__3;

			private VertexJitter.VertexAnim[] _003CvertexAnim_003E5__4;

			private TMP_MeshInfo[] _003CcachedMeshInfo_003E5__5;
		}
	}
}
