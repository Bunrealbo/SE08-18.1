using System;
using UnityEngine;

public class ScrewdriverTool : MonoBehaviour
{
	public bool isPressed
	{
		get
		{
			return !(this.initArguments.inputHandler == null) && this.initArguments.inputHandler.IsAnyDown;
		}
	}

	public DrillModel GetDrillModel(CarScene carScene)
	{
		if (this.drillModel_ == null)
		{
			this.drillModel_ = carScene.CreateFromPrefab<DrillModel>(this.drillModelPrefab);
			this.drillModel_.transform.localScale = Vector3.one;
		}
		return this.drillModel_;
	}

	public void Init(ScrewdriverTool.InitArguments initArguments)
	{
		this.initArguments = initArguments;
		initArguments.inputHandler.Clear();
		GGUtil.Show(this);
		GGUtil.Hide(this.buttonTransform);
		if (this.drillModel_ != null)
		{
			this.drillModel_.Hide();
		}
	}

	public void Hide()
	{
		GGUtil.Hide(this);
		this.initArguments.onPress = null;
		if (this.drillModel_ != null)
		{
			this.drillModel_.Hide();
		}
	}

	private void Update()
	{
		bool isPressed = this.isPressed;
		if (isPressed)
		{
			ScrewdriverTool.PressArguments obj = default(ScrewdriverTool.PressArguments);
			obj.isPressed = isPressed;
			Action<ScrewdriverTool.PressArguments> onPress = this.initArguments.onPress;
			if (onPress == null)
			{
				return;
			}
			onPress(obj);
		}
	}

	[SerializeField]
	private Transform buttonTransform;

	[NonSerialized]
	private ScrewdriverTool.InitArguments initArguments;

	[SerializeField]
	private GameObject drillModelPrefab;

	[NonSerialized]
	private DrillModel drillModel_;

	public struct PressArguments
	{
		public bool isPressed;
	}

	public struct InitArguments
	{
		public Action<ScrewdriverTool.PressArguments> onPress;

		public InputHandler inputHandler;

		public CarScene carScene;
	}
}
