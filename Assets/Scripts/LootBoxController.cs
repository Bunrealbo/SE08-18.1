using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class LootBoxController : MonoBehaviour
{
	private void Start()
	{
		this.idEffect++;
		this.idIcon++;
		this.effectsText.text = "Type       " + this.idEffect + " / 25";
		this.nameEffectText.text = this.EffectPrefabs[this.idEffect].gameObject.name;
		this.SetupVfx();
		this.isOpened = false;
	}

	private void OnMouseDown()
	{
		if (!this.isOpened)
		{
			base.StartCoroutine(this.PlayFx());
		}
	}

	private IEnumerator PlayFx()
	{
		return new LootBoxController._003CPlayFx_003Ed__12(0)
		{
			_003C_003E4__this = this
		};
	}

	private IEnumerator PlayIcon()
	{
		return new LootBoxController._003CPlayIcon_003Ed__13(0)
		{
			_003C_003E4__this = this
		};
	}

	public void ChangedFx(int i)
	{
		this.ResetVfx();
		this.idEffect += i;
		this.idEffect = Mathf.Clamp(this.idEffect, 1, 25);
		this.effectsText.text = "Type       " + this.idEffect + " / 25";
		this.nameEffectText.text = this.EffectPrefabs[this.idEffect].gameObject.name;
	}

	public void SetupVfx()
	{
		this.Lootbox = UnityEngine.Object.Instantiate<GameObject>(this.IconPrefabs[1], base.transform.position, base.transform.rotation);
	}

	public void PlayAllVfx()
	{
		if (!this.isOpened)
		{
			base.StartCoroutine(this.PlayFx());
		}
	}

	public void ResetVfx()
	{
		this.DesFxObjs = GameObject.FindGameObjectsWithTag("Effects");
		GameObject[] array = this.DesFxObjs;
		for (int i = 0; i < array.Length; i++)
		{
			UnityEngine.Object.Destroy(array[i].gameObject);
		}
		this.isOpened = false;
		this.DesIconObjs = GameObject.FindGameObjectsWithTag("Icon");
		array = this.DesIconObjs;
		for (int i = 0; i < array.Length; i++)
		{
			UnityEngine.Object.Destroy(array[i].gameObject);
		}
		base.StartCoroutine(this.PlayIcon());
	}

	public int idIcon;

	public int idEffect;

	public bool isOpened;

	public GameObject[] IconPrefabs;

	public GameObject[] EffectPrefabs;

	public GameObject[] DesFxObjs;

	public GameObject[] DesIconObjs;

	private GameObject Lootbox;

	public Text effectsText;

	public Text nameEffectText;

	private sealed class _003CPlayFx_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CPlayFx_003Ed__12(int _003C_003E1__state)
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
			LootBoxController lootBoxController = this._003C_003E4__this;
			switch (num)
			{
			case 0:
				this._003C_003E1__state = -1;
				lootBoxController.isOpened = true;
				lootBoxController.idEffect = Mathf.Clamp(lootBoxController.idEffect, 1, 25);
				lootBoxController.effectsText.text = "Type       " + lootBoxController.idEffect + " / 25";
				this._003C_003E2__current = new WaitForSeconds(0.2f);
				this._003C_003E1__state = 1;
				return true;
			case 1:
				this._003C_003E1__state = -1;
				UnityEngine.Object.Destroy(lootBoxController.Lootbox);
				lootBoxController.Lootbox = UnityEngine.Object.Instantiate<GameObject>(lootBoxController.IconPrefabs[2], lootBoxController.transform.position, lootBoxController.transform.rotation);
				this._003C_003E2__current = new WaitForSeconds(0.1f);
				this._003C_003E1__state = 2;
				return true;
			case 2:
				this._003C_003E1__state = -1;
				UnityEngine.Object.Instantiate<GameObject>(lootBoxController.EffectPrefabs[lootBoxController.idEffect], lootBoxController.transform.position, lootBoxController.transform.rotation);
				CameraShake.myCameraShake.ShakeCamera(0.3f, 0.1f);
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

		public LootBoxController _003C_003E4__this;
	}

	private sealed class _003CPlayIcon_003Ed__13 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CPlayIcon_003Ed__13(int _003C_003E1__state)
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
			LootBoxController lootBoxController = this._003C_003E4__this;
			if (num == 0)
			{
				this._003C_003E1__state = -1;
				lootBoxController.DesIconObjs = GameObject.FindGameObjectsWithTag("Icon");
				GameObject[] desIconObjs = lootBoxController.DesIconObjs;
				for (int i = 0; i < desIconObjs.Length; i++)
				{
					UnityEngine.Object.Destroy(desIconObjs[i].gameObject);
				}
				this._003C_003E2__current = new WaitForSeconds(0.1f);
				this._003C_003E1__state = 1;
				return true;
			}
			if (num != 1)
			{
				return false;
			}
			this._003C_003E1__state = -1;
			lootBoxController.Lootbox = UnityEngine.Object.Instantiate<GameObject>(lootBoxController.IconPrefabs[1], lootBoxController.transform.position, lootBoxController.transform.rotation);
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

		public LootBoxController _003C_003E4__this;
	}
}
