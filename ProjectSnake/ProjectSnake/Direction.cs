﻿
using System;
using System.Collections.Generic;

namespace ProjectSnake
    {
    public class Direction
        {
        public readonly static Direction Left = new Direction(0, -1);
        public readonly static Direction Right = new Direction(0, 1);
        public readonly static Direction Up = new Direction(-1, 0);
        public readonly static Direction Down = new Direction(1, 0);
        public Direction(int rowOffset, int colOffset)
            {
            RowOffset = rowOffset;
            ColOffset = colOffset;
            }

        public int RowOffset { get; set; }
        public int ColOffset { get; set; }

        public override bool Equals(object obj)
            {
            return obj is Direction direction &&
                   RowOffset == direction.RowOffset &&
                   ColOffset == direction.ColOffset;
            }

        public override int GetHashCode()
            {
            return HashCode.Combine(RowOffset, ColOffset);
            }

        public Direction Opposite()
            {
            return new Direction(-RowOffset, -ColOffset);
            }

        public static bool operator ==(Direction left, Direction right)
            {
            return EqualityComparer<Direction>.Default.Equals(left, right);
            }

        public static bool operator !=(Direction left, Direction right)
            {
            return !(left == right);
            }
        }
    }
