using System;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;

using Quoridor.Core.Game;
using Quoridor.Core.Utils;
using Quoridor.Core.Entities;
using Quoridor.Core.Extensions;
using Quoridor.Core.Environment;
using Quoridor.Core.Utils.CustomExceptions;
using static Quoridor.Core.Utils.Direction;

namespace Quoridor.Tests.Game
{
    [TestFixture]
    public class GameEnvironmentTests
    {

        #region AddWall

        [TestCase(South, -1, 0)]
        [TestCase(North, 100, 100)]
        [TestCase(East, 0, 9)]
        public void Should_Throw_If_Incorrect_Wall_Is_Placed_Outside_Of_Board_Bounds(
            Direction dir, int f_x, int f_y)
        {
            //Arrange
            var gameEnv = CreateGameEnvironment().Item2;

            //Act
            Action a = () => gameEnv.AddWall(new Vector2(f_x, f_y), dir);

            //Assert
            a.Should().Throw<InvalidWallException>();
        }

        [TestCase(0, 0, North)]
        [TestCase(0, 8, West)]
        [TestCase(0, 1, West)]
        [TestCase(8, 0, East)]
        [TestCase(8, 0, North)]
        [TestCase(8, 8, South)]
        [TestCase(0, 8, South)]
        [TestCase(0, 8, West)]
        public void Should_Throw_If_Incorrect_Wall_Is_Placed_In_Board_Corners(
            int f_x, int f_y, Direction placement)
        {
            //Arrange
            var gameEnv = CreateGameEnvironment().Item2;

            //Act
            Action a = () => gameEnv.AddWall(new Vector2(f_x, f_y), placement);

            //Assert
            a.Should().Throw<InvalidWallException>();
            
        }

        [TestCase(5, 5, North, 6, 5)]
        [TestCase(5, 5, South, 6, 5)]
        [TestCase(3, 4, East, 3, 4)]
        [TestCase(5, 5, West, 5, 6)]
        public void Should_Throw_If_Wall_Added_WRT_Another_Cell_Is_Already_Present_Or_Intersects_With_Another_Wall(
            int f_x, int f_y, Direction dir, int t_x, int t_y)
        {
            //Arrange
            var gameEnv = CreateGameEnvironment().Item2;
            var pos = new Vector2(f_x, f_y);
            var pos2 = new Vector2(t_x, t_y);

            //Act
            gameEnv.AddWall(pos, dir);
            Action a = () => gameEnv.AddWall(pos2, dir);
            Action b = () => gameEnv.AddWall(pos, dir);

            //Assert
            a.Should().Throw<WallAlreadyPresentException>();
            b.Should().Throw<WallAlreadyPresentException>();
        }

        [Test]
        public void GameEnv_Should_Throw_If_Player_Blocked_By_Wall()
        {
            //Arrange
            var token = CreateGameEnvironment();
            var board = token.Item1;
            var gameEnv = token.Item2;
            gameEnv.Players.Clear();
            var pos = new Vector2(5, 5);
            var player = new Player('A', 10, pos)
            {
                ManhattanHeuristicFn = (cell) => cell.Y,
                IsGoalCell = (cell) => cell.Y == 0
            };
            gameEnv.AddPlayer(player);
            gameEnv.AddWall(new Vector2(5, 5), North);
            gameEnv.AddWall(new Vector2(5, 5), West);
            gameEnv.AddWall(new Vector2(5, 6), South);


            //Act
            Action a = () => gameEnv.AddWall(new Vector2(6, 5), East);

            //Assert
            a.Should().Throw<NewWallBlocksPlayerException>();
        }

        [Test]
        public void Should_Throw_If_Wall_Added_Intersects_With_Another_Wall()
        {
            //Arrange
            var gameEnv = CreateGameEnvironment().Item2;
            var pos = new Vector2(5, 5);

            //Act
            gameEnv.AddWall(pos, North);
            gameEnv.AddWall(pos, South);
            Action a = () => gameEnv.AddWall(pos, East);

            //Assert
            a.Should().Throw<WallIntersectsException>();
        }

