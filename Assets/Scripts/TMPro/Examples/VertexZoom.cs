using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace TMPro.Examples
{
	public class VertexZoom : MonoBehaviour
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
			return new VertexZoom._003CAnimateVertexColors_003Ed__10(0)
			{
				_003C_003E4__this = this
			};
		}

		public float AngleMultiplier = 1f;

		public float SpeedMultiplier = 1f;

		public float CurveScale = 1f;

		private TMP_Text m_TextComponent;

		private bool hasTextChanged;

		private sealed class _003C_003Ec__DisplayClass10_0
		{
			internal int _003CAnimateVertexColors_003Eb__0(int a, int b)
			{
				return this.modifiedCharScale[a].CompareTo(this.modifiedCharScale[b]);
			}

			public List<float> modifiedCharScale;

			public Comparison<int> _003C_003E9__0;
		}

		private sealed class _003CAnimateVertexColors_003Ed__10 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CAnimateVertexColors_003Ed__10(int _003C_003E1__state)
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
				VertexZoom vertexZoom = this._003C_003E4__this;
				switch (num)
				{
				case 0:
					this._003C_003E1__state = -1;
					this._003C_003E8__1 = new VertexZoom._003C_003Ec__DisplayClass10_0();
					vertexZoom.m_TextComponent.ForceMeshUpdate();
					this._003CtextInfo_003E5__2 = vertexZoom.m_TextComponent.textInfo;
					this._003CcachedMeshInfoVertexData_003E5__3 = this._003CtextInfo_003E5__2.CopyMeshInfoVertexData();
					this._003C_003E8__1.modifiedCharScale = new List<float>();
					this._003CscaleSortingOrder_003E5__4 = new List<int>();
					vertexZoom.hasTextChanged = true;
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
				if (vertexZoom.hasTextChanged)
				{
					this._003CcachedMeshInfoVertexData_003E5__3 = this._003CtextInfo_003E5__2.CopyMeshInfoVertexData();
					vertexZoom.hasTextChanged = false;
				}
				int characterCount = this._003CtextInfo_003E5__2.characterCount;
				if (characterCount == 0)
				{
					this._003C_003E2__current = new WaitForSeconds(0.25f);
					this._003C_003E1__state = 1;
					return true;
				}
				this._003C_003E8__1.modifiedCharScale.Clear();
				this._003CscaleSortingOrder_003E5__4.Clear();
				for (int i = 0; i < characterCount; i++)
				{
					if (this._003CtextInfo_003E5__2.characterInfo[i].isVisible)
					{
						int materialReferenceIndex = this._003CtextInfo_003E5__2.characterInfo[i].materialReferenceIndex;
						int vertexIndex = this._003CtextInfo_003E5__2.characterInfo[i].vertexIndex;
						Vector3[] vertices = this._003CcachedMeshInfoVertexData_003E5__3[materialReferenceIndex].vertices;
						Vector3 b = (vertices[vertexIndex] + vertices[vertexIndex + 2]) / 2f;
						Vector3[] vertices2 = this._003CtextInfo_003E5__2.meshInfo[materialReferenceIndex].vertices;
						vertices2[vertexIndex] = vertices[vertexIndex] - b;
						vertices2[vertexIndex + 1] = vertices[vertexIndex + 1] - b;
						vertices2[vertexIndex + 2] = vertices[vertexIndex + 2] - b;
						vertices2[vertexIndex + 3] = vertices[vertexIndex + 3] - b;
						float num2 = UnityEngine.Random.Range(1f, 1.5f);
						this._003C_003E8__1.modifiedCharScale.Add(num2);
						this._003CscaleSortingOrder_003E5__4.Add(this._003C_003E8__1.modifiedCharScale.Count - 1);
						Matrix4x4 matrix4x = Matrix4x4.TRS(new Vector3(0f, 0f, 0f), Quaternion.identity, Vector3.one * num2);
						vertices2[vertexIndex] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex]);
						vertices2[vertexIndex + 1] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex + 1]);
						vertices2[vertexIndex + 2] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex + 2]);
						vertices2[vertexIndex + 3] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex + 3]);
						vertices2[vertexIndex] += b;
						vertices2[vertexIndex + 1] += b;
						vertices2[vertexIndex + 2] += b;
						vertices2[vertexIndex + 3] += b;
						Vector2[] uvs = this._003CcachedMeshInfoVertexData_003E5__3[materialReferenceIndex].uvs0;
						Vector2[] uvs2 = this._003CtextInfo_003E5__2.meshInfo[materialReferenceIndex].uvs0;
						uvs2[vertexIndex] = uvs[vertexIndex];
						uvs2[vertexIndex + 1] = uvs[vertexIndex + 1];
						uvs2[vertexIndex + 2] = uvs[vertexIndex + 2];
						uvs2[vertexIndex + 3] = uvs[vertexIndex + 3];
						Color32[] colors = this._003CcachedMeshInfoVertexData_003E5__3[materialReferenceIndex].colors32;
						Color32[] colors2 = this._003CtextInfo_003E5__2.meshInfo[materialReferenceIndex].colors32;
						colors2[vertexIndex] = colors[vertexIndex];
						colors2[vertexIndex + 1] = colors[vertexIndex + 1];
						colors2[vertexIndex + 2] = colors[vertexIndex + 2];
						colors2[vertexIndex + 3] = colors[vertexIndex + 3];
					}
				}
				for (int j = 0; j < this._003CtextInfo_003E5__2.meshInfo.Length; j++)
				{
					List<int> list = this._003CscaleSortingOrder_003E5__4;
					Comparison<int> comparison;
					if ((comparison = this._003C_003E8__1._003C_003E9__0) == null)
					{
						comparison = (this._003C_003E8__1._003C_003E9__0 = new Comparison<int>(this._003C_003E8__1._003CAnimateVertexColors_003Eb__0));
					}
					list.Sort(comparison);
					this._003CtextInfo_003E5__2.meshInfo[j].SortGeometry(this._003CscaleSortingOrder_003E5__4);
					this._003CtextInfo_003E5__2.meshInfo[j].mesh.vertices = this._003CtextInfo_003E5__2.meshInfo[j].vertices;
					this._003CtextInfo_003E5__2.meshInfo[j].mesh.uv = this._003CtextInfo_003E5__2.meshInfo[j].uvs0;
					this._003CtextInfo_003E5__2.meshInfo[j].mesh.colors32 = this._003CtextInfo_003E5__2.meshInfo[j].colors32;
					vertexZoom.m_TextComponent.UpdateGeometry(this._003CtextInfo_003E5__2.meshInfo[j].mesh, j);
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

			public VertexZoom _003C_003E4__this;

			private VertexZoom._003C_003Ec__DisplayClass10_0 _003C_003E8__1;

			private TMP_TextInfo _003CtextInfo_003E5__2;

			private TMP_MeshInfo[] _003CcachedMeshInfoVertexData_003E5__3;

			private List<int> _003CscaleSortingOrder_003E5__4;
		}
	}
}
