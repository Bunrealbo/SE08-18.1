using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	[RequireComponent(typeof(AudioSource))]
	public class LightningWhipScript : MonoBehaviour
	{
		private IEnumerator WhipForward()
		{
			return new LightningWhipScript._003CWhipForward_003Ed__10(0)
			{
				_003C_003E4__this = this
			};
		}

		private void Start()
		{
			this.whipStart = GameObject.Find("WhipStart");
			this.whipEndStrike = GameObject.Find("WhipEndStrike");
			this.whipHandle = GameObject.Find("WhipHandle");
			this.whipSpring = GameObject.Find("WhipSpring");
			this.audioSource = base.GetComponent<AudioSource>();
		}

		private void Update()
		{
			if (!this.dragging && Input.GetMouseButtonDown(0))
			{
				Vector2 point = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
				Collider2D collider2D = Physics2D.OverlapPoint(point);
				if (collider2D != null && collider2D.gameObject == this.whipHandle)
				{
					this.dragging = true;
					this.prevDrag = point;
				}
			}
			else if (this.dragging && Input.GetMouseButton(0))
			{
				Vector2 a = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
				Vector2 b = a - this.prevDrag;
				Rigidbody2D component = this.whipHandle.GetComponent<Rigidbody2D>();
				component.MovePosition(component.position + b);
				this.prevDrag = a;
			}
			else
			{
				this.dragging = false;
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
			{
				base.StartCoroutine(this.WhipForward());
			}
		}

		public AudioClip WhipCrack;

		public AudioClip WhipCrackThunder;

		private AudioSource audioSource;

		private GameObject whipStart;

		private GameObject whipEndStrike;

		private GameObject whipHandle;

		private GameObject whipSpring;

		private Vector2 prevDrag;

		private bool dragging;

		private bool canWhip = true;

		private sealed class _003CWhipForward_003Ed__10 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CWhipForward_003Ed__10(int _003C_003E1__state)
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
				LightningWhipScript lightningWhipScript = this._003C_003E4__this;
				switch (num)
				{
				case 0:
					this._003C_003E1__state = -1;
					if (lightningWhipScript.canWhip)
					{
						lightningWhipScript.canWhip = false;
						for (int i = 0; i < lightningWhipScript.whipStart.transform.childCount; i++)
						{
							Rigidbody2D component = lightningWhipScript.whipStart.transform.GetChild(i).gameObject.GetComponent<Rigidbody2D>();
							if (component != null)
							{
								component.drag = 0f;
							}
						}
						lightningWhipScript.audioSource.PlayOneShot(lightningWhipScript.WhipCrack);
						lightningWhipScript.whipSpring.GetComponent<SpringJoint2D>().enabled = true;
						lightningWhipScript.whipSpring.GetComponent<Rigidbody2D>().position = lightningWhipScript.whipHandle.GetComponent<Rigidbody2D>().position + new Vector2(-15f, 5f);
						this._003C_003E2__current = new WaitForSecondsLightning(0.2f);
						this._003C_003E1__state = 1;
						return true;
					}
					break;
				case 1:
					this._003C_003E1__state = -1;
					lightningWhipScript.whipSpring.GetComponent<Rigidbody2D>().position = lightningWhipScript.whipHandle.GetComponent<Rigidbody2D>().position + new Vector2(15f, 2.5f);
					this._003C_003E2__current = new WaitForSecondsLightning(0.15f);
					this._003C_003E1__state = 2;
					return true;
				case 2:
					this._003C_003E1__state = -1;
					lightningWhipScript.audioSource.PlayOneShot(lightningWhipScript.WhipCrackThunder, 0.5f);
					this._003C_003E2__current = new WaitForSecondsLightning(0.15f);
					this._003C_003E1__state = 3;
					return true;
				case 3:
					this._003C_003E1__state = -1;
					lightningWhipScript.whipEndStrike.GetComponent<ParticleSystem>().Play();
					lightningWhipScript.whipSpring.GetComponent<SpringJoint2D>().enabled = false;
					this._003C_003E2__current = new WaitForSecondsLightning(0.65f);
					this._003C_003E1__state = 4;
					return true;
				case 4:
					this._003C_003E1__state = -1;
					for (int j = 0; j < lightningWhipScript.whipStart.transform.childCount; j++)
					{
						Rigidbody2D component2 = lightningWhipScript.whipStart.transform.GetChild(j).gameObject.GetComponent<Rigidbody2D>();
						if (component2 != null)
						{
							component2.velocity = Vector2.zero;
							component2.drag = 0.5f;
						}
					}
					lightningWhipScript.canWhip = true;
					break;
				default:
					return false;
				}
				return false;
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

			public LightningWhipScript _003C_003E4__this;
		}
	}
}