        [TestCase(5, 5, North)]
        [TestCase(5, 5, South)]
        [TestCase(3, 4, East)]
        public void Should_Correctly_Set_Walls_If_Added_Correctly(
            int f_x, int f_y, Direction dir)
        {
            //Arrange
            var token = CreateGameEnvironment();
            var gameEnv = token.Item2;
            var board = token.Item1;
            var pos = new Vector2(f_x, f_y);

            //Act
            gameEnv.AddWall(pos, dir);

            //Assert
            board.GetCell(pos).IsAccessible(dir).Should().Be(false);
        }

        [Test]
        public void Should_Correctly_Set_Multiple_Walls_Arround_A_Cell()
        {
            //Arrange
            var items = CreateGameEnvironment();
            var board = items.Item1;
            var gameEnv = items.Item2;
            var pos = new Vector2(5, 5);
            var pos2 = new Vector2(5, 6);
            var pos3 = new Vector2(4, 6);
            var pos4 = new Vector2(6, 5);

            //Act
            gameEnv.AddWall(pos, North);
            gameEnv.AddWall(pos, South);
            gameEnv.AddWall(pos, West);

            //Assert
            //(5,5) is blocked on the northern, southern and western sides
            board.GetCell(pos).IsAccessible(North).Should().Be(false);
            board.GetCell(pos).IsAccessible(South).Should().Be(false);
            board.GetCell(pos).IsAccessible(West).Should().Be(false);
            board.Neighbors(pos).Count().Should().Be(1);

            //(5,6) is blocked on the northern and the western sides
            board.GetCell(pos2).IsAccessible(North).Should().Be(false);
            board.GetCell(pos2).IsAccessible(West).Should().Be(false);
            board.Neighbors(pos2).Count().Should().Be(2);

            //(6,5) is blocked on the northern and southen sides
            board.GetCell(pos4).IsAccessible(North).Should().Be(false);
            board.GetCell(pos4).IsAccessible(South).Should().Be(false);
            board.Neighbors(pos4).Count().Should().Be(2);

            //(4,6) is blocked on the eastern side
            board.GetCell(pos3).IsAccessible(East).Should().Be(false);
            board.Neighbors(pos3).Count().Should().Be(3);
        }

        #endregion


        #region RemoveWall

        [TestCase(5, 5, North)]
        [TestCase(8, 6, West)]
        [TestCase(0, 0, South)]
        [TestCase(0, 6, East)]
        public void Should_Throw_If_Wall_To_Be_Removed_Is_Not_Present(
            int f_x, int f_y, Direction dir)
        {
            //Arrange
            var gameEnv = CreateGameEnvironment().Item2;
            var pos = new Vector2(f_x, f_y);

            //Act
            Action a = () => gameEnv.RemoveWall(pos, dir);

            //Assert
            a.Should().Throw<WallNotPresentException>();
        }

        [TestCase(-1, 0, North)]
        [TestCase(999, 999, South)]
        public void Should_Throw_If_Wall_To_Be_Removed_Is_Outside_Of_Bounds(
            int f_x, int f_y, Direction dir)
        {
            //Arrange
            var gameEnv = CreateGameEnvironment().Item2;
            var pos = new Vector2(f_x, f_y);

            //Act
            Action a = () => gameEnv.RemoveWall(pos, dir);

            //Assert
            a.Should().Throw<InvalidWallException>();
        }

