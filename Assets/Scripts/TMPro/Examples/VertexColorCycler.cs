using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace TMPro.Examples
{
	public class VertexColorCycler : MonoBehaviour
	{
		private void Awake()
		{
			this.m_TextComponent = base.GetComponent<TMP_Text>();
		}

		private void Start()
		{
			base.StartCoroutine(this.AnimateVertexColors());
		}

		private IEnumerator AnimateVertexColors()
		{
			return new VertexColorCycler._003CAnimateVertexColors_003Ed__3(0)
			{
				_003C_003E4__this = this
			};
		}

		private TMP_Text m_TextComponent;

		private sealed class _003CAnimateVertexColors_003Ed__3 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CAnimateVertexColors_003Ed__3(int _003C_003E1__state)
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
				VertexColorCycler vertexColorCycler = this._003C_003E4__this;
				switch (num)
				{
				case 0:
				{
					this._003C_003E1__state = -1;
					this._003CtextInfo_003E5__2 = vertexColorCycler.m_TextComponent.textInfo;
					this._003CcurrentCharacter_003E5__3 = 0;
					Color32 color = vertexColorCycler.m_TextComponent.color;
					break;
				}
				case 1:
					this._003C_003E1__state = -1;
					break;
				case 2:
					this._003C_003E1__state = -1;
					break;
				default:
					return false;
				}
				int characterCount = this._003CtextInfo_003E5__2.characterCount;
				if (characterCount == 0)
				{
					this._003C_003E2__current = new WaitForSeconds(0.25f);
					this._003C_003E1__state = 1;
					return true;
				}
				int materialReferenceIndex = this._003CtextInfo_003E5__2.characterInfo[this._003CcurrentCharacter_003E5__3].materialReferenceIndex;
				Color32[] colors = this._003CtextInfo_003E5__2.meshInfo[materialReferenceIndex].colors32;
				int vertexIndex = this._003CtextInfo_003E5__2.characterInfo[this._003CcurrentCharacter_003E5__3].vertexIndex;
				if (this._003CtextInfo_003E5__2.characterInfo[this._003CcurrentCharacter_003E5__3].isVisible)
				{
					Color32 color = new Color32((byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), byte.MaxValue);
					colors[vertexIndex] = color;
					colors[vertexIndex + 1] = color;
					colors[vertexIndex + 2] = color;
					colors[vertexIndex + 3] = color;
					vertexColorCycler.m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
				}
				this._003CcurrentCharacter_003E5__3 = (this._003CcurrentCharacter_003E5__3 + 1) % characterCount;
				this._003C_003E2__current = new WaitForSeconds(0.05f);
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

			public VertexColorCycler _003C_003E4__this;

			private TMP_TextInfo _003CtextInfo_003E5__2;

			private int _003CcurrentCharacter_003E5__3;
		}
	}
}
