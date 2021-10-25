using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Match3GameStarter : MonoBehaviour
{
	public void DestroyCreatedGameObjects()
	{
		for (int i = 0; i < this.createdGameObjects.Count; i++)
		{
			UnityEngine.Object.Destroy(this.createdGameObjects[i]);
		}
	}

	public Match3Game CreateGame()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.gamePrefab.gameObject, this.parent);
		this.createdGameObjects.Add(gameObject);
		gameObject.SetActive(true);
		return gameObject.GetComponent<Match3Game>();
	}

	public void GoToMainScene()
	{
		SceneManager.LoadScene(0);
	}

	[SerializeField]
	private Match3Game gamePrefab;

	[SerializeField]
	private Transform parent;

	[NonSerialized]
	private List<GameObject> createdGameObjects = new List<GameObject>();
}
