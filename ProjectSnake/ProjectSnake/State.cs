using System;
using System.Collections.Generic;
using System.Windows.Documents;

namespace ProjectSnake
    {
    public class State
        {
        public int Rows { get; }
        public int Cols { get; }
        public GridValue[,] Grid { get; }
        public Direction Dir { get; private set; }
        public int Score { get; private set; }
        public bool GameOver { get; private set; }


        private readonly LinkedList<Direction> dirChanges = new LinkedList<Direction>();
        private readonly LinkedList<Position> snakePositions = new LinkedList<Position>();
        private readonly Random random = new Random();

        public State(int rows, int cols)
            {
            Rows = rows;
            Cols = cols;
            Grid = new GridValue[Rows, Cols];
            Dir = Direction.Right;

            AddSnake();
            AddFood();
            }

        private void AddSnake()
            {
            int r = Rows / 2;

            for (int c = 1; c <= 3; c++)
                {

                Grid[r, c] = GridValue.Snake;
                snakePositions.AddFirst(new Position(r, c));

                }
            }

        private IEnumerable<Position> EmptyPositions()
            {
            for (int r = 0; r < Rows; r++)
                {
                for (var c = 0; c < Cols; c++)
                    {
                    if (Grid[r, c] == GridValue.Empty)
                        {
                        yield return new Position(r, c);
                        }
                    }
                }
            }

        private void AddFood()
            {
            List<Position> empty = new List<Position>(EmptyPositions());

            if (empty.Count == 0)
                {
                return;
                }

            Position pos = empty[random.Next(empty.Count)];
            Grid[pos.Row, pos.Col] = GridValue.Food;
            }

        public Position HeadPosition()
            {
            return snakePositions.First.Value;
            }

        public Position TailPosition()
            {
            return snakePositions.Last.Value;
            }

        public IEnumerable<Position> SnakePositions()
            {
            return snakePositions;
            }

        private void AddHead(Position pos)
            {
            snakePositions.AddFirst(pos);
            Grid[pos.Row, pos.Col] = GridValue.Snake;
            }

        private void RemoveTail()
            {
            Position tail = snakePositions.Last.Value;
            Grid[tail.Row, tail.Col] = GridValue.Empty;
            snakePositions.RemoveLast();
            }

        private Direction GetLastDirection()
            {
            if (dirChanges.Count == 0)
                {
                return Dir;
                }
            return dirChanges.Last.Value;
            }

        private bool CanChangeDirectio(Direction dir)
            {
            if (dirChanges.Count == 2)
                {
                return false;
                }
            Direction lastDir = GetLastDirection();
            return dir != lastDir && dir != lastDir.Opposite();
            }

        public void ChangeDirection(Direction direction)
            {
            if (CanChangeDirectio(direction))
                {
                dirChanges.AddLast(direction);
                }
            }

        private bool OutsideGrid(Position pos)
            {
            return pos.Row < 0 || pos.Col < 0 || pos.Row >= Rows || pos.Col >= Cols;
            }

        private GridValue WillHit(Position pos)
            {
            if (OutsideGrid(pos))
                {
                return GridValue.Outside;
                }
            else if (pos == TailPosition())
                {
                return GridValue.Empty;
                }

            return Grid[pos.Row, pos.Col];
            }

        public void Move()
            {
            if (dirChanges.Count > 0)
                {
                Dir = dirChanges.First.Value;
                dirChanges.RemoveFirst();
                }
            Position head = HeadPosition().Trnslate(Dir);
            GridValue hit = WillHit(head);

            if (hit == GridValue.Outside || hit == GridValue.Snake)
                {
                GameOver = true;
                }
            else if (hit == GridValue.Empty)
                {
                RemoveTail();
                AddHead(head);

                }
            else if (hit == GridValue.Food)
                {
                AddHead(head);
                AddFood();
                Score++;
                }

            }
        }
    }