        [TestCase(5, 5, South, 5, 6)]
        [TestCase(0, 0, East, 1, 0)]
        [TestCase(5, 6, North, 5, 5)]
        public void Should_Correctly_Remove_Walls_If_Present(
            int f_x, int f_y, Direction dir, int t_x, int t_y)
        {
            //Arrange
            var token = CreateGameEnvironment();
            var gameEnv = token.Item2;
            var board = token.Item1;
            var pos = new Vector2(f_x, f_y);
            var pos2 = new Vector2(t_x, t_y);

            //Act
            gameEnv.AddWall(pos, dir);
            Action a = () => gameEnv.RemoveWall(pos, dir);

            //Assert
            board.GetCell(pos2).IsAccessible(dir.Opposite()).Should().Be(false);
            board.GetCell(pos).IsAccessible(dir).Should().Be(false);
            a.Should().NotThrow();
            board.GetCell(pos).IsAccessible(dir).Should().Be(true);
            board.GetCell(pos2).IsAccessible(dir.Opposite()).Should().Be(true);
        }

        #endregion


        #region CreateAndValidateWall

        [TestCase(5, 5, North)]
        [TestCase(7, 5, South)]
        public void Should_Create_And_Validate_Walls(
            int f_x, int f_y, Direction dir)
        {
            //Arrange
            var gameEnvironment = CreateGameEnvironment().Item2;
            var from = new Vector2(f_x, f_y);

            //Act
            var wall = gameEnvironment.CreateAndValidateWall(from, dir);

            //Assert
            wall.From.Should().Be(from);
            wall.Placement.Should().Be(dir);
        }

        [TestCase(0, 0, North)]
        [TestCase(0, 8, South)]
        [TestCase(8, 0, East)]
        [TestCase(2, 8, South)]
        public void Should_Throw_If_Wall_Could_Not_Be_Validated(
            int f_x, int f_y, Direction dir)
        {
            //Arrange
            var gameEnvironment = CreateGameEnvironment().Item2;
            var pos = new Vector2(f_x, f_y);

            //Act
            Action a = () => gameEnvironment.CreateAndValidateWall(pos, dir);

            //Assert
            a.Should().Throw<Exception>();
        }

        #endregion


        #region MovePlayer

        [TestCase(0, 0, North)]
        [TestCase(8, 8, South)]
        [TestCase(8, 0, East)]
        [TestCase(0, 8, West)]
        public void Should_Throw_If_Player_Moved_Out_Of_Board(
            int f_x, int f_y, Direction dir)
        {
            //Arrange
            var token = CreateGameEnvironment();
            var gameEnv = token.Item2;
            gameEnv.Players.Clear();
            var player = new Player('A', 8, new Vector2(4, 0));
            gameEnv.AddPlayer(player);
            player.CurrentPos = new Vector2(f_x, f_y);

            //Act
            Action a = () => gameEnv.MovePlayer(dir);

            //Assert
            a.Should().Throw<InvalidAgentMoveException>();
        }

        [TestCase(5, 5, North, 5, 4)]
        [TestCase(2, 3, East, 3, 3)]
        [TestCase(8, 8, West, 7, 8)]
        [TestCase(0, 0, South, 0, 1)]
        [TestCase(0, 8, East, 1, 8)]
        public void Should_Correctly_Move_Player_If_Valid_Direction_Is_Provided(
            int f_x, int f_y, Direction dir, int t_x, int t_y)
        {
            //Arrange
            var token = CreateGameEnvironment();
            var gameEnv = token.Item2;
            gameEnv.Players.Clear();
            var player = new Player('A', 8, new Vector2(4, 0));
            gameEnv.AddPlayer(player);
            player.CurrentPos = new Vector2(f_x, f_y);
            var expectedNewPos = new Vector2(t_x, t_y);

            //Act
            gameEnv.MovePlayer(dir);

            //Assert
            player.CurrentPos.Should().Be(expectedNewPos);
        }

