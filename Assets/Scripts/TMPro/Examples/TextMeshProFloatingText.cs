using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace TMPro.Examples
{
	public class TextMeshProFloatingText : MonoBehaviour
	{
		private void Awake()
		{
			this.m_transform = base.transform;
			this.m_floatingText = new GameObject(base.name + " floating text");
			this.m_cameraTransform = Camera.main.transform;
		}

		private void Start()
		{
			if (this.SpawnType == 0)
			{
				this.m_textMeshPro = this.m_floatingText.AddComponent<TextMeshPro>();
				this.m_textMeshPro.rectTransform.sizeDelta = new Vector2(3f, 3f);
				this.m_floatingText_Transform = this.m_floatingText.transform;
				this.m_floatingText_Transform.position = this.m_transform.position + new Vector3(0f, 15f, 0f);
				this.m_textMeshPro.alignment = TextAlignmentOptions.Center;
				this.m_textMeshPro.color = new Color32((byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), byte.MaxValue);
				this.m_textMeshPro.fontSize = 24f;
				this.m_textMeshPro.enableKerning = false;
				this.m_textMeshPro.text = string.Empty;
				base.StartCoroutine(this.DisplayTextMeshProFloatingText());
				return;
			}
			if (this.SpawnType == 1)
			{
				this.m_floatingText_Transform = this.m_floatingText.transform;
				this.m_floatingText_Transform.position = this.m_transform.position + new Vector3(0f, 15f, 0f);
				this.m_textMesh = this.m_floatingText.AddComponent<TextMesh>();
				this.m_textMesh.font = Resources.Load<Font>("Fonts/ARIAL");
				this.m_textMesh.GetComponent<Renderer>().sharedMaterial = this.m_textMesh.font.material;
				this.m_textMesh.color = new Color32((byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), byte.MaxValue);
				this.m_textMesh.anchor = TextAnchor.LowerCenter;
				this.m_textMesh.fontSize = 24;
				base.StartCoroutine(this.DisplayTextMeshFloatingText());
				return;
			}
			int spawnType = this.SpawnType;
		}

		public IEnumerator DisplayTextMeshProFloatingText()
		{
			return new TextMeshProFloatingText._003CDisplayTextMeshProFloatingText_003Ed__12(0)
			{
				_003C_003E4__this = this
			};
		}

		public IEnumerator DisplayTextMeshFloatingText()
		{
			return new TextMeshProFloatingText._003CDisplayTextMeshFloatingText_003Ed__13(0)
			{
				_003C_003E4__this = this
			};
		}

		public Font TheFont;

		private GameObject m_floatingText;

		private TextMeshPro m_textMeshPro;

		private TextMesh m_textMesh;

		private Transform m_transform;

		private Transform m_floatingText_Transform;

		private Transform m_cameraTransform;

		private Vector3 lastPOS = Vector3.zero;

		private Quaternion lastRotation = Quaternion.identity;

		public int SpawnType;

		private sealed class _003CDisplayTextMeshProFloatingText_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CDisplayTextMeshProFloatingText_003Ed__12(int _003C_003E1__state)
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
				TextMeshProFloatingText textMeshProFloatingText = this._003C_003E4__this;
				switch (num)
				{
				case 0:
					this._003C_003E1__state = -1;
					this._003CCountDuration_003E5__2 = 2f;
					this._003Cstarting_Count_003E5__3 = UnityEngine.Random.Range(5f, 20f);
					this._003Ccurrent_Count_003E5__4 = this._003Cstarting_Count_003E5__3;
					this._003Cstart_pos_003E5__5 = textMeshProFloatingText.m_floatingText_Transform.position;
					this._003Cstart_color_003E5__6 = textMeshProFloatingText.m_textMeshPro.color;
					this._003Calpha_003E5__7 = 255f;
					this._003CfadeDuration_003E5__8 = 3f / this._003Cstarting_Count_003E5__3 * this._003CCountDuration_003E5__2;
					break;
				case 1:
					this._003C_003E1__state = -1;
					break;
				case 2:
					this._003C_003E1__state = -1;
					textMeshProFloatingText.m_floatingText_Transform.position = this._003Cstart_pos_003E5__5;
					textMeshProFloatingText.StartCoroutine(textMeshProFloatingText.DisplayTextMeshProFloatingText());
					return false;
				default:
					return false;
				}
				if (this._003Ccurrent_Count_003E5__4 <= 0f)
				{
					this._003C_003E2__current = new WaitForSeconds(UnityEngine.Random.Range(0.1f, 1f));
					this._003C_003E1__state = 2;
					return true;
				}
				this._003Ccurrent_Count_003E5__4 -= Time.deltaTime / this._003CCountDuration_003E5__2 * this._003Cstarting_Count_003E5__3;
				if (this._003Ccurrent_Count_003E5__4 <= 3f)
				{
					this._003Calpha_003E5__7 = Mathf.Clamp(this._003Calpha_003E5__7 - Time.deltaTime / this._003CfadeDuration_003E5__8 * 255f, 0f, 255f);
				}
				int num2 = (int)this._003Ccurrent_Count_003E5__4;
				textMeshProFloatingText.m_textMeshPro.text = num2.ToString();
				textMeshProFloatingText.m_textMeshPro.color = new Color32(this._003Cstart_color_003E5__6.r, this._003Cstart_color_003E5__6.g, this._003Cstart_color_003E5__6.b, (byte)this._003Calpha_003E5__7);
				textMeshProFloatingText.m_floatingText_Transform.position += new Vector3(0f, this._003Cstarting_Count_003E5__3 * Time.deltaTime, 0f);
				if (!TMPro_ExtensionMethods.Compare(textMeshProFloatingText.lastPOS, textMeshProFloatingText.m_cameraTransform.position, 1000) || !TMPro_ExtensionMethods.Compare(textMeshProFloatingText.lastRotation, textMeshProFloatingText.m_cameraTransform.rotation, 1000))
				{
					textMeshProFloatingText.lastPOS = textMeshProFloatingText.m_cameraTransform.position;
					textMeshProFloatingText.lastRotation = textMeshProFloatingText.m_cameraTransform.rotation;
					textMeshProFloatingText.m_floatingText_Transform.rotation = textMeshProFloatingText.lastRotation;
					Vector3 vector = textMeshProFloatingText.m_transform.position - textMeshProFloatingText.lastPOS;
					textMeshProFloatingText.m_transform.forward = new Vector3(vector.x, 0f, vector.z);
				}
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

			public TextMeshProFloatingText _003C_003E4__this;

			private float _003CCountDuration_003E5__2;

			private float _003Cstarting_Count_003E5__3;

			private float _003Ccurrent_Count_003E5__4;

			private Vector3 _003Cstart_pos_003E5__5;

			private Color32 _003Cstart_color_003E5__6;

			private float _003Calpha_003E5__7;

			private float _003CfadeDuration_003E5__8;
		}

		private sealed class _003CDisplayTextMeshFloatingText_003Ed__13 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CDisplayTextMeshFloatingText_003Ed__13(int _003C_003E1__state)
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
				TextMeshProFloatingText textMeshProFloatingText = this._003C_003E4__this;
				switch (num)
				{
				case 0:
					this._003C_003E1__state = -1;
					this._003CCountDuration_003E5__2 = 2f;
					this._003Cstarting_Count_003E5__3 = UnityEngine.Random.Range(5f, 20f);
					this._003Ccurrent_Count_003E5__4 = this._003Cstarting_Count_003E5__3;
					this._003Cstart_pos_003E5__5 = textMeshProFloatingText.m_floatingText_Transform.position;
					this._003Cstart_color_003E5__6 = textMeshProFloatingText.m_textMesh.color;
					this._003Calpha_003E5__7 = 255f;
					this._003CfadeDuration_003E5__8 = 3f / this._003Cstarting_Count_003E5__3 * this._003CCountDuration_003E5__2;
					break;
				case 1:
					this._003C_003E1__state = -1;
					break;
				case 2:
					this._003C_003E1__state = -1;
					textMeshProFloatingText.m_floatingText_Transform.position = this._003Cstart_pos_003E5__5;
					textMeshProFloatingText.StartCoroutine(textMeshProFloatingText.DisplayTextMeshFloatingText());
					return false;
				default:
					return false;
				}
				if (this._003Ccurrent_Count_003E5__4 <= 0f)
				{
					this._003C_003E2__current = new WaitForSeconds(UnityEngine.Random.Range(0.1f, 1f));
					this._003C_003E1__state = 2;
					return true;
				}
				this._003Ccurrent_Count_003E5__4 -= Time.deltaTime / this._003CCountDuration_003E5__2 * this._003Cstarting_Count_003E5__3;
				if (this._003Ccurrent_Count_003E5__4 <= 3f)
				{
					this._003Calpha_003E5__7 = Mathf.Clamp(this._003Calpha_003E5__7 - Time.deltaTime / this._003CfadeDuration_003E5__8 * 255f, 0f, 255f);
				}
				int num2 = (int)this._003Ccurrent_Count_003E5__4;
				textMeshProFloatingText.m_textMesh.text = num2.ToString();
				textMeshProFloatingText.m_textMesh.color = new Color32(this._003Cstart_color_003E5__6.r, this._003Cstart_color_003E5__6.g, this._003Cstart_color_003E5__6.b, (byte)this._003Calpha_003E5__7);
				textMeshProFloatingText.m_floatingText_Transform.position += new Vector3(0f, this._003Cstarting_Count_003E5__3 * Time.deltaTime, 0f);
				if (!TMPro_ExtensionMethods.Compare(textMeshProFloatingText.lastPOS, textMeshProFloatingText.m_cameraTransform.position, 1000) || !TMPro_ExtensionMethods.Compare(textMeshProFloatingText.lastRotation, textMeshProFloatingText.m_cameraTransform.rotation, 1000))
				{
					textMeshProFloatingText.lastPOS = textMeshProFloatingText.m_cameraTransform.position;
					textMeshProFloatingText.lastRotation = textMeshProFloatingText.m_cameraTransform.rotation;
					textMeshProFloatingText.m_floatingText_Transform.rotation = textMeshProFloatingText.lastRotation;
					Vector3 vector = textMeshProFloatingText.m_transform.position - textMeshProFloatingText.lastPOS;
					textMeshProFloatingText.m_transform.forward = new Vector3(vector.x, 0f, vector.z);
				}
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

			public TextMeshProFloatingText _003C_003E4__this;

			private float _003CCountDuration_003E5__2;

			private float _003Cstarting_Count_003E5__3;

			private float _003Ccurrent_Count_003E5__4;

			private Vector3 _003Cstart_pos_003E5__5;

			private Color32 _003Cstart_color_003E5__6;

			private float _003Calpha_003E5__7;

			private float _003CfadeDuration_003E5__8;
		}
	}
}
