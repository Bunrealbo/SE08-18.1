using System;
using UnityEngine;

public class CarVariantInteractionButton : MonoBehaviour
{
    private AssembleCarScreen screen
    {
        get
        {
            return this.initParams.screen;
        }
    }

    private Vector3 positionToAttachTo
    {
        get
        {
            return this.initParams.positionToAttachTo;
        }
    }

    public CarModelSubpart subpart
    {
        get
        {
            return this.initParams.subpart;
        }
    }

    public CarModelInfo.VariantGroup variantGroup
    {
        get
        {
            return this.initParams.variantGroup;
        }
    }

    public void Init(CarVariantInteractionButton.InitParams initParams)
    {
        this.shouldBeActive = true;
        this.initParams = initParams;
        GGUtil.SetActive(this.buyButtonContanier, true);
        this.SetPositionOfBuyButton();
    }

    private void SetPositionOfBuyButton()
    {
        Vector3 localPosition = this.screen.TransformWorldCarPointToLocalUIPosition(this.positionToAttachTo);
        localPosition.z = 0f;
        this.buyButtonContanier.localPosition = localPosition;
        bool flag = this.screen.IsFacingCamera(this.initParams.forwardDirection);
        GGUtil.SetActive(this.buyButtonContanier, flag && this.shouldBeActive);
    }

    private void Update()
    {
        //this.screen != null;
        this.SetPositionOfBuyButton();
    }

    public void HideButton()
    {
        this.shouldBeActive = false;
        GGUtil.SetActive(this.buyButtonContanier, false);
    }

    public void ButtonCallback_OnClick()
    {
        this.initParams.CallOnClick(this);
    }

    [SerializeField]
    private Transform buyButtonContanier;

    [NonSerialized]
    private CarVariantInteractionButton.InitParams initParams;

    private bool shouldBeActive;

    public struct InitParams
    {
        public void CallOnClick(CarVariantInteractionButton button)
        {
            Action<CarVariantInteractionButton> action = this.onClick;
            if (action == null)
            {
                return;
            }
            action(button);
        }

        public AssembleCarScreen screen;

        public Vector3 positionToAttachTo;

        public Transform forwardTransform;

        public Vector3 forwardDirection;

        public CarModelInfo.VariantGroup variantGroup;

        public CarModelSubpart subpart;

        public Action<CarVariantInteractionButton> onClick;
    }
}
