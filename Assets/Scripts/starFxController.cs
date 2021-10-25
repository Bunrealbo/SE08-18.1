using System;
using UnityEngine;

public class starFxController : MonoBehaviour
{
	private void Awake()
	{
		starFxController.myStarFxController = this;
	}

	private void Start()
	{
		this.Reset();
	}

	private void Update()
	{
		if (!this.isEnd)
		{
			this.currentDelay -= Time.deltaTime;
			if (this.currentDelay <= 0f)
			{
				if (this.currentEa != this.ea)
				{
					this.currentDelay = this.delay;
					this.starFX[this.currentEa].SetActive(true);
					this.currentEa++;
				}
				else
				{
					this.isEnd = true;
					this.currentDelay = this.delay;
					this.currentEa = 0;
				}
			}
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.DownArrow))
		{
			this.Reset();
		}
	}

	public void Reset()
	{
		for (int i = 0; i < 3; i++)
		{
			this.starFX[i].SetActive(false);
		}
		this.currentDelay = this.delay;
		this.currentEa = 0;
		this.isEnd = false;
		for (int j = 0; j < 3; j++)
		{
			this.starFX[j].SetActive(false);
		}
	}

	public GameObject[] starFX;

	public int ea;

	public int currentEa;

	public float delay;

	public float currentDelay;

	public bool isEnd;

	public int idStar;

	public static starFxController myStarFxController;
}
