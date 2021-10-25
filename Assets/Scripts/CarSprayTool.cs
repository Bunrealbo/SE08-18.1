using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarSprayTool : MonoBehaviour
{
	private AssembleCarScreen screen
	{
		get
		{
			return this.initArguments.screen;
		}
	}

	public PaintTransformation paintTransformation
	{
		get
		{
			return this.initArguments.paintTransformation;
		}
	}

	public void Init(CarSprayTool.InitArguments initArguments)
	{
		this.initArguments = initArguments;
		this.currentFillPercentage = 0f;
		GGUtil.Show(this);
		this.sprayTool.Init(new Action<Vector3>(this.OnDrag));
		this.sprayTool.transform.localPosition = Vector3.zero;
		this.UpdateFill(this.currentFillPercentage);
	}

	private void UpdateFill(float percentage)
	{
		float num = Mathf.InverseLerp(0f, this.fillTreshold, percentage);
		GGUtil.SetFill(this.fillImage, num);
		string text = string.Format("{0}%", Mathf.FloorToInt(num * 100f));
		GGUtil.ChangeText(this.fillText, text);
	}

	private void OnDrag(Vector3 screenPosition)
	{
		Ray ray = this.screen.scene.camera.ScreenPointToRay(screenPosition);
		RaycastHit raycastHit;
		if (!Physics.Raycast(ray, out raycastHit))
		{
			return;
		}
		GameObject gameObject = raycastHit.collider.gameObject;
		Vector3 point = ray.GetPoint(raycastHit.distance);
		this.HandleHitPoint(point);
	}

	private void Update()
	{
		if (this.sprayTool.dragState.isDragging)
		{
			this.HandleCamera(this.sprayTool.dragState.lastScreenPosition);
			return;
		}
		if (this.currentFillPercentage >= this.fillTreshold)
		{
			this.ButtonCallback_OnFinish();
		}
	}

	private void HandleCamera(Vector3 screenPosition)
	{
		CarCamera camera = this.screen.scene.camera;
		Vector3 vector = camera.ScreenToViewPortPoint(screenPosition);
		vector = vector * 2f - Vector3.one;
		float num = Mathf.InverseLerp(this.viewportBoundsX, 1f, Mathf.Abs(vector.x));
		float num2 = Mathf.InverseLerp(this.viewportBoundsX, 1f, Mathf.Abs(vector.y));
		if (num > 0f || num2 > 0f)
		{
			num *= this.viewportSpeed.x * Time.deltaTime;
			num2 *= this.viewportSpeed.y * Time.deltaTime;
			Vector2 distance = new Vector2(num * Mathf.Sign(vector.x), num2 * Mathf.Sign(vector.y));
			camera.Move(distance);
		}
	}

	private void HandleHitPoint(Vector3 hitPointWorldPosition)
	{
		if (this.paintTransformation == null)
		{
			return;
		}
		GGPSphereCommand.Params sphereParams = default(GGPSphereCommand.Params);
		sphereParams.brushColor = Color.white;
		sphereParams.brushHardness = 0.01f;
		sphereParams.brushSize = this.spraySize;
		sphereParams.worldPosition = hitPointWorldPosition;
		this.paintTransformation.RenderSphere(sphereParams);
		this.currentFillPercentage = this.paintTransformation.FillPercent();
		this.UpdateFill(this.currentFillPercentage);
	}

	public void ButtonCallback_OnFinish()
	{
		if (this.initArguments.onDone != null)
		{
			this.initArguments.onDone();
		}
	}

	[SerializeField]
	private Image fillImage;

	[SerializeField]
	private TextMeshProUGUI fillText;

	[SerializeField]
	private CarSprayToolTarget sprayTool;

	[SerializeField]
	private float viewportBoundsX = 0.5f;

	[SerializeField]
	private float viewportBoundsY = 0.9f;

	[SerializeField]
	private Vector3 viewportSpeed = Vector3.one;

	private float fillTreshold = 0.8f;

	private float spraySize = 0.5f;

	private CarSprayTool.InitArguments initArguments;

	private float currentFillPercentage;

	public struct InitArguments
	{
		public AssembleCarScreen screen;

		public PaintTransformation paintTransformation;

		public Action onDone;
	}
}
