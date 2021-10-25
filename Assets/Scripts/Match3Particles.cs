using System;
using System.Collections.Generic;
using GGMatch3;
using UnityEngine;

public class Match3Particles : MonoBehaviour
{
	private List<Match3Particles.PieceCreatorPool> GetPoolsForPositionType(Match3Particles.PositionType type)
	{
		int match3ParticlesVariant = GGTest.match3ParticlesVariant;
		this.positionTypeFilteredPools_.Clear();
		for (int i = 0; i < this.pieceCreatorPools.Count; i++)
		{
			Match3Particles.PieceCreatorPool pieceCreatorPool = this.pieceCreatorPools[i];
			if (pieceCreatorPool.type == type)
			{
				if (pieceCreatorPool.IsAcceptableForVariant(match3ParticlesVariant))
				{
					this.positionTypeFilteredPools_.Insert(0, pieceCreatorPool);
				}
				else if (pieceCreatorPool.abTestVariant == Match3Particles.PieceCreatorPool.AbTestVariant.Normal)
				{
					this.positionTypeFilteredPools_.Add(pieceCreatorPool);
				}
			}
		}
		return this.positionTypeFilteredPools_;
	}

	private Match3Particles.PieceCreatorPool GetPool(Match3Particles.PositionType type)
	{
		List<Match3Particles.PieceCreatorPool> poolsForPositionType = this.GetPoolsForPositionType(type);
		for (int i = 0; i < poolsForPositionType.Count; i++)
		{
			Match3Particles.PieceCreatorPool pieceCreatorPool = poolsForPositionType[i];
			if (pieceCreatorPool.type == type)
			{
				return pieceCreatorPool;
			}
		}
		return null;
	}

	private Match3Particles.PieceCreatorPool GetPool(Match3Particles.PositionType type, ChipType chipType)
	{
		List<Match3Particles.PieceCreatorPool> poolsForPositionType = this.GetPoolsForPositionType(type);
		for (int i = 0; i < poolsForPositionType.Count; i++)
		{
			Match3Particles.PieceCreatorPool pieceCreatorPool = poolsForPositionType[i];
			if (pieceCreatorPool.type == type && pieceCreatorPool.chipTypeList.Contains(chipType))
			{
				return pieceCreatorPool;
			}
		}
		return null;
	}

	private Match3Particles.PieceCreatorPool GetPool(Match3Particles.PositionType type, ChipType chipType, int level)
	{
		List<Match3Particles.PieceCreatorPool> poolsForPositionType = this.GetPoolsForPositionType(type);
		for (int i = 0; i < poolsForPositionType.Count; i++)
		{
			Match3Particles.PieceCreatorPool pieceCreatorPool = poolsForPositionType[i];
			if (pieceCreatorPool.type == type && pieceCreatorPool.chipTypeList.Contains(chipType) && pieceCreatorPool.levelList.Contains(level))
			{
				return pieceCreatorPool;
			}
		}
		return null;
	}

	public GameObject CreateParticles(Vector3 localPositionOfCenter, Match3Particles.PositionType positionType, Quaternion rotation)
	{
		GameObject gameObject = this.CreateParticles(localPositionOfCenter, positionType);
		if (gameObject == null)
		{
			return gameObject;
		}
		gameObject.transform.rotation = rotation;
		return gameObject;
	}

	public GameObject CreateParticles(Slot slot, Match3Particles.PositionType positionType)
	{
		return this.CreateParticles(slot.localPositionOfCenter, positionType);
	}

	public GameObject CreateParticles(Vector3 localPosition, Match3Particles.PositionType positionType)
	{
		if (this.disableParticles)
		{
			return null;
		}
		Match3Particles.PieceCreatorPool pool = this.GetPool(positionType);
		if (pool == null)
		{
			return null;
		}
		GameObject gameObject = pool.pool.Instantiate(false);
		if (gameObject == null)
		{
			return null;
		}
		gameObject.transform.localPosition = localPosition;
		Match3ParticleSystem component = gameObject.GetComponent<Match3ParticleSystem>();
		if (component != null)
		{
			component.StartParticleSystems();
		}
		GGUtil.SetActive(gameObject, true);
		return gameObject;
	}

	public GameObject CreateParticles(Chip chip, Match3Particles.PositionType positionType, ChipType chipType, ItemColor itemColor)
	{
		TransformBehaviour componentBehaviour = chip.GetComponentBehaviour<TransformBehaviour>();
		if (componentBehaviour == null)
		{
			return null;
		}
		return this.CreateParticles(componentBehaviour.localPosition, positionType, chipType, itemColor);
	}

	public GameObject CreateParticlesWorld(Vector3 worldPosition, Match3Particles.PositionType positionType, ChipType chipType, ItemColor itemColor)
	{
		Match3Particles.PieceCreatorPool pool = this.GetPool(positionType, chipType);
		if (pool == null)
		{
			return null;
		}
		Vector3 localPosition = pool.pool.parent.InverseTransformPoint(worldPosition);
		localPosition.z = 0f;
		return this.CreateParticles(localPosition, positionType, chipType, itemColor);
	}

	public GameObject CreateParticles(Vector3 localPosition, Match3Particles.PositionType positionType, ChipType chipType, int level)
	{
		if (this.disableParticles)
		{
			return null;
		}
		Match3Particles.PieceCreatorPool pool = this.GetPool(positionType, chipType, level);
		if (pool == null)
		{
			pool = this.GetPool(positionType, chipType);
		}
		if (pool == null || pool.pool == null)
		{
			return null;
		}
		GameObject gameObject = pool.pool.Instantiate(false);
		if (gameObject == null)
		{
			return null;
		}
		gameObject.transform.localPosition = localPosition;
		Match3ParticleSystem component = gameObject.GetComponent<Match3ParticleSystem>();
		if (component != null)
		{
			component.StartParticleSystems();
		}
		GGUtil.SetActive(gameObject, true);
		return gameObject;
	}