        [TestCase(0, 7, 0, 8, South)]
        [TestCase(1, 0, 0, 0, West)]
        [TestCase(7, 0, 8, 0, East)]
        [TestCase(2, 1, 2, 0, North)]
        public void Should_Throw_If_Player_Tried_Jumping_Outside_Board_Bounds(
            int f_x, int f_y, int t_x, int t_y, Direction dir)
        {
            //Arrange
            var gameEnv = CreateGameEnvironment().Item2;
            gameEnv.Players.Clear();
            var p1 = new Player('A', 8, new Vector2(f_x, f_y));
            var p2 = new Player('B', 8, new Vector2(t_x, t_y));
            gameEnv.AddPlayer(p1);
            gameEnv.AddPlayer(p2);

            //Act
            Action a = () => gameEnv.MovePlayer(dir);

            //Assert
            a.Should().Throw<InvalidAgentMoveException>();
        }


        [TestCase(5, 5, 5, 4, North, 5, 3)]
        [TestCase(1, 3, 2, 3, East, 3, 3)]
        [TestCase(8, 8, 7, 8, West, 6, 8)]
        public void Player_Should_Jump_If_Another_Player_Is_On_Its_Path(
            int f_x, int f_y, int t_x, int t_y, Direction dir, int d_x, int d_y
            )
        {
            //Arrange
            var gameEnv = CreateGameEnvironment().Item2;
            gameEnv.Players.Clear();
            var p1 = new Player('A', 8, new Vector2(f_x, f_y));
            var p2 = new Player('B', 8, new Vector2(t_x, t_y));
            gameEnv.AddPlayer(p1);
            gameEnv.AddPlayer(p2);

            //Act
            gameEnv.MovePlayer(dir);

            //Assert
            p1.CurrentPos.X.Should().Be(d_x);
            p1.CurrentPos.Y.Should().Be(d_y);
        }

        [TestCase(5, 5, North)]
        [TestCase(4, 3, South)]
        [TestCase(2, 2, South)]
        [TestCase(1, 0, East)]
        [TestCase(1, 0, West)]
        public void Should_Throw_If_Player_Tried_To_Move_Past_Wall(
            int f_x, int f_y, Direction dir)
        {
            //Arrange
            var token = CreateGameEnvironment();
            var gameEnv = token.Item2;
            
            var pos = new Vector2(f_x, f_y);
            gameEnv.CurrentPlayer.CurrentPos = pos;

            //Act
            gameEnv.AddWall(pos, dir);
            gameEnv.AddWall(pos, dir.Opposite());
            Action a = () => gameEnv.MovePlayer(dir);
            Action b = () => gameEnv.MovePlayer(dir.Opposite());

            //Assert
            a.Should().Throw<NewMoveBlockedByWallException>();
            b.Should().Throw<NewMoveBlockedByWallException>();
        }

        [TestCase(5, 5, North)]
        [TestCase(4, 4, East)]
        public void Walls_Hashmap_Should_Correctly_Identify_Different_POV_Wall(
            int f_x, int f_y, Direction dir)
        {
            //Arrange
            var token = CreateGameEnvironment();
            var gameEnv = token.Item2;
            var pos = new Vector2(f_x, f_y);

            //Act
            gameEnv.AddWall(pos, dir);

            //Assert
            gameEnv.Walls.Contains(new Wall(dir, pos)).Should().BeTrue();
            var oppPos = pos.GetPosFor(dir);
            var oppDir = dir.Opposite();
            var h = gameEnv.Walls.First().GetHashCode();
            gameEnv.Walls.Contains(new Wall(oppDir, oppPos)).Should().BeTrue();
        }

        #endregion

        private Tuple<IBoard, GameEnvironment> CreateGameEnvironment()
        {
            var board = new Board();
            board.SetDimension(9);
            var gameEnv = new GameEnvironment(board);
            var player = new Player('A', 8, new Vector2(4, 0))
            {
                ManhattanHeuristicFn = (cell) => Math.Abs(8 - cell.Y),
                IsGoalCell = (cell) => cell.Y == 8
            };
            gameEnv.AddPlayer(player);

            return Tuple.Create<IBoard, GameEnvironment>(board, gameEnv);
        }
    }
}
