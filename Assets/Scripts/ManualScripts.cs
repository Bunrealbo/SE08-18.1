using System;
using UnityEngine;
using UnityEngine.UI;

public class ManualScripts : MonoBehaviour
{
	private void Start()
	{
		for (int i = 0; i <= this.maxPage; i++)
		{
			this.PageObj[i].SetActive(false);
		}
		this.PageObj[this.minPage].SetActive(true);
		this.currentPage = this.minPage;
		this.txtPage.text = string.Concat(new object[]
		{
			"PAGE ",
			this.currentPage,
			" / ",
			this.maxPage
		});
	}

	public void ChangedPage(int i)
	{
		this.currentPage = Mathf.Clamp(this.currentPage + i, this.minPage, this.maxPage);
		for (int j = 0; j <= this.maxPage; j++)
		{
			this.PageObj[j].SetActive(false);
		}
		this.PageObj[this.currentPage].SetActive(true);
		this.txtPage.text = string.Concat(new object[]
		{
			"PAGE ",
			this.currentPage,
			" / ",
			this.maxPage
		});
	}

	public GameObject[] PageObj;

	public int currentPage;

	public int minPage = 1;

	public int maxPage = 5;

	public Text txtPage;
}
