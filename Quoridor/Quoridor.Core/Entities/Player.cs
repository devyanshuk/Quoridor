﻿using Quoridor.Core.Utils;
using Quoridor.AI.AStarAlgorithm;

namespace Quoridor.Core.Entities
{
    public class Player : IPlayer
    {
        public int NumWalls { get; set; }
        public char Id { get; }
        public Vector2 StartPos { get; }
        public Vector2 CurrentPos { get; set; }

        //each player's 'is-goal' criteria will be different based on their
        //starting position, so this delegate is necessary.
        public IsGoal<Vector2> IsGoalMove { get; set; }

        //each player's heuristic function also differs based on their starting and
        //goal cells, so this delegate is also necessary. eg: if a player starts at
        //the bottom of the board, their goal is the top, so the heuristic would be
        // |0 - player.Position.Y| (vertical distance to the goal row).
        public H_n<Vector2> ManhattanHeuristicFn { get; set; }

        private int _startNumWalls;
        private readonly object _lock = new object();

        public Player(char id, int numWalls, Vector2 startPos)
        {
            Id = id;
            NumWalls = numWalls;
            StartPos = startPos;
            CurrentPos = startPos;
            _startNumWalls = numWalls;
        }

        public bool Won() => IsGoalMove(CurrentPos);

        public void Initialize()
        {
            lock (_lock)
            {
                CurrentPos = StartPos.Copy();
                NumWalls = _startNumWalls;
            }
        }

        public void Move(Vector2 newPos)
        {
            lock(_lock)
                CurrentPos = newPos.Copy();
        }

        public int CurrX => CurrentPos.X;
        public int CurrY => CurrentPos.Y;

        public void DecreaseWallCount()
        {
            lock(_lock)
                NumWalls--;
        }

        public void IncreaseWallCount()
        {
            lock(_lock)
                NumWalls++;
        }

        public bool Equals(IPlayer other)
            => Id == other.Id && StartPos.Equals(other.StartPos);

        public override string ToString()
            => Id.ToString();

        public Vector2 GetCurrentMove() => CurrentPos;

        Movement IAStarPlayer<Movement>.GetCurrentMove() => CurrentPos;

        public bool IsGoal(Movement move) => IsGoalMove((Vector2)move);

        public double CalculateHeuristic(Movement move) => ManhattanHeuristicFn((Vector2)move);

        public bool IsGoal(Vector2 move) => IsGoalMove(move);

        public double CalculateHeuristic(Vector2 move) => ManhattanHeuristicFn(move);

        public IPlayer DeepCopy()
        {
            return new Player(Id, NumWalls, StartPos.Copy())
            {
                CurrentPos = CurrentPos.Copy(),
                _startNumWalls = _startNumWalls,
                IsGoalMove = IsGoalMove,
                ManhattanHeuristicFn = ManhattanHeuristicFn
            };
        }
    }
}
