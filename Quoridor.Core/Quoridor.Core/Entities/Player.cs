using Quoridor.Core.Utils;
using Quoridor.AI.Interfaces;
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

        public Player(char id, int numWalls, Vector2 startPos)
        {
            Id = id;
            NumWalls = numWalls;
            StartPos = startPos;
            CurrentPos = startPos;
        }

        public void Move(Vector2 newPos)
        {
            CurrentPos = newPos;
        }

        public int CurrX => CurrentPos.X;
        public int CurrY => CurrentPos.Y;

        public void DecreaseWallCount()
        {
            NumWalls--;
        }

        public void IncreaseWallCount()
        {
            NumWalls++;
        }

        public bool Equals(IPlayer other)
        {
            return Id == other.Id && StartPos == other.StartPos;
        }

        public override string ToString()
        {
            return Id.ToString();
        }

        public Vector2 GetCurrentMove() => CurrentPos;

        Movement IAStarPlayer<Movement>.GetCurrentMove() => CurrentPos;

        public bool IsGoal(Movement move) => IsGoalMove((Vector2)move);

        public double CalculateHeuristic(Movement move) => ManhattanHeuristicFn((Vector2)move);

        public bool IsGoal(Vector2 move) => IsGoalMove(move);

        public double CalculateHeuristic(Vector2 move) => ManhattanHeuristicFn(move);
    }
}
