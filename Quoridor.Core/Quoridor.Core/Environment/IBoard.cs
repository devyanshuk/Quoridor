using Quoridor.Core.Utils;
using Quoridor.AI.Interfaces;
using System.Collections.Generic;

namespace Quoridor.Core.Environment
{
    public interface IBoard : INeighbors<Vector2>
    {
        // Dimension of the game board.
        int Dimension { get; }

        // Cell objects representing the board.
        Cell[,] Cells { get; }

        // Initialzes the game cells and other helpers
        void Initialize();

        // set the dimension of the game board.
        void SetDimension(int dimension);

        // Check if a coordinate is within the bounds of the board.
        bool WithinBounds(Vector2 pos);

        // Given a coordinate, return the cell object referenced by that coordinate.
        Cell GetCell(Vector2 vec);
        Cell GetCell(int x, int y);

        // Given a cell and a direction from that cell, return a cell referencing
        // that position. Eg: GetCellAt((5,5), North) would be the cell represented
        // by the coordinate (5,4)
        Cell GetCellAt(Vector2 from, Direction dir);

        // Given a valid cell, or a coordinate to the cell, return all
        // valid directions to neighbors surrounding that cell (no diagonal ones).
        IEnumerable<Direction> NeighborDirs(Cell refCell);
        IEnumerable<Direction> NeighborDirs(Vector2 pos);
    }
}
