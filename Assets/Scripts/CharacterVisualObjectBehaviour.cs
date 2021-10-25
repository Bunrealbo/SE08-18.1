using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVisualObjectBehaviour : MonoBehaviour
{
    public Vector3 lookAtPosition
    {
        get
        {
            if (this.sceneItem == null)
            {
                return Vector3.zero;
            }
            return this.sceneItem.lookAtPosition;
        }
    }

    public bool isLookAtPositionDefined
    {
        get
        {
            return !(this.sceneItem == null) && this.sceneItem.isLookAtPositionDefined;
        }
    }

    public void InitForRuntime()
    {
        if (this.isInitializedForRuntime)
        {
            return;
        }
        this.isInitializedForRuntime = true;
        if (this.sceneItem == null)
        {
            return;
        }
        int stencilIndex = this.sceneItem.stencilIndex;
        this.sceneItem.InitForRuntime();
        for (int i = 0; i < this.allVariations.Count; i++)
        {
            this.allVariations[i].SetStencilIndex(stencilIndex);
        }
    }

    public void Init(VisualObjectBehaviour visualObjectBeh, RoomCharacterScene scene)
    {
        this.scene = scene;
        this.visualObjectBeh = visualObjectBeh;
        for (int i = 0; i < visualObjectBeh.variations.Count; i++)
        {
            VisualObjectVariation variation = visualObjectBeh.variations[i];
            CharacterVisualObjectVariation item = this.CreateVariation(variation);
            this.variations.Add(item);
            this.allVariations.Add(item);
        }
        if (visualObjectBeh.hasDefaultVariation)
        {
            this.defaultVariation = this.CreateVariation(visualObjectBeh.defaultVariation);
            this.allVariations.Add(this.defaultVariation);
        }
    }

    private CharacterVisualObjectVariation CreateVariation(VisualObjectVariation variation)
    {
        CharacterVisualObjectVariation characterVisualObjectVariation = new GameObject
        {
            transform =
            {
                parent = base.transform,
                localPosition = Vector3.zero
            },
            name = variation.name,
            layer = base.gameObject.layer

        }.AddComponent<CharacterVisualObjectVariation>();
        characterVisualObjectVariation.Init(this, variation);
        return characterVisualObjectVariation;
    }

    public void ShowGlobalVariation(int variationIndex)
    {
        bool flag = variationIndex >= 0 && variationIndex < this.allVariations.Count;
        for (int i = 0; i < this.allVariations.Count; i++)
        {
            CharacterVisualObjectVariation characterVisualObjectVariation = this.allVariations[i];
            bool active = variationIndex == i;
            characterVisualObjectVariation.SetActive(active);
        }
        if (this.sceneItem != null)
        {
            this.sceneItem.SetActive(flag);
        }
        if (flag)
        {
            this.InitForRuntime();
        }
    }

    public void Hide()
    {
        this.ShowGlobalVariation(-1);
    }

    public RoomCharacterScene scene;

    [SerializeField]
    public VisualObjectBehaviour visualObjectBeh;

    [SerializeField]
    public CharacterVisualObjectVariation defaultVariation;

    [SerializeField]
    public List<CharacterVisualObjectVariation> variations = new List<CharacterVisualObjectVariation>();

    [SerializeField]
    public List<CharacterVisualObjectVariation> allVariations = new List<CharacterVisualObjectVariation>();

    [SerializeField]
    public CharacterVisualObjectSceneItem sceneItem;

    private bool isInitializedForRuntime;
}
