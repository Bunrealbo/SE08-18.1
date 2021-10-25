using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGMatch3
{
	[DefaultExecutionOrder(-100)]
	public class NavigationManager : MonoBehaviour
	{
		public static NavigationManager instance
		{
			get
			{
				if (NavigationManager.instance_ == null)
				{
					NavigationManager.instance_ = UnityEngine.Object.FindObjectOfType<NavigationManager>();
				}
				return NavigationManager.instance_;
			}
		}

		public NavigationManager.ObjectDefinition GetObjectByName(string name)
		{
			for (int i = 0; i < this.objectsList.Count; i++)
			{
				NavigationManager.ObjectDefinition objectDefinition = this.objectsList[i];
				if (!(objectDefinition.gameObject == null) && objectDefinition.gameObject.name == name)
				{
					return objectDefinition;
				}
			}
			return null;
		}

		public Camera GetCamera()
		{
			for (int i = 0; i < this.objectsList.Count; i++)
			{
				NavigationManager.ObjectDefinition objectDefinition = this.objectsList[i];
				if (!(objectDefinition.gameObject == null))
				{
					Camera component = objectDefinition.gameObject.GetComponent<Camera>();
					if (!(component == null))
					{
						return component;
					}
				}
			}
			return null;
		}

		public T GetObject<T>() where T : MonoBehaviour
		{
			for (int i = 0; i < this.objectsList.Count; i++)
			{
				NavigationManager.ObjectDefinition objectDefinition = this.objectsList[i];
				if (!(objectDefinition.gameObject == null))
				{
					T component = objectDefinition.gameObject.GetComponent<T>();
					if (!(component == null))
					{
						return component;
					}
				}
			}
			return default(T);
		}

		public NavigationManager.HistoryObject CurrentScreen
		{
			get
			{
				if (this.history.Count == 0)
				{
					return null;
				}
				return this.history[this.history.Count - 1];
			}
		}

		private void Awake()
		{
            PlayerPrefs.DeleteAll();
			for (int i = 0; i < this.objectsList.Count; i++)
			{
				NavigationManager.ObjectDefinition objectDefinition = this.objectsList[i];
				if (objectDefinition.isScreen && GGUtil.isPartOfHierarchy(objectDefinition.gameObject))
				{
					GGUtil.Hide(objectDefinition.gameObject);
				}
			}
			if (this.startInApp)
			{
				InAppBackend instance = BehaviourSingletonInit<InAppBackend>.instance;
			}
			if (this.showTermsOfServiceOnStart && !GGPlayerSettings.instance.Model.acceptedTermsOfService)
			{
				this.GetObject<TermsOfServiceDialog>().Show(new Action<bool>(this._003CAwake_003Eb__15_0));
			}
			else
			{
				this.LoadStartLayer();
			}
			GGNotificationCenter instance2 = BehaviourSingletonInit<GGNotificationCenter>.instance;
            //GGPlayerSettings.instance.walletManager.AddCurrency(CurrencyType.diamonds, 99999);
            //GGPlayerSettings.instance.walletManager.AddCurrency(CurrencyType.coins, 99999);
            //GGPlayerSettings.instance.walletManager.AddCurrency(CurrencyType.ggdollars, 99999);
        }

		private void LoadStartLayer()
		{
			NavigationManager.ObjectDefinition objectByName = this.GetObjectByName(this.layerToLoadAtStart);
			if (objectByName == null)
			{
				return;
			}
			this.Push(objectByName.gameObject, false);
		}

		public void Push(MonoBehaviour behaviour, bool isModal = false)
		{
			if (behaviour == null)
			{
				return;
			}
			this.Push(behaviour.gameObject, isModal);
		}

		public void Push(GameObject screen, bool isModal = false)
		{
			NavigationManager.HistoryObject currentScreen = this.CurrentScreen;
			if (!isModal && currentScreen != null)
			{
				currentScreen.Hide();
			}
			NavigationManager.HistoryObject historyObject = new NavigationManager.HistoryObject();
			historyObject.gameObject = screen;
			this.history.Add(historyObject);
			historyObject.Show();
		}

		public void PopMultiple(int screensToPopCount)
		{
			if (screensToPopCount <= 0)
			{
				return;
			}
			screensToPopCount = Mathf.Min(screensToPopCount, this.history.Count);
			for (int i = 0; i < screensToPopCount; i++)
			{
				bool activateNextScreen = i == screensToPopCount - 1;
				this.Pop(activateNextScreen);
			}
		}

		public void Pop(bool activateNextScreen = true)
		{
			NavigationManager.HistoryObject currentScreen = this.CurrentScreen;
			if (currentScreen == null)
			{
				return;
			}
			this.history.RemoveAt(this.history.Count - 1);
			GameObject gameObject = currentScreen.gameObject;
			IRemoveFromHistoryEventListener removeFromHistoryEventListener = null;
			if (gameObject != null)
			{
				removeFromHistoryEventListener = gameObject.GetComponent<IRemoveFromHistoryEventListener>();
			}
			if (removeFromHistoryEventListener != null)
			{
				removeFromHistoryEventListener.OnRemovedFromNavigationHistory();
			}
			currentScreen.Hide();
			NavigationManager.HistoryObject currentScreen2 = this.CurrentScreen;
			if (currentScreen2 == null)
			{
				return;
			}
			currentScreen2.SetActive(activateNextScreen);
		}

		private void Update()
		{
			if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
			{
				this.TryToGoBack();
			}
		}

		private void TryToGoBack()
		{
			NavigationManager.HistoryObject currentScreen = this.CurrentScreen;
			if (currentScreen == null)
			{
				return;
			}
			UILayer component = currentScreen.gameObject.GetComponent<UILayer>();
			if (component == null)
			{
				return;
			}
			component.OnGoBack(this);
		}

		private void _003CAwake_003Eb__15_0(bool success)
		{
			if (!success)
			{
				Application.Quit();
				return;
			}
			GGPlayerSettings.instance.Model.acceptedTermsOfService = true;
			GGPlayerSettings.instance.Save();
			this.Pop(true);
			this.LoadStartLayer();
		}

		[SerializeField]
		private bool startInApp;

		[SerializeField]
		private bool showTermsOfServiceOnStart;

		[SerializeField]
		private string layerToLoadAtStart;

		[SerializeField]
		private List<NavigationManager.ObjectDefinition> objectsList = new List<NavigationManager.ObjectDefinition>();

		private static NavigationManager instance_;

		public List<NavigationManager.HistoryObject> history = new List<NavigationManager.HistoryObject>();

		[Serializable]
		public class ObjectDefinition
		{
			public GameObject gameObject;

			public bool isScreen = true;
		}

		public class HistoryObject
		{
			public void SetActive(bool active)
			{
				GGUtil.SetActive(this.gameObject, active);
			}

			public void Hide()
			{
				GGUtil.SetActive(this.gameObject, false);
			}

			public void Show()
			{
				GGUtil.SetActive(this.gameObject, true);
			}

			public GameObject gameObject;
		}
	}
}
