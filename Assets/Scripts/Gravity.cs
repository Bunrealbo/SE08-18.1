using System;
using System.Collections.Generic;

namespace GGMatch3
{
    public struct Gravity
    {
        public List<Slot> FindSlotsToWhichCanJumpTo(Slot origin, Match3Game game)
        {
            List<Slot> list = new List<Slot>();
            IntVector2 position = origin.position;
            List<IntVector2> forceDirections = this.forceDirections;
            int i = 0;
            while (i < forceDirections.Count)
            {
                IntVector2 a = forceDirections[i];
                int num = 0;
                for (; ; )
                {
                    num++;
                    IntVector2 position2 = position + a * num;
                    if (game.board.IsOutOfBoard(position2))
                    {
                        break;
                    }
                    Slot slot = game.GetSlot(position2);
                    if (slot != null)
                    {
                        if (num != 1)
                        {
                            list.Add(slot);
                        }
                    }
                }

                i++;
            }
            return list;
        }

        public List<IntVector2> orthoDirections
        {
            get
            {
                if (this.orthoDirections_ != null)
                {
                    return this.orthoDirections_;
                }
                this.orthoDirections_ = new List<IntVector2>();
                if (this.up || this.down)
                {
                    this.orthoDirections_.Add(IntVector2.right);
                    this.orthoDirections_.Add(IntVector2.left);
                }
                if (this.left || this.right)
                {
                    this.orthoDirections_.Add(IntVector2.up);
                    this.orthoDirections_.Add(IntVector2.down);
                }
                return this.orthoDirections_;
            }
        }

        public List<IntVector2> forceDirections
        {
            get
            {
                if (this.directions_ != null)
                {
                    return this.directions_;
                }
                this.directions_ = new List<IntVector2>();
                if (this.up)
                {
                    this.directions_.Add(IntVector2.up);
                }
                if (this.down)
                {
                    this.directions_.Add(IntVector2.down);
                }
                if (this.left)
                {
                    this.directions_.Add(IntVector2.left);
                }
                if (this.right)
                {
                    this.directions_.Add(IntVector2.right);
                }
                return this.directions_;
            }
        }

        public List<Gravity.SandflowDirection> sandflowDirections
        {
            get
            {
                if (this.sandflowDirections_ != null)
                {
                    return this.sandflowDirections_;
                }
                this.sandflowDirections_ = new List<Gravity.SandflowDirection>();
                if (this.up)
                {
                    this.sandflowDirections_.Add(new Gravity.SandflowDirection(IntVector2.left, IntVector2.up));
                    this.sandflowDirections_.Add(new Gravity.SandflowDirection(IntVector2.right, IntVector2.up));
                }
                if (this.down)
                {
                    this.sandflowDirections_.Add(new Gravity.SandflowDirection(IntVector2.left, IntVector2.down));
                    this.sandflowDirections_.Add(new Gravity.SandflowDirection(IntVector2.right, IntVector2.down));
                }
                if (this.left)
                {
                    this.sandflowDirections_.Add(new Gravity.SandflowDirection(IntVector2.up, IntVector2.left));
                    this.sandflowDirections_.Add(new Gravity.SandflowDirection(IntVector2.down, IntVector2.left));
                }
                if (this.right)
                {
                    this.sandflowDirections_.Add(new Gravity.SandflowDirection(IntVector2.up, IntVector2.right));
                    this.sandflowDirections_.Add(new Gravity.SandflowDirection(IntVector2.down, IntVector2.right));
                }
                return this.sandflowDirections_;
            }
        }

        public bool up;

        public bool down;

        public bool left;

        public bool right;

        private List<IntVector2> orthoDirections_;

        private List<IntVector2> directions_;

        private List<Gravity.SandflowDirection> sandflowDirections_;

        public struct SandflowDirection
        {
            public SandflowDirection(IntVector2 direction, IntVector2 forceDirection)
            {
                this.direction = direction;
                this.forceDirection = forceDirection;
                this.offset = direction + forceDirection;
            }

            public IntVector2 direction;

            public IntVector2 offset;

            public IntVector2 forceDirection;
        }
    }
}