	public GameObject CreateParticles(Vector3 localPosition, Match3Particles.PositionType positionType, ChipType chipType, ItemColor itemColor)
	{
		if (this.disableParticles)
		{
			return null;
		}
		Match3Particles.PieceCreatorPool pool = this.GetPool(positionType, chipType);
		if (pool == null || pool.pool == null)
		{
			return null;
		}
		GameObject gameObject = pool.pool.Instantiate(false);
		if (gameObject == null)
		{
			return null;
		}
		gameObject.transform.localPosition = localPosition;
		Match3ParticleSystem component = gameObject.GetComponent<Match3ParticleSystem>();
		if (component != null)
		{
			component.StartParticleSystems();
		}
		Match3Particles.ItemColorModifier modifier = pool.GetModifier(itemColor);
		if (modifier != null)
		{
			modifier.Apply(gameObject);
		}
		GGUtil.SetActive(gameObject, true);
		return gameObject;
	}

	[NonSerialized]
	public bool disableParticles;

	[SerializeField]
	private List<Match3Particles.PieceCreatorPool> pieceCreatorPools = new List<Match3Particles.PieceCreatorPool>();

	private List<Match3Particles.PieceCreatorPool> positionTypeFilteredPools_ = new List<Match3Particles.PieceCreatorPool>();

	public enum PositionType
	{
		OnDestroyChip,
		OnExplosion,
		OnSeekingMissleExplosion,
		BoxDestroy,
		OnRocketStart,
		MagicHatCreate,
		PipeExitParticle,
		PipeEnterParticle,
		OnHammerHit,
		OnHammerPowerHit,
		BombCombine,
		ChipSwipeHorizontal,
		ChipSwipeVertical,
		MissleHitTarget,
		OnDestroyChipDiscoBomb,
		OnDestroyChipLightStart,
		OnDestroyChipPowerupCreate,
		BombCreate,
		BubblesDestroy,
		BubblesCreate,
		OnUIGoalCollected,
		BurriedElementBreak,
		PlacePowerupParticles,
		OnDestroyChipCollect,
		OnDestroyChipExplosion,
		OnDestroyChipRocket,
		BurriedElementTravelParticle,
		ChipTap,
		GoalComplete
	}

	[Serializable]
	public class ItemColorModifier
	{
		public void Apply(GameObject go)
		{
			if (!this.modifyColor)
			{
				return;
			}
			if (go == null)
			{
				return;
			}
			Match3ParticleSystem component = go.GetComponent<Match3ParticleSystem>();
			if (component == null)
			{
				return;
			}
			List<ParticleSystem> allParticleSystems = component.GetAllParticleSystems();
			for (int i = 0; i < allParticleSystems.Count; i++)
			{
				ParticleSystem particleSystem = allParticleSystems[i];
				string text = particleSystem.gameObject.name.ToLower();
				if (text.Contains("_itemcolor_"))
				{
					GGUtil.SetActive(particleSystem.gameObject, text.Contains("_itemcolor_" + this.ItemColor.ToString().ToLower()));
				}
				if (!text.EndsWith("ignore") && this.modifyColor)
				{
					ParticleSystem.MainModule main = particleSystem.main;
					ParticleSystem.MinMaxGradient startColor = main.startColor;
					startColor.colorMin = this.colorMin;
					startColor.colorMax = this.colorMax;
					main.startColor = startColor;
				}
			}
		}

		public ItemColor ItemColor;

		public bool modifyColor;

		public Color colorMin;

		public Color colorMax;
	}

	[Serializable]
	public class PieceCreatorPool
	{
		public bool IsAcceptableForVariant(int variant)
		{
			if (this.abTestVariant == (Match3Particles.PieceCreatorPool.AbTestVariant)variant)
			{
				return true;
			}
			for (int i = 0; i < this.abTestVariantsList.Count; i++)
			{
				if (this.abTestVariantsList[i] == (Match3Particles.PieceCreatorPool.AbTestVariant)variant)
				{
					return true;
				}
			}
			return false;
		}

		public Match3Particles.ItemColorModifier GetModifier(ItemColor itemColor)
		{
			for (int i = 0; i < this.itemColorModifiers.Count; i++)
			{
				Match3Particles.ItemColorModifier itemColorModifier = this.itemColorModifiers[i];
				if (itemColorModifier.ItemColor == itemColor)
				{
					return itemColorModifier;
				}
			}
			return null;
		}

		public Match3Particles.PositionType type;

		[SerializeField]
		public Match3Particles.PieceCreatorPool.AbTestVariant abTestVariant;

		[SerializeField]
		private List<Match3Particles.PieceCreatorPool.AbTestVariant> abTestVariantsList = new List<Match3Particles.PieceCreatorPool.AbTestVariant>();

		public List<ChipType> chipTypeList = new List<ChipType>();

		public List<int> levelList = new List<int>();

		public List<Match3Particles.ItemColorModifier> itemColorModifiers = new List<Match3Particles.ItemColorModifier>();

		public ComponentPool pool;

		public enum AbTestVariant
		{
			Normal,
			CandyType,
			CandyAnimType,
			BurriedTest,
			BarryChipTest
		}
	}
}
