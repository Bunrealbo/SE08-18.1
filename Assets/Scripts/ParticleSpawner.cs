using System;
using System.Linq;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
	public void Start()
	{
		this.particles = Enumerable.ToArray<GameObject>(Enumerable.OrderBy<GameObject, string>(this.particles, new Func<GameObject, string>(ParticleSpawner._003C_003Ec._003C_003E9._003CStart_003Eb__14_0)));
		this.pages = (int)Mathf.Ceil((float)((this.particles.Length - 1) / this.maxButtons));
		if (this.spawnOnAwake)
		{
			this.counter = 0;
			this.ReplaceGO(this.particles[this.counter]);
		}
		if (this.autoChangeDelay > 0f)
		{
			base.InvokeRepeating("NextModel", this.autoChangeDelay, this.autoChangeDelay);
		}
	}

	public void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
		{
			if (this._active)
			{
				this._active = false;
			}
			else
			{
				this._active = true;
			}
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
		{
			this.NextModel();
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow))
		{
			this.counter--;
			if (this.counter < 0)
			{
				this.counter = this.particles.Length - 1;
			}
			this.ReplaceGO(this.particles[this.counter]);
		}
	}

	public void NextModel()
	{
		this.counter++;
		if (this.counter > this.particles.Length - 1)
		{
			this.counter = 0;
		}
		this.ReplaceGO(this.particles[this.counter]);
	}

	public void Duplicate()
	{
		UnityEngine.Object.Instantiate<GameObject>(this.currentGO, this.currentGO.transform.position, this.currentGO.transform.rotation);
	}

	public void DestroyAll()
	{
		ParticleSystem[] array = (ParticleSystem[])UnityEngine.Object.FindObjectsOfType(typeof(ParticleSystem));
		for (int i = 0; i < array.Length; i++)
		{
			UnityEngine.Object.Destroy(array[i].gameObject);
		}
	}

	public void OnGUI()
	{
		if (this._active)
		{
			if (this.particles.Length > this.maxButtons)
			{
				if (GUI.Button(new Rect(20f, (float)((this.maxButtons + 1) * 18), 75f, 18f), "Prev"))
				{
					if (this.page > 0)
					{
						this.page--;
					}
					else
					{
						this.page = this.pages;
					}
				}
				if (GUI.Button(new Rect(95f, (float)((this.maxButtons + 1) * 18), 75f, 18f), "Next"))
				{
					if (this.page < this.pages)
					{
						this.page++;
					}
					else
					{
						this.page = 0;
					}
				}
				GUI.Label(new Rect(60f, (float)((this.maxButtons + 2) * 18), 150f, 22f), string.Concat(new object[]
				{
					"Page",
					this.page + 1,
					" / ",
					this.pages + 1
				}));
			}
			if (GUI.Button(new Rect(20f, (float)((this.maxButtons + 4) * 18), 150f, 18f), "Duplicate"))
			{
				this.Duplicate();
			}
			int num = this.particles.Length - this.page * this.maxButtons;
			if (num > this.maxButtons)
			{
				num = this.maxButtons;
			}
			for (int i = 0; i < num; i++)
			{
				string text = this.particles[i + this.page * this.maxButtons].transform.name;
				if (this.removeTextFromButton != "")
				{
					text = text.Replace(this.removeTextFromButton, "");
				}
				if (GUI.Button(new Rect(20f, (float)(i * 18 + 18), 150f, 18f), text))
				{
					this.DestroyAll();
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.particles[i + this.page * this.maxButtons]);
					this.currentGO = gameObject;
					this.counter = i + this.page * this.maxButtons;
				}
			}
		}
	}

	public void ReplaceGO(GameObject _go)
	{
		if (this.currentGO != null)
		{
			UnityEngine.Object.Destroy(this.currentGO);
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(_go);
		this.currentGO = gameObject;
	}

	public void PlayPS(ParticleSystem _ps, int _nr)
	{
		Time.timeScale = 1f;
		_ps.Play();
	}

	public GameObject[] particles;

	public int maxButtons = 10;

	public bool spawnOnAwake = true;

	public string removeTextFromButton;

	public string removeTextFromMaterialButton;

	public float autoChangeDelay;

	private int page;

	private int pages;

	private GameObject currentGO;

	private Color currentColor;

	private bool isPS;

	private bool _active = true;

	private int counter = -1;

	public GUIStyle bigStyle;

	[Serializable]
	private sealed class _003C_003Ec
	{
		internal string _003CStart_003Eb__14_0(GameObject go)
		{
			return go.name;
		}

		public static readonly ParticleSpawner._003C_003Ec _003C_003E9 = new ParticleSpawner._003C_003Ec();

		public static Func<GameObject, string> _003C_003E9__14_0;
	}
}
