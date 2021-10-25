using System;
using System.Collections.Generic;

namespace GGMatch3
{
	public class ActionManager
	{
		public int ActionCount
		{
			get
			{
				return this.actions.Count;
			}
		}

		public void AddAction(BoardAction action)
		{
			this.AddActionAndRun(action);
		}

		public void OnUpdate(float deltaTime)
		{
			LinkedListNode<BoardAction> next;
			for (LinkedListNode<BoardAction> linkedListNode = this.actions.First; linkedListNode != null; linkedListNode = next)
			{
				next = linkedListNode.Next;
				BoardAction value = linkedListNode.Value;
				if (!value.isStarted)
				{
					value.OnStart(this);
					next = linkedListNode.Next;
				}
				if (value.isAlive)
				{
					value.OnUpdate(deltaTime);
					next = linkedListNode.Next;
				}
				if (!value.isAlive)
				{
					this.actions.Remove(linkedListNode);
					this.OnActionRemoved(value);
				}
			}
		}

		private void AddActionAndRun(BoardAction action)
		{
			this.actions.AddLast(action);
			if (!action.isStarted)
			{
				action.OnStart(this);
			}
			if (action.isAlive)
			{
				action.OnUpdate(0f);
			}
			if (!action.isAlive)
			{
				this.actions.Remove(action);
				this.OnActionRemoved(action);
			}
		}

		private void OnActionRemoved(BoardAction action)
		{
			action.lockContainer.UnlockAll();
		}

		public LinkedList<BoardAction> actions = new LinkedList<BoardAction>();
	}
}
