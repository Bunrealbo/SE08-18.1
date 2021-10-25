using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	public class LightningWhipSpell : LightningSpellScript
	{
		private IEnumerator WhipForward()
		{
			return new LightningWhipSpell._003CWhipForward_003Ed__7(0)
			{
				_003C_003E4__this = this
			};
		}

		protected override void Start()
		{
			base.Start();
			this.WhipSpring.SetActive(false);
			this.WhipHandle.SetActive(false);
		}

		protected override void Update()
		{
			base.Update();
			base.gameObject.transform.position = this.AttachTo.transform.position;
			base.gameObject.transform.rotation = this.RotateWith.transform.rotation;
		}

		protected override void OnCastSpell()
		{
			base.StartCoroutine(this.WhipForward());
		}

		protected override void OnStopSpell()
		{
		}

		protected override void OnActivated()
		{
			base.OnActivated();
			this.WhipHandle.SetActive(true);
		}

		protected override void OnDeactivated()
		{
			base.OnDeactivated();
			this.WhipHandle.SetActive(false);
		}

		public GameObject AttachTo;

		public GameObject RotateWith;

		public GameObject WhipHandle;

		public GameObject WhipStart;

		public GameObject WhipSpring;

		public AudioSource WhipCrackAudioSource;

		public Action<Vector3> CollisionCallback;

		private sealed class _003CWhipForward_003Ed__7 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CWhipForward_003Ed__7(int _003C_003E1__state)
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
				LightningWhipSpell lightningWhipSpell = this._003C_003E4__this;
				switch (num)
				{
				case 0:
				{
					this._003C_003E1__state = -1;
					for (int i = 0; i < lightningWhipSpell.WhipStart.transform.childCount; i++)
					{
						Rigidbody component = lightningWhipSpell.WhipStart.transform.GetChild(i).gameObject.GetComponent<Rigidbody>();
						if (component != null)
						{
							component.drag = 0f;
							component.velocity = Vector3.zero;
							component.angularVelocity = Vector3.zero;
						}
					}
					lightningWhipSpell.WhipSpring.SetActive(true);
					Vector3 position = lightningWhipSpell.WhipStart.GetComponent<Rigidbody>().position;
					RaycastHit raycastHit;
					Vector3 position2;
					if (Physics.Raycast(position, lightningWhipSpell.Direction, out raycastHit, lightningWhipSpell.MaxDistance, lightningWhipSpell.CollisionMask))
					{
						Vector3 normalized = (raycastHit.point - position).normalized;
						this._003CwhipPositionForwards_003E5__2 = position + normalized * lightningWhipSpell.MaxDistance;
						position2 = position - normalized * 25f;
					}
					else
					{
						this._003CwhipPositionForwards_003E5__2 = position + lightningWhipSpell.Direction * lightningWhipSpell.MaxDistance;
						position2 = position - lightningWhipSpell.Direction * 25f;
					}
					lightningWhipSpell.WhipSpring.GetComponent<Rigidbody>().position = position2;
					this._003C_003E2__current = new WaitForSecondsLightning(0.25f);
					this._003C_003E1__state = 1;
					return true;
				}
				case 1:
					this._003C_003E1__state = -1;
					lightningWhipSpell.WhipSpring.GetComponent<Rigidbody>().position = this._003CwhipPositionForwards_003E5__2;
					this._003C_003E2__current = new WaitForSecondsLightning(0.1f);
					this._003C_003E1__state = 2;
					return true;
				case 2:
					this._003C_003E1__state = -1;
					if (lightningWhipSpell.WhipCrackAudioSource != null)
					{
						lightningWhipSpell.WhipCrackAudioSource.Play();
					}
					this._003C_003E2__current = new WaitForSecondsLightning(0.1f);
					this._003C_003E1__state = 3;
					return true;
				case 3:
					this._003C_003E1__state = -1;
					if (lightningWhipSpell.CollisionParticleSystem != null)
					{
						lightningWhipSpell.CollisionParticleSystem.Play();
					}
					lightningWhipSpell.ApplyCollisionForce(lightningWhipSpell.SpellEnd.transform.position);
					lightningWhipSpell.WhipSpring.SetActive(false);
					if (lightningWhipSpell.CollisionCallback != null)
					{
						lightningWhipSpell.CollisionCallback(lightningWhipSpell.SpellEnd.transform.position);
					}
					this._003C_003E2__current = new WaitForSecondsLightning(0.1f);
					this._003C_003E1__state = 4;
					return true;
				case 4:
					this._003C_003E1__state = -1;
					for (int j = 0; j < lightningWhipSpell.WhipStart.transform.childCount; j++)
					{
						Rigidbody component2 = lightningWhipSpell.WhipStart.transform.GetChild(j).gameObject.GetComponent<Rigidbody>();
						if (component2 != null)
						{
							component2.velocity = Vector3.zero;
							component2.angularVelocity = Vector3.zero;
							component2.drag = 0.5f;
						}
					}
					return false;
				default:
					return false;
				}
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

			public LightningWhipSpell _003C_003E4__this;

			private Vector3 _003CwhipPositionForwards_003E5__2;
		}
	}
}
