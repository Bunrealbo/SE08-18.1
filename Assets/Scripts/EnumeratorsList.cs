using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumeratorsList
{
	public EnumeratorsList Add(IEnumerator e, float delay = 0f, Action onStart = null, Action interuptionHandler = null, bool useScaledTime = false)
	{
		if (e == null && onStart == null)
		{
			return this;
		}
		this.list.Add(new EnumeratorsList.EnumeratorDesc(e, delay, onStart, interuptionHandler, useScaledTime));
		return this;
	}

	public EnumeratorsList Clear()
	{
		this.list.Clear();
		return this;
	}

	public bool Update()
	{
		bool flag = false;
		for (int i = 0; i < this.list.Count; i++)
		{
			EnumeratorsList.EnumeratorDesc enumeratorDesc = this.list[i];
			if (enumeratorDesc.delay > 0f)
			{
				if (enumeratorDesc.useScaledTime)
				{
					enumeratorDesc.delay -= Time.deltaTime;
				}
				else
				{
					enumeratorDesc.delay -= Time.deltaTime;
				}
				this.list[i] = enumeratorDesc;
				flag = true;
			}
			else
			{
				if (!enumeratorDesc.started && enumeratorDesc.onStart != null)
				{
					enumeratorDesc.started = true;
					enumeratorDesc.onStart();
					this.list[i] = enumeratorDesc;
				}
				IEnumerator enumerator = enumeratorDesc.enumerator;
				bool flag2 = false;
				if (enumerator != null)
				{
					flag2 = enumerator.MoveNext();
				}
				flag = (flag || flag2);
			}
		}
		return flag;
	}

	protected List<EnumeratorsList.EnumeratorDesc> list = new List<EnumeratorsList.EnumeratorDesc>();

	protected struct EnumeratorDesc
	{
		public EnumeratorDesc(IEnumerator enumerator, float delay, Action onStart, Action interuptionHandler, bool useScaledTime = false)
		{
			this.enumerator = enumerator;
			this.delay = delay;
			this.onStart = onStart;
			this.started = false;
			this.interuptionHandler = interuptionHandler;
			this.useScaledTime = useScaledTime;
		}

		public IEnumerator enumerator;

		public float delay;

		public Action onStart;

		public Action interuptionHandler;

		public bool started;

		public bool useScaledTime;
	}
}
