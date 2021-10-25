using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class RoomsStats : MonoBehaviour
{
	public void CheckStats()
	{
		this.action = this.DoCheckStats();
	}

	private IEnumerator DoCheckStats()
	{
		return new RoomsStats._003CDoCheckStats_003Ed__2(0)
		{
			_003C_003E4__this = this
		};
	}

	private int TotalStarsCount(RoomsDB.Room room)
	{
		int num = 0;
		List<VisualObjectBehaviour> visualObjectBehaviours = room.sceneBehaviour.visualObjectBehaviours;
		for (int i = 0; i < visualObjectBehaviours.Count; i++)
		{
			VisualObjectBehaviour visualObjectBehaviour = visualObjectBehaviours[i];
			if (visualObjectBehaviour.isPlayerControlledObject)
			{
				num += visualObjectBehaviour.visualObject.sceneObjectInfo.price.cost;
			}
		}
		return num;
	}

	private void Update()
	{
		if (this.action != null)
		{
			this.action.MoveNext();
		}
	}

	private IEnumerator action;

	private sealed class _003CDoCheckStats_003Ed__2 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoCheckStats_003Ed__2(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		bool IEnumerator.MoveNext()
		{
			int num = this._003C_003E1__state;
			RoomsStats roomsStats = this._003C_003E4__this;
			if (num == 0)
			{
				this._003C_003E1__state = -1;
				this._003CallRoomsStars_003E5__2 = 0;
				this._003CroomsDB_003E5__3 = ScriptableObjectSingleton<RoomsDB>.instance;
				this._003Crooms_003E5__4 = this._003CroomsDB_003E5__3.rooms;
				this._003Ci_003E5__5 = 0;
				goto IL_163;
			}
			if (num != 1)
			{
				return false;
			}
			this._003C_003E1__state = -1;
			IL_B4:
			if (this._003CupdateEnum_003E5__8.MoveNext())
			{
				float progress = this._003CroomRequest_003E5__7.progress;
				this._003C_003E2__current = null;
				this._003C_003E1__state = 1;
				return true;
			}
			if (this._003Croom_003E5__6.sceneBehaviour == null)
			{
				UnityEngine.Debug.Log("CANT LOAD ROOM " + this._003Croom_003E5__6.name);
			}
			else
			{
				int num2 = roomsStats.TotalStarsCount(this._003Croom_003E5__6);
				this._003Croom_003E5__6.totalStarsInRoom = num2;
				UnityEngine.Debug.LogFormat("{0}: {1}", new object[]
				{
					this._003Croom_003E5__6.name,
					num2
				});
				this._003CallRoomsStars_003E5__2 += num2;
				this._003Croom_003E5__6 = null;
				this._003CroomRequest_003E5__7 = null;
				this._003CupdateEnum_003E5__8 = null;
			}
			int num3 = this._003Ci_003E5__5;
			this._003Ci_003E5__5 = num3 + 1;
			IL_163:
			if (this._003Ci_003E5__5 >= this._003Crooms_003E5__4.Count)
			{
				UnityEngine.Debug.LogFormat("Total: {0}", new object[]
				{
					this._003CallRoomsStars_003E5__2
				});
				return false;
			}
			this._003Croom_003E5__6 = this._003Crooms_003E5__4[this._003Ci_003E5__5];
			this._003CroomRequest_003E5__7 = new RoomsDB.LoadRoomRequest(this._003Croom_003E5__6);
			this._003CupdateEnum_003E5__8 = this._003CroomsDB_003E5__3.LoadRoom(this._003CroomRequest_003E5__7);
			goto IL_B4;
		}

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this._003C_003E2__current;
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this._003C_003E2__current;
			}
		}

		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public RoomsStats _003C_003E4__this;

		private int _003CallRoomsStars_003E5__2;

		private RoomsDB _003CroomsDB_003E5__3;

		private List<RoomsDB.Room> _003Crooms_003E5__4;

		private int _003Ci_003E5__5;

		private RoomsDB.Room _003Croom_003E5__6;

		private RoomsDB.LoadRoomRequest _003CroomRequest_003E5__7;

		private IEnumerator _003CupdateEnum_003E5__8;
	}
}
