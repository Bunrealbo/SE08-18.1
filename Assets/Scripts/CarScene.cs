using System;
using UnityEngine;

public class CarScene : MonoBehaviour
{
	public T CreateFromPrefab<T>(GameObject prefab) where T : MonoBehaviour
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
		gameObject.transform.parent = base.transform;
		GGUtil.SetLayerRecursively(gameObject, this.carModel.gameObject.layer);
		return gameObject.GetComponent<T>();
	}

	[SerializeField]
	public CarModel carModel;

	[SerializeField]
	public CarCamera camera;
}
