using System;
using System.Collections.Generic;
using GGMatch3;
using UnityEngine;

public class VisualObjectParticles : MonoBehaviour
{
    private VisualObjectParticles.PieceCreatorPool GetPool(VisualObjectParticles.PositionType type)
    {
        for (int i = 0; i < this.pieceCreatorPools.Count; i++)
        {
            VisualObjectParticles.PieceCreatorPool pieceCreatorPool = this.pieceCreatorPools[i];
            if (pieceCreatorPool.type == type)
            {
                return pieceCreatorPool;
            }
        }
        return null;
    }

    public void CreateParticles(VisualObjectParticles.PositionType positionType, GameObject parent, VisualObjectBehaviour visualObjectBehaviour)
    {
        VisualObjectVariation activeVariation = visualObjectBehaviour.activeVariation;
        for (int i = 0; i < activeVariation.sprites.Count; i++)
        {
            VisualSprite visualSprite = activeVariation.sprites[i];
            if (!visualSprite.visualSprite.isShadow)
            {
                SpriteRenderer spriteRenderer = visualSprite.spriteRenderer;
                GameObject gameObject = this.CreateParticles(positionType, parent);
                GGUtil.Show(gameObject);
                if (!(gameObject == null))
                {
                    Match3ParticleSystem component = gameObject.GetComponent<Match3ParticleSystem>();
                    if (!(component == null))
                    {
                        List<ParticleSystem> allParticleSystems = component.GetAllParticleSystems();
                        for (int j = 0; j < allParticleSystems.Count; j++)
                        {
                            ParticleSystem particleSystem = allParticleSystems[j];
                            var temp = particleSystem.shape;

                            temp.spriteRenderer = spriteRenderer;
                            ParticleSystemRenderer component2 = particleSystem.GetComponent<ParticleSystemRenderer>();
                            component2.sortingLayerID = this.sortingLayer.sortingLayerId;
                            component2.sortingOrder = spriteRenderer.sortingOrder + 1;
                        }
                        component.StartParticleSystems();
                    }
                }
            }
        }
    }

    public GameObject CreateParticles(VisualObjectParticles.PositionType positionType, GameObject parent)
    {
        VisualObjectParticles.PieceCreatorPool pool = this.GetPool(positionType);
        if (pool == null)
        {
            return null;
        }
        GameObject prefab = pool.prefab;
        if (prefab == null)
        {
            return null;
        }
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
        gameObject.transform.parent = parent.transform;
        return gameObject;
    }

    [SerializeField]
    private SpriteSortingSettings sortingLayer = new SpriteSortingSettings();

    [SerializeField]
    private List<VisualObjectParticles.PieceCreatorPool> pieceCreatorPools = new List<VisualObjectParticles.PieceCreatorPool>();

    public enum PositionType
    {
        ChangeSuccess,
        BuySuccess
    }

    [Serializable]
    public class PieceCreatorPool
    {
        public VisualObjectParticles.PositionType type;

        public GameObject prefab;
    }
}
