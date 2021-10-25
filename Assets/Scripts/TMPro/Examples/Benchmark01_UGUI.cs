using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

namespace TMPro.Examples
{
	public class Benchmark01_UGUI : MonoBehaviour
	{
		private IEnumerator Start()
		{
			return new Benchmark01_UGUI._003CStart_003Ed__10(0)
			{
				_003C_003E4__this = this
			};
		}

		public int BenchmarkType;

		public Canvas canvas;

		public TMP_FontAsset TMProFont;

		public Font TextMeshFont;

		private TextMeshProUGUI m_textMeshPro;

		private Text m_textMesh;

		private const string label01 = "The <#0050FF>count is: </color>";

		private const string label02 = "The <color=#0050FF>count is: </color>";

		private Material m_material01;

		private Material m_material02;

		private sealed class _003CStart_003Ed__10 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CStart_003Ed__10(int _003C_003E1__state)
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
				Benchmark01_UGUI benchmark01_UGUI = this._003C_003E4__this;
				switch (num)
				{
				case 0:
					this._003C_003E1__state = -1;
					if (benchmark01_UGUI.BenchmarkType == 0)
					{
						benchmark01_UGUI.m_textMeshPro = benchmark01_UGUI.gameObject.AddComponent<TextMeshProUGUI>();
						if (benchmark01_UGUI.TMProFont != null)
						{
							benchmark01_UGUI.m_textMeshPro.font = benchmark01_UGUI.TMProFont;
						}
						benchmark01_UGUI.m_textMeshPro.fontSize = 48f;
						benchmark01_UGUI.m_textMeshPro.alignment = TextAlignmentOptions.Center;
						benchmark01_UGUI.m_textMeshPro.extraPadding = true;
						benchmark01_UGUI.m_material01 = benchmark01_UGUI.m_textMeshPro.font.material;
						benchmark01_UGUI.m_material02 = Resources.Load<Material>("Fonts & Materials/LiberationSans SDF - BEVEL");
					}
					else if (benchmark01_UGUI.BenchmarkType == 1)
					{
						benchmark01_UGUI.m_textMesh = benchmark01_UGUI.gameObject.AddComponent<Text>();
						if (benchmark01_UGUI.TextMeshFont != null)
						{
							benchmark01_UGUI.m_textMesh.font = benchmark01_UGUI.TextMeshFont;
						}
						benchmark01_UGUI.m_textMesh.fontSize = 48;
						benchmark01_UGUI.m_textMesh.alignment = TextAnchor.MiddleCenter;
					}
					this._003Ci_003E5__2 = 0;
					break;
				case 1:
				{
					this._003C_003E1__state = -1;
					int num2 = this._003Ci_003E5__2;
					this._003Ci_003E5__2 = num2 + 1;
					break;
				}
				case 2:
					this._003C_003E1__state = -1;
					return false;
				default:
					return false;
				}
				if (this._003Ci_003E5__2 > 1000000)
				{
					this._003C_003E2__current = null;
					this._003C_003E1__state = 2;
					return true;
				}
				if (benchmark01_UGUI.BenchmarkType == 0)
				{
					benchmark01_UGUI.m_textMeshPro.text = "The <#0050FF>count is: </color>" + this._003Ci_003E5__2 % 1000;
					if (this._003Ci_003E5__2 % 1000 == 999)
					{
						benchmark01_UGUI.m_textMeshPro.fontSharedMaterial = ((benchmark01_UGUI.m_textMeshPro.fontSharedMaterial == benchmark01_UGUI.m_material01) ? (benchmark01_UGUI.m_textMeshPro.fontSharedMaterial = benchmark01_UGUI.m_material02) : (benchmark01_UGUI.m_textMeshPro.fontSharedMaterial = benchmark01_UGUI.m_material01));
					}
				}
				else if (benchmark01_UGUI.BenchmarkType == 1)
				{
					benchmark01_UGUI.m_textMesh.text = "The <color=#0050FF>count is: </color>" + (this._003Ci_003E5__2 % 1000).ToString();
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

			public Benchmark01_UGUI _003C_003E4__this;

			private int _003Ci_003E5__2;
		}
	}
}
