using System;
using System.Collections.Generic;

namespace ProjectSnake
    {
    public class Position
        {
        public Position(int row, int col)
            {
            Row = row;
            Col = col;
            }

        public int Row { get; }
        public int Col { get; }

        public override bool Equals(object obj)
            {
            return obj is Position possition &&
                   Row == possition.Row &&
                   Col == possition.Col;
            }

        public override int GetHashCode()
            {
            return HashCode.Combine(Row, Col);
            }

        public Position Trnslate(Direction direction)
            {
            return new Position(Row + direction.RowOffset, Col + direction.ColOffset);
            }

        public static bool operator ==(Position left, Position right)
            {
            return EqualityComparer<Position>.Default.Equals(left, right);
            }

        public static bool operator !=(Position left, Position right)
            {
            return !(left == right);
            }
        }
    }
