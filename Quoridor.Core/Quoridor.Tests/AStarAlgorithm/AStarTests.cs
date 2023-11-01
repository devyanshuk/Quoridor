using System;
using NUnit.Framework;
using FluentAssertions;

using Quoridor.Core.Game;
using Quoridor.Core.Utils;
using Quoridor.Core.Entities;
using Quoridor.Core.Environment;
using Quoridor.AI.AStarAlgorithm;
using static Quoridor.Core.Utils.Direction;

namespace Quoridor.Tests.AStarAlgorithm
{
    [TestFixture]
    public class AStarTests
    {
        [TestCase(4, 7, 0, 0, 4, 6)]
        [TestCase(4, 8, 0, 0, 4, 7)]
        [TestCase(4, 0, 8, 8, 4, 1)]
        [TestCase(4, 2, 8, 8, 4, 3)]
        public void AStar_Should_Correctly_Find_Shorest_Path_To_Goal_Vertical(
           int f_x, int f_y, int goalY, int h_n_offset, int goal_x, int goal_y)
        {
            //Arrange
            var token = CreateGameEnvironment();
            var board = token.Item1;
            var gameEnv = token.Item2;
            var pos = new Vector2(f_x, f_y);
            var player = new Player('A', 10, pos)
            {
                ManhattanHeuristicFn = (cell) => Math.Abs(h_n_offset - cell.Y),
                IsGoalCell = (cell) => cell.Y == goalY
            };

            var goal = new Vector2(goal_x, goal_y);

            gameEnv.AddPlayer(player);

            //Act
            var nextBestPos = new AStar<Vector2, IBoard, IPlayer>().BestMove(board, player);

            //Assert
            nextBestPos.Should().Be(goal);
        }

        [TestCase(5, 5, 5, 5, North, 4, 5)]
        [TestCase(5, 5, 5, 5, South, 5, 4)]
        [TestCase(5, 5, 5, 5, West, 5, 4)]
        [TestCase(5, 5, 5, 5, East, 5, 4)]
        [TestCase(5, 5, 5, 4, North, 5, 4)]
        public void Should_Compute_Correct_Path_If_Walls_Are_Present_On_Its_Way(
            int f_x, int f_y, int w_x, int w_y, Direction wallDir, int goal_x, int goal_y)
        {
            //Arrange
            var token = CreateGameEnvironment();
            var board = token.Item1;
            var gameEnv = token.Item2;
            var pos = new Vector2(f_x, f_y);
            var player = new Player('A', 10, pos)
            {
                ManhattanHeuristicFn = (cell) => cell.Y,
                IsGoalCell = (cell) => cell.Y == 0
            };

            var goal = new Vector2(goal_x, goal_y);

            gameEnv.AddPlayer(player);
            gameEnv.AddWall(new Vector2(w_x, w_y), wallDir);

            //Act
            var shortestPath = new AStar<Vector2, IBoard, IPlayer>().BestMove(board, player);

            //Assert
            shortestPath.Should().Be(goal);
        }

        private Tuple<IBoard, GameEnvironment> CreateGameEnvironment()
        {
            var board = new Board();
            board.SetDimension(9);
            var gameEnv = new GameEnvironment(board);

            return Tuple.Create<IBoard, GameEnvironment>(board, gameEnv);
        }
    }
}
