﻿using System.Linq;
using System.Collections.Generic;

using Quoridor.Core.Utils;
using Quoridor.Core.Extensions;
using Quoridor.Core.Utils.CustomExceptions;

namespace Quoridor.Core.Environment
{
    public class GameEnvironment : IGameEnvironment
    {
        private readonly IBoard _board;

        public GameEnvironment(
            IBoard board)
        {
            _board = board;
        }

        public void AddWall(Vector2 from, Direction placement)
        {
            if (!_board.WithinBounds(from))
                throw new InvalidWallException($"{placement}ern wall from '{from}' could not be added. Invalid dimension.");

            var walls = GetWallsForAffectedCells(from, placement);
            if (walls.All(w => _board.GetCell(w.From).IsAccessible(w.Placement)))
            {
                foreach(var wall in walls)
                {
                    _board.GetCell(wall.From).AddWall(wall);
                }
                //TODO: check if player is blocked and if so, throw
            }
            else throw new WallAlreadyPresentException($"{placement}ern wall from '{from}' already present");
        }

        public void RemoveWall(Vector2 from, Direction placement)
        {
            if (!_board.WithinBounds(from))
                throw new InvalidWallException($"{placement}ern wall from '{from}' could not be removed. Invalid dimension");

            var walls = GetWallsForAffectedCells(from, placement);
            if (walls.All(w => !_board.GetCell(w.From).IsAccessible(w.Placement)))
            {
                foreach(var wall in walls)
                {
                    _board.GetCell(wall.From).RemoveWall(wall);
                }
            }
            else throw new WallNotPresentException($"{placement}ern wall from '{from} not present'");
        }
        
        public IEnumerable<IWall> GetWallsForAffectedCells(Vector2 from, Direction placement)
        {
            var wall = CreateAndValidateWall(from, placement);
            yield return wall;
            yield return CreateAndValidateWall(_board.GetCellAt(from, placement).Position, placement.Opposite());

            var newPos = from.Copy();
            if (wall.IsHorizontal()) newPos.X++;
            else newPos.Y++;

            yield return CreateAndValidateWall(newPos, placement);
            yield return CreateAndValidateWall(_board.GetCellAt(newPos, placement).Position, placement.Opposite());
        }

        public IWall CreateAndValidateWall(Vector2 from, Direction dir)
        {
            var wall = new Wall(dir, from);

            if ((wall.From.X == 0 && wall.Placement.Equals(Direction.West))
                || (wall.From.Y == 0 && wall.Placement.Equals(Direction.North))
                || (wall.From.X == _board.Dimension - 1 && wall.Placement.Equals(Direction.East))
                || (wall.From.Y == _board.Dimension - 1 && wall.Placement.Equals(Direction.South))
                || (wall.From.X >= _board.Dimension - 1 && wall.IsHorizontal())
                || (wall.From.Y >= _board.Dimension - 1 && wall.IsVertical()))
            {
                throw new InvalidWallException($"{dir}ern wall from '{from} not possible.'");
            }
            return wall;
        }
    }
}
