using System;
using UnityEngine;

public class vfxController : MonoBehaviour
{
	private void Start()
	{
		this.currentStarImage = 0;
		this.currentStarFx = 0;
		this.currentLevel = 3;
		this.currentBgFx = 1;
	}

	public void ChangedStarImage(int i)
	{
		this.currentStarImage = i;
		this.PlayStarFX();
	}

	public void ChangedStarFX(int i)
	{
		this.currentStarFx = i;
		this.PlayStarFX();
	}

	public void ChangedLevel(int i)
	{
		this.currentLevel = i;
		this.PlayStarFX();
	}

	public void ChangedBgFx(int i)
	{
		this.currentBgFx = i;
		this.PlayStarFX();
	}

	public void PlayStarFX()
	{
		this.DesStarFxObjs = GameObject.FindGameObjectsWithTag("Effects");
		GameObject[] desStarFxObjs = this.DesStarFxObjs;
		for (int i = 0; i < desStarFxObjs.Length; i++)
		{
			UnityEngine.Object.Destroy(desStarFxObjs[i].gameObject);
		}
		if (this.currentBgFx != 0)
		{
			UnityEngine.Object.Instantiate<GameObject>(this.bgFxPrefabs[this.currentBgFx]);
		}
		switch (this.currentStarImage)
		{
		case 0:
			UnityEngine.Object.Instantiate<GameObject>(this.starFx01Prefabs[this.currentStarFx]);
			starFxController.myStarFxController.ea = this.currentLevel;
			return;
		case 1:
			UnityEngine.Object.Instantiate<GameObject>(this.starFx02Prefabs[this.currentStarFx]);
			starFxController.myStarFxController.ea = this.currentLevel;
			return;
		case 2:
			UnityEngine.Object.Instantiate<GameObject>(this.starFx03Prefabs[this.currentStarFx]);
			starFxController.myStarFxController.ea = this.currentLevel;
			return;
		case 3:
			UnityEngine.Object.Instantiate<GameObject>(this.starFx04Prefabs[this.currentStarFx]);
			starFxController.myStarFxController.ea = this.currentLevel;
			return;
		case 4:
			UnityEngine.Object.Instantiate<GameObject>(this.starFx05Prefabs[this.currentStarFx]);
			starFxController.myStarFxController.ea = this.currentLevel;
			return;
		default:
			return;
		}
	}

	public GameObject[] starFx01Prefabs;

	public GameObject[] starFx02Prefabs;

	public GameObject[] starFx03Prefabs;

	public GameObject[] starFx04Prefabs;

	public GameObject[] starFx05Prefabs;

	public GameObject[] DesStarFxObjs;

	public GameObject[] bgFxPrefabs;

	public int currentStarImage;

	public int currentStarFx;

	public int currentLevel;

	public int currentBgFx;
}
