using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGMatch3
{
	public class ConveyorBeltBehaviour : MonoBehaviour
	{
		public void Init(Match3Game game, LevelDefinition.ConveyorBelt conveyorBeltDef, int index)
		{
			base.transform.localPosition = Vector3.zero;
			this.game = game;
			List<LevelDefinition.ConveyorBeltLinearSegment> segmentList = conveyorBeltDef.segmentList;
			for (int i = 0; i < segmentList.Count; i++)
			{
				LevelDefinition.ConveyorBeltLinearSegment segment = segmentList[i];
				this.InitSegment(segment);
			}
			if (conveyorBeltDef.isLoop)
			{
				return;
			}
			Color color = Match3Settings.instance.pipeSettings.GetColor(index);
			this.exitPipe = game.CreatePipeDontAddToSlot();
			this.exitPipe.Init(game.LocalPositionOfCenter(conveyorBeltDef.firstPosition), conveyorBeltDef.firstSegment.direction.ToVector3(), true);
			this.exitPipe.SetColor(color);
			if (game.GetSlot(conveyorBeltDef.firstPosition - conveyorBeltDef.firstSegment.direction) != null)
			{
				GGUtil.SetActive(this.exitPipe, false);
			}
			this.entrancePipe = game.CreatePipeDontAddToSlot();
			this.entrancePipe.Init(game.LocalPositionOfCenter(conveyorBeltDef.lastPosition), conveyorBeltDef.lastSegment.direction.ToVector3(), false);
			this.entrancePipe.SetColor(color);
			if (game.GetSlot(conveyorBeltDef.lastPosition + conveyorBeltDef.lastSegment.direction) != null)
			{
				GGUtil.SetActive(this.entrancePipe, false);
			}
			this.SetTile(0f);
		}

		private void InitSegment(LevelDefinition.ConveyorBeltLinearSegment segment)
		{
		}

		public void SetTile(float tile)
		{
			for (int i = 0; i < this.segments.Count; i++)
			{
				this.segments[i].SetTile(tile);
			}
		}

		public Color GetColor()
		{
			int num = 0;
			if (num >= this.segments.Count)
			{
				return Color.white;
			}
			return this.segments[num].GetColor();
		}

		public void SetColor(Color color)
		{
			for (int i = 0; i < this.segments.Count; i++)
			{
				this.segments[i].SetColor(color);
			}
		}

		[SerializeField]
		private ComponentPool pool = new ComponentPool();

		private Match3Game game;

		[NonSerialized]
		public PipeBehaviour entrancePipe;

		[NonSerialized]
		public PipeBehaviour exitPipe;

		private List<ConveyorBeltSegment> segments = new List<ConveyorBeltSegment>();
	}
}
