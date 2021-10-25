using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace TMPro.Examples
{
	public class TextConsoleSimulator : MonoBehaviour
	{
		private void Awake()
		{
			this.m_TextComponent = base.gameObject.GetComponent<TMP_Text>();
		}

		private void Start()
		{
			base.StartCoroutine(this.RevealCharacters(this.m_TextComponent));
		}

		private void OnEnable()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(new Action<UnityEngine.Object>(this.ON_TEXT_CHANGED));
		}

		private void OnDisable()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(new Action<UnityEngine.Object>(this.ON_TEXT_CHANGED));
		}

		private void ON_TEXT_CHANGED(UnityEngine.Object obj)
		{
			this.hasTextChanged = true;
		}

		private IEnumerator RevealCharacters(TMP_Text textComponent)
		{
			return new TextConsoleSimulator._003CRevealCharacters_003Ed__7(0)
			{
				_003C_003E4__this = this,
				textComponent = textComponent
			};
		}

		private IEnumerator RevealWords(TMP_Text textComponent)
		{
			return new TextConsoleSimulator._003CRevealWords_003Ed__8(0)
			{
				textComponent = textComponent
			};
		}

		private TMP_Text m_TextComponent;

		private bool hasTextChanged;

		private sealed class _003CRevealCharacters_003Ed__7 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CRevealCharacters_003Ed__7(int _003C_003E1__state)
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
				TextConsoleSimulator textConsoleSimulator = this._003C_003E4__this;
				switch (num)
				{
				case 0:
					this._003C_003E1__state = -1;
					this.textComponent.ForceMeshUpdate();
					this._003CtextInfo_003E5__2 = this.textComponent.textInfo;
					this._003CtotalVisibleCharacters_003E5__3 = this._003CtextInfo_003E5__2.characterCount;
					this._003CvisibleCount_003E5__4 = 0;
					break;
				case 1:
					this._003C_003E1__state = -1;
					this._003CvisibleCount_003E5__4 = 0;
					goto IL_B2;
				case 2:
					this._003C_003E1__state = -1;
					break;
				default:
					return false;
				}
				if (textConsoleSimulator.hasTextChanged)
				{
					this._003CtotalVisibleCharacters_003E5__3 = this._003CtextInfo_003E5__2.characterCount;
					textConsoleSimulator.hasTextChanged = false;
				}
				if (this._003CvisibleCount_003E5__4 > this._003CtotalVisibleCharacters_003E5__3)
				{
					this._003C_003E2__current = new WaitForSeconds(1f);
					this._003C_003E1__state = 1;
					return true;
				}
				IL_B2:
				this.textComponent.maxVisibleCharacters = this._003CvisibleCount_003E5__4;
				this._003CvisibleCount_003E5__4++;
				this._003C_003E2__current = null;
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

			public TMP_Text textComponent;

			public TextConsoleSimulator _003C_003E4__this;

			private TMP_TextInfo _003CtextInfo_003E5__2;

			private int _003CtotalVisibleCharacters_003E5__3;

			private int _003CvisibleCount_003E5__4;
		}

		private sealed class _003CRevealWords_003Ed__8 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CRevealWords_003Ed__8(int _003C_003E1__state)
			{
				this._003C_003E1__state = _003C_003E1__state;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			bool IEnumerator.MoveNext()
			{
				switch (this._003C_003E1__state)
				{
				case 0:
					this._003C_003E1__state = -1;
					this.textComponent.ForceMeshUpdate();
					this._003CtotalWordCount_003E5__2 = this.textComponent.textInfo.wordCount;
					this._003CtotalVisibleCharacters_003E5__3 = this.textComponent.textInfo.characterCount;
					this._003Ccounter_003E5__4 = 0;
					this._003CvisibleCount_003E5__5 = 0;
					break;
				case 1:
					this._003C_003E1__state = -1;
					goto IL_109;
				case 2:
					this._003C_003E1__state = -1;
					break;
				default:
					return false;
				}
				int num = this._003Ccounter_003E5__4 % (this._003CtotalWordCount_003E5__2 + 1);
				if (num == 0)
				{
					this._003CvisibleCount_003E5__5 = 0;
				}
				else if (num < this._003CtotalWordCount_003E5__2)
				{
					this._003CvisibleCount_003E5__5 = this.textComponent.textInfo.wordInfo[num - 1].lastCharacterIndex + 1;
				}
				else if (num == this._003CtotalWordCount_003E5__2)
				{
					this._003CvisibleCount_003E5__5 = this._003CtotalVisibleCharacters_003E5__3;
				}
				this.textComponent.maxVisibleCharacters = this._003CvisibleCount_003E5__5;
				if (this._003CvisibleCount_003E5__5 >= this._003CtotalVisibleCharacters_003E5__3)
				{
					this._003C_003E2__current = new WaitForSeconds(1f);
					this._003C_003E1__state = 1;
					return true;
				}
				IL_109:
				this._003Ccounter_003E5__4++;
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

			public TMP_Text textComponent;

			private int _003CtotalWordCount_003E5__2;

			private int _003CtotalVisibleCharacters_003E5__3;

			private int _003Ccounter_003E5__4;

			private int _003CvisibleCount_003E5__5;
		}
	}
}
