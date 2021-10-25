using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace TMPro.Examples
{
	public class TeleType : MonoBehaviour
	{
		private void Awake()
		{
			this.m_textMeshPro = base.GetComponent<TMP_Text>();
			this.m_textMeshPro.text = this.label01;
			this.m_textMeshPro.enableWordWrapping = true;
			this.m_textMeshPro.alignment = TextAlignmentOptions.Top;
		}

		private IEnumerator Start()
		{
			return new TeleType._003CStart_003Ed__4(0)
			{
				_003C_003E4__this = this
			};
		}

		private string label01 = "Example <sprite=2> of using <sprite=7> <#ffa000>Graphics Inline</color> <sprite=5> with Text in <font=\"Bangers SDF\" material=\"Bangers SDF - Drop Shadow\">TextMesh<#40a0ff>Pro</color></font><sprite=0> and Unity<sprite=1>";

		private string label02 = "Example <sprite=2> of using <sprite=7> <#ffa000>Graphics Inline</color> <sprite=5> with Text in <font=\"Bangers SDF\" material=\"Bangers SDF - Drop Shadow\">TextMesh<#40a0ff>Pro</color></font><sprite=0> and Unity<sprite=2>";

		private TMP_Text m_textMeshPro;

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
				TeleType teleType = this._003C_003E4__this;
				switch (num)
				{
				case 0:
					this._003C_003E1__state = -1;
					teleType.m_textMeshPro.ForceMeshUpdate();
					this._003CtotalVisibleCharacters_003E5__2 = teleType.m_textMeshPro.textInfo.characterCount;
					this._003Ccounter_003E5__3 = 0;
					break;
				case 1:
					this._003C_003E1__state = -1;
					teleType.m_textMeshPro.text = teleType.label02;
					this._003C_003E2__current = new WaitForSeconds(1f);
					this._003C_003E1__state = 2;
					return true;
				case 2:
					this._003C_003E1__state = -1;
					teleType.m_textMeshPro.text = teleType.label01;
					this._003C_003E2__current = new WaitForSeconds(1f);
					this._003C_003E1__state = 3;
					return true;
				case 3:
					this._003C_003E1__state = -1;
					goto IL_105;
				case 4:
					this._003C_003E1__state = -1;
					break;
				default:
					return false;
				}
				int num2 = this._003Ccounter_003E5__3 % (this._003CtotalVisibleCharacters_003E5__2 + 1);
				teleType.m_textMeshPro.maxVisibleCharacters = num2;
				if (num2 >= this._003CtotalVisibleCharacters_003E5__2)
				{
					this._003C_003E2__current = new WaitForSeconds(1f);
					this._003C_003E1__state = 1;
					return true;
				}
				IL_105:
				this._003Ccounter_003E5__3++;
				this._003C_003E2__current = new WaitForSeconds(0.05f);
				this._003C_003E1__state = 4;
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

			public TeleType _003C_003E4__this;

			private int _003CtotalVisibleCharacters_003E5__2;

			private int _003Ccounter_003E5__3;
		}
	}
}
