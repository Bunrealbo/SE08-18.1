using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
    public abstract class LightningSpellScript : MonoBehaviour
    {
        private IEnumerator StopAfterSecondsCoRoutine(float seconds)
        {
            return new LightningSpellScript._003CStopAfterSecondsCoRoutine_003Ed__19(0)
            {
                _003C_003E4__this = this,
                seconds = seconds
            };
        }

        private protected float DurationTimer
        {
            get
            {
                return this._003CDurationTimer_003Ek__BackingField;
            }
            private set
            {
                this._003CDurationTimer_003Ek__BackingField = value;
            }
        }

        private protected float CooldownTimer
        {
            get
            {
                return this._003CCooldownTimer_003Ek__BackingField;
            }
            private set
            {
                this._003CCooldownTimer_003Ek__BackingField = value;
            }
        }

        protected void ApplyCollisionForce(Vector3 point)
        {
            if (this.CollisionForce > 0f && this.CollisionRadius > 0f)
            {
                Collider[] array = Physics.OverlapSphere(point, this.CollisionRadius, this.CollisionMask);
                for (int i = 0; i < array.Length; i++)
                {
                    Rigidbody component = array[i].GetComponent<Rigidbody>();
                    if (component != null)
                    {
                        if (this.CollisionIsExplosion)
                        {
                            component.AddExplosionForce(this.CollisionForce, point, this.CollisionRadius, this.CollisionForce * 0.02f, this.CollisionForceMode);
                        }
                        else
                        {
                            component.AddForce(this.CollisionForce * this.Direction, this.CollisionForceMode);
                        }
                    }
                }
            }
        }

        protected void PlayCollisionSound(Vector3 pos)
        {
            if (this.CollisionAudioSource != null && this.CollisionAudioClips != null && this.CollisionAudioClips.Length != 0)
            {
                int num = UnityEngine.Random.Range(0, this.CollisionAudioClips.Length - 1);
                float volumeScale = UnityEngine.Random.Range(this.CollisionVolumeRange.Minimum, this.CollisionVolumeRange.Maximum);
                this.CollisionAudioSource.transform.position = pos;
                this.CollisionAudioSource.PlayOneShot(this.CollisionAudioClips[num], volumeScale);
            }
        }

        protected virtual void Start()
        {
            if (this.EmissionLight != null)
            {
                this.EmissionLight.enabled = false;
            }
        }

        protected virtual void Update()
        {
            this.CooldownTimer = Mathf.Max(0f, this.CooldownTimer - LightningBoltScript.DeltaTime);
            this.DurationTimer = Mathf.Max(0f, this.DurationTimer - LightningBoltScript.DeltaTime);
        }

        protected virtual void LateUpdate()
        {
        }

        protected virtual void OnDestroy()
        {
        }

        protected abstract void OnCastSpell();

        protected abstract void OnStopSpell();

        protected virtual void OnActivated()
        {
        }

        protected virtual void OnDeactivated()
        {
        }

        public bool CastSpell()
        {
            if (!this.CanCastSpell)
            {
                return false;
            }
            this.Casting = true;
            this.DurationTimer = this.Duration;
            this.CooldownTimer = this.Cooldown;
            this.OnCastSpell();
            if (this.Duration > 0f)
            {
                this.StopAfterSeconds(this.Duration);
            }
            if (this.EmissionParticleSystem != null)
            {
                this.EmissionParticleSystem.Play();
            }
            if (this.EmissionLight != null)
            {
                this.EmissionLight.transform.position = this.SpellStart.transform.position;
                this.EmissionLight.enabled = true;
            }
            if (this.EmissionSound != null)
            {
                this.EmissionSound.Play();
            }
            return true;
        }

        public void StopSpell()
        {
            if (this.Casting)
            {
                this.stopToken++;
                if (this.EmissionParticleSystem != null)
                {
                    this.EmissionParticleSystem.Stop();
                }
                if (this.EmissionLight != null)
                {
                    this.EmissionLight.enabled = false;
                }
                if (this.EmissionSound != null && this.EmissionSound.loop)
                {
                    this.EmissionSound.Stop();
                }
                this.DurationTimer = 0f;
                this.Casting = false;
                this.OnStopSpell();
            }
        }

        public void ActivateSpell()
        {
            this.OnActivated();
        }

        public void DeactivateSpell()
        {
            this.OnDeactivated();
        }

        public void StopAfterSeconds(float seconds)
        {
            base.StartCoroutine(this.StopAfterSecondsCoRoutine(seconds));
        }

        public static GameObject FindChildRecursively(Transform t, string name)
        {
            if (t.name == name)
            {
                return t.gameObject;
            }
            for (int i = 0; i < t.childCount; i++)
            {
                GameObject gameObject = LightningSpellScript.FindChildRecursively(t.GetChild(i), name);
                if (gameObject != null)
                {
                    return gameObject;
                }
            }
            return null;
        }

        public bool Casting
        {
            get
            {
                return this._003CCasting_003Ek__BackingField;
            }
            private set
            {
                this._003CCasting_003Ek__BackingField = value;
            }
        }

        public bool CanCastSpell
        {
            get
            {
                return !this.Casting && this.CooldownTimer <= 0f;
            }
        }

        public GameObject SpellStart;

        public GameObject SpellEnd;

        public Vector3 Direction;

        public float MaxDistance = 15f;

        public bool CollisionIsExplosion;

        public float CollisionRadius = 1f;

        public float CollisionForce = 50f;

        public ForceMode CollisionForceMode = ForceMode.Impulse;

        public ParticleSystem CollisionParticleSystem;

        public LayerMask CollisionMask = -1;

        public AudioSource CollisionAudioSource;

        public AudioClip[] CollisionAudioClips;

        public RangeOfFloats CollisionVolumeRange = new RangeOfFloats
        {
            Minimum = 0.4f,
            Maximum = 0.6f
        };

        public float Duration;

        public float Cooldown;

        public AudioSource EmissionSound;

        public ParticleSystem EmissionParticleSystem;

        public Light EmissionLight;

        private int stopToken;

        private float _003CDurationTimer_003Ek__BackingField;

        private float _003CCooldownTimer_003Ek__BackingField;

        private bool _003CCasting_003Ek__BackingField;

        private sealed class _003CStopAfterSecondsCoRoutine_003Ed__19 : IEnumerator<object>, IEnumerator, IDisposable
        {
            [DebuggerHidden]
            public _003CStopAfterSecondsCoRoutine_003Ed__19(int _003C_003E1__state)
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
                LightningSpellScript lightningSpellScript = this._003C_003E4__this;
                if (num == 0)
                {
                    this._003C_003E1__state = -1;
                    this._003Ctoken_003E5__2 = lightningSpellScript.stopToken;
                    this._003C_003E2__current = new WaitForSecondsLightning(this.seconds);
                    this._003C_003E1__state = 1;
                    return true;
                }
                if (num != 1)
                {
                    return false;
                }
                this._003C_003E1__state = -1;
                if (this._003Ctoken_003E5__2 == lightningSpellScript.stopToken)
                {
                    lightningSpellScript.StopSpell();
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

            public LightningSpellScript _003C_003E4__this;

            public float seconds;

            private int _003Ctoken_003E5__2;
        }
    }
}
